using ProyectoFinalParalela.Models;
using ProyectoFinalParalela.Services;

namespace ProyectoFinalParalela
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool continueProgram = true;

            while (continueProgram)
            {
                try
                {
                    Console.Clear();

                    var fileService = new FileService();
                    var sequentialService = new SequentialAnalysisService();
                    var parallelService = new ParallelAnalysisService();
                    var metricsService = new MetricsService();

                    string selectedFile = fileService.ShowFileMenuAndSelect();

                    Console.WriteLine("\nArchivo seleccionado:");
                    Console.WriteLine(selectedFile);

                    var metadata = fileService.GetFileMetadata(selectedFile);

                    Console.WriteLine("\n=== METADATA DEL ARCHIVO ===");
                    Console.WriteLine($"Nombre: {metadata.FileName}");
                    Console.WriteLine($"Ruta: {metadata.FilePath}");
                    Console.WriteLine($"Tamaño: {metadata.FileSizeFormatted}");
                    Console.WriteLine($"Cantidad de registros: {metadata.RecordCount}");
                    Console.WriteLine($"Cantidad de columnas: {metadata.ColumnCount}");
                    Console.WriteLine("Columnas:");

                    foreach (var column in metadata.ColumnNames)
                    {
                        Console.WriteLine($"- {column}");
                    }

                    int totalProcessors = Environment.ProcessorCount;
                    Console.WriteLine($"\nProcesadores lógicos disponibles: {totalProcessors}");

                    int executionMode = AskExecutionMode();

                    int blockSize = 100_000;

                    Console.WriteLine("\nCargando registros...");

                    var sequentialBlocks = fileService.ReadCsvInBlocks(selectedFile, blockSize);
                    var (sequentialResult, sequentialTime) = sequentialService.RunByBlocks(sequentialBlocks);

                    Console.WriteLine($"\n=== RESULTADO SECUENCIAL ===");
                    Console.WriteLine($"Total registros: {sequentialResult.TotalRecords}");
                    Console.WriteLine($"Suma price: {sequentialResult.TotalPrice:F2}");
                    Console.WriteLine($"Suma freight: {sequentialResult.TotalFreight:F2}");
                    Console.WriteLine($"Suma payment: {sequentialResult.TotalPayment:F2}");
                    Console.WriteLine($"Promedio payment: {sequentialResult.AveragePayment:F2}");
                    Console.WriteLine($"Total quantity: {sequentialResult.TotalQuantity}");
                    Console.WriteLine($"Tiempo secuencial: {sequentialTime} ms");

                    if (executionMode == 1)
                    {
                        int[] processorsToTest = { 2, 4, 6, 8, 10, 12, 16 };

                        foreach (int processors in processorsToTest)
                        {
                            if (processors > totalProcessors)
                                continue;

                            EjecutarAnalisisParalelo(
                                selectedFile,
                                blockSize,
                                processors,
                                fileService,
                                parallelService,
                                metricsService,
                                sequentialResult,
                                sequentialTime
                            );
                        }
                    }
                    else
                    {
                        int customProcessors = AskCustomProcessors(totalProcessors);

                        EjecutarAnalisisParalelo(
                            selectedFile,
                            blockSize,
                            customProcessors,
                            fileService,
                            parallelService,
                            metricsService,
                            sequentialResult,
                            sequentialTime
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }

                continueProgram = AskContinue();
            }

            Console.WriteLine("\nPrograma finalizado.");
        }

        // ========================= MÉTODOS AUXILIARES =========================

        static int AskExecutionMode()
        {
            while (true)
            {
                Console.WriteLine("\nSeleccione el modo de análisis:");
                Console.WriteLine("1. Automático (varios núcleos)");
                Console.WriteLine("2. Personalizado (elegir núcleos)");

                Console.Write("\nIngrese una opción: ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int option) && (option == 1 || option == 2))
                    return option;

                Console.WriteLine("Opción inválida.");
            }
        }

        static int AskCustomProcessors(int totalProcessors)
        {
            while (true)
            {
                Console.Write($"\nIngrese cantidad de núcleos (1 - {totalProcessors}): ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= 1 && value <= totalProcessors)
                    return value;

                Console.WriteLine("Valor inválido.");
            }
        }

        static bool AskContinue()
        {
            while (true)
            {
                Console.Write("\n¿Desea analizar otro archivo? (s/n): ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "s")
                    return true;

                if (input == "n")
                    return false;

                Console.WriteLine("Respuesta inválida.");
            }
        }

        static void EjecutarAnalisisParalelo(
            string selectedFile,
            int blockSize,
            int processors,
            FileService fileService,
            ParallelAnalysisService parallelService,
            MetricsService metricsService,
            AnalysisResult sequentialResult,
            long sequentialTime)
        {
            Console.WriteLine($"\n=== ANÁLISIS PARALELO ({processors} procesadores) ===");

            var parallelBlocks = fileService.ReadCsvInBlocks(selectedFile, blockSize);
            var (parallelResult, parallelTime) = parallelService.RunByBlocks(parallelBlocks, processors);

            double speedup = metricsService.CalculateSpeedup(sequentialTime, parallelTime);
            double efficiency = metricsService.CalculateEfficiency(speedup, processors);

            bool isValid = metricsService.ValidateResults(sequentialResult, parallelResult);

            Console.WriteLine($"Total registros: {parallelResult.TotalRecords}");
            Console.WriteLine($"Suma price: {parallelResult.TotalPrice:F2}");
            Console.WriteLine($"Suma freight: {parallelResult.TotalFreight:F2}");
            Console.WriteLine($"Suma payment: {parallelResult.TotalPayment:F2}");
            Console.WriteLine($"Promedio payment: {parallelResult.AveragePayment:F2}");
            Console.WriteLine($"Total quantity: {parallelResult.TotalQuantity}");

            Console.WriteLine($"\nTiempo paralelo: {parallelTime} ms");
            Console.WriteLine($"Speedup: {speedup:F2}x");
            Console.WriteLine($"Eficiencia: {efficiency:P2}");
            Console.WriteLine($"Validación: {(isValid ? "Correcta" : "Incorrecta")}");
            Console.WriteLine($"Conclusión: {metricsService.GetEfficiencyMessage(efficiency)}");
        }
    }
}
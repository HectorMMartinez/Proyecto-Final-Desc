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

                    var fileService = new ArchivosService();
                    var simulationService = new SimulacionVentaService();
                    var benchmarkService = new MetricaService();

                    string selectedFile = fileService.MostrarMenuArchivos();

                    Console.WriteLine("\nArchivo seleccionado:");
                    Console.WriteLine(selectedFile);

                    var metadata = fileService.ObtenerMetadata(selectedFile);

                    Console.WriteLine("\n=== METADATA DEL ARCHIVO ===");
                    Console.WriteLine($"Nombre: {metadata.FileName}");
                    Console.WriteLine($"Ruta: {metadata.FilePath}");
                    Console.WriteLine($"Tamaño: {metadata.FileSizeFormatted}");
                    Console.WriteLine($"Cantidad de registros: {metadata.RecordCount}");
                    Console.WriteLine($"Cantidad de columnas: {metadata.ColumnCount}");

                    int totalProcessors = Environment.ProcessorCount;
                    Console.WriteLine($"\nProcesadores lógicos disponibles: {totalProcessors}");

                    int executionMode = AskExecutionMode();

                    Console.WriteLine("\nCargando CSV en memoria...");

                    var loadSw = System.Diagnostics.Stopwatch.StartNew();
                    var records = fileService.CargarCsv(selectedFile);
                    loadSw.Stop();

                    Console.WriteLine($"Registros cargados: {records.Count}");
                    Console.WriteLine($"Tiempo de carga: {loadSw.ElapsedMilliseconds} ms");

                    var (sequentialResult, sequentialTime) = simulationService.EjecutarSecuencial(records);

                    PrintResults("SECUENCIAL", sequentialResult, sequentialTime);

                    if (executionMode == 1)
                    {
                        int[] processorsToTest = { 2, 4, 6, 8, 10, 12, 16 };

                        foreach (int processors in processorsToTest)
                        {
                            if (processors > totalProcessors)
                                continue;

                            ExecuteParallelSimulation(
                                records,
                                processors,
                                simulationService,
                                benchmarkService,
                                sequentialResult,
                                sequentialTime
                            );
                        }
                    }
                    else
                    {
                        int customProcessors = AskCustomProcessors(totalProcessors);

                        ExecuteParallelSimulation(
                            records,
                            customProcessors,
                            simulationService,
                            benchmarkService,
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

        static void ExecuteParallelSimulation(
            List<RegistroVenta> records,
            int processors,
            SimulacionVentaService simulationService,
            MetricaService benchmarkService,
            SimulationResult sequentialResult,
            long sequentialTime)
        {
            var (parallelResult, parallelTime) = simulationService.EjecutarParalelo(records, processors);

            double speedup = benchmarkService.CalcularSpeedup(sequentialTime, parallelTime);
            double efficiency = benchmarkService.CalcularEficiencia(speedup, processors);
            bool isValid = benchmarkService.ValidarResultados(sequentialResult, parallelResult);

            PrintResults($"PARALELO ({processors})", parallelResult, parallelTime);

            Console.WriteLine($"Speedup: {speedup:F2}x");
            Console.WriteLine($"Eficiencia: {efficiency:P2}");
            Console.WriteLine($"Validación: {(isValid ? "Correcta" : "Incorrecta")}");
            Console.WriteLine($"Conclusión: {benchmarkService.ObtenerMensajeEficiencia(speedup, efficiency)}");
        }

        static void PrintResults(string title, SimulationResult result, long time)
        {
            Console.WriteLine($"\n=== RESULTADO {title} ===");
            Console.WriteLine($"Total registros: {result.TotalRecords}");
            Console.WriteLine($"Total revenue: {result.TotalRevenue:F2}");
            Console.WriteLine($"Costo estimado total: {result.TotalEstimatedCost:F2}");
            Console.WriteLine($"Margen estimado total: {result.TotalEstimatedMargin:F2}");
            Console.WriteLine($"Promedio revenue: {result.AverageRevenue:F2}");
            Console.WriteLine($"Promedio margen: {result.AverageMargin:F2}");

            Console.WriteLine($"Ventas rentables: {result.ProfitableSales}");
            Console.WriteLine($"Ventas en riesgo: {result.RiskSales}");
            Console.WriteLine($"Ventas con pérdida: {result.LossSales}");

            Console.WriteLine($"Score total: {result.TotalScore:F2}");
            Console.WriteLine($"Score promedio: {result.AverageScore:F2}");
            Console.WriteLine($"Score máximo: {result.MaxScore:F2}");

            Console.WriteLine($"Mejor escenario de descuento: {result.BestDiscountScenario}%");
            Console.WriteLine($"Margen del mejor escenario: {result.BestDiscountScenarioMargin:F2}");

            Console.WriteLine($"Tiempo: {time} ms");
        }

        static int AskExecutionMode()
        {
            while (true)
            {
                Console.WriteLine("\nModo:");
                Console.WriteLine("1. Automático");
                Console.WriteLine("2. Personalizado");

                Console.Write("Opción: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int option) && (option == 1 || option == 2))
                    return option;

                Console.WriteLine("Opción inválida.");
            }
        }

        static int AskCustomProcessors(int max)
        {
            while (true)
            {
                Console.Write($"Procesadores (1 - {max}): ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= 1 && value <= max)
                    return value;

                Console.WriteLine("Valor inválido.");
            }
        }

        static bool AskContinue()
        {
            while (true)
            {
                Console.Write("\n¿Desea analizar otro archivo? (s/n): ");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (input == "s")
                    return true;

                if (input == "n")
                    return false;

                Console.WriteLine("Respuesta inválida.");
            }
        }
    }
}
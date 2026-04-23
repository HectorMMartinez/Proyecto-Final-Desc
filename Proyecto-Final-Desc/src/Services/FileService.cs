using CsvHelper;
using CsvHelper.Configuration;
using ProyectoFinalParalela.Models;
using ProyectoFinalParalela.Utils;
using System.Globalization;

namespace ProyectoFinalParalela.Services
{
    public class FileService
    {
        private readonly string _inputFolder;

        public FileService()
        {
            _inputFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "data", "input");
            _inputFolder = Path.GetFullPath(_inputFolder);
        }

        public List<string> GetCsvFiles()
        {
            if (!Directory.Exists(_inputFolder))
                throw new DirectoryNotFoundException($"No se encontró la carpeta de entrada: {_inputFolder}");

            return Directory.GetFiles(_inputFolder, "*.csv")
                            .OrderBy(f => f)
                            .ToList();
        }
        public IEnumerable<List<SalesRecord>> ReadCsvInBlocks(string filePath, int blockSize)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            var block = new List<SalesRecord>(blockSize);

            foreach (var record in csv.GetRecords<SalesRecord>())
            {
                block.Add(record);

                if (block.Count >= blockSize)
                {
                    yield return block;
                    block = new List<SalesRecord>(blockSize);
                }
            }

            if (block.Count > 0)
            {
                yield return block;
            }
        }
        public string ShowFileMenuAndSelect()
        {
            var files = GetCsvFiles();

            if (files.Count == 0)
                throw new Exception("No se encontraron archivos CSV en data/input.");

            Console.WriteLine("Seleccione el archivo a procesar:");
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
            }

            while (true)
            {
                Console.Write("\nIngrese una opción: ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int option) && option >= 1 && option <= files.Count)
                {
                    return files[option - 1];
                }

                Console.WriteLine("Opción inválida. Intente nuevamente.");
            }
        }

        public List<SalesRecord> LoadCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            return csv.GetRecords<SalesRecord>().ToList();
        }

        public FileMetadata GetFileMetadata(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            using var reader = new StreamReader(filePath);
            string? headerLine = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(headerLine))
                throw new Exception("El archivo está vacío o no tiene encabezados.");

            var columns = headerLine.Split(',').Select(c => c.Trim()).ToList();

            int recordCount = File.ReadLines(filePath).Skip(1).Count();

            return new FileMetadata
            {
                FileName = fileInfo.Name,
                FilePath = fileInfo.FullName,
                FileSizeBytes = fileInfo.Length,
                FileSizeFormatted = FileHelper.FormatFileSize(fileInfo.Length),
                RecordCount = recordCount,
                ColumnCount = columns.Count,
                ColumnNames = columns
            };
        }
    }
}

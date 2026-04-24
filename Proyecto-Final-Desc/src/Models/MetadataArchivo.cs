namespace ProyectoFinalParalela.Models
{
    public class FileMetadata
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public string FileSizeFormatted { get; set; } = string.Empty;
        public int RecordCount { get; set; }
        public int ColumnCount { get; set; }
        public List<string> ColumnNames { get; set; } = new();
    }
}
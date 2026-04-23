namespace ProyectoFinalParalela.Models
{
    public class AnalysisResult
    {
        public long TotalRecords { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalFreight { get; set; }
        public decimal TotalPayment { get; set; }
        public long TotalQuantity { get; set; }

        public decimal AveragePayment =>
            TotalRecords == 0 ? 0 : TotalPayment / TotalRecords;
    }
}
using ProyectoFinalParalela.Models;

namespace ProyectoFinalParalela.Services
{
    public class MetricsService
    {
        public double CalculateSpeedup(long sequentialTimeMs, long parallelTimeMs)
        {
            if (parallelTimeMs == 0)
                return 0;

            return (double)sequentialTimeMs / parallelTimeMs;
        }

        public double CalculateEfficiency(double speedup, int processors)
        {
            if (processors == 0)
                return 0;

            return speedup / processors;
        }

        public string GetEfficiencyMessage(double efficiency)
        {
            double percent = efficiency * 100;

            if (percent >= 70)
                return "Vale la pena: eficiencia buena.";

            if (percent >= 65)
                return "Aceptable: todavía puede considerarse.";

            return "No vale la pena: eficiencia baja.";
        }

        public bool ValidateResults(AnalysisResult sequential, AnalysisResult parallel)
        {
            return sequential.TotalRecords == parallel.TotalRecords
                && sequential.TotalPrice == parallel.TotalPrice
                && sequential.TotalFreight == parallel.TotalFreight
                && sequential.TotalPayment == parallel.TotalPayment
                && sequential.TotalQuantity == parallel.TotalQuantity;
        }
    }
}
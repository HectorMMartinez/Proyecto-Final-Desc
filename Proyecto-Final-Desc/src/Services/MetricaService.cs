using ProyectoFinalParalela.Models;

namespace ProyectoFinalParalela.Services
{
    public class MetricaService
    {
        public double CalcularSpeedup(long sequentialTimeMs, long parallelTimeMs)
        {
            if (parallelTimeMs <= 0)
                return 0;

            return (double)sequentialTimeMs / parallelTimeMs;
        }

        public double CalcularEficiencia(double speedup, int processors)
        {
            if (processors <= 0)
                return 0;

            return speedup / processors;
        }

        public string ObtenerMensajeEficiencia(double speedup, double efficiency)
        {
            if (speedup < 1)
                return "No vale la pena: el paralelo fue más lento que el secuencial.";

            double percent = efficiency * 100;

            if (percent >= 70)
                return "Vale la pena: eficiencia buena.";

            if (percent >= 65)
                return "Aceptable: todavía puede considerarse.";

            return "No vale la pena: eficiencia baja.";
        }

        public bool ValidarResultados(SimulationResult sequential, SimulationResult parallel)
        {
            bool countsAreEqual =
                sequential.TotalRecords == parallel.TotalRecords
                && sequential.ProfitableSales == parallel.ProfitableSales
                && sequential.RiskSales == parallel.RiskSales
                && sequential.LossSales == parallel.LossSales;

            bool totalsAreClose =
                SonCercanos(sequential.TotalRevenue, parallel.TotalRevenue)
                && SonCercanos(sequential.TotalEstimatedCost, parallel.TotalEstimatedCost)
                && SonCercanos(sequential.TotalEstimatedMargin, parallel.TotalEstimatedMargin)
                && SonCercanos(sequential.TotalScore, parallel.TotalScore)
                && SonCercanos(sequential.MaxScore, parallel.MaxScore)
                && SonCercanos(sequential.BestDiscountScenarioMargin, parallel.BestDiscountScenarioMargin);

            bool scenarioIsEqual =
                sequential.BestDiscountScenario == parallel.BestDiscountScenario;

            return countsAreEqual && totalsAreClose && scenarioIsEqual;
        }

        private bool SonCercanos(double a, double b, double tolerance = 0.000001)
        {
            double difference = Math.Abs(a - b);
            double scale = Math.Max(Math.Abs(a), Math.Abs(b));

            if (scale == 0)
                return difference <= tolerance;

            return difference / scale <= tolerance;
        }
    }
}
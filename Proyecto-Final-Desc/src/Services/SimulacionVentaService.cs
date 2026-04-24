using System.Collections.Concurrent;
using System.Diagnostics;
using ProyectoFinalParalela.Models;

namespace ProyectoFinalParalela.Services
{
    public class SimulacionVentaService
    {
        private readonly SalesCalculator _calculator = new SalesCalculator();

        public (SimulationResult Result, long ElapsedMs) EjecutarSecuencial(List<RegistroVenta> records)
        {
            Console.WriteLine("\n=== SIMULACIÓN SECUENCIAL ===");

            var sw = Stopwatch.StartNew();

            var result = new SimulationResult();

            foreach (var record in records)
            {
                ProcessRecord(record, result);
            }

            sw.Stop();

            return (result, sw.ElapsedMilliseconds);
        }

        public (SimulationResult Result, long ElapsedMs) EjecutarParalelo(List<RegistroVenta> records, int processors)
        {
            Console.WriteLine($"\n=== SIMULACIÓN PARALELA ({processors} procesadores) ===");

            var sw = Stopwatch.StartNew();

            var partialResults = new ConcurrentBag<SimulationResult>();

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = processors
            };

            Parallel.ForEach(
                Partitioner.Create(0, records.Count),
                options,
                range =>
                {
                    var partial = new SimulationResult();

                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        ProcessRecord(records[i], partial);
                    }

                    partialResults.Add(partial);
                });

            var finalResult = new SimulationResult();

            foreach (var partial in partialResults)
            {
                finalResult.Merge(partial);
            }

            sw.Stop();

            return (finalResult, sw.ElapsedMilliseconds);
        }
        private void ProcessRecord(RegistroVenta record, SimulationResult result)
        {
            double revenue = _calculator.CalcularIngreso(record);
            double cost = _calculator.CalcularCostoEstimado(record);
            double margin = _calculator.CalcularMargen(revenue, cost);

            result.TotalRecords++;
            result.TotalRevenue += revenue;
            result.TotalEstimatedCost += cost;
            result.TotalEstimatedMargin += margin;

            string classification = _calculator.ClasificarVenta(revenue, margin);

            if (classification == "Profitable")
                result.ProfitableSales++;
            else if (classification == "Risk")
                result.RiskSales++;
            else
                result.LossSales++;

            var discountScenario = _calculator.ObtenerMejorDescuento(revenue, cost);

            if (discountScenario.BestMargin > result.BestDiscountScenarioMargin)
            {
                result.BestDiscountScenarioMargin = discountScenario.BestMargin;
                result.BestDiscountScenario = discountScenario.BestDiscount;
            }

            double score = _calculator.CalcularScoreComercial(record, revenue, margin);

            result.TotalScore += score;

            if (score > result.MaxScore)
                result.MaxScore = score;
        }
    }
}
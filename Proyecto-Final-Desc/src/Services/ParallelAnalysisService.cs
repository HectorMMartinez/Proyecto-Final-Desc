using System.Collections.Concurrent;
using System.Diagnostics;
using ProyectoFinalParalela.Models;

namespace ProyectoFinalParalela.Services
{
    public class ParallelAnalysisService
    {
        public (AnalysisResult Result, long ElapsedMs) RunByBlocks(
            IEnumerable<List<SalesRecord>> blocks,
            int maxDegreeOfParallelism)
        {
            Console.WriteLine($"\n=== ANÁLISIS PARALELO POR BLOQUES ({maxDegreeOfParallelism} procesadores) ===");

            var sw = Stopwatch.StartNew();

            var partialResults = new ConcurrentBag<AnalysisResult>();

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            Parallel.ForEach(blocks, options, block =>
            {
                var partial = new AnalysisResult();

                foreach (var r in block)
                {
                    partial.TotalRecords++;
                    partial.TotalPrice += r.price;
                    partial.TotalFreight += r.freight_value;
                    partial.TotalPayment += r.payment_value;
                    partial.TotalQuantity += r.quantity;
                }

                partialResults.Add(partial);
            });

            var finalResult = new AnalysisResult();

            foreach (var partial in partialResults)
            {
                finalResult.TotalRecords += partial.TotalRecords;
                finalResult.TotalPrice += partial.TotalPrice;
                finalResult.TotalFreight += partial.TotalFreight;
                finalResult.TotalPayment += partial.TotalPayment;
                finalResult.TotalQuantity += partial.TotalQuantity;
            }

            sw.Stop();

            return (finalResult, sw.ElapsedMilliseconds);
        }
    }
}
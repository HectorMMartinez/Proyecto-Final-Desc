using System.Diagnostics;
using ProyectoFinalParalela.Models;

namespace ProyectoFinalParalela.Services
{
    public class SequentialAnalysisService
    {
        public (AnalysisResult Result, long ElapsedMs) RunByBlocks(IEnumerable<List<SalesRecord>> blocks)
        {
            Console.WriteLine("\n=== ANÁLISIS SECUENCIAL POR BLOQUES ===");

            var sw = Stopwatch.StartNew();

            var result = new AnalysisResult();

            foreach (var block in blocks)
            {
                foreach (var r in block)
                {
                    result.TotalRecords++;
                    result.TotalPrice += r.price;
                    result.TotalFreight += r.freight_value;
                    result.TotalPayment += r.payment_value;
                    result.TotalQuantity += r.quantity;
                }
            }

            sw.Stop();

            return (result, sw.ElapsedMilliseconds);
        }
    }
}
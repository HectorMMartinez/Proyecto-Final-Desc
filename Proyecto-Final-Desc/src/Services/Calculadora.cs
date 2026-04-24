using ProyectoFinalParalela.Models;

namespace ProyectoFinalParalela.Services
{
    public class SalesCalculator
    {
        public double CalcularIngreso(RegistroVenta record)
        {
            return record.payment_value;
        }

        public double CalcularCostoEstimado(RegistroVenta record)
        {
            return (record.price * 0.65) + record.freight_value;
        }

        public double CalcularMargen(double revenue, double cost)
        {
            return revenue - cost;
        }

        public string ClasificarVenta(double revenue, double margin)
        {
            if (margin > revenue * 0.20)
                return "Profitable";

            if (margin >= 0)
                return "Risk";

            return "Loss";
        }

        public (int BestDiscount, double BestMargin) ObtenerMejorDescuento(double revenue, double cost)
        {
            int[] discounts = { 0, 5, 10, 15, 20, 25 };

            int bestDiscount = 0;
            double bestMargin = revenue - cost;

            foreach (int discount in discounts)
            {
                double discountRate = discount / 100.0;
                double newRevenue = revenue * (1 - discountRate);
                double newMargin = newRevenue - cost;

                if (newMargin > bestMargin)
                {
                    bestMargin = newMargin;
                    bestDiscount = discount;
                }
            }

            return (bestDiscount, bestMargin);
        }

        public double CalcularScoreComercial(RegistroVenta record, double revenue, double margin)
        {
            double score = revenue + (margin * 1.5) + (record.quantity * 10) - record.freight_value;

            for (int i = 1; i <= 50; i++)
            {
                score += Math.Sqrt(Math.Abs(score) + i);
                score += Math.Log(Math.Abs(score) + i + 1);
            }

            return score;
        }
    }
}
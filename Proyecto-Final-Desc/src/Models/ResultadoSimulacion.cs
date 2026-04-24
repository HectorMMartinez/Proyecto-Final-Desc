namespace ProyectoFinalParalela.Models
{
    public class SimulationResult
    {
        public long TotalRecords { get; set; }

        public double TotalRevenue { get; set; }
        public double TotalEstimatedCost { get; set; }
        public double TotalEstimatedMargin { get; set; }

        public long ProfitableSales { get; set; }
        public long RiskSales { get; set; }
        public long LossSales { get; set; }

        public double TotalScore { get; set; }
        public double MaxScore { get; set; }

        public double AverageRevenue =>
            TotalRecords == 0 ? 0 : TotalRevenue / TotalRecords;

        public double AverageMargin =>
            TotalRecords == 0 ? 0 : TotalEstimatedMargin / TotalRecords;

        public double AverageScore =>
            TotalRecords == 0 ? 0 : TotalScore / TotalRecords;

        public int BestDiscountScenario { get; set; }
        public double BestDiscountScenarioMargin { get; set; }

        public void Merge(SimulationResult other)
        {
            TotalRecords += other.TotalRecords;

            TotalRevenue += other.TotalRevenue;
            TotalEstimatedCost += other.TotalEstimatedCost;
            TotalEstimatedMargin += other.TotalEstimatedMargin;

            ProfitableSales += other.ProfitableSales;
            RiskSales += other.RiskSales;
            LossSales += other.LossSales;

            TotalScore += other.TotalScore;

            if (other.MaxScore > MaxScore)
                MaxScore = other.MaxScore;

            if (other.BestDiscountScenarioMargin > BestDiscountScenarioMargin)
            {
                BestDiscountScenarioMargin = other.BestDiscountScenarioMargin;
                BestDiscountScenario = other.BestDiscountScenario;
            }
        }
    }
}
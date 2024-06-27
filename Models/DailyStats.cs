namespace EnergyDashboardApp.Models
{
    public class DailyStats
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double MinConsumption { get; set; }
        public double MaxConsumption { get; set; }
        public double MinGeneration { get; set; }
        public double MaxGeneration { get; set; }
    }
}

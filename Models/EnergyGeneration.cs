namespace EnergyDashboardApp.Models
{
    public class EnergyGeneration
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public double Generation { get; set; } // Assuming generation is measured in a specific unit (e.g., kWh)
    }
}

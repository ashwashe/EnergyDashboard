namespace EnergyDashboardApp.Models
{
    public class EnergyConsumption
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public double Consumption { get; set; } // Assuming consumption is measured in a specific unit (e.g., kWh)
    }
}

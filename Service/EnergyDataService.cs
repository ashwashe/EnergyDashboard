using EnergyDashboardApp.Models;
using MongoDB.Driver;

namespace EnergyDashboardApp.Service
{
    public class EnergyDataService
    {
        private readonly IMongoCollection<EnergyConsumption> _energyConsumptions;
        private readonly IMongoCollection<EnergyGeneration> _energyGenerations;

        public EnergyDataService(IMongoDatabase database)
        {
            _energyConsumptions = database.GetCollection<EnergyConsumption>("EnergyConsumptions");
            _energyGenerations = database.GetCollection<EnergyGeneration>("EnergyGenerations");
        }

        public async Task<List<EnergyConsumption>> GetEnergyConsumptionByUserIdAsync(string userId)
        {
            return await _energyConsumptions.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task<List<EnergyGeneration>> GetEnergyGenerationByUserIdAsync(string userId)
        {
            // Assuming EnergyGeneration also has a UserId to filter by
            return await _energyGenerations.Find(x => x.UserId == userId).ToListAsync();
        }
    }
}

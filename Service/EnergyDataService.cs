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

        public double CalculateTotalConsumptionAsync(string userId)
        {
            //return await _energyConsumptions.Find(x => x.UserId == userId).ToListAsync();

            var filterBuilder = Builders<EnergyConsumption>.Filter;
            var userIdFilter = filterBuilder.Eq(x => x.UserId, userId);

            return _energyConsumptions.Find(userIdFilter).ToList().Sum(x=>x.Consumption);
        }

        public double CalculateTotalGenerationAsync(string userId)
        {
            // Assuming EnergyGeneration also has a UserId to filter by
            //return await _energyGenerations.Find(x => x.UserId == userId).ToListAsync();

            var filterBuilder = Builders<EnergyGeneration>.Filter;
            var userIdFilter = filterBuilder.Eq(x => x.UserId, userId);
            return  _energyGenerations.Find(userIdFilter).ToList().Sum(x => x.Generation);
        }

        public async Task<List<EnergyConsumption>> GetEnergyConsumptionByUserIdAsync(string userId,DateTime startDate, DateTime endDate)
        {
            //return await _energyConsumptions.Find(x => x.UserId == userId).ToListAsync();

            var filterBuilder = Builders<EnergyConsumption>.Filter;
            var dateFilter = filterBuilder.Gte(x => x.Date, startDate) & filterBuilder.Lte(x => x.Date, endDate);
            var userIdFilter = filterBuilder.Eq(x => x.UserId, userId);
            var combinedFilter = filterBuilder.And(userIdFilter, dateFilter);

            return await _energyConsumptions.Find(combinedFilter).ToListAsync();
        }

        public async Task<List<EnergyGeneration>> GetEnergyGenerationByUserIdAsync(string userId, DateTime startDate, DateTime endDate)
        {
            // Assuming EnergyGeneration also has a UserId to filter by
            //return await _energyGenerations.Find(x => x.UserId == userId).ToListAsync();

            var filterBuilder = Builders<EnergyGeneration>.Filter;
            var dateFilter = filterBuilder.Gte(x => x.Date, startDate) & filterBuilder.Lte(x => x.Date, endDate);
            var userIdFilter = filterBuilder.Eq(x => x.UserId, userId);
            var combinedFilter = filterBuilder.And(userIdFilter, dateFilter);

            return await _energyGenerations.Find(combinedFilter).ToListAsync();
        }
    }
}

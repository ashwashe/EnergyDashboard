using EnergyDashboardApp.Models;
using MongoDB.Driver;

namespace EnergyDashboardApp.Service
{
    public interface IAdminService
    {
        Task<bool> Authenticate(string userId, string password);
    }

    public class AdminService : IAdminService
    {
        private readonly IMongoCollection<AdminUser> _adminUsers;

        public AdminService(IMongoDatabase database)
        {
            _adminUsers = database.GetCollection<AdminUser>("adminusers");
        }

        public async Task<bool> Authenticate(string userId, string password)
        {
            var adminUser = await _adminUsers.Find(x => x.UserId == userId && x.Password == password).FirstOrDefaultAsync();
            return adminUser != null;
        }
    }
}

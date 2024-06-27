using EnergyDashboardApp.Models;
using EnergyDashboardApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace EnergyDashboardApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserService _userService;
        private readonly EnergyDataService _energyDataService;

        public DashboardController(UserService userService, EnergyDataService energyDataService)
        {
            _userService = userService;
            _energyDataService = energyDataService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            DashboardViewModel dashboardViewModel = new DashboardViewModel
            {
                Users = users
            };
            return View(dashboardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetEnergyData(string userId, string timePeriod)
        {
            DateTime startDate, endDate;

            switch (timePeriod)
            {
                case "month":
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case "week":
                    int diff = (7 + (DateTime.Now.DayOfWeek - DayOfWeek.Monday)) % 7;
                    startDate = DateTime.Now.AddDays(-1 * diff).Date;
                    endDate = startDate.AddDays(6);
                    break;
                case "year":
                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    endDate = new DateTime(DateTime.Now.Year, 12, 31);
                    break;
                default:
                    return BadRequest("Invalid time period specified.");
            }

            var consumptionData = await _energyDataService.GetEnergyConsumptionByUserIdAsync(userId, startDate, endDate);
            var generationData = await _energyDataService.GetEnergyGenerationByUserIdAsync(userId, startDate, endDate);

            // Example transformation, adjust according to your actual data structure
            var labels = consumptionData.Select(c => c.Date.ToString("yyyy-MM-dd")).ToList();
            var consumptionDataTransformed = consumptionData.Select(c => c.Consumption).ToList();
            var generationDataTransformed = generationData.Select(g => g.Generation).ToList();

            return Json(new { labels, consumptionData = consumptionDataTransformed, generationData = generationDataTransformed });

        }

    }
}

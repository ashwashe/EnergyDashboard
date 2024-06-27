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
        public async Task<IActionResult> GetEnergyData(string userId)
        {
            //var consumptionData = await _energyDataService.GetEnergyConsumptionByUserIdAsync(userId);
            //var generationData = await _energyDataService.GetEnergyGenerationByUserIdAsync(userId);

            //// Transform the data as needed for your frontend
            //var consumptionDataTransformed = consumptionData.Select(c => new { c.Consumption }).ToList();
            //var generationDataTransformed = generationData.Select(g => new { g.Generation }).ToList();

            //return Json(new { consumptionData = consumptionDataTransformed, generationData = generationDataTransformed });

            var consumptionData = await _energyDataService.GetEnergyConsumptionByUserIdAsync(userId);
            var generationData = await _energyDataService.GetEnergyGenerationByUserIdAsync(userId);

            // Example transformation, adjust according to your actual data structure
            var labels = consumptionData.Select(c => c.Date.ToString("yyyy-MM-dd")).ToList();
            var consumptionDataTransformed = consumptionData.Select(c => c.Consumption).ToList();
            var generationDataTransformed = generationData.Select(g => g.Generation).ToList();

            return Json(new { labels, consumptionData = consumptionDataTransformed, generationData = generationDataTransformed });

        }

    }
}

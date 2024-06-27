﻿using EnergyDashboardApp.Models;
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
        public async Task<IActionResult> GetTotalEnergyData(string userId)
        {
            var totalConsumption =  _energyDataService.CalculateTotalConsumptionAsync(userId);
            var totalGeneration =  _energyDataService.CalculateTotalGenerationAsync(userId);

            return Json(new { totalConsumption, totalGeneration });
        }

        [HttpGet]
        public async Task<IActionResult> GetEnergyData(string userId, string timePeriod)
        {
            DateTime startDate, endDate;
            var labels = new List<string>();

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
                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    endDate = new DateTime(DateTime.Now.Year, 12, 31);
                    break;
            }

            var consumptionData = await _energyDataService.GetEnergyConsumptionByUserIdAsync(userId, startDate, endDate);
            var generationData = await _energyDataService.GetEnergyGenerationByUserIdAsync(userId, startDate, endDate);

            // Example transformation, adjust according to your actual data structure

            switch (timePeriod)
            {
                case "month":
                    labels = consumptionData.Select(c => c.Date.ToString("MMM")).ToList();
                    break;
                case "week":
                    labels = consumptionData.Select(c => GetWeekNumber(c.Date)).ToList();
                    break;
                case "year":
                    labels = consumptionData.Select(c => c.Date.ToString("yyyy")).ToList();
                    break;
                default:
                    labels = consumptionData.Select(c => c.Date.ToString("yyyy-MM-dd")).ToList();
                    break;
            }

           
            var consumptionDataTransformed = consumptionData.Select(c => c.Consumption).ToList();
            var generationDataTransformed = generationData.Select(g => g.Generation).ToList();

            return Json(new { labels, consumptionData = consumptionDataTransformed, generationData = generationDataTransformed });

        }

        public string GetWeekNumber(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return "Week " + weekNum;
        }

    }
}

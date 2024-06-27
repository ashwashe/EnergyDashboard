using EnergyDashboardApp.Models;
using Microsoft.AspNetCore.Mvc;
using EnergyDashboardApp.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace EnergyDashboardApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAdminService _adminService;

        public LoginController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminUser user)
        {
            var result = await _adminService.Authenticate(user.UserId, user.Password);
            if (result)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserId),
                        // Add other claims as needed
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Configure your authentication properties as needed
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);


                // Redirect to the admin dashboard or appropriate page
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // Show an error message or redirect back to the login page
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Assuming "Index" is the action for your home page in the "Home" controller
        }
    }
}

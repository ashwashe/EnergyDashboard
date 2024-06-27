using MongoDB.Driver;
using EnergyDashboardApp.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using EnergyDashboardApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// MongoDB configuration
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(mongoDbSettings.ConnectionString));



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // Adjust the path as necessary
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddSingleton<IMongoDatabase>(s =>
{
    var client = new MongoClient(mongoDbSettings.ConnectionString);
    return client.GetDatabase(mongoDbSettings.DatabaseName);
});

// Registering the AdminService
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EnergyDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Define MongoDbSettings class to bind the settings from appsettings.json
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;
using TechGear.Models;
using TechGear.Services;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Set console output encoding to UTF-8
Console.OutputEncoding = Encoding.UTF8;

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // Enable runtime compilation for Razor views

// Add HttpClient for API calls
builder.Services.AddHttpClient("TechGearAPI", client =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register ApiService
builder.Services.AddScoped<IApiService, ApiService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();

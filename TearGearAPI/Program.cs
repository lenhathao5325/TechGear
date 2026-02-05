using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
using TearGearAPI.Model;
using TechGearAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with proper generic type specification
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Identity services
builder.Services.AddDefaultIdentity<ApplicationUserAPI>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//cấu hình cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

//builder.Services.Configure<CloudinarySettings>(
//builder.Configuration.GetSection("CloudinarySettings"));
//builder.Services.AddScoped<IComboService, ComboService>();
// ------------------- Cloudinary -------------------
//var cloudCfg = configuration.GetSection("CloudinarySettings");
//var cloudinary = new Cloudinary(new Account(
//    cloudCfg["CloudName"],
//    cloudCfg["ApiKey"],
//    cloudCfg["ApiSecret"]
//));
//builder.Services.AddSingleton(cloudinary);
//builder.Services.AddScoped<CloudinaryService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowMVC");
app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// DbContext - update connection string in appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed some demo data if empty
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Categories.Any() && !db.Brands.Any() && !db.Products.Any())
    {
        var c1 = new Category { Name = "Laptops" };
        var c2 = new Category { Name = "Accessories" };

        var b1 = new Brand { Name = "Acme" };
        var b2 = new Brand { Name = "TechCo" };

        var p1 = new Product
        {
            Name = "Acme Laptop 15",
            Description = "Powerful laptop",
            Price = 1299.99m,
            Category = c1,
            Brand = b1,
            ProductVariants = new List<ProductVariant>
            {
                new ProductVariant{ SKU = "ACM15-STD", Price = 1299.99m, Stock = 10 }
            },
            ProductImages = new List<ProductImage> { new ProductImage{ Url = "https://via.placeholder.com/400x300" } }
        };

        db.AddRange(c1, c2, b1, b2, p1);
        db.SaveChanges();
    }
}

app.Run();

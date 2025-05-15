using Microsoft.EntityFrameworkCore;
using ShopService;
using ShopService.DataBase;
using ShopService.DataBase.Entities;

const string baseUrl = "/shop-service";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UsePathBase(baseUrl);

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
    await db.Database.MigrateAsync();
    
    if (!await db.Set<Customer>().AnyAsync())
    {
        //Клієнти
        var customer1 = new Customer
        {
            Id = Guid.Parse("b116cbcf-7f9f-45e7-abef-261cbe2c3dd2"),
            FullName = "Олександр Олександров",
            BirthDate = new DateTime(1990, 5, 15, 0, 0, 0, DateTimeKind.Utc),
            RegistrationDate = DateTime.UtcNow.AddMonths(-3)
        };
        var customer2 = new Customer {
            Id = Guid.Parse("a9b607d5-9832-4b81-a6fe-3fa19a416f06"),
            FullName = "Петро Петренко",
            BirthDate = new DateTime(1985, 8, 20, 0, 0, 0, DateTimeKind.Utc),
            RegistrationDate = DateTime.UtcNow.AddYears(-1)
        };
        db.Set<Customer>().AddRange(customer1, customer2);

        //Продукти
        var product1 = new Product {
            Id = Guid.NewGuid(),
            Name = "Samsung Galaxy S25 Edge",
            Category = "SmartPhones",
            SKU = "SM-001",
            Price = 45000
        };
        var product2 = new Product {
            Id = Guid.NewGuid(),
            Name = "Super Laptop",
            Category = "Computers",
            SKU = "SL-123",
            Price = 100000
        };
        db.Set<Product>().AddRange(product1, product2);

        //Покупки
        var purchase1Id = Guid.NewGuid();
        var purchase1 = new Purchase
        {
            Id = purchase1Id,
            PurchaseNumber = "PUR-0001",
            PurchaseDate = DateTime.UtcNow.AddDays(-2),
            TotalAmount = product1.Price * 1 + product2.Price * 2,
            CustomerId = customer1.Id,
            Customer = customer1,
            Items = []
        };

        var purchase2Id = Guid.NewGuid();
        var purchase2 = new Purchase {
            Id = purchase2Id,
            PurchaseNumber = "PUR-0002",
            PurchaseDate = DateTime.UtcNow.AddDays(-10),
            TotalAmount = product2.Price * 2,
            CustomerId = customer2.Id,
            Customer = customer2,
            Items = []
        };
        db.Set<Purchase>().AddRange(purchase1, purchase2);

        //Позиції
        db.Set<PurchaseItem>().AddRange(
            new PurchaseItem
            {
                PurchaseId = purchase1.Id,
                Product = product1,
                ProductId = product1.Id,
                Purchase = purchase1,
                Quantity = 1
            },
            new PurchaseItem
            {
                PurchaseId = purchase1.Id,
                Purchase = purchase1,
                ProductId = product2.Id,
                Product = product2,
                Quantity = 2
            },
            new PurchaseItem
            {
                PurchaseId = purchase2.Id,
                Purchase = purchase2,
                ProductId = product2.Id,
                Product = product2,
                Quantity = 1
            }
        );

        await db.SaveChangesAsync();
    }
}

app.MapGroup("/")
    .MapShopEndpoints();

app.Run();
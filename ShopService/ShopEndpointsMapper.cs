using Microsoft.EntityFrameworkCore;
using ShopService.DataBase;
using ShopService.DataBase.Entities;

namespace ShopService;

public static class ShopEndpointsMapper
{
    public static RouteGroupBuilder MapShopEndpoints(this RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/birthdays", async (DateTime date, ShopDbContext context) =>
        {
            var result = await context.Set<Customer>()
                .AsNoTracking()
                .Where(x => x.BirthDate.Month == date.Month && x.BirthDate.Day == date.Day)
                .Select(x => new
                {
                    x.Id,
                    x.FullName
                })
                .ToListAsync();
        
            return Results.Ok(result);
        });
        
        groupBuilder.MapGet("/recent", async (int days, ShopDbContext context) =>
            {
                var fromDate = DateTime.UtcNow.Date.AddDays(-days);
                var result = await context.Set<Purchase>()
                    .AsNoTracking()
                    .Where(x => x.PurchaseDate >= fromDate)
                    .GroupBy(x => x.Customer)
                    .Select(x => new
                    {
                        CustomerId = x.Key.Id,
                        FullName = x.Key.FullName,
                        LastPurchaseDate = x.Max(p => p.PurchaseDate)
                    })
                    .ToListAsync();
                
                return Results.Ok(result);
            });
        
        groupBuilder.MapGet("/customers/{id:guid}/popular-categories", async (Guid id, ShopDbContext context) =>
        {
            var result = await context.Set<PurchaseItem>()
                .AsNoTracking()
                .Where(x => x.Purchase.CustomerId == id)
                .GroupBy(x => x.Product.Category)
                .Select(x => new
                {
                    Category = x.Key,
                    Quantity = x.Sum(purchaseItem => purchaseItem.Quantity)
                })
                .ToListAsync();
        
            return Results.Ok(result);
        });

        return groupBuilder;
    }
}
namespace ShopService.DataBase.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public required string Category { get; set; }
    public required string SKU { get; set; }
    public decimal Price { get; set; }
    public ICollection<PurchaseItem>? PurchaseItems { get; set; }
}
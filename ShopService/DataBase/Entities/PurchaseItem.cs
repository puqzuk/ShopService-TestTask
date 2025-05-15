namespace ShopService.DataBase.Entities;

public class PurchaseItem
{
    public Guid PurchaseId { get; set; }
    public required Purchase Purchase { get; set; }
    public Guid ProductId { get; set; }
    public required Product Product { get; set; }
    public int Quantity { get; set; }
}
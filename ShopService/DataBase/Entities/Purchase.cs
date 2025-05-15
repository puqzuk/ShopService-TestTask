namespace ShopService.DataBase.Entities;

public class Purchase
{
    public Guid Id { get; set; }
    public required string PurchaseNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid CustomerId { get; set; }
    public required Customer Customer { get; set; }
    public required ICollection<PurchaseItem> Items { get; set; }
}
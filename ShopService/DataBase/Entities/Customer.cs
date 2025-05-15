namespace ShopService.DataBase.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime RegistrationDate { get; set; }
    public ICollection<Purchase>? Purchases { get; set; }
}
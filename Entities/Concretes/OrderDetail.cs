using Core.Entity;
using Entities.Concretes;

public class OrderDetail : BaseEntity<int>
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Navigation properties
    public Order Order { get; set; }
    public Product Product { get; set; }
}
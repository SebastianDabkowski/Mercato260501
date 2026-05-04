using Mercato.Shared;

namespace Mercato.Modules.Orders.Domain;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public decimal TotalAmount { get; set; }
}

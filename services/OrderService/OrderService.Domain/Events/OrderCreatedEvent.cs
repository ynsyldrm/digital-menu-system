using OrderService.Domain.Common;
using MassTransit;

namespace OrderService.Domain.Events;

public class OrderCreatedEvent : DomainEvent, CorrelatedBy<Guid>
{
    public Guid CorrelationId => OrderId;
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public decimal TotalAmount { get; }
    public IEnumerable<OrderItemEventData> OrderItems { get; }

    public OrderCreatedEvent(Guid orderId, Guid customerId, decimal totalAmount, IEnumerable<OrderItemEventData> orderItems)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        OrderItems = orderItems;
    }
}

public class OrderItemEventData
{
    public Guid MenuItemId { get; set; }
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
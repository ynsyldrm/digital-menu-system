using MassTransit;
using OrderService.Domain.Interfaces;
using KitchenService.Domain.Entities;

namespace OrderService.Application.Consumers;

public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
{
    private readonly IOrderRepository _orderRepository;

    public OrderPlacedConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        var message = context.Message;

        // Create kitchen order from the order placed event
        var kitchenOrder = new KitchenOrder
        {
            Id = Guid.NewGuid(),
            OrderId = message.OrderId,
            Status = KitchenOrderStatus.Received,
            ReceivedAt = DateTime.UtcNow,
            OrderItems = message.OrderItems.Select(item => new KitchenOrderItem
            {
                Id = Guid.NewGuid(),
                MenuItemId = item.MenuItemId,
                MenuItemName = item.MenuItemName,
                Quantity = item.Quantity,
                SpecialInstructions = null // Could be added later
            }).ToList()
        };

        // Note: This would need to be saved to Kitchen Service database
        // For now, this demonstrates the event consumption pattern
        await Task.CompletedTask;
    }
}

public class OrderPlacedEvent
{
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public IEnumerable<OrderItemEventData> OrderItems { get; set; } = new List<OrderItemEventData>();
}

public class OrderItemEventData
{
    public Guid MenuItemId { get; set; }
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
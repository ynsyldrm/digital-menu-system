using MassTransit;
using KitchenService.Domain.Interfaces;
using KitchenService.Domain.Entities;

namespace KitchenService.Application.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IKitchenOrderRepository _kitchenOrderRepository;

    public OrderCreatedConsumer(IKitchenOrderRepository kitchenOrderRepository)
    {
        _kitchenOrderRepository = kitchenOrderRepository;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        // Create kitchen order from the order created event
        var kitchenOrder = new KitchenOrder
        {
            Id = Guid.NewGuid(),
            OrderId = message.OrderId,
            Status = KitchenOrderStatus.Received,
            ReceivedAt = DateTime.UtcNow,
            OrderItems = message.OrderItems.Select(item => new KitchenOrderItem
            {
                Id = Guid.NewGuid(),
                KitchenOrderId = Guid.Empty, // Will be set when saved
                MenuItemId = item.MenuItemId,
                MenuItemName = item.MenuItemName,
                Quantity = item.Quantity,
                SpecialInstructions = null
            }).ToList()
        };

        await _kitchenOrderRepository.AddAsync(kitchenOrder);
    }
}

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
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
using MassTransit;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Events;

namespace OrderService.Application.Sagas;

public class OrderProcessingSaga : ISaga, InitiatedBy<OrderCreatedEvent>, Orchestrates<OrderReadyEvent>, Orchestrates<OrderDeliveredEvent>
{
    public Guid CorrelationId { get; set; }
    public Guid OrderId { get; set; }
    public OrderStatus CurrentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? KitchenNotifiedAt { get; set; }
    public DateTime? OrderReadyAt { get; set; }
    public DateTime? DeliveredAt { get; set; }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        // Initialize saga state
        OrderId = context.Message.OrderId;
        CurrentStatus = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        // Publish event to notify kitchen
        await context.Publish(new OrderPlacedEvent(OrderId, context.Message.TotalAmount, context.Message.OrderItems));
        KitchenNotifiedAt = DateTime.UtcNow;

        // Update order status to Confirmed
        CurrentStatus = OrderStatus.Confirmed;
    }

    public async Task Consume(ConsumeContext<OrderReadyEvent> context)
    {
        if (CurrentStatus == OrderStatus.Confirmed)
        {
            OrderReadyAt = context.Message.ReadyAt;
            CurrentStatus = OrderStatus.Ready;

            // Could publish event for delivery notification here
        }
    }

    public async Task Consume(ConsumeContext<OrderDeliveredEvent> context)
    {
        if (CurrentStatus == OrderStatus.Ready)
        {
            DeliveredAt = context.Message.OrderId == OrderId ? DateTime.UtcNow : DeliveredAt;
            CurrentStatus = OrderStatus.Delivered;

            // Saga completes when order is delivered
            await context.Publish(new OrderCompletedEvent(OrderId));
        }
    }
}

public class OrderPlacedEvent
{
    public Guid OrderId { get; }
    public decimal TotalAmount { get; }
    public IEnumerable<OrderItemEventData> OrderItems { get; }

    public OrderPlacedEvent(Guid orderId, decimal totalAmount, IEnumerable<OrderItemEventData> orderItems)
    {
        OrderId = orderId;
        TotalAmount = totalAmount;
        OrderItems = orderItems;
    }
}

public class OrderCompletedEvent
{
    public Guid OrderId { get; }

    public OrderCompletedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}
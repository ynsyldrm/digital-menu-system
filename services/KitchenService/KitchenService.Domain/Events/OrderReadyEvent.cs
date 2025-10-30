using KitchenService.Domain.Common;

namespace KitchenService.Domain.Events;

public class OrderReadyEvent : DomainEvent
{
    public Guid OrderId { get; }
    public DateTime ReadyAt { get; }

    public OrderReadyEvent(Guid orderId, DateTime readyAt)
    {
        OrderId = orderId;
        ReadyAt = readyAt;
    }
}
using OrderService.Domain.Common;
using MassTransit;

namespace OrderService.Domain.Events;

public class OrderReadyEvent : DomainEvent, CorrelatedBy<Guid>
{
    public Guid CorrelationId => OrderId;
    public Guid OrderId { get; }
    public DateTime ReadyAt { get; }

    public OrderReadyEvent(Guid orderId, DateTime readyAt)
    {
        OrderId = orderId;
        ReadyAt = readyAt;
    }
}
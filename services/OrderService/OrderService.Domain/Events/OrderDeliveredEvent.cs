using OrderService.Domain.Common;
using MassTransit;

namespace OrderService.Domain.Events;

public class OrderDeliveredEvent : DomainEvent, CorrelatedBy<Guid>
{
    public Guid CorrelationId => OrderId;
    public Guid OrderId { get; }

    public OrderDeliveredEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}
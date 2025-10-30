using MassTransit;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Events;

namespace OrderService.Application.Sagas;

public class OrderProcessingStateMachine : MassTransitStateMachine<OrderProcessingSagaState>
{
    public State OrderPlaced { get; private set; } = null!;
    public State OrderConfirmed { get; private set; } = null!;
    public State OrderReady { get; private set; } = null!;
    public State OrderDelivered { get; private set; } = null!;

    public Event<OrderCreatedEvent> OrderCreated { get; private set; } = null!;
    public Event<OrderReadyEvent> OrderReadyEvent { get; private set; } = null!;
    public Event<OrderDeliveredEvent> OrderDeliveredEvent { get; private set; } = null!;

    public OrderProcessingStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderReadyEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderDeliveredEvent, x => x.CorrelateById(context => context.Message.OrderId));

        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.TotalAmount = context.Message.TotalAmount;
                    context.Saga.CreatedAt = DateTime.UtcNow;
                })
                .Publish(context => new OrderPlacedEvent(
                    context.Message.OrderId,
                    context.Message.TotalAmount,
                    context.Message.OrderItems))
                .TransitionTo(OrderPlaced)
        );

        During(OrderPlaced,
            When(OrderReadyEvent)
                .Then(context =>
                {
                    context.Saga.OrderReadyAt = context.Message.ReadyAt;
                })
                .TransitionTo(OrderReady)
        );

        During(OrderReady,
            When(OrderDeliveredEvent)
                .Then(context =>
                {
                    context.Saga.DeliveredAt = DateTime.UtcNow;
                })
                .TransitionTo(OrderDelivered)
                .Publish(context => new OrderCompletedEvent(context.Message.OrderId))
        );

        SetCompletedWhenFinalized();
    }
}
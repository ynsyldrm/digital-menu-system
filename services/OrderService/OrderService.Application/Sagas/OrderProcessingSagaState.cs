using MassTransit;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Sagas;

public class OrderProcessingSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? KitchenNotifiedAt { get; set; }
    public DateTime? OrderReadyAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}
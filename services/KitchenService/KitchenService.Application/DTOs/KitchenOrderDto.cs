using KitchenService.Domain.Entities;

namespace KitchenService.Application.DTOs;

public class KitchenOrderDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public KitchenOrderStatus Status { get; set; }
    public DateTime ReceivedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    public IEnumerable<KitchenOrderItemDto> OrderItems { get; set; } = new List<KitchenOrderItemDto>();
}
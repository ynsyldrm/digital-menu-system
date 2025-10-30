namespace KitchenService.Domain.Entities;

public class KitchenOrder
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public KitchenOrderStatus Status { get; set; } = KitchenOrderStatus.Received;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    public ICollection<KitchenOrderItem> OrderItems { get; set; } = new List<KitchenOrderItem>();
}

public enum KitchenOrderStatus
{
    Received,
    Preparing,
    Ready,
    Completed
}
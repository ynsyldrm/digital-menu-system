namespace KitchenService.Domain.Entities;

public class KitchenOrderItem
{
    public Guid Id { get; set; }
    public Guid KitchenOrderId { get; set; }
    public Guid MenuItemId { get; set; }
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? SpecialInstructions { get; set; }
    public KitchenOrder KitchenOrder { get; set; } = null!;
}
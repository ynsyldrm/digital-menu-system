namespace KitchenService.Application.DTOs;

public class KitchenOrderItemDto
{
    public Guid Id { get; set; }
    public Guid MenuItemId { get; set; }
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? SpecialInstructions { get; set; }
}
using MenuService.Domain.Common;

namespace MenuService.Domain.Events;

public class MenuItemCreatedEvent : DomainEvent
{
    public Guid MenuItemId { get; }
    public string Name { get; }
    public decimal Price { get; }

    public MenuItemCreatedEvent(Guid menuItemId, string name, decimal price)
    {
        MenuItemId = menuItemId;
        Name = name;
        Price = price;
    }
}
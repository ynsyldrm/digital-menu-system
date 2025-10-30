using MediatR;
using MenuService.Application.DTOs;
using MenuService.Application.Queries;
using MenuService.Domain.Interfaces;

namespace MenuService.Application.Handlers;

public class GetMenuItemsQueryHandler : IRequestHandler<GetMenuItemsQuery, IEnumerable<MenuItemDto>>
{
    private readonly IMenuItemRepository _menuItemRepository;

    public GetMenuItemsQueryHandler(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<IEnumerable<MenuItemDto>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var menuItems = await _menuItemRepository.GetAllAsync(request.CategoryId, request.IsAvailable);

        return menuItems.Select(item => new MenuItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            CategoryId = item.CategoryId,
            CategoryName = item.Category?.Name ?? string.Empty,
            IsAvailable = item.IsAvailable
        });
    }
}
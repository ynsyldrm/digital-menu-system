using MediatR;
using MenuService.Application.DTOs;

namespace MenuService.Application.Queries;

public class GetMenuItemsQuery : IRequest<IEnumerable<MenuItemDto>>
{
    public Guid? CategoryId { get; set; }
    public bool? IsAvailable { get; set; }
}
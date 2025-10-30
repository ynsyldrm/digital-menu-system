using MediatR;
using MenuService.Application.Commands;
using MenuService.Application.DTOs;
using MenuService.Domain.Entities;
using MenuService.Domain.Events;
using MenuService.Domain.Interfaces;

namespace MenuService.Application.Handlers;

public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, MenuItemDto>
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMediator _mediator;

    public CreateMenuItemCommandHandler(IMenuItemRepository menuItemRepository, IMediator mediator)
    {
        _menuItemRepository = menuItemRepository;
        _mediator = mediator;
    }

    public async Task<MenuItemDto> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.CategoryId,
            IsAvailable = true
        };

        await _menuItemRepository.AddAsync(menuItem);

        // Publish domain event
        await _mediator.Publish(new MenuItemCreatedEvent(menuItem.Id, menuItem.Name, menuItem.Price), cancellationToken);

        return new MenuItemDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Description = menuItem.Description,
            Price = menuItem.Price,
            CategoryId = menuItem.CategoryId,
            IsAvailable = menuItem.IsAvailable
        };
    }
}
using MediatR;
using KitchenService.Application.Commands;
using KitchenService.Application.DTOs;
using KitchenService.Domain.Entities;
using KitchenService.Domain.Events;
using KitchenService.Domain.Interfaces;

namespace KitchenService.Application.Handlers;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, KitchenOrderDto>
{
    private readonly IKitchenOrderRepository _kitchenOrderRepository;
    private readonly IMediator _mediator;

    public UpdateOrderStatusCommandHandler(IKitchenOrderRepository kitchenOrderRepository, IMediator mediator)
    {
        _kitchenOrderRepository = kitchenOrderRepository;
        _mediator = mediator;
    }

    public async Task<KitchenOrderDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await _kitchenOrderRepository.GetByOrderIdAsync(request.OrderId);
        if (kitchenOrder == null)
        {
            throw new KeyNotFoundException($"Kitchen order for Order ID {request.OrderId} not found");
        }

        // Update status and timestamps
        var oldStatus = kitchenOrder.Status;
        kitchenOrder.Status = request.Status;
        kitchenOrder.Notes = request.Notes ?? kitchenOrder.Notes;

        switch (request.Status)
        {
            case KitchenOrderStatus.Preparing:
                kitchenOrder.StartedAt = DateTime.UtcNow;
                break;
            case KitchenOrderStatus.Ready:
                kitchenOrder.CompletedAt = DateTime.UtcNow;
                break;
            case KitchenOrderStatus.Completed:
                kitchenOrder.CompletedAt = DateTime.UtcNow;
                break;
        }

        await _kitchenOrderRepository.UpdateAsync(kitchenOrder);

        // Publish domain event if order is ready
        if (request.Status == KitchenOrderStatus.Ready && oldStatus != KitchenOrderStatus.Ready)
        {
            await _mediator.Publish(new OrderReadyEvent(request.OrderId, kitchenOrder.CompletedAt.Value), cancellationToken);
        }

        return new KitchenOrderDto
        {
            Id = kitchenOrder.Id,
            OrderId = kitchenOrder.OrderId,
            Status = kitchenOrder.Status,
            ReceivedAt = kitchenOrder.ReceivedAt,
            StartedAt = kitchenOrder.StartedAt,
            CompletedAt = kitchenOrder.CompletedAt,
            Notes = kitchenOrder.Notes,
            OrderItems = kitchenOrder.OrderItems.Select(item => new KitchenOrderItemDto
            {
                Id = item.Id,
                MenuItemId = item.MenuItemId,
                MenuItemName = item.MenuItemName,
                Quantity = item.Quantity,
                SpecialInstructions = item.SpecialInstructions
            }).ToList()
        };
    }
}
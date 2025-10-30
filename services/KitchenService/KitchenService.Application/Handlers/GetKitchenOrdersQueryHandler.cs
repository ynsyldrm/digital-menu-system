using MediatR;
using KitchenService.Application.DTOs;
using KitchenService.Application.Queries;
using KitchenService.Domain.Interfaces;

namespace KitchenService.Application.Handlers;

public class GetKitchenOrdersQueryHandler : IRequestHandler<GetKitchenOrdersQuery, IEnumerable<KitchenOrderDto>>
{
    private readonly IKitchenOrderRepository _kitchenOrderRepository;

    public GetKitchenOrdersQueryHandler(IKitchenOrderRepository kitchenOrderRepository)
    {
        _kitchenOrderRepository = kitchenOrderRepository;
    }

    public async Task<IEnumerable<KitchenOrderDto>> Handle(GetKitchenOrdersQuery request, CancellationToken cancellationToken)
    {
        var kitchenOrders = await _kitchenOrderRepository.GetAllAsync(request.Status);

        return kitchenOrders.Select(order => new KitchenOrderDto
        {
            Id = order.Id,
            OrderId = order.OrderId,
            Status = order.Status,
            ReceivedAt = order.ReceivedAt,
            StartedAt = order.StartedAt,
            CompletedAt = order.CompletedAt,
            Notes = order.Notes,
            OrderItems = order.OrderItems.Select(item => new KitchenOrderItemDto
            {
                Id = item.Id,
                MenuItemId = item.MenuItemId,
                MenuItemName = item.MenuItemName,
                Quantity = item.Quantity,
                SpecialInstructions = item.SpecialInstructions
            }).ToList()
        });
    }
}
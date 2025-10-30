using MediatR;
using KitchenService.Application.DTOs;
using KitchenService.Domain.Entities;

namespace KitchenService.Application.Queries;

public class GetKitchenOrdersQuery : IRequest<IEnumerable<KitchenOrderDto>>
{
    public KitchenOrderStatus? Status { get; set; }
}
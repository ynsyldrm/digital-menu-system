using MediatR;
using KitchenService.Application.DTOs;
using KitchenService.Domain.Entities;

namespace KitchenService.Application.Commands;

public class UpdateOrderStatusCommand : IRequest<KitchenOrderDto>
{
    public Guid OrderId { get; set; }
    public KitchenOrderStatus Status { get; set; }
    public string? Notes { get; set; }
}
using MediatR;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands;

public class UpdateOrderStatusCommand : IRequest<OrderDto>
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
}
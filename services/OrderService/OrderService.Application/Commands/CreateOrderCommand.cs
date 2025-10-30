using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Commands;

public class CreateOrderCommand : IRequest<OrderDto>
{
    public Guid CustomerId { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}
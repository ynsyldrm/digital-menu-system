using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries;

public class GetOrderQuery : IRequest<OrderDto>
{
    public Guid OrderId { get; set; }
}
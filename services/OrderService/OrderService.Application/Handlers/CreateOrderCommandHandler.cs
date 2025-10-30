using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using OrderService.Domain.Events;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMediator _mediator;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
    {
        _orderRepository = orderRepository;
        _mediator = mediator;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Status = OrderStatus.Pending,
            TotalAmount = request.OrderItems.Sum(item => item.Quantity * item.UnitPrice),
            OrderItems = request.OrderItems.Select(item => new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(), // Will be set after order creation
                MenuItemId = item.MenuItemId,
                MenuItemName = item.MenuItemName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };

        // Set OrderId for order items
        foreach (var item in order.OrderItems)
        {
            item.OrderId = order.Id;
        }

        await _orderRepository.AddAsync(order);

        // Publish domain event
        var orderItemsEventData = order.OrderItems.Select(item => new OrderItemEventData
        {
            MenuItemId = item.MenuItemId,
            MenuItemName = item.MenuItemName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).ToList();

        await _mediator.Publish(new OrderCreatedEvent(order.Id, order.CustomerId, order.TotalAmount, orderItemsEventData), cancellationToken);

        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            OrderItems = order.OrderItems.Select(item => new OrderItemDto
            {
                MenuItemId = item.MenuItemId,
                MenuItemName = item.MenuItemName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };
    }
}
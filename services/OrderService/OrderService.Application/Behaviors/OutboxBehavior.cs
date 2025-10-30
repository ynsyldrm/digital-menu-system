using MediatR;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Common;
using System.Text.Json;

namespace OrderService.Application.Behaviors;

public class OutboxBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IOutboxMessageRepository _outboxRepository;

    public OutboxBehavior(IOutboxMessageRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        // Check if the request is a command that should publish domain events
        if (request is INotification notification)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = notification.GetType().FullName!,
                Content = JsonSerializer.Serialize(notification),
                OccurredOnUtc = DateTime.UtcNow
            };

            await _outboxRepository.AddAsync(outboxMessage);
        }

        return response;
    }
}
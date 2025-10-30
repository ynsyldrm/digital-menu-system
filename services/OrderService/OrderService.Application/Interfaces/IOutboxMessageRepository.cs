using OrderService.Infrastructure.Outbox;

namespace OrderService.Application.Interfaces;

public interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage message);
    Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 50);
    Task MarkAsProcessedAsync(Guid messageId);
}
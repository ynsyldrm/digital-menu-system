using MenuService.Infrastructure.Outbox;

namespace MenuService.Application.Interfaces;

public interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage message);
    Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 50);
    Task MarkAsProcessedAsync(Guid messageId);
}
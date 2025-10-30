using MenuService.Domain.Common;

namespace MenuService.Domain.Interfaces;

public interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage message);
    Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 50);
    Task MarkAsProcessedAsync(Guid messageId);
}
using OrderService.Domain.Interfaces;
using OrderService.Infrastructure.Data;
using OrderService.Domain.Common;
using OrderService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Infrastructure.Repositories;

public class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly OrderDbContext _context;

    public OutboxMessageRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Domain.Common.OutboxMessage message)
    {
        var infraMessage = new Outbox.OutboxMessage
        {
            Id = message.Id,
            Type = message.Type,
            Content = message.Content,
            OccurredOnUtc = message.OccurredOnUtc,
            ProcessedOnUtc = message.ProcessedOnUtc,
            Error = message.Error
        };
        await _context.OutboxMessages.AddAsync(infraMessage);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Domain.Common.OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 50)
    {
        var infraMessages = await _context.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(batchSize)
            .ToListAsync();

        return infraMessages.Select(m => new Domain.Common.OutboxMessage
        {
            Id = m.Id,
            Type = m.Type,
            Content = m.Content,
            OccurredOnUtc = m.OccurredOnUtc,
            ProcessedOnUtc = m.ProcessedOnUtc,
            Error = m.Error
        });
    }

    public async Task MarkAsProcessedAsync(Guid messageId)
    {
        var message = await _context.OutboxMessages.FindAsync(messageId);
        if (message != null)
        {
            message.ProcessedOnUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
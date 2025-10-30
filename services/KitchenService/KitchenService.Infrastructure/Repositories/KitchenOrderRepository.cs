using KitchenService.Domain.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Infrastructure.Repositories;

public class KitchenOrderRepository : IKitchenOrderRepository
{
    private readonly KitchenDbContext _context;

    public KitchenOrderRepository(KitchenDbContext context)
    {
        _context = context;
    }

    public async Task<KitchenOrder?> GetByIdAsync(Guid id)
    {
        return await _context.KitchenOrders
            .Include(ko => ko.OrderItems)
            .FirstOrDefaultAsync(ko => ko.Id == id);
    }

    public async Task<KitchenOrder?> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.KitchenOrders
            .Include(ko => ko.OrderItems)
            .FirstOrDefaultAsync(ko => ko.OrderId == orderId);
    }

    public async Task<IEnumerable<KitchenOrder>> GetAllAsync(KitchenOrderStatus? status = null)
    {
        var query = _context.KitchenOrders.Include(ko => ko.OrderItems).AsQueryable();

        if (status.HasValue)
            query = query.Where(ko => ko.Status == status.Value);

        return await query.OrderBy(ko => ko.ReceivedAt).ToListAsync();
    }

    public async Task AddAsync(KitchenOrder kitchenOrder)
    {
        await _context.KitchenOrders.AddAsync(kitchenOrder);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(KitchenOrder kitchenOrder)
    {
        _context.KitchenOrders.Update(kitchenOrder);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var kitchenOrder = await GetByIdAsync(id);
        if (kitchenOrder != null)
        {
            _context.KitchenOrders.Remove(kitchenOrder);
            await _context.SaveChangesAsync();
        }
    }
}
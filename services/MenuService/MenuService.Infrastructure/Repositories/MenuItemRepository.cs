using MenuService.Domain.Interfaces;
using MenuService.Domain.Entities;
using MenuService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly MenuDbContext _context;

    public MenuItemRepository(MenuDbContext context)
    {
        _context = context;
    }

    public async Task<MenuItem?> GetByIdAsync(Guid id)
    {
        return await _context.MenuItems
            .Include(m => m.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync(Guid? categoryId = null, bool? isAvailable = null)
    {
        var query = _context.MenuItems.Include(m => m.Category).AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(m => m.CategoryId == categoryId.Value);

        if (isAvailable.HasValue)
            query = query.Where(m => m.IsAvailable == isAvailable.Value);

        return await query.ToListAsync();
    }

    public async Task AddAsync(MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        _context.MenuItems.Update(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var menuItem = await GetByIdAsync(id);
        if (menuItem != null)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}
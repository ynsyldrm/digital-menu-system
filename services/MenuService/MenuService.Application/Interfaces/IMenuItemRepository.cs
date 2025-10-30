using MenuService.Domain.Entities;

namespace MenuService.Application.Interfaces;

public interface IMenuItemRepository
{
    Task<MenuItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<MenuItem>> GetAllAsync(Guid? categoryId = null, bool? isAvailable = null);
    Task AddAsync(MenuItem menuItem);
    Task UpdateAsync(MenuItem menuItem);
    Task DeleteAsync(Guid id);
}
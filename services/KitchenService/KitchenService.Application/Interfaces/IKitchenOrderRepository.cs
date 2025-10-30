using KitchenService.Domain.Entities;

namespace KitchenService.Application.Interfaces;

public interface IKitchenOrderRepository
{
    Task<KitchenOrder?> GetByIdAsync(Guid id);
    Task<KitchenOrder?> GetByOrderIdAsync(Guid orderId);
    Task<IEnumerable<KitchenOrder>> GetAllAsync(KitchenOrderStatus? status = null);
    Task AddAsync(KitchenOrder kitchenOrder);
    Task UpdateAsync(KitchenOrder kitchenOrder);
    Task DeleteAsync(Guid id);
}
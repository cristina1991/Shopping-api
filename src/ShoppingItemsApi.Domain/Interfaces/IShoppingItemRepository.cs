using ShoppingItemsApi.Domain.Entities;

namespace ShoppingItemsApi.Domain.Interfaces;

public interface IShoppingItemRepository
{
    Task<IEnumerable<ShoppingItem>> GetAllAsync();
    Task<ShoppingItem?> GetByIdAsync(int id);
    Task<ShoppingItem> AddAsync(ShoppingItem item);
    Task<ShoppingItem> UpdateAsync(ShoppingItem item);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ShoppingItem>> GetByCategoryAsync(string category);
    Task<bool> ExistsAsync(int id);
}
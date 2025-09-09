using Microsoft.EntityFrameworkCore;
using ShoppingItemsApi.Domain.Entities;
using ShoppingItemsApi.Domain.Interfaces;
using ShoppingItemsApi.Infrastructure.Data;

namespace ShoppingItemsApi.Infrastructure.Repositories;

public class ShoppingItemRepository : IShoppingItemRepository
{
    private readonly ShoppingItemsDbContext _context;

    public ShoppingItemRepository(ShoppingItemsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShoppingItem>> GetAllAsync()
    {
        return await _context.ShoppingItems
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<ShoppingItem?> GetByIdAsync(int id)
    {
        return await _context.ShoppingItems
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
    }

    public async Task<ShoppingItem> AddAsync(ShoppingItem item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        
        _context.ShoppingItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<ShoppingItem> UpdateAsync(ShoppingItem item)
    {
        item.UpdatedAt = DateTime.UtcNow;
        
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await GetByIdAsync(id);
        if (item == null)
            return false;

        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ShoppingItem>> GetByCategoryAsync(string category)
    {
        return await _context.ShoppingItems
            .Where(x => x.Category == category && x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.ShoppingItems
            .AnyAsync(x => x.Id == id && x.IsActive);
    }
}
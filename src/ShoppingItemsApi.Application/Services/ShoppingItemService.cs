using ShoppingItemsApi.Application.DTOs;
using ShoppingItemsApi.Application.Interfaces;
using ShoppingItemsApi.Domain.Entities;
using ShoppingItemsApi.Domain.Interfaces;

namespace ShoppingItemsApi.Application.Services;

public class ShoppingItemService : IShoppingItemService
{
    private readonly IShoppingItemRepository _repository;

    public ShoppingItemService(IShoppingItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ShoppingItemDto>> GetAllItemsAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<ShoppingItemDto?> GetItemByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<ShoppingItemDto> CreateItemAsync(CreateShoppingItemDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.Name))
            throw new ArgumentException("Item name is required", nameof(createDto.Name));

        if (createDto.Price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(createDto.Price));

        if (createDto.Quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(createDto.Quantity));

        var item = new ShoppingItem
        {
            Name = createDto.Name.Trim(),
            Description = createDto.Description?.Trim() ?? string.Empty,
            Price = createDto.Price,
            Quantity = createDto.Quantity,
            Category = createDto.Category.Trim(),
            IsActive = true
        };

        var createdItem = await _repository.AddAsync(item);
        return MapToDto(createdItem);
    }

    public async Task<ShoppingItemDto?> UpdateItemAsync(int id, UpdateShoppingItemDto updateDto)
    {
        var existingItem = await _repository.GetByIdAsync(id);
        if (existingItem == null)
            return null;

        if (string.IsNullOrWhiteSpace(updateDto.Name))
            throw new ArgumentException("Item name is required", nameof(updateDto.Name));

        if (updateDto.Price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(updateDto.Price));

        if (updateDto.Quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(updateDto.Quantity));

        existingItem.Name = updateDto.Name.Trim();
        existingItem.Description = updateDto.Description?.Trim() ?? string.Empty;
        existingItem.Price = updateDto.Price;
        existingItem.Quantity = updateDto.Quantity;
        existingItem.Category = updateDto.Category.Trim();

        var updatedItem = await _repository.UpdateAsync(existingItem);
        return MapToDto(updatedItem);
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ShoppingItemDto>> GetItemsByCategoryAsync(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return [];

        var items = await _repository.GetByCategoryAsync(category.Trim());
        return items.Select(MapToDto);
    }

    private static ShoppingItemDto MapToDto(ShoppingItem item)
    {
        return new ShoppingItemDto(
            item.Id,
            item.Name,
            item.Description,
            item.Price,
            item.Quantity,
            item.Category,
            item.CreatedAt,
            item.UpdatedAt
        );
    }
}
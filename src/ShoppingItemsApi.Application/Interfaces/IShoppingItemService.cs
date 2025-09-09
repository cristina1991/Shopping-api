using ShoppingItemsApi.Application.DTOs;

namespace ShoppingItemsApi.Application.Interfaces;

public interface IShoppingItemService
{
    Task<IEnumerable<ShoppingItemDto>> GetAllItemsAsync();
    Task<ShoppingItemDto?> GetItemByIdAsync(int id);
    Task<ShoppingItemDto> CreateItemAsync(CreateShoppingItemDto createDto);
    Task<ShoppingItemDto?> UpdateItemAsync(int id, UpdateShoppingItemDto updateDto);
    Task<bool> DeleteItemAsync(int id);
    Task<IEnumerable<ShoppingItemDto>> GetItemsByCategoryAsync(string category);
}
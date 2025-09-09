using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ShoppingItemsApi.Application.DTOs;

[SwaggerSchema("Shopping item response model")]
public record ShoppingItemDto(
    [property: SwaggerSchema("Unique identifier for the shopping item")] 
    int Id,
    
    [property: SwaggerSchema("Name of the shopping item")] 
    string Name,
    
    [property: SwaggerSchema("Description of the shopping item")] 
    string Description,
    
    [property: SwaggerSchema("Price of the shopping item", Format = "decimal")] 
    decimal Price,
    
    [property: SwaggerSchema("Available quantity")] 
    int Quantity,
    
    [property: SwaggerSchema("Category of the shopping item")] 
    string Category,
    
    [property: SwaggerSchema("Creation timestamp")] 
    DateTime CreatedAt,
    
    [property: SwaggerSchema("Last update timestamp")] 
    DateTime UpdatedAt
);

[SwaggerSchema("Model for creating a new shopping item")]
public record CreateShoppingItemDto(
    [property: Required, SwaggerSchema("Name of the shopping item (required, max 100 characters)")] 
    string Name,
    
    [property: SwaggerSchema("Description of the shopping item (max 500 characters)")] 
    string Description,
    
    [property: Required, SwaggerSchema("Price of the shopping item (required, must be >= 0)", Format = "decimal")] 
    decimal Price,
    
    [property: Required, SwaggerSchema("Available quantity (required, must be >= 0)")] 
    int Quantity,
    
    [property: Required, SwaggerSchema("Category of the shopping item (required, max 50 characters)")] 
    string Category
);

[SwaggerSchema("Model for updating an existing shopping item")]
public record UpdateShoppingItemDto(
    [property: Required, SwaggerSchema("Name of the shopping item (required, max 100 characters)")] 
    string Name,
    
    [property: SwaggerSchema("Description of the shopping item (max 500 characters)")] 
    string Description,
    
    [property: Required, SwaggerSchema("Price of the shopping item (required, must be >= 0)", Format = "decimal")] 
    decimal Price,
    
    [property: Required, SwaggerSchema("Available quantity (required, must be >= 0)")] 
    int Quantity,
    
    [property: Required, SwaggerSchema("Category of the shopping item (required, max 50 characters)")] 
    string Category
);
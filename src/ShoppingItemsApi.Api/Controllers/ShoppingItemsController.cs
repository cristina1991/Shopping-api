using Microsoft.AspNetCore.Mvc;
using ShoppingItemsApi.Application.DTOs;
using ShoppingItemsApi.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ShoppingItemsApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Shopping Items API - Manage your shopping items")]
public class ShoppingItemsController : ControllerBase
{
    private readonly IShoppingItemService _shoppingItemService;
    private readonly ILogger<ShoppingItemsController> _logger;

    public ShoppingItemsController(IShoppingItemService shoppingItemService, ILogger<ShoppingItemsController> logger)
    {
        _shoppingItemService = shoppingItemService;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all shopping items",
        Description = "Retrieves all active shopping items from the database"
    )]
    [SwaggerResponse(200, "Returns all shopping items", typeof(IEnumerable<ShoppingItemDto>))]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<ShoppingItemDto>>> GetAllItems()
    {
        try
        {
            var items = await _shoppingItemService.GetAllItemsAsync();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all shopping items");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get shopping item by ID",
        Description = "Retrieves a specific shopping item by its unique identifier"
    )]
    [SwaggerResponse(200, "Returns the shopping item", typeof(ShoppingItemDto))]
    [SwaggerResponse(400, "Invalid item ID")]
    [SwaggerResponse(404, "Shopping item not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<ShoppingItemDto>> GetItemById(
        [SwaggerParameter("The unique identifier of the shopping item")] int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Invalid item ID");

            var item = await _shoppingItemService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound($"Shopping item with ID {id} not found");

            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting shopping item with ID {ItemId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new shopping item",
        Description = "Creates a new shopping item with the provided details"
    )]
    [SwaggerResponse(201, "Shopping item created successfully", typeof(ShoppingItemDto))]
    [SwaggerResponse(400, "Invalid input data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<ShoppingItemDto>> CreateItem(
        [FromBody, SwaggerRequestBody("Shopping item data")] CreateShoppingItemDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _shoppingItemService.CreateItemAsync(createDto);
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument provided while creating shopping item");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating shopping item");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Update a shopping item",
        Description = "Updates an existing shopping item with the provided details"
    )]
    [SwaggerResponse(200, "Shopping item updated successfully", typeof(ShoppingItemDto))]
    [SwaggerResponse(400, "Invalid input data or item ID")]
    [SwaggerResponse(404, "Shopping item not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<ShoppingItemDto>> UpdateItem(
        [SwaggerParameter("The unique identifier of the shopping item")] int id,
        [FromBody, SwaggerRequestBody("Updated shopping item data")] UpdateShoppingItemDto updateDto)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Invalid item ID");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _shoppingItemService.UpdateItemAsync(id, updateDto);
            if (item == null)
                return NotFound($"Shopping item with ID {id} not found");

            return Ok(item);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument provided while updating shopping item with ID {ItemId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating shopping item with ID {ItemId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete a shopping item",
        Description = "Soft deletes a shopping item by setting it as inactive"
    )]
    [SwaggerResponse(204, "Shopping item deleted successfully")]
    [SwaggerResponse(400, "Invalid item ID")]
    [SwaggerResponse(404, "Shopping item not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult> DeleteItem(
        [SwaggerParameter("The unique identifier of the shopping item to delete")] int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Invalid item ID");

            var result = await _shoppingItemService.DeleteItemAsync(id);
            if (!result)
                return NotFound($"Shopping item with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting shopping item with ID {ItemId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("category/{category}")]
    [SwaggerOperation(
        Summary = "Get shopping items by category",
        Description = "Retrieves all active shopping items belonging to a specific category"
    )]
    [SwaggerResponse(200, "Returns shopping items in the specified category", typeof(IEnumerable<ShoppingItemDto>))]
    [SwaggerResponse(400, "Invalid category")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<ShoppingItemDto>>> GetItemsByCategory(
        [SwaggerParameter("The category name to filter by")] string category)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest("Category cannot be empty");

            var items = await _shoppingItemService.GetItemsByCategoryAsync(category);
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting shopping items by category {Category}", category);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}
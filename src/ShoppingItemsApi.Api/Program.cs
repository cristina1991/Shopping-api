using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShoppingItemsApi.Application.Interfaces;
using ShoppingItemsApi.Application.Services;
using ShoppingItemsApi.Domain.Interfaces;
using ShoppingItemsApi.Infrastructure.Data;
using ShoppingItemsApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shopping Items API",
        Version = "v1",
        Description = "A simple API to manage shopping items with SOLID principles and repository pattern",
        Contact = new OpenApiContact
        {
            Name = "Shopping Items API",
            Email = "info@shoppingitems.com"
        }
    });

    c.EnableAnnotations();
});

// Add Entity Framework
builder.Services.AddDbContext<ShoppingItemsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IShoppingItemRepository, ShoppingItemRepository>();

// Register services
builder.Services.AddScoped<IShoppingItemService, ShoppingItemService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Items API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();

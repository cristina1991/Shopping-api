-- Create the ShoppingItems database
CREATE DATABASE ShoppingItemsDb;
GO

USE ShoppingItemsDb;
GO

-- Create the ShoppingItems table
CREATE TABLE ShoppingItems (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    Description nvarchar(500) NULL,
    Price decimal(18,2) NOT NULL,
    Quantity int NOT NULL,
    Category nvarchar(50) NOT NULL,
    CreatedAt datetime2(7) NOT NULL,
    UpdatedAt datetime2(7) NOT NULL,
    IsActive bit NOT NULL DEFAULT 1
);
GO

-- Create indexes for better performance
CREATE INDEX IX_ShoppingItems_Category ON ShoppingItems(Category);
CREATE INDEX IX_ShoppingItems_IsActive ON ShoppingItems(IsActive);
CREATE INDEX IX_ShoppingItems_Name ON ShoppingItems(Name);
GO

-- Insert sample data
INSERT INTO ShoppingItems (Name, Description, Price, Quantity, Category, CreatedAt, UpdatedAt, IsActive)
VALUES 
    ('Apple iPhone 14', 'Latest iPhone model with advanced features', 999.99, 10, 'Electronics', GETUTCDATE(), GETUTCDATE(), 1),
    ('Samsung Galaxy S23', 'High-performance Android smartphone', 899.99, 15, 'Electronics', GETUTCDATE(), GETUTCDATE(), 1),
    ('Nike Air Max 90', 'Classic running shoes for everyday wear', 129.99, 25, 'Footwear', GETUTCDATE(), GETUTCDATE(), 1),
    ('Adidas Ultraboost 22', 'Premium running shoes with boost technology', 189.99, 20, 'Footwear', GETUTCDATE(), GETUTCDATE(), 1),
    ('The Great Gatsby', 'Classic American novel by F. Scott Fitzgerald', 12.99, 50, 'Books', GETUTCDATE(), GETUTCDATE(), 1),
    ('To Kill a Mockingbird', 'Pulitzer Prize-winning novel by Harper Lee', 14.99, 30, 'Books', GETUTCDATE(), GETUTCDATE(), 1),
    ('MacBook Pro 16"', 'Professional laptop for creative work', 2399.99, 5, 'Electronics', GETUTCDATE(), GETUTCDATE(), 1),
    ('Organic Bananas', 'Fresh organic bananas per lb', 2.99, 100, 'Groceries', GETUTCDATE(), GETUTCDATE(), 1),
    ('Greek Yogurt', 'Natural Greek yogurt with probiotics', 5.99, 40, 'Groceries', GETUTCDATE(), GETUTCDATE(), 1),
    ('Wireless Headphones', 'Bluetooth headphones with noise cancellation', 199.99, 18, 'Electronics', GETUTCDATE(), GETUTCDATE(), 1);
GO

-- Create a stored procedure to get items by category
CREATE PROCEDURE GetShoppingItemsByCategory
    @Category nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT Id, Name, Description, Price, Quantity, Category, CreatedAt, UpdatedAt
    FROM ShoppingItems
    WHERE Category = @Category AND IsActive = 1
    ORDER BY Name;
END
GO

-- Create a stored procedure to get active items count by category
CREATE PROCEDURE GetActiveItemsCountByCategory
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT Category, COUNT(*) as ItemCount
    FROM ShoppingItems
    WHERE IsActive = 1
    GROUP BY Category
    ORDER BY Category;
END
GO

-- Display sample data
SELECT 'Sample data inserted successfully' as Message;
SELECT COUNT(*) as TotalItems FROM ShoppingItems WHERE IsActive = 1;
SELECT Category, COUNT(*) as ItemCount FROM ShoppingItems WHERE IsActive = 1 GROUP BY Category ORDER BY Category;
GO
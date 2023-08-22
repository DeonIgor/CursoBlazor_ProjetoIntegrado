SELECT TOP (1000) * FROM [ProductsProject].[dbo].[Products]

SELECT TOP (1000) * FROM [ProductsProject].[dbo].[Categories]

SELECT TOP (1000) p.Name, p.Description, p.Price, c.Name
FROM [ProductsProject].[dbo].[Products] p
JOIN [ProductsProject].[dbo].[Categories] c ON p.FK_CategoryId = c.CategoryId;
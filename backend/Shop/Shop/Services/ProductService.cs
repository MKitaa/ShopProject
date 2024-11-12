using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.DTOs.product;
using Shop.Interfaces;
using Shop.Models;

namespace Shop.Services;

public class ProductService(ShopDbContext shopDbContext, IMapper mapper) : IProductService
{
    private IMapper Mapper { get; } = mapper;

    public async Task<Product> GetProductById(int id)
    {
        return await shopDbContext.Products.FindAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<Product> CreateProduct(CreateProductDto createProductDto)
    {
        var product = Mapper.Map<Product>(createProductDto);
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        shopDbContext.Products.Add(product);
        await shopDbContext.SaveChangesAsync();

        return product;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await shopDbContext.Products.ToListAsync();
    }

    public async Task<Product> UpdateProduct(int id, UpdateProductDto updateProductDto)
    {
        var product = await shopDbContext.Products.FindAsync(id);
        if (product is null) return null;

        Mapper.Map(updateProductDto, product);
        await shopDbContext.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await shopDbContext.Products.FindAsync(id);
        if (product == null) return false;

        shopDbContext.Products.Remove(product);
        await shopDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Product>> GetProductsByPriceRangeUsingSubquery(decimal minPrice, decimal maxPrice)
    {
        var products = await shopDbContext.Products
            .FromSqlRaw(@"
            SELECT *
            FROM public.""Products"" p
            WHERE p.""Price"" BETWEEN
                (SELECT MIN(""Price"") FROM public.""Products"" WHERE ""Price"" >= @p0) AND
                (SELECT MAX(""Price"") FROM public.""Products"" WHERE ""Price"" <= @p1);
        ", minPrice, maxPrice)
            .ToListAsync();

        return products;
    }
}

using Shop.DTOs.product;
using Shop.Models;

namespace Shop.Interfaces;

public interface IProductService
{
    Task<Product> GetProductById(int id);
    Task<Product> CreateProduct(CreateProductDto createProductDto);
    Task<List<Product>> GetAllProducts();
    Task<Product> UpdateProduct(int id, UpdateProductDto updateProductDto);
    Task<bool> DeleteProduct(int id);
    Task<List<Product>> GetProductsByPriceRangeUsingSubquery(decimal minPrice, decimal maxPrice);
}

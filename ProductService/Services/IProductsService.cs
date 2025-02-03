using ProductService.Data.DTOs;
using ProductService.Entities;

namespace ProductService.Services
{
    public interface IProductsService
    {
        Task<Product> CreateProductAsync(CreateProductDto dto, string userId);
        Task DeleteProductAsync(int id, string userId);
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsAsync(string search, decimal? minPrice, decimal? maxPrice);
        Task UpdateProductAsync(int id, UpdateProductDto dto, string userId);
    }
}

using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.DTOs;
using ProductService.Entities;

namespace ProductService.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ProductDbContext _context;

        public ProductsService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync(string search, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return await query.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");
            return product;
        }

        public async Task<Product> CreateProductAsync(CreateProductDto dto, string userId)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                CreatorUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto dto, string userId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Produkt nie został znaleziony.");

            if (product.CreatorUserId != userId)
                throw new UnauthorizedAccessException("Brak uprawnień do edycji tego produktu.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id, string userId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Produkt nie został znaleziony.");

            if (product.CreatorUserId != userId)
                throw new UnauthorizedAccessException("Brak uprawnień do usunięcia tego produktu.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.Data;
using System.IO;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
        {
            var currentTime = DateTime.Now;
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.StartDate <= currentTime && currentTime <= p.StartDate.AddDays(p.Duration.TotalDays))
                .ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                StartDate = p.StartDate,
                Duration = p.Duration,
                Price = p.Price,
                ImagePath = p.ImagePath,
                CategoryName = p.Category.Name
            });
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                StartDate = p.StartDate,
                Duration = p.Duration,
                Price = p.Price,
                ImagePath = p.ImagePath,
                CategoryName = p.Category.Name
            });
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                StartDate = product.StartDate,
                Duration = product.Duration,
                Price = product.Price,
                ImagePath = product.ImagePath,
                CategoryName = product.Category.Name
            };
        }

        public async Task<bool> AddProductAsync(ProductDto productDto, IFormFile image)
        {
            if (image.Length > 1 * 1024 * 1024 || !(new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(image.FileName).ToLower())))
                return false;

            var imagePath = Path.Combine("wwwroot/images", Guid.NewGuid() + Path.GetExtension(image.FileName));
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var product = new Product
            {
                Name = productDto.Name,
                StartDate = productDto.StartDate,
                Duration = productDto.Duration,
                Price = productDto.Price,
                ImagePath = imagePath,
                CategoryId = productDto.Id,
                CreationDate = DateTime.Now
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProductAsync(ProductDto productDto, IFormFile image)
        {
            var product = await _context.Products.FindAsync(productDto.Id);
            if (product == null) return false;

            if (image != null)
            {
                if (image.Length > 1 * 1024 * 1024 || !(new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(image.FileName).ToLower())))
                    return false;

                var imagePath = Path.Combine("wwwroot/images", Guid.NewGuid() + Path.GetExtension(image.FileName));
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                product.ImagePath = imagePath;
            }

            product.Name = productDto.Name;
            product.StartDate = productDto.StartDate;
            product.Duration = productDto.Duration;
            product.Price = productDto.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

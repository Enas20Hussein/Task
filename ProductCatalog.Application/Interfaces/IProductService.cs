using Microsoft.AspNetCore.Http;
using ProductCatalog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(ProductDto productDto, IFormFile image);
        Task<bool> UpdateProductAsync(ProductDto productDto, IFormFile image);
        Task<bool> DeleteProductAsync(int id);
    }

}

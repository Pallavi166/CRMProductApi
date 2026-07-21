using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    
        public interface IProductService
        {
            Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IEnumerable<ProductDto>> GetAllAsync(PaginationParams paginationParams);
        Task<ProductDto?> GetByIdAsync(int id);
            Task<ProductDto> CreateAsync(CreateProductDto dto);
            Task<bool> UpdateAsync(UpdateProductDto dto);
            Task<bool> DeleteAsync(int id);
        }
    }


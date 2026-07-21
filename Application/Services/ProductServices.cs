using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    
        public class ProductService : IProductService
        {
            private readonly IProductRepository _repository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public ProductService(
                IProductRepository repository,
                IUnitOfWork unitOfWork,
                IMapper mapper)
            {
                _repository = repository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ProductDto>> GetAllAsync()
            {
                var products = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }

            public async Task<ProductDto?> GetByIdAsync(int id)
            {
                var product = await _repository.GetByIdAsync(id);

                if (product == null)
                    return null;

                return _mapper.Map<ProductDto>(product);
            }

            public async Task<ProductDto> CreateAsync(CreateProductDto dto)
            {
                var product = _mapper.Map<Product>(dto);

                product.CreatedOn = DateTime.UtcNow;

                await _repository.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<ProductDto>(product);
            }

            public async Task<bool> UpdateAsync(UpdateProductDto dto)
            {
                var product = await _repository.GetByIdAsync(dto.Id);

                if (product == null)
                    return false;

                product.ProductName = dto.ProductName;
                product.ModifiedBy = dto.ModifiedBy;
                product.ModifiedOn = DateTime.UtcNow;

                _repository.Update(product);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var product = await _repository.GetByIdAsync(id);

                if (product == null)
                    return false;

                _repository.Delete(product);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(PaginationParams paginationParams)
        {
            var products = await _repository.GetAllAsync(paginationParams);

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
    }



using Xunit;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _service;



        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _service = new ProductService(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }


        [Fact]
        public async Task CreateProduct_ShouldCreateProductSuccessfully()
        {
            
            var createDto = new CreateProductDto
            {
                ProductName = "Laptop",
                CreatedBy = "TestUser"
            };

            var product = new Product
            {
                ProductName = createDto.ProductName,
                CreatedBy = createDto.CreatedBy
            };

            var productDto = new ProductDto
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "TestUser"
            };

            _mapperMock.Setup(m => m.Map<Product>(createDto))
                       .Returns(product);

            _mapperMock.Setup(m => m.Map<ProductDto>(product))
                       .Returns(productDto);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                           .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                           .ReturnsAsync(1);

            
            var result = await _service.CreateAsync(createDto);

            
            Assert.NotNull(result);
            Assert.Equal("Laptop", result.ProductName);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            
            var products = new List<Product>
    {
        new Product
        {
            Id = 1,
            ProductName = "Laptop",
            CreatedBy = "Admin"
        },
        new Product
        {
            Id = 2,
            ProductName = "Mobile",
            CreatedBy = "Admin"
        }
    };

            var productDtos = new List<ProductDto>
    {
        new ProductDto
        {
            Id = 1,
            ProductName = "Laptop",
            CreatedBy = "Admin"
        },
        new ProductDto
        {
            Id = 2,
            ProductName = "Mobile",
            CreatedBy = "Admin"
        }
    };

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<ProductDto>>(products))
                .Returns(productDtos);

            
            var result = await _service.GetAllAsync();

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Laptop", result.First().ProductName);

            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            
            int productId = 1;

            var product = new Product
            {
                Id = productId,
                ProductName = "Laptop",
                CreatedBy = "Admin"
            };

            var productDto = new ProductDto
            {
                Id = productId,
                ProductName = "Laptop",
                CreatedBy = "Admin"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _mapperMock
                .Setup(m => m.Map<ProductDto>(product))
                .Returns(productDto);

          
            var result = await _service.GetByIdAsync(productId);

           
            Assert.NotNull(result);
            Assert.Equal(productId, result!.Id);
            Assert.Equal("Laptop", result.ProductName);

            _repositoryMock.Verify(r => r.GetByIdAsync(productId), Times.Once);
        }
        [Fact]
        public async Task UpdateProduct_ShouldUpdateProductSuccessfully()
        {
           
            var updateDto = new UpdateProductDto
            {
                Id = 1,
                ProductName = "Updated Laptop",
                ModifiedBy = "Admin"
            };

            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "Admin"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(updateDto.Id))
                .ReturnsAsync(product);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            
            var result = await _service.UpdateAsync(updateDto);

            
            Assert.True(result);
            Assert.Equal("Updated Laptop", product.ProductName);
            Assert.Equal("Admin", product.ModifiedBy);

            _repositoryMock.Verify(r => r.Update(product), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteProduct_ShouldDeleteProductSuccessfully()
        {
            
            int productId = 1;

            var product = new Product
            {
                Id = productId,
                ProductName = "Laptop",
                CreatedBy = "Admin"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

          
            var result = await _service.DeleteAsync(productId);

           
            Assert.True(result);

            _repositoryMock.Verify(r => r.Delete(product), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

    }
}

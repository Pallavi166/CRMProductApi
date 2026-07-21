using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
           
            using var context = GetDbContext();

            var repository = new ProductRepository(context);

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "TestUser"
            };

            
            await repository.AddAsync(product);
            await context.SaveChangesAsync();

            
            Assert.Single(context.Products);
            Assert.Equal("Laptop", context.Products.First().ProductName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            
            using var context = GetDbContext();

            var product = new Product
            {
                ProductName = "Mobile",
                CreatedBy = "TestUser"
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);

           
            var result = await repository.GetByIdAsync(product.Id);

            
            Assert.NotNull(result);
            Assert.Equal("Mobile", result!.ProductName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProducts()
        {
            
            using var context = GetDbContext();

            context.Products.AddRange(
                new Product
                {
                    ProductName = "Laptop",
                    CreatedBy = "TestUser"
                },
                new Product
                {
                    ProductName = "Mobile",
                    CreatedBy = "TestUser"
                });

            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);

            
            var result = await repository.GetAllAsync();

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Update_ShouldUpdateProduct()
        {
            
            using var context = GetDbContext();

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "TestUser"
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);

          
            product.ProductName = "Updated Laptop";

            repository.Update(product);
            await context.SaveChangesAsync();

            var updatedProduct = await context.Products.FindAsync(product.Id);

            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Laptop", updatedProduct!.ProductName);
        }

        [Fact]
        public async Task Delete_ShouldDeleteProduct()
        {
            
            using var context = GetDbContext();

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "TestUser"
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);

        
            repository.Delete(product);
            await context.SaveChangesAsync();

           
            Assert.Empty(context.Products);
        }
    }
}
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    
        public class ProductRepository : IProductRepository
        {
            private readonly ApplicationDbContext _context;

            public ProductRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Product>> GetAllAsync()
            {
                return await _context.Products
                                     .Include(p => p.Items)
                                     .ToListAsync();
            }

            public async Task<Product?> GetByIdAsync(int id)
            {
                return await _context.Products
                                     .Include(p => p.Items)
                                     .FirstOrDefaultAsync(p => p.Id == id);
            }

            public async Task AddAsync(Product product)
            {
                await _context.Products.AddAsync(product);
            }
        public async Task<IEnumerable<Product>> GetAllAsync(PaginationParams paginationParams)
        {
            return await _context.Products
                .AsNoTracking()
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();
        }
        public void Update(Product product)
            {
                _context.Products.Update(product);
            }

            public void Delete(Product product)
            {
                _context.Products.Remove(product);
            }
        }
    }


using AutoMapper;
using Business.Abstracts;
using Dapper;
using DataAccess;
using Entities.Concretes;
using Business.DTOs.Product;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Business.Concretes
{
    public class ProductManager : IProductService
    {
        private readonly DapperContext _context;
        private readonly IMapper _mapper;

        public ProductManager(DapperContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GetAllAsync metodunda DTO kullanımı
        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT id, name, price, stock, category FROM products"; // id'yi kullandık
            var products = await connection.QueryAsync<Product>(sql);

            // Entity'yi DTO'ya dönüştür
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return productDTOs;
        }

        // GetByIdAsync metodunda DTO kullanımı
        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT id, name, price, stock, category FROM products WHERE id = @Id"; // id'yi kullandık
            var product = await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });

            if (product == null) return null;

            // Entity'yi DTO'ya dönüştür
            var productDTO = _mapper.Map<ProductDTO>(product);

            return productDTO;
        }

        // AddAsync metodunda DTO'yu Entity'ye dönüştürme
        public async Task<int> AddAsync(ProductDTO productDTO)
        {
            using var connection = _context.CreateConnection();

            // DTO'yu Entity'ye dönüştür
            var product = _mapper.Map<Product>(productDTO);
            product.CreatedDate = DateTime.UtcNow;
            product.UpdatedDate = DateTime.UtcNow;

            var sql = "INSERT INTO products (name, price, stock, category) VALUES (@Name, @Price, @Stock, @Category) RETURNING id"; // id'yi kullandık
            return await connection.ExecuteScalarAsync<int>(sql, product);
        }

        // UpdateAsync metodunda DTO'yu Entity'ye dönüştürme
        public async Task<int> UpdateAsync(ProductDTO productDTO)
        {
            using var connection = _context.CreateConnection();

            // DTO'yu Entity'ye dönüştür
            var product = _mapper.Map<Product>(productDTO);
            product.UpdatedDate = DateTime.UtcNow;

            var sql = "UPDATE products SET name = @Name, price = @Price, stock = @Stock, category = @Category, updated_date = @UpdatedDate WHERE id = @Id"; // id'yi kullandık
            return await connection.ExecuteAsync(sql, product);
        }

        // DeleteAsync metodunda DTO kullanımı
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM products WHERE id = @Id"; // id'yi kullandık
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

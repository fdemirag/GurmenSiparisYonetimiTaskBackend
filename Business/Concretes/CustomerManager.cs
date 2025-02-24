using AutoMapper;
using Business.Abstracts;
using Dapper;
using DataAccess;
using Entities.Concretes;
using Business.DTOs.Customer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concretes
{
    public class CustomerManager : ICustomerService
    {
        private readonly DapperContext _context;
        private readonly IMapper _mapper;

        public CustomerManager(DapperContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GetAllAsync: Tüm müşterileri listeleme
        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM customers";
            var customers = await connection.QueryAsync<Customer>(sql);

            // Entity'yi DTO'ya dönüştür
            var customerDTOs = _mapper.Map<IEnumerable<CustomerDTO>>(customers);
            return customerDTOs;
        }

        // GetByIdAsync: ID ile müşteri sorgulama
        public async Task<CustomerDTO> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM customers WHERE id = @Id";
            var customer = await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id });

            if (customer == null) return null;

            // Entity'yi DTO'ya dönüştür
            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return customerDTO;
        }

        // AddAsync: Yeni müşteri ekleme
        public async Task<int> AddAsync(CustomerDTO customerDTO)
        {
            using var connection = _context.CreateConnection();

            // DTO'yu Entity'ye dönüştür
            var customer = _mapper.Map<Customer>(customerDTO);
            var sql = "INSERT INTO customers (name, email, phone, address) VALUES (@Name, @Email, @Phone, @Address) RETURNING id";
            return await connection.ExecuteScalarAsync<int>(sql, customer);
        }

        // UpdateAsync: Müşteri güncelleme
        public async Task<int> UpdateAsync(int id, CustomerDTO customerDTO)
        {
            using var connection = _context.CreateConnection();

            // DTO'yu Entity'ye dönüştür
            var customer = _mapper.Map<Customer>(customerDTO);
            customer.Id = id;  // Burada id'yi DTO'dan alarak set ediyoruz

            // UpdatedDate değerini şu anki UTC zamanı olarak ayarla
            customer.UpdatedDate = DateTime.UtcNow;

            // SQL sorgusunda UpdatedDate'i de güncelliyoruz
            var sql = "UPDATE customers SET name = @Name, email = @Email, phone = @Phone, address = @Address, updated_date = @UpdatedDate WHERE id = @Id";

            return await connection.ExecuteAsync(sql, customer);
        }

        // DeleteAsync: Müşteri silme
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM customers WHERE id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

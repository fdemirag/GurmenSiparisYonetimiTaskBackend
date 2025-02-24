using AutoMapper;
using Business.Abstracts;
using Business.DTOs.OrderDetail;
using Dapper;
using DataAccess;
using Entities.Concretes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concretes
{
    public class OrderDetailManager : IOrderDetailService
    {
        private readonly DapperContext _context;
        private readonly IMapper _mapper;

        public OrderDetailManager(DapperContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDetailDTO>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM orderdetails";
            var orderDetails = await connection.QueryAsync<OrderDetail>(sql);
            return _mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetails);
        }

        public async Task<OrderDetailDTO> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM orderdetails WHERE id = @Id";
            var orderDetail = await connection.QuerySingleOrDefaultAsync<OrderDetail>(sql, new { Id = id });
            return _mapper.Map<OrderDetailDTO>(orderDetail);
        }

        public async Task<int> AddAsync(OrderDetailDTO orderDetailDTO)
        {
            using var connection = _context.CreateConnection();
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
            var sql = "INSERT INTO orderdetails (orderid, productid, quantity, unitprice) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice) RETURNING id";
            return await connection.ExecuteScalarAsync<int>(sql, orderDetail);
        }

        public async Task<int> UpdateAsync(int id, OrderDetailDTO orderDetailDTO)
        {
            using var connection = _context.CreateConnection();
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
            orderDetail.Id = id;
            var sql = "UPDATE orderdetails SET orderid = @OrderId, productid = @ProductId, quantity = @Quantity, unitprice = @UnitPrice WHERE id = @Id";
            return await connection.ExecuteAsync(sql, orderDetail);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM orderdetails WHERE id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

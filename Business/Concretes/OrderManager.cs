using AutoMapper;
using Business.Abstracts;
using Business.BusinessRules;
using Business.DTOs.Order;
using Business.DTOs.OrderDetail;
using Dapper;
using DataAccess;
using Entities.Concretes;

namespace Business.Concretes
{
    public class OrderManager : IOrderService
    {
        private readonly DapperContext _context;
        private readonly IMapper _mapper;
        private readonly ICampaignService _campaignService;
        private readonly OrderBusinessRules _orderBusinessRules;

        public OrderManager(DapperContext context, IMapper mapper, ICampaignService campaignService, OrderBusinessRules orderBusinessRules)
        {
            _context = context;
            _mapper = mapper;
            _campaignService = campaignService;
            _orderBusinessRules = orderBusinessRules;
        }

        // Tüm siparişleri getir
        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var orderDictionary = new Dictionary<int, OrderDTO>();

            var orders = await connection.QueryAsync<OrderDTO, OrderDetailDTO, OrderDTO>(
                @"SELECT o.id, o.customerid, o.orderdate, o.totalamount, o.status, o.discountcode,
                         od.id, od.orderid, od.productid, od.quantity, od.unitprice
                  FROM orders o
                  LEFT JOIN orderdetails od ON o.id = od.orderid",
                (order, orderDetail) =>
                {
                    if (!orderDictionary.TryGetValue(order.Id, out var orderEntry))
                    {
                        orderEntry = order;
                        orderEntry.OrderDetails = new List<OrderDetailDTO>();
                        orderDictionary.Add(order.Id, orderEntry);
                    }

                    if (orderDetail != null)
                    {
                        orderEntry.OrderDetails.Add(orderDetail);
                    }

                    return orderEntry;
                },
                splitOn: "id"
            );

            return orderDictionary.Values;
        }

        // ID ile sipariş getir
        public async Task<OrderDTO?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var orderDictionary = new Dictionary<int, OrderDTO>();

            var orderWithDetails = await connection.QueryAsync<OrderDTO, OrderDetailDTO, OrderDTO>(
                @"SELECT o.id, o.customerid, o.orderdate, o.totalamount, o.status, o.discountcode,
                         od.id, od.orderid, od.productid, od.quantity, od.unitprice
                  FROM orders o
                  LEFT JOIN orderdetails od ON o.id = od.orderid
                  WHERE o.id = @Id",
                (order, orderDetail) =>
                {
                    if (!orderDictionary.TryGetValue(order.Id, out var orderEntry))
                    {
                        orderEntry = order;
                        orderEntry.OrderDetails = new List<OrderDetailDTO>();
                        orderDictionary.Add(order.Id, orderEntry);
                    }

                    if (orderDetail != null)
                    {
                        orderEntry.OrderDetails.Add(orderDetail);
                    }

                    return orderEntry;
                },
                new { Id = id },
                splitOn: "id"
            );

            return orderDictionary.Values.FirstOrDefault();
        }

        // Son bir hafta içinde en çok sipariş veren 5 müşteriyi getir
        public async Task<IEnumerable<OrderDTO>> GetTopCustomersLastWeekAsync()
        {
            using var connection = _context.CreateConnection();

            var query = @"
                SELECT o.customerid,
                COUNT(o.id) AS ordercount
                FROM orders o
                WHERE o.orderdate >= NOW() - INTERVAL '7 days'
                GROUP BY o.customerid
                ORDER BY ordercount DESC
                LIMIT 5";

            // Veriyi al ve DTO'ya dönüştür
            var result = await connection.QueryAsync<OrderDTO>(query);
            return result;
        }

        // Yeni sipariş ekle
        public async Task<int> AddAsync(OrderDTO orderDTO)
        {
            using var connection = _context.CreateConnection();

            // Müşteri için son 24 saat içinde kaç sipariş verdiğini kontrol et
            await _orderBusinessRules.ValidateOrderLimitAsync(orderDTO.CustomerId);

            // Yeni siparişi oluştur ve ID'sini al
            var order = _mapper.Map<Order>(orderDTO);
            order.OrderDate = DateTime.Now;

            var orderId = await connection.ExecuteScalarAsync<int>(
                "INSERT INTO orders (customerid, orderdate, totalamount, status) VALUES (@CustomerId, @OrderDate, @TotalAmount, @Status) RETURNING id",
                order);

            decimal totalAmount = 0;

            // Sipariş detaylarını işle
            foreach (var detail in orderDTO.OrderDetails)
            {
                var product = await connection.QuerySingleOrDefaultAsync<Product>(
                    "SELECT * FROM products WHERE id = @ProductId", new { ProductId = detail.ProductId });

                if (product != null)
                {
                    decimal unitPrice = product.Price;
                    totalAmount += unitPrice * detail.Quantity;

                    await connection.ExecuteAsync(
                        "INSERT INTO orderdetails (orderid, productid, quantity, unitprice) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)",
                        new { OrderId = orderId, ProductId = detail.ProductId, Quantity = detail.Quantity, UnitPrice = unitPrice });
                }
            }

            // Toplam tutarı güncelle
            await connection.ExecuteAsync(
                "UPDATE orders SET totalamount = @TotalAmount WHERE id = @OrderId",
                new { TotalAmount = totalAmount, OrderId = orderId });

            // Uygulanabilir kampanya kontrol et ve indirim uygula
            var applicableCampaign = await connection.QueryFirstOrDefaultAsync<Campaign>(
                "SELECT * FROM campaigns WHERE minimumAmount <= @TotalAmount AND expirationDate >= @CurrentDate ORDER BY minimumAmount DESC LIMIT 1",
                new { TotalAmount = totalAmount, CurrentDate = DateTime.Now });

            if (applicableCampaign != null)
            {
                decimal discountedAmount = totalAmount * (1 - applicableCampaign.DiscountRate);
                await connection.ExecuteAsync(
                    "UPDATE orders SET totalamount = @TotalAmount, discountcode = @DiscountCode WHERE id = @OrderId",
                    new { TotalAmount = discountedAmount, DiscountCode = applicableCampaign.Code, OrderId = orderId });
            }

            return orderId;
        }

        // Sipariş güncelle
        public async Task<int> UpdateAsync(int id, OrderDTO orderDTO)
        {
            using var connection = _context.CreateConnection();
            var order = _mapper.Map<Order>(orderDTO);
            order.Id = id;

            return await connection.ExecuteAsync(
                "UPDATE orders SET customerid = @CustomerId, orderdate = @OrderDate, totalamount = @TotalAmount, status = @Status WHERE id = @Id",
                order);
        }

        // Sipariş sil
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM orders WHERE id = @Id", new { Id = id });
        }
    }
}

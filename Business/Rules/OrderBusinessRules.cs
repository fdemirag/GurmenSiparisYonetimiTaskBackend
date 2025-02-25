using Core.Business.Rules;
using Dapper;
using DataAccess;
using System;
using System.Threading.Tasks;

namespace Business.BusinessRules
{
    public class OrderBusinessRules:BaseBusinessRules
    {
        private readonly DapperContext _context;

        public OrderBusinessRules(DapperContext context)
        {
            _context = context;
        }

        // Müşterinin son 24 saat içinde daha fazla sipariş verip vermediğini kontrol et
        public async Task ValidateOrderLimitAsync(int customerId)
        {
            using var connection = _context.CreateConnection();

            var orderCount = await connection.QuerySingleOrDefaultAsync<int>(
                @"SELECT COUNT(*) 
                  FROM orders 
                  WHERE customerid = @CustomerId 
                  AND orderdate >= @StartDate",
                new
                {
                    CustomerId = customerId,
                    StartDate = DateTime.Now.AddHours(-24) // Son 24 saat
                });

            // Eğer müşteri 3 siparişten fazla verdi ise, istisna fırlat
            if (orderCount >= 3)
            {
                throw new InvalidOperationException("Bir müşteri 24 saat içinde yalnızca 3 sipariş verebilir.");
            }
        }
    }
}

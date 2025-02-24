using Business.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<OrderDTO> GetByIdAsync(int id);
        Task<int> AddAsync(OrderDTO orderDTO);
        Task<int> UpdateAsync(int id, OrderDTO orderDTO);
        Task<int> DeleteAsync(int id);
    }
}

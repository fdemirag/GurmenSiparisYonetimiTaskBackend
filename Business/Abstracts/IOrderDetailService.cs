using Business.DTOs.OrderDetail;

namespace Business.Abstracts
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDTO>> GetAllAsync();
        Task<OrderDetailDTO> GetByIdAsync(int id);
        Task<int> AddAsync(OrderDetailDTO orderDetailDTO);
        Task<int> UpdateAsync(int id, OrderDetailDTO orderDetailDTO);
        Task<int> DeleteAsync(int id);
    }
}

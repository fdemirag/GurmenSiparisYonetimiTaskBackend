using Business.DTOs.Customer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstracts
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllAsync();
        Task<CustomerDTO> GetByIdAsync(int id);
        Task<int> AddAsync(CustomerDTO customerDTO);
        Task<int> UpdateAsync(int id, CustomerDTO customerDTO);
        Task<int> DeleteAsync(int id);
    }
}
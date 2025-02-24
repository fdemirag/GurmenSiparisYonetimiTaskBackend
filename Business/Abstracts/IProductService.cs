using Business.DTOs.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task<int> AddAsync(ProductDTO product);
        Task<int> UpdateAsync(ProductDTO product);
        Task<int> DeleteAsync(int id);
    }
}

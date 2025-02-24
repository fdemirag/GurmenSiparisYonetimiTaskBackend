using Business.DTOs.Campaign;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstracts
{
    public interface ICampaignService
    {
        Task<IEnumerable<CampaignDTO>> GetAllAsync();
        Task<int> AddAsync(CampaignDTO campaignDTO);
        Task<decimal> ApplyDiscountAsync(int productId, decimal unitPrice); // Kampanya fiyat hesaplama
        Task<int> ApplyDiscountToOrderAsync(int orderId, decimal totalAmount); // Siparişe indirim uygulama
    }
}

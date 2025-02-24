using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Campaign
{
    public class CampaignDTO
    {
        public string Code { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal MinimumAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}

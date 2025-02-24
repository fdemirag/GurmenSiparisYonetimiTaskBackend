using Core.Entity;

namespace Entities.Concretes
{
    public class Campaign : BaseEntity<int>
    {
        public string Code { get; set; } // Kampanya kodu
        public decimal DiscountRate { get; set; } // İndirim yüzdesi (örn: %10 = 0.10)
        public decimal MinimumAmount { get; set; } // Kampanyanın geçerli olması için minimum sipariş tutarı
        public DateTime ExpirationDate { get; set; } // Kampanyanın geçerlilik süresi
        public bool IsExpired { get; set; } = false;


        public int CustomerId; //null olursa global olur

        public Customer Customer { get; set; }
    }
}

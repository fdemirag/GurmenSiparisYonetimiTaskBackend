using Business.DTOs.OrderDetail;

namespace Business.DTOs.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? DiscountCode { get; set; }
        public string Status { get; set; }

        // OrderDetails listesi OrderDetailDTO olarak güncellendi
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }
}

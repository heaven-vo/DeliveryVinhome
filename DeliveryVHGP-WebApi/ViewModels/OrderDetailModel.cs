namespace DeliveryVHGP_WebApi.ViewModels
{
    public class OrderDetailModel
    {
        public string Id { get; set; } = null!;
        public string? Time { get; set; }
        public string? PaymentId { get; set; }
        public string? PaymentName { get; set; }

        public List<OrderDetailDto> ListOrderDetail { get; set; }

    }
}

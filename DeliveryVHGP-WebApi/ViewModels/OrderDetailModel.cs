namespace DeliveryVHGP_WebApi.ViewModels
{
    public class OrderDetailModel
    {
        public string Id { get; set; } = null!;
        public string? Time { get; set; }
        public double? Total { get; set; }
        public double? ShipCost { get; set; }
        public string? PaymentId { get; set; }
        public string? PaymentName { get; set; }
        public string? BuildingId { get; set; }
        public string? StoreId { get; set; }
        public string? Note { get; set; }


        public List<OrderDetailDto> ListProInMenu { get; set; }

    }
}

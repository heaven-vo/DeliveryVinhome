namespace DeliveryVHGP_WebApi.ViewModels
{
    public class OrderModels
    {
        public string Id { get; set; } = null!;
        public string? Type { get; set; }
        public double? Total { get; set; }
        public string? BuildingId { get; set; }
        public string? DurationId { get; set; }
        public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? Status { get; set; }
        public List<OrderDetailDto> listProInMenuOrder { get; set; }
    }
}

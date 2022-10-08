namespace DeliveryVHGP_WebApi.ViewModels
{
    public class OrderModels
    {
        public string Id { get; set; } = null!;
        public double? Total { get; set; }
        public string? BuildingId { get; set; }
        public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? BuildingName { get; set; }
        public string? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Time { get; set; }
    }
}

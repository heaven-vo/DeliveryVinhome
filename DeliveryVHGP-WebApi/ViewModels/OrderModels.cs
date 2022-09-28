namespace DeliveryVHGP_WebApi.ViewModels
{
    public class OrderModels
    {
        public string Id { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Total { get; set; }
        public string? Type { get; set; }
        public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? MenuId { get; set; }
        public string? MenuName { get; set; }
        public string? BuildingId { get; set; }
        public string? BuildingName { get; set; }

    }
}

namespace DeliveryVHGP_WebApi.ViewModels
{
    public class ProductModel
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Unit { get; set; }
        public double? PricePerPack { get; set; }
        public double? PackNetWeight { get; set; }
        public string? PackDescription { get; set; }
        public double? MaximumQuantity { get; set; }
        public double? MinimumQuantity { get; set; }
        public double? MinimumDeIn { get; set; }
        public string? StoreId { get; set; }
        public string? CategoryId { get; set; }
        public double? Rate { get; set; }
        public string? Description { get; set; }

    }
}

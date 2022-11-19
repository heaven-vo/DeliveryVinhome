namespace DeliveryVHGP.Core.Models
{
    public class OrderModels
    {
        public string Id { get; set; } = null!;
        public double? Total { get; set; }
        public string? BuildingId { get; set; }
        public string? buildingName { get; set; }
        public string? CustomerId { get; set; }
        public string? StoreId { get; set; }
        public string? storeName { get; set; }
        public int? status { get; set; }
        public DateTime? Time { get; set; }
        public double? quantity { get; set; }
        public string? TimeDuration { get; set; }
        public string? Dayfilter { get; set; }
        public List<double> productInmenuId { get; set; }


    }
}

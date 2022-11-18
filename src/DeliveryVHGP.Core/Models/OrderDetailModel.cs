namespace DeliveryVHGP.Core.Models
{
    public class OrderDetailModel
    {
        public string Id { get; set; } = null!;
        public string? Time { get; set; }
        public double? Total { get; set; }
        public double? ShipCost { get; set; }
        //public string? PaymentId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public int? PaymentName { get; set; }
        public int? PaymentStatus { get; set; }
        public string? ModeId { get; set; }

        //public string? BuildingId { get; set; }
        public string? BuildingName { get; set; }
        //public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? StoreBuilding { get; set; }
        public string? ShipperName { get; set; } = "";
        public string? ShipperPhone { get; set; } = "";
        public string? Note { get; set; }
        public string? TimeDuration { get; set; }
        public string? Dayfilter { get; set; }


        public List<ViewListDetail> ListProInMenu { get; set; }
        public List<ListStatusOrder> ListStatusOrder { get; set; }

    }
}

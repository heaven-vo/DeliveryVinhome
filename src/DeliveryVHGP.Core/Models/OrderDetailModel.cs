namespace DeliveryVHGP.Core.Models
{
    public class OrderDetailModel
    {
        public string Id { get; set; } = null!;
        public string? Time { get; set; }
        public double? Total { get; set; }
        public double? ShipCost { get; set; }
        //public string? PaymentId { get; set; }
        public string? PaymentName { get; set; }
        public string? ModeId { get; set; }

        //public string? BuildingId { get; set; }
        public string? BuildingName { get; set; }
        //public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? Note { get; set; }


        public List<ViewListDetail> ListProInMenu { get; set; }
        public List<ListStatusOrder> ListStatusOrder { get; set; }

    }
}

namespace DeliveryVHGP.Core.Models
{
    public class OrderAdminDto
    {
        public string Id { get; set; } = null!;
        public string? StoreName { get; set; }
        public string? BuildingName { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public double? Total { get; set; }
        public string? Note { get; set; }
        public double? ShipCost { get; set; }
        public string? PaymentName { get; set; }
        public string? ModeId { get; set; }
        public string? StatusName { get; set; }
        public string? Time { get; set; }
        //public TimeCreateOrder TimeCreate{ get; set; }
    }
}

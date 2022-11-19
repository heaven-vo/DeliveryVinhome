namespace DeliveryVHGP.Core.Models
{
    public class OrderDto
    {
        public string Id { get; set; } = null!;
        //public string? CustomerId { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Total { get; set; }
        public string? StoreId { get; set; }
        public string? MenuId { get; set; }
        public string? BuildingId { get; set; }
        public string? Note { get; set; }
        public string? FullName { get; set; }
        public double? ShipCost { get; set; }
        public string? DeliveryTimeId { get; set; }
        public string? ServiceId { get; set; }
        public string? ModeId { get; set; }

        public List<OrderDetailDto> OrderDetail { get; set; }
        public List<PaymentDto> Payments { get; set; }


    }
    public class OrderInfor
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
        //public string PaymentId { get; set; }
    }
}

namespace DeliveryVHGP.Core.Models
{
    public class EdgeModel
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int OrderNum { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
    }
    public class OrderActionModel
    {
        public string ActionId { get; set; }
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string CusName { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public int? PaymentType { get; set; }
        public double? ShipCost { get; set; }
        public double? Total { get; set; }
        public string ServiceName { get; set; }
        public int? ActionType { get; set; }
        public int? ActionStatus { get; set; }

        public List<OrderDetailActionModel> OrderDetailActions { get; set; }
    }
    public class OrderDetailActionModel
    {
        public int? Quantity { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
    }
}

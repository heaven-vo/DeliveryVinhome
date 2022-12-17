namespace DeliveryVHGP.Core.Models
{
    public class ShipperHistoryModel
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public double? Total { get; set; }
        public double? ShippingCost { get; set; }
        public int RouteType { get; set; }
        public int? ActionType { get; set; }
        public double Profit { get; set; }
        public DateTime? Date { get; set; }
    }
    public class HistoryDetail
    {
        public string? OrderId { get; set; }
        public string Start { get; set; }
        public string StartBuilding { get; set; }
        public string End { get; set; }
        public string EndBuilding { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public string messageFail { get; set; }
        public int? PaymentType { get; set; }
        public double? ShipCost { get; set; }
        public double? Total { get; set; }
        public string ServiceName { get; set; }
        public List<OrderDetailActionModel> OrderDetails { get; set; }
    }
}

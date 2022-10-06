namespace DeliveryVHGP_WebApi.ViewModels
{
    public class OrderDetailModel
    {
        public string Id { get; set; } = null!;
        public string? ProductInMenuId { get; set; }
        public string? OrderId { get; set; }
        public string? Quantity { get; set; }
        public double? OTotal { get; set; }
        public string? OType { get; set; }
        public String proId { get; set; }
        public String proImage { get; set; }
        public String proName { get; set; }
        public String StoreName { get; set; }
        public double? proPricePerPack { get; set; }
        public String proPackDes { get; set; }
        public String menuId { get; set; }
        public String menuName { get; set; }

    }
}

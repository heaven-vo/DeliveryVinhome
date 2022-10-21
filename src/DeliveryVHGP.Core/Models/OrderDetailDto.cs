namespace DeliveryVHGP.Core.Models
{
    public class OrderDetailDto
    {
        //public string Id { get; set; } = null!;
        public string? ProductInMenuId { get; set; }
        public string? Quantity { get; set; }
        //public string? ProductName { get; set; }
        public double? Price { get; set; }


    }
}

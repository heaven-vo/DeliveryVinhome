namespace DeliveryVHGP.Core.Models
{
    public class ProductsInMenuModel
    {
        public string menuId { get; set; }
        public List<ProductAddMenu> products { get; set; }
    }
    public class ProductAddMenu
    {
        public string id { get; set; }
        public double price { get; set; }
    }
}

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
    public class ProductsInMenuUpdateModel
    {
        public string menuId { get; set; }
        public string productId { get; set; }
        public double price { get; set; }
        public bool status { get; set; }
    }

}

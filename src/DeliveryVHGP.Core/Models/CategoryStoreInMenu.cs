namespace DeliveryVHGP.Core.Models
{
    public class CategoryStoreInMenu
    {
        public string Id { get; set; } = null!;
        public string? Image { get; set; }
        public string? Name { get; set; }
        public List<ProductViewInList> ListProducts { get; set; }
    }
}

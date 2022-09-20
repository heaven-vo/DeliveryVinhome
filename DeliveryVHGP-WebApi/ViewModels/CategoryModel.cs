namespace DeliveryVHGP_WebApi.ViewModels
{
    public class CategoryModel
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }

        public List<string> ListCateInMenus { get; set; }
        public List<string> ListProducts { get; set; }

    }
}

namespace DeliveryVHGP_WebApi.ViewModels
{
    public class CategoryModel
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? CreateAt { get; set; }
        public string? UpdateAt { get; set; }
        //public string? Status { get; set; }
    }
}

namespace DeliveryVHGP_WebApi.ViewModels
{
    public class StoreModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string? StoreCategoryId { get; set; }
        public string? StoreCategoyName { get; set; }

    }
}

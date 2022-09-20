namespace DeliveryVHGP_WebApi.ViewModels
{
    public class BrandModels
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }

        public List<string> ListStore { get; set; }
    }
}

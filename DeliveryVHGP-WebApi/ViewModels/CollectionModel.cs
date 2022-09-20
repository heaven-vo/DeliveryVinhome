namespace DeliveryVHGP_WebApi.ViewModels
{
    public class CollectionModel
    {
        public string Id { get; set; } = null!;
        public string? StoreId { get; set; }
        public string? Name { get; set; }

        public List<string> Store { get; set; }
        public List<string>  ListProductInCollections { get; set; }
    }
}

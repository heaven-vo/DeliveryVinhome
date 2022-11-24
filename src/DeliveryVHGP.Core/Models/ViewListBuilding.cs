namespace DeliveryVHGP.Core.Models
{
    public class ViewListBuilding
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? HubId { get; set; }
        
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
    }
}

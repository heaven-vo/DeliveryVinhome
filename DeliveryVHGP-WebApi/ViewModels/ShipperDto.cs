namespace DeliveryVHGP_WebApi.ViewModels
{
    public class ShipperDto
    {
        public string Id { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? VehicleType { get; set; }
        public string? Image { get; set; }
        public string? DeliveryTeam { get; set; }
        public string? Password { get; set; }
        
    }
}

namespace DeliveryVHGP.Core.Models
{
    public class ShipperModel
    {
        public string Id { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? DeliveryTeam { get; set; }
        public string? VehicleType { get; set; }
        public string? LicensePlates { get; set; }
        public string? Colour { get; set; }
        public string? Password { get; set; }
        public bool? Status { get; set; }
        public string? CreateAt { get; set; }
        public string? UpdateAt { get; set; }
       
    }
    public class FilterRequestInShipper
    {
        public string? SearchByName { get; set; } = "";
    }
}

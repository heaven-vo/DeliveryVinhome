namespace DeliveryVHGP.Core.Models
{
    public class HubModels
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? BuildingId { get; set; }
    }
    public class HubDto
    {
        //public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? BuildingId { get; set; }
    }
    public class FilterRequestInHub {
        public string? SearchByName { get; set; } = "";

    }
}

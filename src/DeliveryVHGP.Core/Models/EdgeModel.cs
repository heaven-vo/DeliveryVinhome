namespace DeliveryVHGP.Core.Models
{
    public class EdgeModel
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int OrderNum { get; set; }
        public int? Priority { get; set; }
        public int? Staus { get; set; }
    }
}

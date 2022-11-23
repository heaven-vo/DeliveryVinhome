namespace DeliveryVHGP.Core.Models
{
    public class AreaDto
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public List<ViewListCluster>? listCluster { get; set; }
    }
}

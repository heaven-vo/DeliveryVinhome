using DeliveryVHGP.Core.Entities;

namespace DeliveryVHGP.Core.Models
{
    public class ViewListArea
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public List<ViewListClusterInArea> ListCluster { get; set; }
    }
}

using DeliveryVHGP_WebApi.Models;

namespace DeliveryVHGP_WebApi.ViewModels
{
    public class ViewListArea
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public List<ViewListClusterInArea> ListCluster { get; set; }
        //public List<ViewListBuilding> ListBuilding { get; set; }
    }
}

using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interfaces.IRepositories
{
    public interface IRouteActionRepository : IRepositoryBase<SegmentDeliveryRoute>
    {
        Task<List<RouteModel>> GetCurrentAvalableRoute();
        Task CreateRoute(List<SegmentDeliveryRoute> route, List<SegmentModel> listSegments);
        Task CreateActionOrder(List<NodeModel> listNode, List<SegmentModel> listSegments);
        Task RemoveRouteActionNotShipper();
        Task AcceptRouteByShipper(string routeId, string shipperId);
        Task<List<EdgeModel>> GetListEdgeInRoute(string routeId);
        Task<List<OrderActionModel>> GetListOrderAction(string edgeId);
    }
}

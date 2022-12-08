using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interfaces.IRepositories
{
    public interface IRouteActionRepository : IRepositoryBase<SegmentDeliveryRoute>
    {
        Task<List<RouteModel>> GetCurrentAvalableRoute();
        Task<int> CreateRoute(List<SegmentDeliveryRoute> route, List<SegmentModel> listSegments);
        Task CreateActionOrder(List<NodeModel> listNode, List<SegmentModel> listSegments);
        Task RemoveRouteActionNotShipper();
        Task RemoveAllRouteAction();
        Task AcceptRouteByShipper(string routeId, string shipperId);
        Task<List<EdgeModel>> GetListEdgeInRoute(string routeId);
        Task<EdgeModel> GetCurrentEdgeInRoute(string shipperId);
        Task<List<OrderActionModel>> GetListOrderAction(string edgeId);
    }
}

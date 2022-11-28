using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Core.Models.Noti;

namespace DeliveryVHGP.Infrastructure.Services
{
    public interface IFirestoreService
    {
        Task AddRoute(RouteModel route);
        Task UpdateRoute(string routeId, RouteUpdateModel route);
        Task<RouteModel> GetRouteData(string id);
        Task<UsersFcm> GetUserData(string id);
        Task DeleteEm(string id);
        Task DeleteAllRoutes();
    }
}

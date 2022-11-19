using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Infrastructure.Repositories
{
    public class RouteActionRepository : RepositoryBase<SegmentDeliveryRoute>, IRouteActionRepository
    {
        public RouteActionRepository(DeliveryVHGP_DBContext context) : base(context)
        {

        }

        public async Task CreateRoute(List<SegmentDeliveryRoute> route)
        {
            //SegmentDeliveryRoute route = new SegmentDeliveryRoute() { Id = Guid.NewGuid().ToString(), Distance = 10, Status = 1 };
            //route.RouteEdges = new List<RouteEdge>() {
            //    new RouteEdge() { Id = Guid.NewGuid().ToString(), RouteId = route.Id, Priority = 3},
            //    new RouteEdge() { Id = Guid.NewGuid().ToString(), RouteId = route.Id, Priority = 4}
            //};
            await context.AddRangeAsync(route);
            await context.SaveChangesAsync();
            
        }
    }
}

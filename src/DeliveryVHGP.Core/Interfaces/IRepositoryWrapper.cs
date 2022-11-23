using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Core.Interfaces
{
    public interface IRepositoryWrapper
    {
        IMenuRepository Menu { get; }
        IAccountRepository Account { get; }
        IAreaRepository Area { get; }
        IBrandRepository Brand { get; }
        IHubRepository Hub { get; }
        IBuildingRepository Building { get; }
        ICategoriesRepository Category { get; }
        ICollectionRepository Collection { get; }
        ICustomerRepository Customer { get; }
        IOrderRepository Order { get; }
        IProductRepository Product { get; }
        IShipperRepository Shipper { get; }
        IStoreRepository Store { get; }
        IStoreCategoryRepository StoreCategory { get; }
        ISegmentRepository Segment { get; }
        ICacheRepository Cache { get; }
        IRouteActionRepository RouteAction { get; }
    }
}

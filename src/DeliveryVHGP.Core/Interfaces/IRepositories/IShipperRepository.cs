using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IShipperRepository : IRepositoryBase<Shipper>
    {
        Task<IEnumerable<ShipperModel>> GetListShipper(int pageIndex, int pageSize);
        Task<Object> GetShipperById(string shipId);
        Task<ShipperDto> CreateShipper(ShipperDto ship);
    }
}

using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IShipperRepository : IRepositoryBase<Shipper>
    {
        Task<IEnumerable<ShipperModel>> GetListShipper(int pageIndex, int pageSize, FilterRequestInShipper request);
        Task<IEnumerable<ShipperModel>> GetListShipperByName(string shipName, int pageIndex, int pageSize);
        Task<Object> GetShipperById(string shipId);
        Task<ShipperDto> CreateShipper(ShipperDto ship);
        Task<ShipperDto> UpdateShipper(string shipId, ShipperDto ship, Boolean imgUpdate);
        Task<StatusShipDto> UpdateStatusShipper(string ShipId, StatusShipDto shipper);
        Task<Object> DeleteShipper(string shipperId);
    }
}

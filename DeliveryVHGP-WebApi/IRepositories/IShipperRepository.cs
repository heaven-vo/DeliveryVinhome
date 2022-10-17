using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IShipperRepository
    {
        Task<IEnumerable<ShipperModel>> GetListShipper(int pageIndex, int pageSize);
        Task<Object> GetShipperById(string shipId);
        Task<ShipperDto> CreateShipper(ShipperDto ship);
    }
}

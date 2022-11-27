using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interfaces.IRepositories
{
    public interface IShipperHistoryRepository : IRepositoryBase<ShipperHistory>
    {
        Task<List<ShipperHistoryModel>> GetShipperHistories(string shipperId, int status, int page, int pageSize);
        Task<HistoryDetail> GetShipperHistoryDetail(string shipperHistoryId);
    }
}

using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Models;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

namespace DeliveryVHGP.Core.Interfaces.IRepositories
{
    public interface IShipperHistoryRepository : IRepositoryBase<ShipperHistory>
    {
        Task<List<ShipperHistoryModel>> GetShipperHistories(string shipperId, int status, int page, int pageSize);
        Task<HistoryDetail> GetShipperHistoryDetail(string shipperHistoryId);
        Task<ShipperReportModel> GetShipperReport(string shipperId, DateFilterRequest request, MonthFilterRequest monthFilter);
        Task<DeliveryShipperReportModel> GetDeliveryAllShipperReport(DateFilterRequest request, MonthFilterRequest monthFilter, int page, int pageSize);
    }
}

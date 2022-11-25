using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP_WebApi.ViewModels;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IStoreRepository : IRepositoryBase<Store>
    {
        Task<IEnumerable<StoreModel>> GetListStore( int pageIndex, int pageSize, FilterRequestInStore request);
        Task<SystemReportModelInStore> GetListOrdersReport(string storeId, DateFilterRequest request);
        Task<IEnumerable<StoreModel>> GetListStoreInBrand(string brandName, int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreByName(string storeName, int pageIndex, int pageSize);
        Task<Object> GetStoreById(string storeId);
        Task<List<OrderAdminDtoInStore>> GetListOrderPreparingsByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDtoInStore>> GetListOrderDeliveringByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDtoInStore>> GetListOrderCompletedByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDtoInStore>> GetListOrderByStoreByModeId(string StoreId, string modeId, DateFilterRequest request, int pageIndex, int pageSize);
        Task<StoreDto> CreatNewStore(StoreDto store);
        Task<Object> DeleteStore(string storeId);
        Task<StoreDto> UpdateStore(string storeId, StoreDto store, Boolean imgUpdate);
        Task<StatusStoreDto> UpdateStatusStore(string storeId, StatusStoreDto store);
        }
}

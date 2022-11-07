using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IStoreRepository : IRepositoryBase<Store>
    {
        Task<IEnumerable<StoreModel>> GetListStore( int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreInBrand(string brandName, int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreByName(string storeName, int pageIndex, int pageSize);
        Task<Object> GetStoreById(string storeId);
        Task<List<OrderAdminDto>> GetListOrderPreparingsByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrderDeliveringByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrderCompletedByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrderByStoreByModeId(string StoreId, string modeId, int pageIndex, int pageSize);
        Task<StoreDto> CreatNewStore(StoreDto store);
        Task<Object> DeleteStore(string storeId);
        Task<StoreDto> UpdateStore(string storeId, StoreDto store, Boolean imgUpdate);
        Task<StatusStoreDto> UpdateStatusStore(string storeId, StatusStoreDto store);
        }
}

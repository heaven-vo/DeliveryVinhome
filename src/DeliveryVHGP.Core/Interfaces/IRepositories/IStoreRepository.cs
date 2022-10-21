using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IStoreRepository : IRepositoryBase<Store>
    {
        Task<IEnumerable<StoreModel>> GetListStore( int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreInBrand(string brandName, int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreByName(string storeName, int pageIndex, int pageSize);
        Task<Object> GetStoreById(string storeId);

        Task<StoreDto> CreatNewStore(StoreDto store);
        Task<Object> DeleteStore(string storeId);
        Task<StoreDto> UpdateStore(string storeId, StoreDto store, Boolean imgUpdate);
        }
}

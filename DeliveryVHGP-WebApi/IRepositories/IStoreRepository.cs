using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IStoreRepository
    {
        Task<IEnumerable<StoreModel>> GetListStore( int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreInBrand(string brandName, int pageIndex, int pageSize);
        Task<IEnumerable<StoreModel>> GetListStoreByName(string storeName, int pageIndex, int pageSize);
        Task<Object> GetStoreById(string storeId);

        Task<StoreDto> CreatNewStore(StoreDto store);
        Task<Object> DeleteStore(string storeId);
        Task<StoreDto> UpdateStore(string storeId, StoreDto store);
        Task<Object> PostFireBase(IFormFile file);
        }
}

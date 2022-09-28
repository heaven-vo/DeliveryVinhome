using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IStoreCategoryRepository
    {
        Task<IEnumerable<StoreCategoryModel>> GetAll(int pageIndex, int pageSize);
        Task<StoreCategoryModel> CreateStoreCategory(StoreCategoryModel storeCate);
        Task<Object> DeleteById(string proId);
        Task<Object> UpdateStoreCateById(string storecaId, StoreCategoryModel storeCate);
    }
}

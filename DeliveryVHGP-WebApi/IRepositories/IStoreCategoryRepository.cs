using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IStoreCategoryRepository
    {
        Task<IEnumerable<StoreCategoryModel>> GetAll(int pageIndex, int pageSize);
        Task<StoreCategoryDto> CreateStoreCategory(StoreCategoryDto storeCate);
        Task<Object> DeleteById(string proId);
        Task<Object> UpdateStoreCateById(string storecaId, StoreCategoryModel storeCate);
    }
}

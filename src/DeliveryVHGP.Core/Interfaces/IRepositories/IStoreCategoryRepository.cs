using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IStoreCategoryRepository : IRepositoryBase<StoreCategory>
    {
        Task<IEnumerable<StoreCategoryModel>> GetAll(int pageIndex, int pageSize);
        Task<StoreCategoryDto> CreateStoreCategory(StoreCategoryDto storeCate);
        Task<Object> DeleteById(string proId);
        Task<Object> UpdateStoreCateById(string storecaId, StoreCategoryModel storeCate);
    }
}

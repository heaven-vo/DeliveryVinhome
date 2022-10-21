using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IBrandRepository : IRepositoryBase<Brand>
    {
        Task<IEnumerable<BrandModels>> GetAll(int pageIndex, int pageSize);
        Task<BrandModels> GetById(string Id);
        Task<BrandModels> CreateBrand(BrandModels brand);
        Task<Object> DeleteById(string proId);
        Task<Object> UpdateBrandById(string brandId, BrandModels brand);
    }
}

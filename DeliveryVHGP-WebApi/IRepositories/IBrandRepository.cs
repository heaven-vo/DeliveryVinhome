using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<BrandModels>> GetAll(int pageIndex, int pageSize);
        Task<BrandModels> GetById(string Id);
        Task<BrandModels> CreateBrand(BrandModels brand);
        Task<Object> DeleteById(string proId);
        Task<Object> UpdateBrandById(string brandId, BrandModels brand);
    }
}

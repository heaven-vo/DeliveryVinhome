using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<BrandModels>> GetAll(int pageIndex, int pageSize);
        Task<BrandModels> GetById(string Id);
    }
}

using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IStoreCategoryRepository
    {
        Task<IEnumerable<StoreCategoryModel>> GetAll(int pageIndex, int pageSize);
    }
}

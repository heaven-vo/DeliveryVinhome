using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<CategoryModel>> GetAll(int pageIndex, int pageSize);
        Task<CategoryModel> GetById(string Id);
    }
}

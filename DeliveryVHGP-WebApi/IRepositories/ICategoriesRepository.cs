using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<CategoryModel>> GetAll(int pageIndex, int pageSize);
        Task<List<CategoryModel>> GetListCategoryByMenuId(string id, int page, int pageSize);
        Task<CategoryModel> CreateCategory(CategoryModel category);
        Task<Object> DeleteCateInMenuById(string CateInMenuId);
        Task<Object> UpdateCategoryById(string categoryId, CategoryModel category);

    }
}

using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<CategoryModel>> GetAll(int pageIndex, int pageSize);
        Task<List<CategoryModel>> GetListCategoryByMenuId(string id, int page, int pageSize);
        Task<Object> GetCategoryById(string cateId);
        Task<IEnumerable<CategoryModel>> GetListCategoryByName(string cateName, int pageIndex, int pageSize);
        Task<CategoryDto> CreateCategory(CategoryDto category);
        Task<Object> DeleteCateInMenuById(string CateInMenuId);
        Task<Object> UpdateCategoryById(string categoryId, CategoryDto category, Boolean imgUpdate);

    }
}

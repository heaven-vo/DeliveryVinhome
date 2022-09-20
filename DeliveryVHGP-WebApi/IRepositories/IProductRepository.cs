using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDetailsModel>> GetAll(int pageIndex, int pageSize);
        Task<Object> GetById(string proId);
        Task<Object> UpdateProductDetailById(string proId, ProductDetailsModel product);
        Task<Object> DeleteProductById(string id);
        Task<List<ProductViewInList>> GetListProductInStore(string storeId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInCategory(string categoryId, int page, int pageSize);
    }
}

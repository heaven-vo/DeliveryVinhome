using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IProductRepository
    {
        Task<List<ProductViewInList>> GetListProductInStore(string storeId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInCategory(string categoryId, int page, int pageSize);
    }
}

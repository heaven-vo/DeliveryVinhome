using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IProductRepository
    {
        Task<List<ProductViewInList>> getListProductInStore(string storeId, int page, int pageSize);
        Task<List<ProductViewInList>> getListProductInCategory(string categoryId, int page, int pageSize);
    }
}

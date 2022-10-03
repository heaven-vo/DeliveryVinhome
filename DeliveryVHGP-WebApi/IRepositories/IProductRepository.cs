using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDetailsModel>> GetAll(string storeId,int pageIndex, int pageSize);
        Task<Object> GetById(string proId);
        Task<ProductModel> CreatNewProduct(ProductModel pro);
        Task<Object> UpdateProductDetailById(string proId, ProductDetailsModel product);
        Task<Object> DeleteProductById(string id);
        Task<List<ProductViewInList>> GetListProductInStore(string storeId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInCategory(string categoryId, int page, int pageSize);
        Task<Object> PostFireBase(IFormFile file);
    }
}

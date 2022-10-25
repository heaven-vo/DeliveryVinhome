using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<ProductDetailsModel>> GetListProduct(string menuId, int pageIndex, int pageSize);
        Task<IEnumerable<ProductDetailsModel>> GetAll(string storeId,int pageIndex, int pageSize);
        Task<ProductDetailsModel> GetById(string proId);
        Task<ProductModel> CreatNewProduct(ProductModel pro);
        Task<Object> UpdateProductById(string proId, ProductDto product);
        Task<Object> DeleteProductById(string id);
    }
}

using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IProductv1Repository
    {
        Task<IEnumerable<ProductDetailsModel>> GetAll(int pageIndex, int pageSize);
        Task<Object> UpdateProductDetailById(string proId, ProductDetailsModel product);

        Task<Object> DeleteProductById(string id);
    }
}
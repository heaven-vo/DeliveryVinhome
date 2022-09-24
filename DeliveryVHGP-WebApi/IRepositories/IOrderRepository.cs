using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using System.Collections;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderModels>> GetOrders(int pageIndex, int pageSize);

        Task<Object> GetOrdersById(string orderId);
    }
}

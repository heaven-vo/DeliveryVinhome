using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using System.Collections;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IOrderRepository
    {
        Task<List<OrderModels>> GetListOrders(string CusId,int pageIndex, int pageSize);

        Task<Object> GetOrdersById(string orderId);

        Task<OrderDto> CreatNewOrder(OrderDto order);
        Task<OrderDto> OrderUpdate(string orderId, OrderDto order);
    }
}

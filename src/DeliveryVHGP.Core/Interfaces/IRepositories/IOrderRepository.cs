using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using System.Collections;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<List<OrderModels>> GetListOrders(string CusId,int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrdersByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrdersByStoreByStatus(string StoreId, string StatusId, int pageIndex, int pageSize);
        Task<Object> GetOrdersById(string orderId);

        Task<OrderDto> CreatNewOrder(OrderDto order);
        Task<OrderStatusModel> OrderUpdateStatus(string orderId, OrderStatusModel order);
    }
}

using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using System.Collections;
using System.Collections.Generic;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<List<OrderAdminDto>> GetAll(int pageIndex, int pageSize, DateFilterRequest request);
        Task<List<OrderModels>> GetListOrders(string CusId,int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrdersByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrdersByStoreByStatus(string StoreId, int StatusId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetOrderByPayment(string PaymentType, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetOrderByStatus(int status, int pageIndex, int pageSize);
        Task<List<TimeDurationOrder>> GetDurationOrder(string menuId, int pageIndex, int pageSize);
        Task<Object> GetOrdersById(string orderId);

        Task<OrderDto> CreatNewOrder(OrderDto order);
        Task<OrderStatusModel> OrderUpdateStatus(string orderId, OrderStatusModel order);
        Task<Object> PaymentOrder(string orderId);
    }
}

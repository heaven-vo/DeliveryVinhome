using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<List<OrderAdminDto>> GetAll(int pageIndex, int pageSize, FilterRequest request);
        Task<List<OrderAdminDto>> GetAllOrder(FilterRequest request);
        Task<SystemReportModel> GetListOrdersReport(DateFilterRequest request, MonthFilterRequest monthFilter);
        Task<PriceReportModel> GetPriceOrdersReports(DateFilterRequest request, MonthFilterRequest monthFilter);
        //Task<List<OrderModels>> GetListOrders(string CusId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrdersByStore(string StoreId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetListOrdersByStoreByStatus(string StoreId, int StatusId, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetOrderByPayment(int PaymentType, int pageIndex, int pageSize);
        Task<List<OrderAdminDto>> GetOrderByPhone(int pageIndex, int pageSize, string phone);
        Task<List<OrderAdminDto>> GetOrderByStatus(int status, int pageIndex, int pageSize);
        Task<List<TimeDurationOrder>> GetDurationOrder(string menuId, int pageIndex, int pageSize);
        Task<Object> GetOrdersById(string orderId);

        Task<OrderDto> CreatNewOrder(OrderDto order);
        Task<OrderStatusModel> OrderUpdateStatus(string orderId, int status);
        Task<Object> PaymentOrder(string orderId);
        Task<Object> PaymentOrderSuccessfull(string orderId);
        Task<Object> PaymentOrderFalse(string orderId);
        Task<List<string>> CheckAvailableOrder();
        Task CompleteOrder(string orderActionId, string shipperId, int actionType);
        Task CancelOrder(string orderActionId, string shipperId, int actionType, string messageFail);
        Task CancelOrderByAdmin(string orderId, int orderStatus, string messageFail);
        Task CancelOrderByStore(string orderId, string messageFail);
        Task<Object> DeleteOrder();
        Task<object> GetWalletsStore(int pageIndex, int pageSize, WalletsFilter request);
        Task<Object> getWalletById(string walletId);
    }
}

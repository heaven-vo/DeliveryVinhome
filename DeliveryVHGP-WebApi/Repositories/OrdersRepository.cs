using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class OrdersRepository : IOrderRepository
    {
        private readonly DeliveryVHGP_DBContext context;

        public OrdersRepository(DeliveryVHGP_DBContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OrderModels>> GetOrders(int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join c in context.Customers on order.CustomerId equals c.Id
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join m in context.Menus on order.MenuId equals m.Id
                                  select new OrderModels()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      Type = order.Type,
                                      CustomerId = c.Id,
                                      CustomerName = c.FullName,
                                      StoreId = s.Id,
                                      StoreName = s.Name,
                                      MenuId = m.Id,
                                      MenuName = m.Name,
                                      BuildingId = b.Id,
                                      BuildingName = b.Name
                                  }
                                  ).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();


            return lstOrder;
        }
        public async Task<Object> GetOrdersById(string orderId)
        {
            var order = await ( from od in context.OrderDetails
                                join pm in context.ProductInMenus on od.ProductInMenuId equals pm.Id
                                join m in context.Menus on pm.MenuId equals m.Id
                                join p in context.Products on pm.ProductId equals p.Id
                                join s in context.Stores on p.StoreId equals s.Id
                                join o in context.Orders on od.OrderId equals o.Id
                                where (o.Id == orderId)
                                select new OrderDetailModel()
                                {
                                    Id = od.Id,
                                    OrderId = o.Id,
                                    OTotal = o.Total,
                                    OType = o.Type,
                                    Quantity = od.Quantity,
                                    ProductInMenuId = pm.Id,
                                    proId = p.Id,
                                    proImage = p.Image,
                                    proName = p.Name,
                                    proPackDes = p.PackDescription,
                                    proPricePerPack = p.PricePerPack,
                                    StoreName = s.Name,
                                    menuId = m.Id,
                                    menuName = m.Name,
                                }
                                ).ToListAsync();
            return order;
        }
    }
}
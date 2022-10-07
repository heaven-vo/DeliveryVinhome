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

        public async Task<List<OrderModels>> GetListOrders(int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join od in context.OrderDetails on order.Id equals od.OrderId
                                  join pm in context.ProductInMenus on od.ProductInMenuId equals pm.Id
                                  select new OrderModels()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreId = s.Id,
                                      StoreName = s.Name,
                                      BuildingId = order.BuildingId,
                                      DurationId = order.DurationId,
                                  }
                                  ).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return lstOrder;
        }
        public async Task<Object> GetOrdersById(string orderId)
        {
            var order = await (from od in context.OrderDetails
                               join pm in context.ProductInMenus on od.ProductInMenuId equals pm.Id
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
                               }
                                ).ToListAsync();
            return order;
        }
        public async Task<OrderDto> CreatNewOrder(OrderDto order)
        {
            var od = new Order
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = order.CustomerId,
                Total = order.Total,
                Type = order.Type,
                StoreId = order.StoreId,
                Note = order.Note,
                FullName = order.FullName,
                PhoneNumber = order.PhoneNumber,
                ShipCost = order.ShipCost,
                DurationId = order.DurationId,
            };
                await context.Orders.AddAsync(od);

            foreach (var ord in order.OrderDetail)
            {
                var odd = new OrderDetail{ Id = Guid.NewGuid().ToString(),
                                           ProductInMenuId = ord.ProductInMenuId,
                                           Quantity = ord.Quantity,
                                           OrderId = od.Id
                };
                await context.OrderDetails.AddAsync(odd);
            }

            foreach (var pay in order.Payments)
            {
                var payment = new Payment{ Id = Guid.NewGuid().ToString(),
                                           Type = pay.Type,
                                           OrderId = od.Id
                };

                await context.Payments.AddAsync(payment);
            }
            await context.SaveChangesAsync();

            return order;
        }

    }
}
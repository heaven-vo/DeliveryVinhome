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

        public async Task<List<OrderModels>> GetListOrders(string CusId,int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join c in context.Customers on order.CustomerId equals c.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.StatusId equals sta.Id
                                  where c.Id == CusId
                                  select new OrderModels()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreId = s.Id,
                                      StoreName = s.Name,
                                      BuildingId = order.BuildingId,
                                      BuildingName = b.Name,
                                      StatusId = order.StatusId,
                                      StatusName = sta.Name,
                                      Time = t.Time
                                  }
                                  ).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
           
            return lstOrder;
        }
        public async Task<Object> GetOrdersById(string orderId)
        {

            var order = await (from o in context.Orders
                               join odd in context.OrderDetails on o.Id equals odd.OrderId
                               //join pm in context.ProductInMenus on od.ProductInMenuId equals pm.Id
                               join t in context.TimeOfOrderStages on o.Id equals t.OrderId
                               join p in context.Payments on o.Id equals p.OrderId
                               where (o.Id == orderId)
                               select new OrderDetailModel()
                               {
                                   Id = o.Id,
                                   Time = t.Time,
                                   PaymentId = p.Id,
                                   PaymentName = p.Type,
                               }
                                ).FirstOrDefaultAsync();
            foreach (var item in order.ListOrderDetail)
            {
                var proInMenu = await context.OrderDetails.Select(o => new OrderDetailDto()
                {
                    ProductInMenuId = item.ProductInMenuId,
                    Quantity = item.Quantity,
                }).ToListAsync();
            }
           

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
                DurationId = order.DurationId
            };
                await context.Orders.AddAsync(od);

            foreach (var ord in order.OrderDetail)
            {
                var proInMenu = context.ProductInMenus.FirstOrDefault(pm => pm.Id == ord.ProductInMenuId);
                var odd = new OrderDetail{ Id = Guid.NewGuid().ToString(),
                                           ProductInMenuId = ord.ProductInMenuId,
                                           Quantity = ord.Quantity,
                                           Price = proInMenu.Price,
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

            string time = await GetTime();

            var timeOfOrder = new TimeOfOrderStage()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = od.Id,
                StatusId = "1" ,
                Time = time
            };
            await context.TimeOfOrderStages.AddAsync(timeOfOrder);
            await context.SaveChangesAsync();

            return order;
        }
        public async Task<string> GetTime()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("MM/dd/yyyy HH:mm");
            return time;
        }

    }
}
using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class OrdersRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrdersRepository(DeliveryVHGP_DBContext context): base(context)
        {
        }

        public async Task<List<OrderModels>> GetListOrders(string CusId ,int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join c in context.Customers on order.CustomerId equals c.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.StatusId equals sta.Id
                                  join od in context.OrderDetails on order.Id equals od.OrderId
                                  join pm in context.ProductInMenus on od.ProductInMenuId equals pm.Id
                                  where c.Id == CusId && t.StatusId == order.StatusId
                                  select new OrderModels()
                                  {
                                      Id = order.Id,    
                                      Total = order.Total,
                                      CustomerId = c.Id,
                                      StoreId = s.Id,
                                      storeName = s.Name,
                                      statusName = sta.Name,
                                      BuildingId = b.Id,
                                      buildingName = b.Name,
                                      statusId = sta.Id,
                                      Time = t.Time
                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        public async Task<List<OrderAdminDto>> GetListOrdersByStore(string StoreId, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.StatusId equals sta.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where s.Id == StoreId && t.StatusId == order.StatusId
                                  select new OrderAdminDto()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreName = s.Name,
                                      Phone = order.PhoneNumber,
                                      Note = order.Note,
                                      ShipCost = order.ShipCost,
                                      StatusName = sta.Name,
                                      CustomerName = order.FullName,
                                      PaymentName = p.Type,
                                      BuildingName = b.Name,
                                      //ShipperName = sp.FullName,
                                      Time = t.Time,

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        //public async Task<List<OrderAdminDto>> GetListOrdersByStore(string StoreId, int pageIndex, int pageSize)
        //{
        //    var listOrder = await(from order in context.Orders
        //                          join s in context.Stores on order.StoreId equals s.Id
        //                          join b in context.Buildings on order.BuildingId equals b.Id
        //                          join sta in context.OrderStatuses on order.StatusId equals sta.Id
        //                          join t in context.TimeOfOrderStages on order.Id equals t.OrderId
        //                          join p in context.Payments on order.Id equals p.OrderId
        //                          where s.Id == StoreId && t.StatusId == order.StatusId
        //                          select new OrderAdminDto()
        //                          {
        //                              Id = order.Id,
        //                              StoreName = s.Name,
        //                              Total = order.Total,
        //                              Note = order.Note,
        //                              Phone = order.PhoneNumber,
        //                              ShipCost = order.ShipCost,
        //                              StatusName = sta.Name,
        //                              BuildingName = b.Name,
        //                              PaymentName = p.Type,
        //                              Time = t.Time
        //                          }
        //                          ).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        //    return listOrder;
        //}
        public async Task<List<OrderAdminDto>> GetListOrdersByStoreByStatus(string StoreId ,string StatusId, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  //join c in context.Customers on order.CustomerId equals c.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.StatusId equals sta.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where s.Id == StoreId && order.StatusId == StatusId && t.StatusId == order.StatusId
                                  select new OrderAdminDto()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreName = s.Name,
                                      Phone = order.PhoneNumber,
                                      Note = order.Note,
                                      ShipCost = order.ShipCost,
                                      StatusName = sta.Name,
                                      CustomerName = order.FullName,
                                      PaymentName = p.Type,
                                      BuildingName = b.Name,
                                      //ShipperName = sp.FullName,
                                      Time = t.Time,

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return lstOrder;
        }
        public async Task<Object> GetOrdersById(string orderId)
        {
            var order = await (from o in context.Orders
                               join odd in context.OrderDetails on o.Id equals odd.OrderId
                               join b in context.Buildings on o.BuildingId equals b.Id
                               join s in context.Stores on o.StoreId equals s.Id
                               //join pm in context.ProductInMenus on od.ProductInMenuId equals pm.Id
                               join t in context.TimeOfOrderStages on o.Id equals t.OrderId
                               join p in context.Payments on o.Id equals p.OrderId
                               where (o.Id == orderId)
                               select new OrderDetailModel()
                               {
                                   Id = o.Id,
                                   Total = o.Total,
                                   Time = t.Time,
                                   //PaymentId = p.Id,
                                   PaymentName = p.Type,
                                   //StoreId= o.StoreId,
                                   StoreName = s.Name,
                                   //BuildingId= o.BuildingId,
                                   BuildingName = b.Name,
                                   Note = o.Note,
                                   ShipCost = o.ShipCost,
                               }
                                ).FirstOrDefaultAsync();
            var listPro = await (from o in context.Orders
                                 join odd in context.OrderDetails on o.Id equals odd.OrderId
                                 join pm in context.ProductInMenus on odd.ProductInMenuId equals pm.Id
                                 where o.Id == order.Id
                                 select new ViewListDetail
                                 {
                                     ProductInMenuId = pm.Id,
                                     Price = odd.Price,
                                     Quantity = odd.Quantity,
                                     ProductName = odd.ProductName,
                                 }).ToListAsync();
            order.ListProInMenu = listPro;

            var listStatus = await (from o in context.Orders
                                    join t in context.TimeOfOrderStages on o.Id equals t.OrderId
                                    join s in context.OrderStatuses on t.StatusId equals s.Id
                                    where t.OrderId == order.Id
                                    select new ListStatusOrder
                                    {
                                        Name = s.Name,
                                        Time = t.Time,
                                    }
                                    ).OrderBy(t => t.Time).ToListAsync();
            order.ListStatusOrder = listStatus;

            return order;
        }
        public async Task<OrderDto> CreatNewOrder(OrderDto order)
        {
            var od = new Order
            {
                Id = order.Id = Guid.NewGuid().ToString(),
                Total = order.Total,
                Type = order.Type,
                StoreId = order.StoreId,
                BuildingId = order.BuildingId,
                Note = order.Note,
                FullName = order.FullName,
                PhoneNumber = order.PhoneNumber,
                ShipCost = order.ShipCost,
                DurationId = order.DurationId,
                StatusId = "1"
            };
                await context.Orders.AddAsync(od);

            foreach (var ord in order.OrderDetail)
            {
                var proInMenu = context.ProductInMenus.FirstOrDefault(pm => pm.Id == ord.ProductInMenuId);
                var pro = context.Products.FirstOrDefault(p => p.Id == proInMenu.ProductId);
                var odd = new OrderDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductInMenuId = ord.ProductInMenuId,
                    Quantity = ord.Quantity,
                    Price = ord.Price,
                    OrderId = od.Id,
                    ProductName = pro.Name,
                    ProductId = proInMenu.ProductId,
                };
                await context.OrderDetails.AddAsync(odd);
            }

            foreach (var pay in order.Payments)
            {
                var payment = new Payment
                {
                    Id = Guid.NewGuid().ToString(),
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
        public async Task<OrderStatusModel> OrderUpdateStatus(string orderId, OrderStatusModel order)
        {
            var orderUpdate = await context.Orders.FindAsync(orderId);
            orderUpdate.Id = orderUpdate.Id;
            orderUpdate.StatusId = order.StatusId;
            context.Entry(orderUpdate).State = EntityState.Modified;
            await context.SaveChangesAsync();

            string time = await GetTime();
            var timeOfOrder = new TimeOfOrderStage()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = orderId,
                StatusId = order.StatusId,
                Time = time
            };
            await context.TimeOfOrderStages.AddAsync(timeOfOrder);
            await context.SaveChangesAsync();

            return order;
        }
        public async Task<List<string>> GetListProInMenu(string orderDetailId)
        {
            List<string> listpro = await (from od in context.OrderDetails
                                          join o in context.Orders on od.OrderId equals o.Id

                                          where o.Id == orderDetailId
                                          select od.ProductInMenuId
                                                ).ToListAsync();
            return listpro;
        }
        public async Task<string> GetTime()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("yyyy/MM/dd HH:mm");
            return time;
        }

    }
}
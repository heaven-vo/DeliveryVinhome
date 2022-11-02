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
        public async Task<List<OrderAdminDto>> GetAll(int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.Status equals sta.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where t.StatusId == "1"
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
                                      ModeId = m.ModeId,
                                      //ShipperName = sp.FullName,
                                      Time = t.Time,

                                  }
                                ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        public async Task<List<OrderAdminDto>> GetOrderByPayment(string PayName ,int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.Status equals sta.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where t.StatusId == "1" && p.Type.Contains(PayName)
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
                                      ModeId = m.ModeId,
                                      BuildingName = b.Name,
                                      //ShipperName = sp.FullName,
                                      Time = t.Time,

                                  }
                                ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        public async Task<List<OrderAdminDto>> GetOrderByStatus(string statusName ,int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.Status equals sta.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where t.StatusId == "1" && sta.Name.Contains(statusName)
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
                                      ModeId = m.ModeId,
                                      //ShipperName = sp.FullName,
                                      Time = t.Time,

                                  }
                                ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        public async Task<List<OrderModels>> GetListOrders(string CusId ,int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join c in context.Customers on order.CustomerId equals c.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.Status equals sta.Id
                                  join od in context.OrderDetails on order.Id equals od.OrderId
                                  where c.Id == CusId && t.StatusId == order.Status
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
                                  join sta in context.OrderStatuses on order.Status equals sta.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  where s.Id == StoreId &&  t.StatusId == "1"
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
                                      ModeId = m.ModeId,
                                      //ShipperName = sp.FullName,
                                      Time = t.Time,

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        public async Task<List<OrderAdminDto>> GetListOrdersByStoreByStatus(string StoreId ,string StatusId, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  //join c in context.Customers on order.CustomerId equals c.Id
                                  join t in context.TimeOfOrderStages on order.Id equals t.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join sta in context.OrderStatuses on order.Status equals sta.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where s.Id == StoreId && order.Status == StatusId && t.StatusId == order.Status
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
                                      ModeId = m.ModeId,
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
                               join m in context.Menus on o.MenuId equals m.Id
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
                                   ModeId = m.ModeId,
                                   BuildingName = b.Name,
                                   Note = o.Note,
                                   ShipCost = o.ShipCost,
                               }
                                ).FirstOrDefaultAsync();
            var listPro = await (from o in context.Orders
                                 join odd in context.OrderDetails on o.Id equals odd.OrderId
                                 join pm in context.Products on odd.ProductId equals pm.Id
                                 where o.Id == order.Id
                                 select new ViewListDetail
                                 {
                                     ProductId = odd.ProductId,
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
            string refixOrderCode = "CDCC";
            var orderCount = context.Orders
               .Count() + 1;
            order.Id = refixOrderCode + "-" + orderCount.ToString().PadLeft(6, '0');
            var odCOde = await context.Orders.Where(o => o.Id == order.Id).ToListAsync();
            if (odCOde.Any())
            { 
                    order.Id = refixOrderCode + "-" + orderCount.ToString().PadLeft(7, '0');
            }
            var store = context.Stores.FirstOrDefault(s => s.Id == order.StoreId);
            var od = new Order
            {
                Id = order.Id,
                Total = order.Total,
                StoreId = store.Id, 
                BuildingId = order.BuildingId,
                Note = order.Note,
                FullName = order.FullName,
                PhoneNumber = order.PhoneNumber,
                MenuId = order.MenuId,
                ShipCost = order.ShipCost,
                DurationId = order.DurationId,
                Status = "1"
            };
            if (store.Status == false)
            {
                throw new Exception("Có đơn hàng không hợp lệ ");
            }
            await context.Orders.AddAsync(od);
            //await context.SaveChangesAsync();
            foreach (var ord in order.OrderDetail)
            {
                //var proInMenu = context.ProductInMenus.FirstOrDefault(pm => pm.Id == ord.ProductInMenuId);
                var pro = context.Products.FirstOrDefault(p => p.Id == ord.ProductId);
                var odd = new OrderDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    Quantity = ord.Quantity,
                    Price = ord.Price,
                    OrderId = od.Id,
                    ProductName = pro.Name,
                    ProductId = ord.ProductId,
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
                //await context.SaveChangesAsync();
            }   
            //await context.SaveChangesAsync();
            string time = await GetTime();

            var timeOfOrder = new TimeOfOrderStage()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = od.Id,
                StatusId = "1" ,
                Time = time
            };
            await context.TimeOfOrderStages.AddAsync(timeOfOrder);
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return order;
        }
        public async Task<OrderStatusModel> OrderUpdateStatus(string orderId, OrderStatusModel order)
        {
            var orderUpdate = await context.Orders.FindAsync(orderId);
            orderUpdate.Id = orderUpdate.Id;
            orderUpdate.Status = order.StatusId;
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
                                          select od.ProductId
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
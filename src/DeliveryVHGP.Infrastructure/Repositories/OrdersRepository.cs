﻿using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using DeliveryVHGP.Core.Enums;
using static DeliveryVHGP.Core.Models.OrderAdminDto;
using DeliveryVHGP.Infrastructure.Services;
namespace DeliveryVHGP.WebApi.Repositories
{
    public class OrdersRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrdersRepository(DeliveryVHGP_DBContext context) : base(context)
        {
        }
        //Get list order (in admin web)
        public async Task<List<OrderAdminDto>> GetAll(int pageIndex, int pageSize, DateFilterRequest request)
        {
            //var fromm = request?.FromDate;
            //var to = request?.ToDate;
            //if (fromm != null && to != null)
            //{
            //    fromm = ((DateTime)fromm).GetStartOfDate();
            //    to = ((DateTime)to).GetEndOfDate();
            //}
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id  tamm
                                  where h.ToStatus == 0 && h.CreateDate.Contains(request.DateFilter)
                                  select new OrderAdminDto()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreName = s.Name,
                                      Phone = order.PhoneNumber,
                                      Note = order.Note,
                                      ShipCost = order.ShipCost,
                                      CustomerName = order.FullName,
                                      PaymentName = p.Type,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Status = order.Status,
                                      Time = h.CreateDate

                                  }
                                ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        public async Task<List<OrderAdminDto>> GetOrderByPayment(string PaymentType, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where p.Type == PaymentType && h.ToStatus == 0
                                  select new OrderAdminDto()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreName = s.Name,
                                      Phone = order.PhoneNumber,
                                      Note = order.Note,
                                      ShipCost = order.ShipCost,
                                      Status = order.Status,
                                      CustomerName = order.FullName,
                                      PaymentName = p.Type,
                                      ModeId = m.SaleMode,
                                      BuildingName = b.Name,
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate

                                  }
                                ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        public async Task<List<OrderAdminDto>> GetOrderByStatus(int status, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where order.Status == status && h.ToStatus == 0
                                  select new OrderAdminDto()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreName = s.Name,
                                      Phone = order.PhoneNumber,
                                      Note = order.Note,
                                      ShipCost = order.ShipCost,
                                      Status = order.Status,
                                      CustomerName = order.FullName,
                                      PaymentName = p.Type,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate

                                  }
                                ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        //Get list order by Customer(in customer web)
        public async Task<List<OrderModels>> GetListOrders(string CusId, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join c in context.Customers on order.CustomerId equals c.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join od in context.OrderDetails on order.Id equals od.OrderId
                                  where c.Id == CusId
                                  select new OrderModels()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      CustomerId = c.Id,
                                      StoreId = s.Id,
                                      storeName = s.Name,
                                      status = order.Status,
                                      BuildingId = b.Id,
                                      buildingName = b.Name,
                                      Time = h.CreateDate
                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        //Get list order by store(in app store)
        public async Task<List<OrderAdminDto>> GetListOrdersByStore(string StoreId, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  where s.Id == StoreId && h.ToStatus == 0
                                  select new OrderAdminDto()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreName = s.Name,
                                      Phone = order.PhoneNumber,
                                      Note = order.Note,
                                      ShipCost = order.ShipCost,
                                      Status = order.Status,
                                      CustomerName = order.FullName,
                                      PaymentName = p.Type,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        public async Task<List<OrderAdminDto>> GetListOrdersByStoreByStatus(string StoreId, int StatusId, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  //join c in context.Customers on order.CustomerId equals c.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where s.Id == StoreId && order.Status == StatusId && h.ToStatus == StatusId
                                  select new OrderAdminDto()
                                  {
                                      Id = order.Id,
                                      Total = order.Total,
                                      StoreName = s.Name,
                                      Phone = order.PhoneNumber,
                                      Note = order.Note,
                                      ShipCost = order.ShipCost,
                                      Status = order.Status,
                                      CustomerName = order.FullName,
                                      PaymentName = p.Type,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return lstOrder;
        }
        //Get Order Detail by Id (in admin web ,store app, cus web)
        public async Task<Object> GetOrdersById(string orderId)
        {
            var order = await (from o in context.Orders
                               join odd in context.OrderDetails on o.Id equals odd.OrderId
                               join b in context.Buildings on o.BuildingId equals b.Id
                               join s in context.Stores on o.StoreId equals s.Id
                               join bs in context.Buildings on s.BuildingId equals bs.Id
                               join m in context.Menus on o.MenuId equals m.Id
                               //join pm in context.ProductInMenus on od.ProductInMenuId equals pm.Id
                               join h in context.OrderActionHistories on o.Id equals h.OrderId
                               //join sg in context.SegmentDeliveries on o.Id equals sg.OrderId
                               //join ship in context.Shippers on sg.ShipperId equals ship.Id
                               join p in context.Payments on o.Id equals p.OrderId
                               where (o.Id == orderId)
                               select new OrderDetailModel()
                               {
                                   Id = o.Id,
                                   Total = o.Total,
                                   Time = h.CreateDate,
                                   //PaymentId = p.Id,
                                   FullName = o.FullName,
                                   PhoneNumber = o.PhoneNumber,
                                   PaymentName = p.Type,
                                   //StoreId= o.StoreId,
                                   StoreName = s.Name,
                                   StoreBuilding = bs.Name,
                                   //ShipperName = ship.FullName,
                                   //ShipperPhone = ship.Phone,
                                   ModeId = m.SaleMode,
                                   BuildingName = b.Name,
                                   Note = o.Note,
                                   ShipCost = o.ShipCost,
                               }
                                ).FirstOrDefaultAsync();
            if (order == null)
            {
                throw new KeyNotFoundException();
            }
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
                                    join h in context.OrderActionHistories on o.Id equals h.OrderId
                                    where h.OrderId == order.Id
                                    select new ListStatusOrder
                                    {
                                        Status = h.ToStatus,//status
                                        Time = h.CreateDate
                                    }
                                    ).OrderBy(t => t.Status).ToListAsync();
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
                DeliveryTimeId = order.DeliveryTimeId,
                Status = (int)OrderStatusEnum.New
            };
            if (store.Status == false)
            {
                throw new Exception("Đơn hàng không hợp lệ");
            }
            await context.Orders.AddAsync(od);
            //await context.SaveChangesAsync();
            foreach (var ord in order.OrderDetail)
            {
                //var proInMenu = context.ProductInMenus.FirstOrDefault(pm => pm.Id == ord.ProductInMenuId);
                var pro = context.Products.FirstOrDefault(p => p.Id == ord.ProductId); //low performent
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
                    OrderId = od.Id,
                    Amount = order.Total
                };
                await context.Payments.AddAsync(payment);
                //await context.SaveChangesAsync();
            }
            //await context.SaveChangesAsync();
            string time = await GetTime();

            //var timeOfOrder = new TimeOfOrderStage()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    OrderId = od.Id,
            //    StatusId = 1 ,
            //    Time = time
            //};
            var actionHistory = new OrderActionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = od.Id,
                FromStatus = (int)OrderStatusEnum.New,
                ToStatus = (int)OrderStatusEnum.New,
                CreateDate = time,
                TypeId = "1"
            };
            await context.OrderActionHistories.AddAsync(actionHistory);
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
            if (orderUpdate == null)
            {
                return null;
            }
            int oldStatus = (int)orderUpdate.Status;
            orderUpdate.Status = order.StatusId;
            context.Entry(orderUpdate).State = EntityState.Modified;

            string time = await GetTime();
            var actionHistory = new OrderActionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = orderId,
                FromStatus = oldStatus,
                ToStatus = order.StatusId,
                CreateDate = time,
                TypeId = "1"
            };
            await context.OrderActionHistories.AddAsync(actionHistory);
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
        public async Task<List<TimeDurationOrder>> GetDurationOrder(string menuId, int pageIndex, int pageSize)
        {

            var lsrDuration = await context.DeliveryTimeFrames.Where(d => d.MenuId == menuId)
                                    .Select(d => new TimeDurationOrder
                                    {
                                        Id = d.Id,
                                        MenuId = d.MenuId,
                                        FromDate = d.FromDate,
                                        ToDate = d.ToDate,
                                        ToHour = TimeSpan.FromHours((double)d.ToHour).ToString(@"hh\:mm"),
                                        FromHour = TimeSpan.FromHours((double)d.FromHour).ToString(@"hh\:mm"),
                                    }
                                    ).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lsrDuration;
        }
        public async Task<Object> DeleteOrderById(string orderId)
        {
            var order = await context.Orders.FindAsync(orderId);
            var orderHistory = context.OrderActionHistories.FirstOrDefault(d => d.OrderId == order.Id);
            var payment = context.Payments.FirstOrDefault(p => p.OrderId == order.Id);
            var orderDetail = context.OrderDetails.FirstOrDefault(od => od.OrderId == order.Id);


            context.Orders.Remove(order);
            context.OrderActionHistories.Remove(orderHistory);
            context.Payments.Remove(payment);
            context.OrderDetails.Remove(orderDetail);
            await context.SaveChangesAsync();

            return order;

        }
        public async Task<string> GetTime()
        {
            DateTime utcDateTime = DateTime.UtcNow;

            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("yyyy/MM/dd HH:mm");
            return time;
        }
        public async Task<Object> PaymentOrder(string orderId)
        {
            string vnp_Returnurl = "https://localhost:7102/api/v1/orders/Payment-confirm"; //URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "MM9A0YQZ"; //Ma website
            string vnp_HashSecret = "YLGGIJRNXHISHHCZSMHXFRVXUTJIFMSZ"; //Chuoi bi mat

            var o = await context.Orders.FindAsync(orderId);
            var payy = context.Payments.FirstOrDefault(p => p.OrderId == orderId);
            OrderInfor order = new OrderInfor();

            order.OrderId = orderId;
            order.Amount = (double)payy.Amount * 100;
            order.Status = 0;

            VnPayLibrary pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", vnp_TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", order.Amount.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_IpAddr", orderId); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;

        }
        public async Task<Object> PaymentOrderAction(string orderId)
        {
            var o = await context.Orders.FindAsync(orderId);
            var payy = context.Payments.FirstOrDefault(p => p.OrderId == orderId);
            var PaymentHistory = new Payment()
            {
                Id = orderId,
                Status = "1"
            };
            await context.Payments.AddAsync(PaymentHistory);
            await context.SaveChangesAsync();
            return o;
        }

    } 
}
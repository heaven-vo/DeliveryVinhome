using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using DeliveryVHGP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class OrdersRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly IFirestoreService firestoreService;
        public OrdersRepository(DeliveryVHGP_DBContext context, IFirestoreService firestoreService) : base(context)
        {
            this.firestoreService = firestoreService;
        }
        //Get list order (in admin web)
        public async Task<List<OrderAdminDto>> GetAll(int pageIndex, int pageSize, FilterRequest request)
        {
            //var fromm = request?.FromDate;
            //var to = request?.ToDate;
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
                                  join dt in context.DeliveryTimeFrames on order.DeliveryTimeId equals dt.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id  tamm
                                  where h.ToStatus == 0 && h.CreateDate.ToString().Contains(request.DateFilter)
                                  && p.Type.ToString().Contains(request.SearchByPayment)
                                  && (request.SearchByStatus == -1 || order.Status == request.SearchByStatus)
                                  && m.SaleMode.Contains(request.SearchByMode)
                                  //&& order.Status == request.SearchByStatus
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
                                      PaymentStatus = p.Status,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Status = order.Status,
                                      Time = h.CreateDate,
                                      TimeDuration = dt.Id,
                                      ToHour = TimeSpan.FromHours((double)dt.ToHour).ToString(@"hh\:mm"),
                                      FromHour = TimeSpan.FromHours((double)dt.FromHour).ToString(@"hh\:mm"),
                                      Dayfilter = m.DayFilter.ToString()

                                  }
                                ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var order in lstOrder)
            {
                var listShipper = await (from od in context.ShipperHistories
                                         join o in context.Orders on od.OrderId equals o.Id
                                         join s in context.Shippers on od.ShipperId equals s.Id
                                         where o.Id == order.Id
                                         select new ViewListShipper()
                                         {
                                             ShipperId = od.ShipperId,
                                             Phone = s.Phone,
                                             ShipperName = s.FullName
                                         }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                order.ListShipper = listShipper;
            }
            return lstOrder;
        }
        public async Task<List<OrderAdminDto>> GetAllOrder(FilterRequest request)
        {
            //var lstOrder = await context.Orders
            //    .Select(o => new OrderCountModels()
            //    {
            //    }).ToListAsync();
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  join dt in context.DeliveryTimeFrames on order.DeliveryTimeId equals dt.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id  tamm
                                  where h.ToStatus == 0 && h.CreateDate.ToString().Contains(request.DateFilter)
                                  && p.Type.ToString().Contains(request.SearchByPayment)
                                  && (request.SearchByStatus == -1 || order.Status == request.SearchByStatus)
                                  && m.SaleMode.Contains(request.SearchByMode)
                                  //&& order.Status == request.SearchByStatus
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
                                      PaymentStatus = p.Status,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Status = order.Status,
                                      Time = h.CreateDate,
                                      TimeDuration = dt.Id,
                                      ToHour = TimeSpan.FromHours((double)dt.ToHour).ToString(@"hh\:mm"),
                                      FromHour = TimeSpan.FromHours((double)dt.FromHour).ToString(@"hh\:mm"),
                                      Dayfilter = m.DayFilter.ToString()

                                  }
                                ).OrderByDescending(t => t.Time).ToListAsync();
            foreach (var order in lstOrder)
            {
                var listShipper = await (from od in context.ShipperHistories
                                         join o in context.Orders on od.OrderId equals o.Id
                                         join s in context.Shippers on od.ShipperId equals s.Id
                                         where o.Id == order.Id
                                         select new ViewListShipper()
                                         {
                                             ShipperId = od.ShipperId,
                                             ShipperName = s.FullName,
                                             Phone = s.Phone
                                         }).ToListAsync();
                order.ListShipper = listShipper;
            }
            //int CountOrder = lstOrder.Count;

            return lstOrder;
        }

        public async Task<SystemReportModel> GetListOrdersReport(DateFilterRequest request, MonthFilterRequest monthFilter)
        {
            SystemReportModel report = new SystemReportModel()
            {
                TotalOrderNew = 0,
                TotalOrderUnpaidVNpay = 0,
                TotalOrderCancel = 0,
                TotalOrderCompleted = 0,
                TotalOrder = 0,
                TotalStore = 0, // tong store
                TotalShipper = 0, // tong store

            };

            //SystemReportModelInStore report = new SystemReportModelInStore()
            if (request.DateFilter != "")
            {
                DateTime dateTime = DateTime.Parse(request.DateFilter);
                var nextDay = dateTime.AddDays(1);

                var lstOrder = await (from orderr in context.Orders
                                      join h in context.OrderActionHistories on orderr.Id equals h.OrderId
                                      where h.ToStatus == 0 && h.CreateDate > dateTime && h.CreateDate < nextDay
                                      select orderr).ToListAsync();
                var countStore = context.Stores.Count();
                var countShipper = context.Shippers.Count();
                if (!lstOrder.Any())
                {
                    return report;
                }

                report.TotalOrderNew = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.Assigning || order.Status == (int)OrderStatusEnum.Accepted
                                                    || order.Status == (int)OrderStatusEnum.InProcess || order.Status == (int)InProcessStatus.HubDelivery
                                                    || order.Status == (int)InProcessStatus.AtHub || order.Status == (int)InProcessStatus.CustomerDelivery).Count(); //don hang moi
                report.TotalOrderUnpaidVNpay = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.New).Count();//don hang chua thanh toan vnpay
                report.TotalOrderCancel = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)FailStatus.CustomerFail
                                                    || order.Status == (int)FailStatus.OutTime || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail).Count();//don hang chua thanh toan vnpay
                report.TotalOrderCompleted = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Completed).Count(); //don hang thanh cong
                report.TotalOrder = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.New
                                                    || order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)FailStatus.CustomerFail
                                                    || order.Status == (int)FailStatus.OutTime || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail
                                                    || order.Status == (int)OrderStatusEnum.Completed || order.Status == (int)OrderStatusEnum.Assigning || order.Status == (int)OrderStatusEnum.Accepted
                                                    || order.Status == (int)OrderStatusEnum.InProcess || order.Status == (int)InProcessStatus.HubDelivery
                                                    || order.Status == (int)InProcessStatus.AtHub || order.Status == (int)InProcessStatus.CustomerDelivery
                                                  ).Count(); //tong don hang
                report.TotalStore = countStore; // tong store
                report.TotalShipper = countShipper; // tong store
                return report;
            }
            else if (monthFilter.Month != 0)
            {

                var lstOrder = await (from orderr in context.Orders
                                      join h in context.OrderActionHistories on orderr.Id equals h.OrderId
                                      where  h.ToStatus == 0 && h.CreateDate.Value.Year == monthFilter.Year && h.CreateDate.Value.Month == monthFilter.Month
                                      select orderr).ToListAsync();
                var countStore = context.Stores.Count();
                var countShipper = context.Shippers.Count();

                if (!lstOrder.Any())
                {
                    return report;
                }
                report.TotalOrderNew = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.Assigning || order.Status == (int)OrderStatusEnum.Accepted
                                                    || order.Status == (int)OrderStatusEnum.InProcess || order.Status == (int)InProcessStatus.HubDelivery
                                                    || order.Status == (int)InProcessStatus.AtHub || order.Status == (int)InProcessStatus.CustomerDelivery).Count(); //don hang moi
                report.TotalOrderUnpaidVNpay = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.New).Count();//don hang chua thanh toan vnpay
                report.TotalOrderCancel = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)FailStatus.CustomerFail
                                                    || order.Status == (int)FailStatus.OutTime || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail).Count();//don hang chua thanh toan vnpay
                report.TotalOrderCompleted = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Completed).Count(); //don hang thanh cong
                report.TotalOrder = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.New
                                                    || order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)FailStatus.CustomerFail
                                                    || order.Status == (int)FailStatus.OutTime || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail
                                                    || order.Status == (int)OrderStatusEnum.Completed || order.Status == (int)OrderStatusEnum.Assigning || order.Status == (int)OrderStatusEnum.Accepted
                                                    || order.Status == (int)OrderStatusEnum.InProcess || order.Status == (int)InProcessStatus.HubDelivery
                                                    || order.Status == (int)InProcessStatus.AtHub || order.Status == (int)InProcessStatus.CustomerDelivery
                                                  ).Count(); //tong don hang
                report.TotalStore = countStore; // tong store
                report.TotalShipper = countShipper; // tong store
                return report;
            }
            return null;
        }
       
        public async Task<PriceReportModel> GetPriceOrdersReports(DateFilterRequest request, MonthFilterRequest monthFilter)
        {
            PriceReportModel report = new PriceReportModel()
            {
                TotalShipFree = 0,
                TotalPaymentVNPay = 0,
                TotalPaymentCash = 0,
                TotalOrder = 0,
                TotalRevenueOrder = 0,
                TotalProfitOrder = 0

            };

            //SystemReportModelInStore report = new SystemReportModelInStore()
            if (request.DateFilter != "")
            {
                DateTime dateTime = DateTime.Parse(request.DateFilter);
                var nextDay = dateTime.AddDays(1);

                var lstOrder = await (from orderr in context.Orders
                                      join h in context.OrderActionHistories on orderr.Id equals h.OrderId
                                      join p in context.Payments on orderr.Id equals p.OrderId
                                      where h.ToStatus == 0 && h.CreateDate > dateTime && h.CreateDate < nextDay
                                      select orderr).ToListAsync();

                var lstPayment = await (from pay in context.Payments
                                        join o in context.Orders on pay.OrderId equals o.Id
                                        join h in context.OrderActionHistories on o.Id equals h.OrderId
                                        where h.ToStatus == 0 && h.CreateDate > dateTime && h.CreateDate < nextDay
                                        select pay).ToListAsync();
                if (!lstOrder.Any())
                {
                    return report;
                }

                report.TotalShipFree = (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.ShipCost); // tổng tiền ship
                //TotalSurcharge = lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Where(x => x.ServiceId == "1").Count() * 10000, // tổng tiền service
                report.TotalPaymentVNPay = (double)lstPayment.Where(p => p.Type == (int)PaymentEnum.VNPay).Sum(o => o.Amount);// tổng tiền thanh toán VnPay
                report.TotalPaymentCash = (double)lstPayment.Where(p => p.Type == (int)PaymentEnum.Cash).Sum(o => o.Amount);// tổng tiền thanh toán Cash
                report.TotalOrder = (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.Total); // tổng tiền order
                report.TotalRevenueOrder = (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.ShipCost) + (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.Total); // Doanh thu
                report.TotalProfitOrder = (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.ShipCost); // Lợi nhuận
            }
            else if (monthFilter.Month != 0)
            {
                var lstOrder = await (from orderr in context.Orders
                                      join h in context.OrderActionHistories on orderr.Id equals h.OrderId
                                      join p in context.Payments on orderr.Id equals p.OrderId
                                      join s in context.Stores on orderr.StoreId equals s.Id
                                      where h.ToStatus == 0 && h.CreateDate.Value.Year == monthFilter.Year && h.CreateDate.Value.Month == monthFilter.Month
                                      select orderr).ToListAsync();
                var lstPayment = await (from pay in context.Payments
                                        join o in context.Orders on pay.OrderId equals o.Id
                                        join h in context.OrderActionHistories on o.Id equals h.OrderId
                                        join s in context.Stores on o.StoreId equals s.Id
                                        where h.ToStatus == 0 && h.CreateDate.Value.Year == monthFilter.Year && h.CreateDate.Value.Month == monthFilter.Month
                                        select pay).ToListAsync();

                if (!lstOrder.Any())
                {
                    return report;
                }
                report.TotalShipFree = (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.ShipCost); // tổng tiền ship
                                                                                                                                     //TotalSurcharge = lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Where(x => x.ServiceId == "1").Count() * 10000, // tổng tiền service
                report.TotalPaymentVNPay = (double)lstPayment.Where(p => p.Type == (int)PaymentEnum.VNPay).Sum(o => o.Amount);// tổng tiền thanh toán VnPay
                report.TotalPaymentCash = (double)lstPayment.Where(p => p.Type == (int)PaymentEnum.Cash).Sum(o => o.Amount);// tổng tiền thanh toán Cash
                report.TotalOrder = (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.Total); // tổng tiền order
                report.TotalRevenueOrder = (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.ShipCost) + (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.Total); // Doanh thu
                report.TotalProfitOrder = (double)lstOrder.Where(p => p.Status == (int)OrderStatusEnum.Completed).Sum(o => o.ShipCost); // Lợi nhuận
                return report;
            }
                return null;
            }
        public async Task<List<OrderAdminDto>> GetOrderByPayment(int PaymentType, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  join dt in context.DeliveryTimeFrames on order.DeliveryTimeId equals dt.Id
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
                                      PaymentStatus = p.Status,
                                      ModeId = m.SaleMode,
                                      BuildingName = b.Name,
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate,
                                      TimeDuration = dt.Id,
                                      Dayfilter = m.DayFilter.ToString()
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
                                  join dt in context.DeliveryTimeFrames on order.DeliveryTimeId equals dt.Id
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
                                      PaymentStatus = p.Status,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate,
                                      TimeDuration = dt.Id,
                                      Dayfilter = m.DayFilter.ToString()

                                  }
                                ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return lstOrder;
        }
        //Get list order by Customer(in customer web)
        //public async Task<List<OrderModels>> GetListOrders(string CusId, int pageIndex, int pageSize)
        //{
        //    var lstOrder = await (from order in context.Orders
        //                          join s in context.Stores on order.StoreId equals s.Id
        //                          join h in context.OrderActionHistories on order.Id equals h.OrderId
        //                          join b in context.Buildings on order.BuildingId equals b.Id
        //                          join m in context.Menus on order.MenuId equals m.Id
        //                          join od in context.OrderDetails on order.Id equals od.OrderId
        //                          join dt in context.DeliveryTimeFrames on order.DeliveryTimeId equals dt.Id
        //                          where c.Id == CusId
        //                          select new OrderModels()
        //                          {
        //                              Id = order.Id,
        //                              Total = order.Total,
        //                              CustomerId = "None",
        //                              StoreId = s.Id,
        //                              storeName = s.Name,
        //                              status = order.Status,
        //                              BuildingId = b.Id,
        //                              buildingName = b.Name,
        //                              Time = h.CreateDate,
        //                              TimeDuration = dt.Id,
        //                              Dayfilter = m.DayFilter.ToString()
        //                          }
        //                          ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        //    return lstOrder;
        //}
        //Get list order by store(in app store)
        public async Task<List<OrderAdminDto>> GetListOrdersByStore(string StoreId, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  join dt in context.DeliveryTimeFrames on order.DeliveryTimeId equals dt.Id
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
                                      PaymentStatus = p.Status,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate,
                                      TimeDuration = dt.Id,
                                      Dayfilter = m.DayFilter.ToString()

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
                                  join dt in context.DeliveryTimeFrames on order.DeliveryTimeId equals dt.Id
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
                                      PaymentStatus = p.Status,
                                      BuildingName = b.Name,
                                      ModeId = m.SaleMode,
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate,
                                      TimeDuration = dt.Id,
                                      Dayfilter = m.DayFilter.ToString()

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
                               join dt in context.DeliveryTimeFrames on o.DeliveryTimeId equals dt.Id
                               where (o.Id == orderId) && h.ToStatus == 0
                               select new OrderDetailModel()
                               {
                                   Id = o.Id,
                                   Total = o.Total,
                                   Time = h.CreateDate,
                                   //PaymentId = p.Id,
                                   FullName = o.FullName,
                                   PhoneNumber = o.PhoneNumber,
                                   PaymentName = p.Type,
                                   PaymentStatus = p.Status,
                                   //StoreId= o.StoreId,
                                   StoreName = s.Name,
                                   StoreBuilding = bs.Name,
                                   //ShipperName = ship.FullName,
                                   //ShipperPhone = ship.Phone,
                                   ServiceId = o.ServiceId,
                                   ModeId = m.SaleMode,
                                   BuildingName = b.Name,
                                   Note = o.Note,
                                   ShipCost = o.ShipCost,
                                   TimeDuration = dt.Id,
                                   ToHour = TimeSpan.FromHours((double)dt.ToHour).ToString(@"hh\:mm"),
                                   FromHour = TimeSpan.FromHours((double)dt.FromHour).ToString(@"hh\:mm"),
                                   Dayfilter = m.DayFilter.ToString()
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

            var listShipper = await (from sm in context.SegmentDeliveryRoutes
                                     join s in context.Shippers on sm.ShipperId equals s.Id
                                     join r in context.RouteEdges on sm.Id equals r.RouteId
                                     join oa in context.OrderActions on r.Id equals oa.RouteEdgeId
                                     join o in context.Orders on oa.OrderId equals o.Id
                                     //leftJOi sh in context.ShipperHistories on s.Id equals sh.ShipperId
                                     where o.Id == orderId && (oa.OrderActionType == (int)OrderActionEnum.PickupStore || oa.OrderActionType == (int)OrderActionEnum.DeliveryCus)
                                     select new ViewListShipp()
                                     {
                                         ShipperId = sm.ShipperId,
                                         Phone = s.Phone,
                                         ShipperName = s.FullName,
                                         OrderActionType = oa.OrderActionType
                                     }).OrderBy(t => t.OrderActionType).ToListAsync();
            order.ListShipper = listShipper;

            return order;
        }

        public async Task<OrderDto> CreatNewOrder(OrderDto order)
        {
            var listProductId = order.OrderDetail.Select(x => x.ProductId);
            foreach (var proId in listProductId)
            {
                var pro = await (from menu in context.Menus
                                 join pm in context.ProductInMenus on menu.Id equals pm.MenuId
                                 join product in context.Products on pm.ProductId equals product.Id
                                 join sto in context.Stores on product.StoreId equals sto.Id
                                 where menu.Id == order.MenuId && product.Id == proId && sto.Status == true
                                 select product.Id
                                 ).FirstOrDefaultAsync();
                if (pro == null)
                {
                    throw new Exception("Sản phẩm hiện không khả dụng");
                }
            }
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
            if (store.Status == false)
            {
                throw new Exception("Đơn hàng không hợp lệ");
            }
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
                ServiceId = order.ServiceId,
                Status = (int)OrderStatusEnum.New
            };
            if (order.ServiceId == "1")
            {
                od.ShipCost += 10000;
            }

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
                    Amount = order.Total,
                    Status = (int)PaymentStatusEnum.unpaid,
                };
                await context.Payments.AddAsync(payment);
            }
            string time = await GetTime();

            var actionNewHistory = new OrderActionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = od.Id,
                FromStatus = (int)OrderStatusEnum.New,
                ToStatus = (int)OrderStatusEnum.New,
                CreateDate = DateTime.UtcNow.AddHours(7),
                TypeId = "1"
            };
            if (order.Payments[0].Type == (int)PaymentEnum.Cash)
            {
                var actionReviceHistory = new OrderActionHistory()// Beacause Store not need accept orrder, so order status change to next status
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = od.Id,
                    FromStatus = (int)OrderStatusEnum.New,
                    ToStatus = (int)OrderStatusEnum.Received,
                    CreateDate = DateTime.UtcNow.AddHours(7),
                    TypeId = "1"
                };
                od.Status = (int)OrderStatusEnum.Received;
                await context.OrderActionHistories.AddAsync(actionReviceHistory);
            }
            await context.Orders.AddAsync(od);
            await context.OrderActionHistories.AddAsync(actionNewHistory);
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Tạo đơn hàng không thành công");
            }

            return order;
        }
        public async Task<OrderStatusModel> OrderUpdateStatus(string orderId, int status)
        {
            var orderUpdate = await context.Orders.FindAsync(orderId);
            if (orderUpdate == null)
            {
                return null;
            }
            int oldStatus = (int)orderUpdate.Status;
            orderUpdate.Status = status;
            context.Entry(orderUpdate).State = EntityState.Modified;

            string time = await GetTime();
            var actionHistory = new OrderActionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = orderId,
                FromStatus = oldStatus,
                ToStatus = status,
                CreateDate = DateTime.UtcNow.AddHours(7),
                TypeId = "1"
            };
            await context.OrderActionHistories.AddAsync(actionHistory);
            await context.SaveChangesAsync();

            return new OrderStatusModel() { OrderId = orderId, StatusId = status };
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
        public async Task<Object> DeleteOrder()
        {
            var orderHistory = await context.OrderActionHistories.ToListAsync();
            var payment = await context.Payments.ToListAsync();
            var orderDetail = await context.OrderDetails.ToListAsync();
            var orderCache = await context.OrderCaches.ToListAsync();
            var segment = await context.Segments.ToListAsync();
            var orderAction = await context.OrderActions.ToListAsync();
            var Transactions = await context.Transactions.ToListAsync();
            var ShipperHistory = await context.ShipperHistories.ToListAsync();

            context.OrderActionHistories.RemoveRange(orderHistory);
            context.Payments.RemoveRange(payment);
            context.OrderDetails.RemoveRange(orderDetail);
            context.OrderCaches.RemoveRange(orderCache);
            context.Segments.RemoveRange(segment);
            context.OrderActions.RemoveRange(orderAction);
            context.Transactions.RemoveRange(Transactions);
            context.ShipperHistories.RemoveRange(ShipperHistory);

            await context.SaveChangesAsync();
            var order = await context.Orders.ToListAsync();
            context.Orders.RemoveRange(order);
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
            string vnp_Returnurl = "https://deliveryvhgp-webapi.azurewebsites.net/api/v1/orders/Payment-confirm"; //URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "MM9A0YQZ"; //Ma website
            string vnp_HashSecret = "YLGGIJRNXHISHHCZSMHXFRVXUTJIFMSZ"; //Chuoi bi mat

            var o = await context.Orders.FindAsync(orderId);
            var payy = context.Payments.FirstOrDefault(p => p.OrderId == orderId);
            OrderInfor order = new OrderInfor();

            order.OrderId = orderId;
            order.Amount = (double)payy.Amount * 100;
            //order.Status = 0;1         

            VnPayLibrary pay = new VnPayLibrary();
            if (payy.Type == 1)
            {
                pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                pay.AddRequestData("vnp_TmnCode", vnp_TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                pay.AddRequestData("vnp_Amount", order.Amount.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                //pay.AddRequestData("vnp_IpAddr", orderId); //Địa chỉ IP của khách hàng thực hiện giao dịch
                pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
                pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                pay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                pay.AddRequestData("vnp_TxnRef", orderId.ToString()); //mã hóa đơn
            }
            else
            {
                throw new Exception("Đơn hàng đã hoàn thành");
            }
            string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);


            return paymentUrl;
        }
        public async Task<Object> PaymentOrderSuccessfull(string orderId)
        {
            var order = await context.Orders.FindAsync(orderId);
            order.Status = (int)OrderStatusEnum.Received;
            var actionReviceHistory = new OrderActionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = orderId,
                FromStatus = (int)OrderStatusEnum.New,
                ToStatus = (int)OrderStatusEnum.Received,
                CreateDate = DateTime.UtcNow.AddHours(7),
                TypeId = "1"
            };
            var payy = context.Payments.FirstOrDefault(p => p.OrderId == orderId);
            payy.Status = (int)PaymentStatusEnum.successful;
            await context.AddAsync(actionReviceHistory);
            context.Entry(payy).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return payy;
        }
        public async Task<Object> PaymentOrderFalse(string orderId)
        {
            string time = await GetTime();
            var order = await context.Orders.FindAsync(orderId);
            var payy = context.Payments.FirstOrDefault(p => p.OrderId == orderId);

            order.Status = (int)FailStatus.CustomerFail;
            payy.Status = (int)PaymentStatusEnum.failed;

            var actionReviceHistory = new OrderActionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = orderId,
                FromStatus = (int)OrderStatusEnum.Received,
                ToStatus = (int)FailStatus.CustomerFail,
                CreateDate = DateTime.UtcNow.AddHours(7),
                TypeId = "1"
            };
            await context.OrderActionHistories.AddAsync(actionReviceHistory);
            await context.SaveChangesAsync();

            context.Entry(payy).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return order;
        }
        public async Task<List<string>> CheckAvailableOrder() // Remove mode 1
        {
            var time = Double.Parse(DateTime.UtcNow.AddHours(7).ToString("HH.mm"));
            DateTime date = DateTime.UtcNow.AddHours(7).Date;
            var listOrder = await context.Orders.Include(x => x.OrderCache)//.Include(x => x.OrderActionHistories)
                .Where(x => x.Status == (int)OrderStatusEnum.Received && (x.Menu.SaleMode == "1"
                             || (x.Menu.SaleMode == "2" && x.DeliveryTime.FromHour <= time)
                             || (x.Menu.SaleMode == "3" && x.Menu.DayFilter == date && x.DeliveryTime.FromHour <= time)
                             ) && (x.Payments.FirstOrDefault().Type == (int)PaymentEnum.Cash
                                || (x.Payments.FirstOrDefault().Type == (int)PaymentEnum.VNPay && x.Payments.FirstOrDefault().Status == (int)PaymentStatusEnum.successful)
                                )
                             //&& x.OrderCache != null
                             )
                .Take(100).ToListAsync(); //improve performace take 100 Select(x => x.Id)
            //Console.WriteLine("Order date: " + listOrder[0].OrderActionHistories.First().CreateDate.ToString());
            var list = listOrder.Where(x => x.OrderCache == null).Select(x => x.Id).ToList();
            return list;
        }
        public async Task CompleteOrder(string orderActionId, string shipperId, int actionType)
        {
            var orderAction = await context.OrderActions.FindAsync(orderActionId);
            var service = await context.Orders.Where(x => x.Id == orderAction.OrderId).Select(x => x.ServiceId).FirstOrDefaultAsync();
            if (orderAction == null)
            {
                throw new Exception("Order action Id not valid");
            }
            orderAction.Status = (int)OrderActionStatusEnum.Done;
            if (actionType == (int)OrderActionEnum.PickupStore)
            {
                //thieu cus delivery
                if (service == "1")
                {
                    var order = await OrderUpdateStatus(orderAction.OrderId, (int)InProcessStatus.CustomerDelivery);
                }
                if (service == "2")
                {
                    var order = await OrderUpdateStatus(orderAction.OrderId, (int)InProcessStatus.HubDelivery);
                }
            }
            if (actionType == (int)OrderActionEnum.PickupHub)
            {
                var order = await OrderUpdateStatus(orderAction.OrderId, (int)InProcessStatus.CustomerDelivery);
            }
            if (actionType == (int)OrderActionEnum.DeliveryHub) // creat shipper history
            {
                var order = await OrderUpdateStatus(orderAction.OrderId, (int)InProcessStatus.AtHub);
                if (order != null)
                {
                    //turn on order in cache and segment 
                    var orderCache = await context.OrderCaches.Where(x => x.OrderId == orderAction.OrderId).FirstOrDefaultAsync();
                    orderCache.IsReady = true;
                    var listSegment = await context.Segments.Where(
                        x => x.OrderId == orderAction.OrderId).ToListAsync();
                    foreach (var segment in listSegment)
                    {
                        if (segment.SegmentMode == (int)SegmentModeEnum.StoreToHub)
                        {
                            segment.Status = (int)SegmentStatusEnum.Done;
                        }
                        if (segment.SegmentMode == (int)SegmentModeEnum.HubToCus)
                        {
                            segment.Status = (int)SegmentStatusEnum.Viable;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            if (actionType == (int)OrderActionEnum.DeliveryCus) // creat shipper history
            {
                var order = await OrderUpdateStatus(orderAction.OrderId, (int)OrderStatusEnum.Completed);
                await RemoveOrderFromCache(orderAction.OrderId);
            }

            await CheckDoneRoute(orderActionId);
            //Create transaction, history after complete a order action
            await CreateTransaction(shipperId, orderAction.OrderId, actionType);
            await CreateShipperHistory(shipperId, orderAction.OrderId, actionType, (int)StatusEnum.success);
        }
        public async Task CancelOrder(string orderActionId, string shipperId, int actionType, string messageFail)
        {
            var orderAction = await context.OrderActions.Include(x => x.RouteEdge).Where(x => x.Id == orderActionId).FirstOrDefaultAsync();
            var orderFail = await context.Orders.FindAsync(orderAction.OrderId);
            orderFail.MessageFail = messageFail;
            orderAction.Status = (int)OrderActionStatusEnum.Fail;
            var listAction = await context.OrderActions.Where(x => x.OrderId == orderAction.OrderId && x.Status == (int)OrderActionStatusEnum.Todo).ToListAsync();
            if (listAction.Any())
            {
                listAction.ForEach(x => x.Status = (int)OrderActionStatusEnum.Fail);
            }

            await context.SaveChangesAsync();
            if (actionType == (int)OrderActionEnum.PickupStore)
            {
                var order = await OrderUpdateStatus(orderAction.OrderId, (int)FailStatus.StoreFail);
            }
            if (actionType == (int)OrderActionEnum.PickupHub)
            {
                var order = await OrderUpdateStatus(orderAction.OrderId, (int)FailStatus.ShipperFail);
            }
            if (actionType == (int)OrderActionEnum.DeliveryHub)
            {
                var order = await OrderUpdateStatus(orderAction.OrderId, (int)FailStatus.ShipperFail);
            }
            if (actionType == (int)OrderActionEnum.DeliveryCus)
            {
                var order = await OrderUpdateStatus(orderAction.OrderId, (int)FailStatus.CustomerFail);
            }
            await CheckDoneRoute(orderActionId);
            await CreateShipperHistory(shipperId, orderAction.OrderId, actionType, (int)StatusEnum.fail);
            await RemoveOrderFromCache(orderAction.OrderId);
        }
        public async Task RemoveOrderFromCache(string orderId)
        {
            var orderCache = await context.OrderCaches.Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
            context.Remove(orderCache);
            await context.SaveChangesAsync();

        }
        public async Task CreateTransaction(string shipperId, string orderId, int actionType)
        {
            var order = await context.Orders.Include(x => x.Payments).Where(x => x.Id == orderId).FirstOrDefaultAsync();
            if (order == null)
                throw new Exception("Order id is not valid");
            else
            {
                var wallet = await context.Wallets.Where(x => x.AccountId == shipperId).FirstOrDefaultAsync();
                Transaction tran = new Transaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    WalletId = wallet.Id,
                    OrderId = orderId,
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Status = (int)StatusEnum.success
                };
                if (order.Payments.FirstOrDefault().Type == (int)PaymentEnum.VNPay)
                {
                    if (order.ServiceId == "1")
                    {
                        if (actionType == (int)OrderActionEnum.DeliveryCus)
                        {
                            tran.Amount = order.Total;
                            tran.Action = (int)TransactionActionEnum.plus;
                            tran.Type = (int)TransactionTypeEnum.refund;
                            await context.AddAsync(tran);
                            wallet.Amount += order.Total;
                        }
                    }
                    if (order.ServiceId == "2")
                    {
                        if (actionType == (int)OrderActionEnum.DeliveryHub)
                        {
                            tran.Amount = order.Total;
                            tran.Action = (int)TransactionActionEnum.plus;
                            tran.Type = (int)TransactionTypeEnum.refund;
                            await context.AddAsync(tran);
                            wallet.Amount += order.Total;
                        }
                    }
                }
                if (order.Payments.FirstOrDefault().Type == (int)PaymentEnum.Cash)
                {
                    if (order.ServiceId == "1")
                    {
                        if (actionType == (int)OrderActionEnum.DeliveryCus)
                        {
                            tran.Amount = order.ShipCost;
                            tran.Action = (int)TransactionActionEnum.minus;
                            tran.Type = (int)TransactionTypeEnum.shippingcost;
                            await context.AddAsync(tran);
                            wallet.Amount -= order.ShipCost;
                        }
                    }
                    if (order.ServiceId == "2")
                    {
                        if (actionType == (int)OrderActionEnum.DeliveryHub)
                        {
                            tran.Amount = order.Total;
                            tran.Action = (int)TransactionActionEnum.plus;
                            tran.Type = (int)TransactionTypeEnum.refund;
                            await context.AddAsync(tran);
                            wallet.Amount += order.Total;
                        }
                        if (actionType == (int)OrderActionEnum.PickupHub)
                        {
                            tran.Amount = order.Total + order.ShipCost;
                            tran.Action = (int)TransactionActionEnum.minus;
                            tran.Type = (int)TransactionTypeEnum.cod;
                            await context.AddAsync(tran);
                            wallet.Amount -= (order.Total + order.ShipCost);
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
        }
        public async Task CreateShipperHistory(string shipperId, string orderId, int actionType, int status)
        {
            if (status == (int)StatusEnum.success && (actionType == (int)OrderActionEnum.DeliveryHub || actionType == (int)OrderActionEnum.DeliveryCus))
            {
                ShipperHistory history = new ShipperHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = orderId,
                    ShipperId = shipperId,
                    ActionType = actionType,
                    Status = status,
                    CreateDate = DateTime.UtcNow.AddHours(7)
                };
                await context.AddAsync(history);
                await context.SaveChangesAsync();
            }
            if (status == (int)StatusEnum.fail)
            {
                ShipperHistory history = new ShipperHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = orderId,
                    ShipperId = shipperId,
                    ActionType = actionType,
                    Status = status,
                    CreateDate = DateTime.UtcNow.AddHours(7)
                };
                await context.AddAsync(history);
                await context.SaveChangesAsync();
            }
        }
        public async Task CheckDoneRoute(string orderActionId)
        {
            var action = await context.OrderActions.Include(x => x.RouteEdge).Where(x => x.Id == orderActionId).FirstOrDefaultAsync();
            if (action == null)
            {
                throw new Exception("action null");
            }
            var actionTodo = await context.OrderActions
                .Where(x => x.RouteEdgeId == action.RouteEdgeId && x.Status == (int)OrderActionStatusEnum.Todo)
                .ToListAsync();
            if (!actionTodo.Any() || actionTodo == null)
            {
                action.RouteEdge.Status = (int)EdgeStatusEnum.Done;
                var lastEdge = await context.RouteEdges.Where(x => x.RouteId == action.RouteEdge.RouteId)
                    .OrderByDescending(x => x.Priority).Select(x => x.Priority).FirstOrDefaultAsync();
                //Console.WriteLine("Last edge: " + lastEdge);
                if (action.RouteEdge.Priority == lastEdge)
                {
                    var route = await context.SegmentDeliveryRoutes.FindAsync(action.RouteEdge.RouteId);
                    route.Status = (int)RouteStatusEnum.Done;
                    await firestoreService.DeleteEm(route.Id);
                }
                else
                {
                    var index = await context.RouteEdges.Where(x => x.Id == action.RouteEdgeId)
                        .Select(x => x.Priority).FirstOrDefaultAsync();
                    //Console.WriteLine("index: " + index);
                    var nextEdge = await context.RouteEdges
                        .Where(x => x.RouteId == action.RouteEdge.RouteId && x.Priority == (index + 1)).FirstOrDefaultAsync();
                    if (nextEdge == null)
                    {
                        throw new Exception("nextEdge null");
                    }
                    nextEdge.Status = (int)EdgeStatusEnum.ToDo;
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
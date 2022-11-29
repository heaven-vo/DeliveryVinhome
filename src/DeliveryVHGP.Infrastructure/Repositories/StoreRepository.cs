using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Infrastructure.Services;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using DeliveryVHGP_WebApi.ViewModels;
using DeliveryVHGP.Core.Enums;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class StoreRepository : RepositoryBase<Store>, IStoreRepository
    {
        private readonly IFileService _fileService;
        private readonly ITimeStageService _timeStageService ;
        public StoreRepository(ITimeStageService timeStageService, IFileService fileService, DeliveryVHGP_DBContext context): base(context)
        {
            _fileService = fileService;
            _timeStageService = timeStageService;
        }
        public async Task<IEnumerable<StoreModel>> GetListStore(int pageIndex, int pageSize , FilterRequestInStore request)
        {
            var listStore = await (from store in context.Stores
                                   join b in context.Brands on store.BrandId equals b.Id
                                   join building in context.Buildings on store.BuildingId equals building.Id
                                   join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
                                   where store.Name.Contains(request.SearchByStoreName)
                                   where b.Name.Contains(request.SearchByBrand)
                                   where building.Name.Contains(request.SearchByBuilding)
                                   where sc.Name.Contains(request.SearchByStoreCategory)
                                   select new StoreModel()
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Phone = store.Phone,
                                       BrandStoreId = b.Id,
                                       BrandStoreName = b.Name,
                                       BuildingId = building.Id,
                                       BuildingStore = building.Name,
                                       StoreCateId = sc.Id,
                                       StoreCateName = sc.Name,
                                       Status = store.Status,
                                       CreateAt = store.CreateAt,
                                       UpdateAt = store.UpdateAt

                                   }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach(var store in listStore)
            {
                store.Account = await GetAccountInStore(store.Id);
            }    
            return listStore;
        }
        public async Task<SystemReportModelInStore> GetListOrdersReport(string storeId ,DateFilterRequest request)
        {
            var lstOrder = await (from orderr in context.Orders
                                  join h in context.OrderActionHistories on orderr.Id equals h.OrderId
                                  join s in context.Stores on orderr.StoreId equals s.Id
                                  where s.Id == storeId && h.ToStatus == 0 && h.CreateDate.ToString().Contains(request.DateFilter) 
                                  select orderr).ToListAsync();

            SystemReportModelInStore report = new SystemReportModelInStore()
            {
                TotalOrderNew = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Received).Count(), //don hang moi
                TotalOrderUnpaidVNpay = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.New).Count(),//don hang chua thanh toan vnpay
                TotalOrderCancel = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)FailStatus.CustomerFail
                                                    || order.Status == (int)FailStatus.OutTime || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail).Count(),//don hang chua thanh toan vnpay
                TotalOrderCompleted = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Completed).Count(), //don hang thanh cong
                TotalOrder = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.New
                                                    || order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)FailStatus.CustomerFail
                                                    || order.Status == (int)FailStatus.OutTime || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail
                                                    || order.Status == (int)OrderStatusEnum.Completed
                                                  ).Count(), //tong don hang
            };
            return report;
        }
        public async Task<PriceReportModel> GetPriceOrdersReport(string storeId, DateFilterRequest request)
        {
            var lstOrder = await (from orderr in context.Orders
                                  join h in context.OrderActionHistories on orderr.Id equals h.OrderId
                                  join p in context.Payments on orderr.Id equals p.OrderId
                                  join s in context.Stores on orderr.StoreId equals s.Id
                                  where s.Id == storeId && h.ToStatus == 0 && h.CreateDate.ToString().Contains(request.DateFilter)
                                  select orderr).ToListAsync();
            var lstPayment = await (from pay in context.Payments
                                    join o in context.Orders on pay.OrderId equals o.Id
                                    join h in context.OrderActionHistories on o.Id equals h.OrderId
                                    join s in context.Stores on o.StoreId equals s.Id
                                    where s.Id == storeId && h.ToStatus == 0 && h.CreateDate.ToString().Contains(request.DateFilter)
                                    select pay)
                                    .ToListAsync();
            PriceReportModel report = new PriceReportModel()
            {
                //TotalOrder = lstOrder.Where(order => order.Status == (int)OrderStatusEnum.Completed).Select()
                TotalShipFree = (double)lstOrder.Sum(o => o.ShipCost), // tổng tiền ship
                TotalSurcharge = lstOrder.Where(x => x.ServiceId == "1").Count() * 10000, // tổng tiền service
                TotalPaymentVNPay = (double)lstPayment.Where(p => p.Type == (int)PaymentEnum.VNPay).Sum(o => o.Amount),// tổng tiền thanh toán VnPay
                TotalPaymentCash = (double)lstPayment.Where(p => p.Type == (int)PaymentEnum.Cash).Sum(o => o.Amount),// tổng tiền thanh toán Cash
                TotalOrder = (double)lstOrder.Sum(o => o.Total), // tổng tiền order
                TotalRevenueOrder = (double)lstOrder.Sum(o => o.ShipCost) + lstOrder.Where(x => x.ServiceId == "1").Count() * 10000 + (double)lstOrder.Sum(o => o.Total), // Doanh thu
                TotalProfitOrder = (double)lstOrder.Sum(o => o.ShipCost) + lstOrder.Where(x => x.ServiceId == "1").Count() * 10000, // Lợi nhuận
            };
            return report;
        }
        public async Task<IEnumerable<StoreModel>> GetListStoreInBrand(string brandName, int pageIndex, int pageSize)
        {
            var listStore = await (from store in context.Stores
                                   join b in context.Brands on store.BrandId equals b.Id
                                   join building in context.Buildings on store.BuildingId equals building.Id
                                   join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
                                   where b.Name.Contains(brandName)
                                   select new StoreModel()
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Phone = store.Phone,
                                       BrandStoreId = b.Id,
                                       BrandStoreName = b.Name,
                                       BuildingId = building.Id,
                                       BuildingStore = building.Name,
                                       StoreCateId = sc.Id,
                                       StoreCateName = sc.Name,
                                       Status = store.Status,
                                       CreateAt= store.CreateAt,
                                       UpdateAt= store.UpdateAt

                                   }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listStore;
        }
        public async Task<IEnumerable<StoreModel>> GetListStoreByName(string storeName, int pageIndex, int pageSize)
        {
            var listStore = await (from store in context.Stores.Where(store => store.Name.Contains(storeName))
                                   join b in context.Brands on store.BrandId equals b.Id
                                   join building in context.Buildings on store.BuildingId equals building.Id
                                   join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
                                   select new StoreModel()
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Phone = store.Phone,
                                       BrandStoreId = b.Id,
                                       BrandStoreName = b.Name,
                                       BuildingId = building.Id,
                                       BuildingStore = building.Name,
                                       StoreCateId = sc.Id,
                                       StoreCateName = sc.Name,
                                       Status = store.Status,
                                       CreateAt = store.CreateAt,
                                       UpdateAt = store.UpdateAt

                                   }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var store in listStore)
            {
                store.Account = await GetAccountInStore(store.Id);
            }
            return listStore;
        }
        public async Task<List<OrderAdminDtoInStore>> GetListOrderDeliveringByStore(string StoreId, int pageIndex, int pageSize)
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
                                  where s.Id == StoreId && h.ToStatus == 0  && (order.Status == (int)OrderStatusEnum.InProcess || order.Status == (int)InProcessStatus.HubDelivery || order.Status == (int)InProcessStatus.AtHub || order.Status == (int)InProcessStatus.CustomerDelivery)
                                  select new OrderAdminDtoInStore()
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
                                      TimeDuration = dt.Id,
                                      ToHour = TimeSpan.FromHours((double)dt.ToHour).ToString(@"hh\:mm"),
                                      FromHour = TimeSpan.FromHours((double)dt.FromHour).ToString(@"hh\:mm"),
                                      Time = h.CreateDate,
                                      Dayfilter = m.DayFilter.ToString()

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var or in lstOrder)
            {
                var countpro = context.OrderDetails.Where(o => o.OrderId == or.Id).Count();
                or.CountProduct = countpro.ToString();

            }
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
                                         }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                order.ListShipper = listShipper;
            }
            return lstOrder;
        }
        public async Task<List<OrderAdminDtoInStore>> GetListOrderCompletedByStore(string StoreId, int pageIndex, int pageSize)
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
                                  where s.Id == StoreId && h.ToStatus == 0 && (order.Status == (int)OrderStatusEnum.Completed || order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)FailStatus.CustomerFail || order.Status == (int)FailStatus.OutTime || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail)
                                  select new OrderAdminDtoInStore()
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
                                      TimeDuration = dt.Id,
                                      ToHour = TimeSpan.FromHours((double)dt.ToHour).ToString(@"hh\:mm"),
                                      FromHour = TimeSpan.FromHours((double)dt.FromHour).ToString(@"hh\:mm"),
                                      Time = h.CreateDate,
                                      Dayfilter = m.DayFilter.ToString()

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var or in lstOrder)
            {
                var countpro = context.OrderDetails.Where(o => o.OrderId == or.Id).Count();
                or.CountProduct = countpro.ToString();

            }
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
                                         }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                order.ListShipper = listShipper;
            }
            return lstOrder;
        }
        public async Task<List<OrderAdminDtoInStore>> GetListOrderByStoreByModeId(string StoreId,string modeId, DateFilterRequest request, int pageIndex, int pageSize)
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
                                  where s.Id == StoreId && modeId == m.SaleMode && h.ToStatus == 0
                                  && (order.Status == (int)OrderStatusEnum.New || order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.Assigning || order.Status == (int)OrderStatusEnum.Accepted)
                                  where h.CreateDate.ToString().Contains(request.DateFilter)
                                  select new OrderAdminDtoInStore()
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
                                      TimeDuration = dt.Id,
                                      ToHour = TimeSpan.FromHours((double)dt.ToHour).ToString(@"hh\:mm"),
                                      FromHour = TimeSpan.FromHours((double)dt.FromHour).ToString(@"hh\:mm"),
                                      //ShipperName = sp.FullName,
                                      Time = h.CreateDate,
                                      Dayfilter = m.DayFilter.ToString()

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var or in lstOrder)
            {
                var countpro = context.OrderDetails.Where(o => o.OrderId == or.Id).Count();
                or.CountProduct = countpro.ToString();

            }
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
                                         }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                order.ListShipper = listShipper;
            }
            return lstOrder;
        }
        public async Task<List<OrderAdminDtoInStore>> GetListOrderPreparingsByStore(string StoreId, int pageIndex, int pageSize)
        {
            var lstOrder = await (from order in context.Orders
                                  join s in context.Stores on order.StoreId equals s.Id
                                  //join c in context.Customers on order.CustomerId equals c.Id
                                  join h in context.OrderActionHistories on order.Id equals h.OrderId
                                  join b in context.Buildings on order.BuildingId equals b.Id
                                  join p in context.Payments on order.Id equals p.OrderId
                                  join m in context.Menus on order.MenuId equals m.Id
                                  join dt in context.DeliveryTimeFrames on order.DeliveryTimeId equals dt.Id
                                  //join sp in context.Shippers on order.ShipperId equals sp.Id
                                  where s.Id == StoreId && h.ToStatus == 0 && (order.Status == (int)OrderStatusEnum.New || order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.Assigning || order.Status == (int)OrderStatusEnum.Accepted) 
                                  
                                  select new OrderAdminDtoInStore()
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
                                      TimeDuration = dt.Id,
                                      ToHour = TimeSpan.FromHours((double)dt.ToHour).ToString(@"hh\:mm"),
                                      FromHour = TimeSpan.FromHours((double)dt.FromHour).ToString(@"hh\:mm"),
                                      Time = h.CreateDate,
                                      Dayfilter = m.DayFilter.ToString()

                                  }
                                  ).OrderByDescending(t => t.Time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var or in lstOrder)
            {
                var countpro = context.OrderDetails.Where(o => o.OrderId == or.Id).Count();
                or.CountProduct = countpro.ToString();
                
    }
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

                                         }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                order.ListShipper = listShipper;
            }
            return lstOrder;
        }
        public async Task<Object> GetStoreById(string storeId)
        {
            var store = await (from s in context.Stores
                               join b in context.Brands on s.BrandId equals b.Id
                               join sc in context.StoreCategories on s.StoreCategoryId equals sc.Id
                               join bs in context.Buildings on s.BuildingId equals bs.Id
                               where s.Id == storeId 
                               select new ViewListStoreModel()
                               {
                                   Id = s.Id,
                                   Name = s.Name,
                                   Phone = s.Phone,
                                   Image = s.Image,
                                   CloseTime = s.CloseTime,
                                   OpenTime = s.OpenTime,
                                   Slogan = s.Slogan,
                                   Description = s.Description,
                                   BrandId = b.Id,
                                   BuildingId = bs.Id,
                                   StoreCategoryId = sc.Id,
                                   Status = s.Status,
                                   CreateAt = s.CreateAt,
                                   UpdateAt = s.UpdateAt,
                               }).FirstOrDefaultAsync();
            store.Account = await GetAccountInStore(storeId);
            return store;
        }
        public async Task<StoreDto> CreatNewStore(StoreDto store)
        {
            string fileImg = "ImagesStores";
            string time = await _timeStageService.GetTime();
            var categoryStore = context.StoreCategories.FirstOrDefault(sc => sc.Id == store.StoreCategoryId);
            var brand = context.Brands.FirstOrDefault(b => b.Id == store.BrandId);
            var building = context.Buildings.FirstOrDefault(bs => bs.Id == store.BuildingId);
            context.Stores.Add(
                new Store
                {
                    Id = store.Id,
                    Name = store.Name,
                    Phone = store.Phone,
                    Slogan = store.Slogan,
                    Description = store.Description,
                    Image = await _fileService.UploadFile(fileImg, store.Image),
                    Rate = store.Rate,
                    OpenTime = store.OpenTime,
                    CloseTime = store.CloseTime,
                    StoreCategoryId = categoryStore.Id,
                    BrandId = brand.Id,
                    BuildingId = building.Id,
                    Status = true,
                    CreateAt = time
                });
            context.Accounts.Add(
                new Account
                {
                    Id = store.Id,
                    Name = store.Name,
                    Password = store.Password,
                    RoleId = "2",
                    Status = "true"
                });
            await context.SaveChangesAsync();
            return store;

        }
        public async Task<Object> DeleteStore(string storeId)
        {
            var deStore = await context.Stores.FindAsync(storeId);
            context.Stores.Remove(deStore);
            var account = await context.Accounts.FindAsync(storeId);
            context.Accounts.Remove(account);
            await context.SaveChangesAsync();

            return deStore;
        }
        public async Task<AccountInRole> GetAccountInStore(string storeId)
        {
            var account = await context.Accounts.Where(x => x.Id == storeId)
                                    .Select(x => new AccountInRole
                                    {
                                        Password = x.Password,
                                    }).FirstOrDefaultAsync();
            return account;
        }

        public async Task<StoreDto> UpdateStore(string storeId, StoreDto store , Boolean imgUpdate)
        {
            string fileImg = "ImagesStores";
            string time = await _timeStageService.GetTime();
            var result = await context.Stores.FindAsync(storeId);
            var account = context.Accounts.FirstOrDefault(x => x.Id == storeId);

            result.Id = store.Id;
            result.Name = store.Name;
            result.Rate = store.Rate;
            result.BrandId = store.BrandId;
            result.BuildingId = store.BuildingId;
            result.StoreCategoryId = store.StoreCategoryId;
            if (imgUpdate == true)
            {
                result.Image = await _fileService.UploadFile(fileImg, store.Image);
            }
            result.OpenTime = store.OpenTime;
            result.CloseTime = store.CloseTime;
            result.Phone = store.Phone;
            result.Slogan = store.Slogan;
            result.Description = store.Description;
            result.Status = store.Status;
            result.UpdateAt = time;
            account.Password = store.Password;

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return store;
        }
        public async Task<StatusStoreDto> UpdateStatusStore(string storeId, StatusStoreDto store)
        {
            var result = await context.Stores.FindAsync(storeId);
            var status = context.Orders.OrderByDescending(x => x.Id).FirstOrDefault(x => x.StoreId == storeId);
            if (status != null)
            {
                //var OrderStatus = context.OrderStatuses.FirstOrDefault(os => os.Id == status.Status);
                if (status.Status == (int)OrderStatusEnum.Fail || status.Status == (int)OrderStatusEnum.Completed)
                {
                    result.Status = store.Status;
                }
                if (status.Status == (int)OrderStatusEnum.New || status.Status == (int)OrderStatusEnum.Received || status.Status == (int)OrderStatusEnum.Assigning
                    || status.Status == (int)OrderStatusEnum.Accepted || status.Status == (int)OrderStatusEnum.InProcess)
                    throw new Exception("Hiện tại cửa hàng đang có đơn hàng chưa hoàn thành!!" +
                                                 "Vui lòng kiểm tra lại đơn hàng và thử lại");
            }
            if (status == null)
            { 
                result.Status = store.Status; 
            }
            
            try
            {
                context.Entry(result).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return store;
        }
    }
}

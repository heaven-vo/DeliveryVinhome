using DeliveryVHGP.Core.Const;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

namespace DeliveryVHGP.Infrastructure.Repositories
{
    public class ShipperHistoryRepository : RepositoryBase<ShipperHistory>, IShipperHistoryRepository
    {
        public ShipperHistoryRepository(DeliveryVHGP_DBContext context) : base(context)
        {
        }
        public async Task<List<ShipperHistoryModel>> GetShipperHistories(string shipperId, int status, int page, int pageSize)
        {
            List<ShipperHistoryModel> shipperHistoryModels = new List<ShipperHistoryModel>();
            var listHistory = await context.ShipperHistories.Where(x => x.ShipperId == shipperId && x.Status == status).OrderByDescending(x => x.CreateDate)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var history in listHistory)
            {
                var order = await context.Orders.Include(x => x.Menu).Include(x => x.Service).Where(x => x.Id == history.OrderId).FirstOrDefaultAsync();
                int routeType = 1;
                double profit = 0;
                if (order.Menu.SaleMode == "2" || order.Menu.SaleMode == "3")
                {
                    routeType = 2;
                }
                if (order.ServiceId == DeliveryService.FastService)
                {
                    profit = (double)order.ShipCost * ShipFee.ShipperCommission * 2;
                }
                if (order.ServiceId == DeliveryService.NormalService)
                {
                    profit = (double)order.ShipCost * ShipFee.ShipperCommission;
                }
                ShipperHistoryModel shipperHistoryModel = new ShipperHistoryModel()
                {
                    Id = history.Id,
                    ServiceName = order.Service.Name,
                    Total = order.Total,
                    ShippingCost = order.ShipCost,
                    RouteType = routeType,
                    ActionType = history.ActionType,
                    Profit = profit,
                    Date = history.CreateDate
                };
                shipperHistoryModels.Add(shipperHistoryModel);
            }
            return shipperHistoryModels;
        }
        public async Task<HistoryDetail> GetShipperHistoryDetail(string shipperHistoryId)
        {
            var history = await context.ShipperHistories.Include(x => x.Order).ThenInclude(x => x.Service)
                .Where(x => x.Id == shipperHistoryId).FirstOrDefaultAsync();
            if (history == null)
            {
                throw new Exception("Not found historyId");
            }
            HistoryDetail detail = new HistoryDetail()
            {
                OrderId = history.OrderId,
                Phone = history.Order.PhoneNumber,
                Note = history.Order.Note,
                messageFail = history.Order.MessageFail,
                ServiceName = history.Order.Service.Name,
                Total = history.Order.Total,
                ShipCost = history.Order.ShipCost
            };
            detail.PaymentType = context.Payments.Where(x => x.OrderId == history.OrderId).Select(x => x.Type).FirstOrDefault();
            detail.OrderDetails = await context.OrderDetails.Where(x => x.OrderId == history.OrderId).Select(x => new OrderDetailActionModel
            {
                ProductName = x.ProductName,
                Quantity = int.Parse(x.Quantity),
                Price = x.Price

            }).ToListAsync();
            if (history.ActionType == (int)OrderActionEnum.DeliveryHub || history.ActionType == (int)OrderActionEnum.PickupStore)
            {
                if (history.Order.ServiceId == "1")
                {
                    var segment = await context.Segments.Where(x => x.OrderId == history.OrderId).FirstOrDefaultAsync();
                    var store = await context.Stores.Include(x => x.Building).Where(x => x.Id == history.Order.StoreId).FirstOrDefaultAsync();
                    detail.Start = store.Name;
                    detail.End = history.Order.FullName;
                    detail.StartBuilding = store.Building.Name;
                    detail.EndBuilding = await context.Buildings.Where(x => x.Id == history.Order.BuildingId).Select(x => x.Name).FirstOrDefaultAsync();
                }
                if (history.Order.ServiceId == "2")
                {
                    var segment = await context.Segments.Where(x => x.OrderId == history.OrderId && x.SegmentMode == (int)SegmentModeEnum.StoreToHub).FirstOrDefaultAsync();
                    var store = await context.Stores.Include(x => x.Building).Where(x => x.Id == history.Order.StoreId).FirstOrDefaultAsync();
                    var hub = await context.Hubs.FindAsync(segment.HubId);

                    detail.Start = store.Name;
                    detail.End = hub.Name;
                    detail.StartBuilding = store.Building.Name;
                    detail.EndBuilding = await context.Buildings.Where(x => x.Id == hub.BuildingId).Select(x => x.Name).FirstOrDefaultAsync();
                }

            }
            if (history.ActionType == (int)OrderActionEnum.DeliveryCus || history.ActionType == (int)OrderActionEnum.PickupHub)
            {
                if (history.Order.ServiceId == "1")
                {
                    var segment = await context.Segments.Where(x => x.OrderId == history.OrderId).FirstOrDefaultAsync();
                    var store = await context.Stores.Include(x => x.Building).Where(x => x.Id == history.Order.StoreId).FirstOrDefaultAsync();
                    detail.Start = store.Name;
                    detail.End = history.Order.FullName;
                    detail.StartBuilding = store.Building.Name;
                    detail.EndBuilding = await context.Buildings.Where(x => x.Id == history.Order.BuildingId).Select(x => x.Name).FirstOrDefaultAsync();
                }
                if (history.Order.ServiceId == "2")
                {
                    var segment = await context.Segments.Where(x => x.OrderId == history.OrderId && x.SegmentMode == (int)SegmentModeEnum.HubToCus).FirstOrDefaultAsync();
                    var hub = await context.Hubs.FindAsync(segment.HubId);
                    detail.Start = hub.Name;
                    detail.End = history.Order.FullName;
                    detail.StartBuilding = await context.Buildings.Where(x => x.Id == hub.BuildingId).Select(x => x.Name).FirstOrDefaultAsync();
                    detail.EndBuilding = await context.Buildings.Where(x => x.Id == history.Order.BuildingId).Select(x => x.Name).FirstOrDefaultAsync();
                }
            }
            return detail;
        }
        public async Task<ShipperReportModel> GetShipperReport(string shipperId, DateFilterRequest request, MonthFilterRequest monthFilter)
        {
            ShipperReportModel report = new ShipperReportModel() { total = 0, success = 0, canceled = 0, customerFail = 0 };
            if (request.DateFilter != "")
            {
                DateTime dateTime = DateTime.Parse(request.DateFilter);
                var nextDay = dateTime.AddDays(1);
                var history = await context.ShipperHistories.Include(x => x.Order).Where(x => x.ShipperId == shipperId
                && x.CreateDate > dateTime && x.CreateDate < nextDay).ToListAsync();
                if (!history.Any())
                {
                    return report;
                }
                report.total = history.Count();
                report.success = history.Where(x => x.Status == (int)StatusEnum.success).Count();
                report.canceled = history.Where(x => x.Status == (int)StatusEnum.fail
                    && (x.Order.Status == (int)FailStatus.StoreFail || x.Order.Status == (int)FailStatus.ShipperFail)
                    ).Count();
                report.customerFail = history.Where(x => x.Status == (int)StatusEnum.fail && x.Order.Status == (int)FailStatus.CustomerFail).Count();
                return report;
            }
            else if (monthFilter.Month != 0)
            {
                var history = await context.ShipperHistories.Include(x => x.Order).Where(x => x.ShipperId == shipperId
                && x.CreateDate.Value.Year == monthFilter.Year && x.CreateDate.Value.Month == monthFilter.Month).ToListAsync();
                if (!history.Any())
                {
                    return report;
                }
                report.total = history.Count();
                report.success = history.Where(x => x.Status == (int)StatusEnum.success).Count();
                report.canceled = history.Where(x => x.Status == (int)StatusEnum.fail
                    && (x.Order.Status == (int)FailStatus.StoreFail || x.Order.Status == (int)FailStatus.ShipperFail)
                    ).Count();
                report.customerFail = history.Where(x => x.Status == (int)StatusEnum.fail && x.Order.Status == (int)FailStatus.CustomerFail).Count();
                return report;
            }
            return null;
        }
        public async Task<DeliveryShipperReportModel> GetDeliveryAllShipperReport(DateFilterRequest request, MonthFilterRequest monthFilter, int page, int pageSize)
        {
            DeliveryShipperReportModel report = new DeliveryShipperReportModel() { total = 0, success = 0, cancel = 0 };
            List<ShipperHistory> history = new List<ShipperHistory>();
            if (request.DateFilter != "")
            {
                DateTime dateTime = DateTime.Parse(request.DateFilter);
                var nextDay = dateTime.AddDays(1);
                history = await context.ShipperHistories.Include(x => x.Order).Where(x => x.CreateDate > dateTime && x.CreateDate < nextDay).ToListAsync();
                if (history.Any())
                {
                    report.total = history.Count();
                    report.success = history.Where(x => x.Status == (int)StatusEnum.success).Count();
                    report.cancel = history.Where(x => x.Status == (int)StatusEnum.fail).Count();
                }
            }
            else if (monthFilter.Month != 0)
            {
                history = await context.ShipperHistories.Include(x => x.Order).Where(x => x.CreateDate.Value.Year == monthFilter.Year && x.CreateDate.Value.Month == monthFilter.Month).ToListAsync();
                if (history.Any())
                {
                    report.total = history.Count();
                    report.success = history.Where(x => x.Status == (int)StatusEnum.success).Count();
                    report.cancel = history.Where(x => x.Status == (int)StatusEnum.fail).Count();
                }
            }
            //
            List<ShipperInReport> shipperInReports = new List<ShipperInReport>();
            var listShipper = await context.Shippers.OrderBy(x => x.FullName).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (listShipper.Any())
            {
                foreach (var shipper in listShipper)
                {
                    ShipperInReport shipperInReport = new ShipperInReport() { fullname = shipper.FullName, phone = shipper.Phone, distance = 0 };
                    shipperInReport.totalOrder = history.Where(x => x.ShipperId == shipper.Id).Count();
                    shipperInReport.successfulOrder = history.Where(x => x.ShipperId == shipper.Id && x.Status == (int)StatusEnum.success).Count();
                    shipperInReport.canceledOrder = history.Where(x => x.ShipperId == shipper.Id && x.Status == (int)StatusEnum.fail).Count();
                    shipperInReport.refundBalance = await context.Wallets.Where(x => x.AccountId == shipper.Id && x.Type == (int)WalletTypeEnum.Refund && x.Active == true).Select(x => x.Amount).FirstOrDefaultAsync();
                    shipperInReport.debitBalance = await context.Wallets.Where(x => x.AccountId == shipper.Id && x.Type == (int)WalletTypeEnum.Debit && x.Active == true).Select(x => x.Amount).FirstOrDefaultAsync();
                    var distance = await (from his in context.ShipperHistories
                                          join order in context.Orders on his.OrderId equals order.Id
                                          join orderAction in context.OrderActions on order.Id equals orderAction.OrderId
                                          join edge in context.RouteEdges on orderAction.RouteEdgeId equals edge.Id
                                          where his.ShipperId == shipper.Id && his.Status == (int)StatusEnum.success
                                          && his.ActionType == orderAction.OrderActionType
                                          select edge.Distance).FirstOrDefaultAsync();
                    if (distance != null)
                    {
                        shipperInReport.distance = distance;
                    }
                    shipperInReports.Add(shipperInReport);
                }
                report.shipperInReports = shipperInReports;
            }
            return report;
        }
    }
}

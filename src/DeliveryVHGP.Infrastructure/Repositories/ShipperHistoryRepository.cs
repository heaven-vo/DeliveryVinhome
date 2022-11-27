using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

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
            var listHistory = await context.ShipperHistories.Where(x => x.ShipperId == shipperId && x.Status == status)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var history in listHistory)
            {
                var order = await context.Orders.Include(x => x.Service).Where(x => x.Id == history.OrderId).FirstOrDefaultAsync();
                ShipperHistoryModel shipperHistoryModel = new ShipperHistoryModel()
                {
                    Id = history.Id,
                    ServiceName = order.Service.Name,
                    Total = order.Total,
                    ShippingCost = order.ShipCost,
                    ActionType = history.ActionType,
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
            if (history.ActionType == (int)OrderActionEnum.DeliveryHub)
            {
                var segment = await context.Segments.Where(x => x.OrderId == history.OrderId && x.SegmentMode == (int)SegmentModeEnum.StoreToHub).FirstOrDefaultAsync();
                var store = await context.Stores.Include(x => x.Building).Where(x => x.Id == history.Order.StoreId).FirstOrDefaultAsync();
                var hub = await context.Hubs.FindAsync(segment.HubId);

                detail.Start = store.Name;
                detail.End = hub.Name;
                detail.StartBuilding = store.Building.Name;
                detail.EndBuilding = await context.Buildings.Where(x => x.Id == hub.BuildingId).Select(x => x.Name).FirstOrDefaultAsync();

            }
            if (history.ActionType == (int)OrderActionEnum.DeliveryCus)
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
    }
}

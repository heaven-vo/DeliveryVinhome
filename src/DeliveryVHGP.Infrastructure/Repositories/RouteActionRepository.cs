using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Core.Models.Noti;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using DeliveryVHGP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.Infrastructure.Repositories
{
    public class RouteActionRepository : RepositoryBase<SegmentDeliveryRoute>, IRouteActionRepository
    {
        private readonly IFirestoreService firestoreService;
        private readonly INotificationService _notificationService;
        public RouteActionRepository(DeliveryVHGP_DBContext context, IFirestoreService firestoreService, INotificationService notificationService) : base(context)
        {
            this.firestoreService = firestoreService;
            _notificationService = notificationService;
        }
        public async Task<List<RouteModel>> GetCurrentAvalableRoute()
        {
            List<RouteModel> listRouteModel = new List<RouteModel>();
            var listRoute = await context.SegmentDeliveryRoutes.Include(x => x.RouteEdges).ThenInclude(r => r.OrderActions)
                .Where(x => x.Status == (int)RouteStatusEnum.NotAssign || x.Status == (int)RouteStatusEnum.ToDo).ToListAsync();
            if (listRoute.Count() > 0)
            {
                foreach (var route in listRoute)
                {
                    RouteModel routeModel = new RouteModel() { RouteId = route.Id, EdgeNum = route.RouteEdges.Count(), ShipperId = route.ShipperId, Status = route.Status };
                    var first = route.RouteEdges.Where(x => x.Priority == 1).Select(x => x.ToBuildingId).FirstOrDefault();
                    var last = route.RouteEdges.OrderByDescending(x => x.Priority).Select(x => x.ToBuildingId).FirstOrDefault();
                    var buildingNameFirst = await context.Buildings.Where(x => x.Id == first).Select(x => x.Name).FirstOrDefaultAsync();
                    var buildingNameLast = await context.Buildings.Where(x => x.Id == last).Select(x => x.Name).FirstOrDefaultAsync();

                    routeModel.FirstEdge = buildingNameFirst;
                    routeModel.LastEdge = buildingNameLast;
                    List<OrderAction> orderActions = new List<OrderAction>();
                    foreach (var edge in route.RouteEdges)
                    {
                        orderActions.AddRange(edge.OrderActions);
                    }
                    var listOrderAction = orderActions.GroupBy(x => x.OrderId).Select(x => x.First()).ToList();
                    var listOrderId = listOrderAction.Select(x => x.OrderId).ToList();
                    routeModel.OrderNum = listOrderAction.Count;
                    var listOrder = await context.Orders.Include(x => x.Payments).Where(x => listOrderId.Contains(x.Id)).ToListAsync();

                    double? total = 0;
                    double? totalCod = 0;
                    foreach (var order in listOrder)
                    {
                        if (order.Payments.Any())
                        {
                            Console.WriteLine(order.Id + ": " + order.Payments.First().Type);
                        }
                        //total += await context.Orders.Where(x => x.Id == order.OrderId).Select(x => x.Total).FirstOrDefaultAsync();
                        total += order.Total;
                        if (order.Payments.First().Type == (int)PaymentEnum.Cash)
                        {
                            totalCod += order.Total;
                        }
                    }
                    routeModel.TotalBill = total;
                    routeModel.TotalCod = totalCod;

                    listRouteModel.Add(routeModel);
                }
            }
            return listRouteModel;
        }
        public async Task<int> CreateRoute(List<SegmentDeliveryRoute> route, List<SegmentModel> listSegments)
        {
            try
            {
                await context.AddRangeAsync(route);
                await context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public async Task CreateActionOrder(List<NodeModel> listNode, List<SegmentModel> listSegments)
        {
            List<OrderAction> listAction = new List<OrderAction>();
            foreach (var node in listNode)
            {
                foreach (var segment in listSegments)// from: pickup, to: delivery
                {
                    OrderAction action = new OrderAction() { Id = Guid.NewGuid().ToString(), RouteEdgeId = node.EdgeId, OrderId = segment.OrderId, Status = (int)OrderActionStatusEnum.Todo };

                    //----------------------------------------------
                    if (node.Type == (int)EdgeTypeEnum.Pickup)
                    {
                        if (segment.fromBuilding.Equals(node.ToBuildingId) && (segment.SegmentMode == (int)SegmentModeEnum.StoreToHub || segment.SegmentMode == (int)SegmentModeEnum.StoreToCus))
                        {
                            //pickup store
                            action.OrderActionType = (int)OrderActionEnum.PickupStore;
                            listAction.Add(action);
                        }
                        if (segment.fromBuilding.Equals(node.ToBuildingId) && segment.SegmentMode == (int)SegmentModeEnum.HubToCus)
                        {
                            //pickup hub
                            action.OrderActionType = (int)OrderActionEnum.PickupHub;
                            listAction.Add(action);
                        }
                    }
                    if (node.Type == (int)EdgeTypeEnum.Delivery)
                    {
                        if (segment.toBuilding.Equals(node.ToBuildingId) && segment.SegmentMode == (int)SegmentModeEnum.StoreToHub)
                        {
                            //delivery hub
                            action.OrderActionType = (int)OrderActionEnum.DeliveryHub;
                            listAction.Add(action);
                        }
                        if (segment.toBuilding.Equals(node.ToBuildingId) && (segment.SegmentMode == (int)SegmentModeEnum.HubToCus || segment.SegmentMode == (int)SegmentModeEnum.StoreToCus))
                        {
                            //delivery customer
                            action.OrderActionType = (int)OrderActionEnum.DeliveryCus;
                            listAction.Add(action);
                        }
                    }
                }
            }
            if (listAction.Count > 0)
            {
                await context.AddRangeAsync(listAction);
                await context.SaveChangesAsync();
            }
        }
        public async Task RemoveRouteActionNotShipper()
        {
            List<RouteEdge> listEdge = new List<RouteEdge>();
            List<OrderAction> listAction = new List<OrderAction>();
            var listRouteAction = await context.SegmentDeliveryRoutes.Include(x => x.RouteEdges).ThenInclude(r => r.OrderActions)
                .Where(x => x.Status == (int)RouteStatusEnum.NotAssign).ToListAsync();
            if (listRouteAction.Count > 0)
            {
                foreach (var route in listRouteAction)
                {
                    listEdge.AddRange(route.RouteEdges.ToList());
                }
                foreach (var edge in listEdge)
                {
                    listAction.AddRange(edge.OrderActions.ToList());
                }
                context.RemoveRange(listRouteAction);
                context.RemoveRange(listEdge);
                context.RemoveRange(listAction);
                await context.SaveChangesAsync();
            }
        }
        public async Task RemoveAllRouteAction()// to test
        {
            List<RouteEdge> listEdge = new List<RouteEdge>();
            List<OrderAction> listAction = new List<OrderAction>();
            var listRouteAction = await context.SegmentDeliveryRoutes.Include(x => x.RouteEdges).ThenInclude(r => r.OrderActions)
                .ToListAsync();
            if (listRouteAction.Count > 0)
            {
                foreach (var route in listRouteAction)
                {
                    listEdge.AddRange(route.RouteEdges.ToList());
                }
                foreach (var edge in listEdge)
                {
                    listAction.AddRange(edge.OrderActions.ToList());
                }
                context.RemoveRange(listRouteAction);
                context.RemoveRange(listEdge);
                context.RemoveRange(listAction);
                await context.SaveChangesAsync();
            }
        }
        public async Task AcceptRouteByShipper(string routeId, string shipperId)
        {
            var routeTodo = await context.SegmentDeliveryRoutes.Where(x => x.ShipperId == shipperId && x.Status == (int)RouteStatusEnum.ToDo).FirstOrDefaultAsync();
            if (routeTodo != null)
            {
                throw new Exception("Shipper can only accept 1 route ");
            }
            var route = await context.SegmentDeliveryRoutes.Include(x => x.RouteEdges).ThenInclude(r => r.OrderActions)
                .Where(x => x.Id == routeId && x.ShipperId == null && x.Status == (int)RouteStatusEnum.NotAssign).FirstOrDefaultAsync();
            if (route == null)
            {
                throw new Exception("The route is not avalable");
            }
            route.ShipperId = shipperId;
            route.Status = (int)RouteStatusEnum.ToDo;
            List<OrderAction> orderActions = new List<OrderAction>();
            foreach (var edge in route.RouteEdges)
            {
                orderActions.AddRange(edge.OrderActions);
                if (edge.Priority == 1)
                {
                    edge.Status = (int)EdgeStatusEnum.ToDo;
                }
            }
            var listOrderAction = orderActions.GroupBy(x => x.OrderId).Select(x => x.First()).ToList();
            var listOrderId = listOrderAction.Select(x => x.OrderId).ToList();

            var listOrder = await context.Orders.Where(x => listOrderId.Contains(x.Id)).ToListAsync();
            var listOrderCache = await context.OrderCaches.Where(x => listOrderId.Contains(x.OrderId)).ToListAsync();
            listOrderCache.ForEach(x => x.IsReady = false);
            foreach (var order in listOrder)
            {
                if (order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.Assigning)
                {
                    order.Status = (int)OrderStatusEnum.Accepted;
                    var actionHistory = new OrderActionHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        OrderId = order.Id,
                        FromStatus = (int)OrderStatusEnum.Assigning,
                        ToStatus = (int)OrderStatusEnum.Accepted,
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        TypeId = "1"
                    };
                    await context.OrderActionHistories.AddAsync(actionHistory);
                }
                //if (order.Status == (int)InProcessStatus.AtHub)
                //{
                //    order.Status = (int)InProcessStatus.CustomerDelivery;
                //    var actionHistory = new OrderActionHistory()
                //    {
                //        Id = Guid.NewGuid().ToString(),
                //        OrderId = order.Id,
                //        FromStatus = (int)InProcessStatus.AtHub,
                //        ToStatus = (int)InProcessStatus.CustomerDelivery,
                //        CreateDate = DateTime.UtcNow.AddHours(7),
                //        TypeId = "1"
                //    };
                //    await context.OrderActionHistories.AddAsync(actionHistory);
                //}
            }
            await context.SaveChangesAsync();

            var listStore = listOrder.GroupBy(x => x.StoreId).Select(x => x.First()).ToList();
            var listStoreId = listStore.Where(x => x.Status == (int)OrderStatusEnum.Accepted || x.Status == (int)OrderStatusEnum.Assigning).Select(x => x.StoreId).ToList();
            List<string> tokens = new List<string>();
            foreach (var id in listStoreId)
            {
                var user = await firestoreService.GetUserData(id);
                if (user.fcmToken != null)
                {
                    tokens.Add(user.fcmToken);
                }
            }
            NotificationModel notificationModel = new NotificationModel()
            {
                DeviceId = tokens,
                IsAndroiodDevice = true,
                Title = "Đơn mới",
                Body = "Bạn có đơn mới" +
                ", hãy vào app để kiểm tra "
            };
            await _notificationService.SendNotification(notificationModel);
            await firestoreService.UpdateRoute(routeId, new RouteUpdateModel { ShipperId = shipperId, Status = (int)RouteStatusEnum.ToDo });

        }
        public async Task<List<EdgeModel>> GetListEdgeInRoute(string routeId)
        {
            var listEdgeModel = new List<EdgeModel>();
            var listEdge = await context.RouteEdges.Include(x => x.OrderActions).Where(x => x.RouteId == routeId).ToListAsync();
            if (listEdge.Count == 0)
            {
                throw new Exception("Not found route");
            }
            foreach (var edge in listEdge)
            {
                var buildingName = await context.Buildings.Where(x => x.Id == edge.ToBuildingId).Select(x => x.Name).FirstOrDefaultAsync();
                var edgeModel = new EdgeModel()
                {
                    Id = edge.Id,
                    BuildingId = edge.ToBuildingId,
                    BuildingName = buildingName,
                    OrderNum = edge.OrderActions.Count,
                    Priority = edge.Priority,
                    Status = edge.Status
                };
                listEdgeModel.Add(edgeModel);
            }
            listEdgeModel = listEdgeModel.OrderBy(x => x.Priority).ToList();
            return listEdgeModel;
        }
        public async Task<EdgeModel> GetCurrentEdgeInRoute(string shipperId)
        {
            var edge = await context.RouteEdges.Include(x => x.Route).Include(x => x.OrderActions)
                .Where(x => x.Route.ShipperId == shipperId && x.Status == (int)EdgeStatusEnum.ToDo && x.Route.Status == (int)RouteStatusEnum.ToDo).FirstOrDefaultAsync();
            if (edge == null)
            {
                return null;
            }
            var buildingName = await context.Buildings.Where(x => x.Id == edge.ToBuildingId).Select(x => x.Name).FirstOrDefaultAsync();
            var edgeModel = new EdgeModel()
            {
                Id = edge.Id,
                BuildingId = edge.ToBuildingId,
                BuildingName = buildingName,
                OrderNum = edge.OrderActions.Count,
                Priority = edge.Priority,
                Status = edge.Status
            };
            return edgeModel;
        }
        public async Task<List<OrderActionModel>> GetListOrderAction(string edgeId)
        {
            var listAction = await context.OrderActions.Include(x => x.Order).ThenInclude(x => x.Payments).Where(x => x.RouteEdgeId == edgeId).ToListAsync();
            if (listAction.Count == 0)
            {
                Console.WriteLine("listAction null");
                throw new Exception("Not foud egde id");
            }
            List<OrderActionModel> listOrderActions = new List<OrderActionModel>();
            foreach (var action in listAction)
            {
                OrderActionModel orderActionModel = new OrderActionModel() { ActionId = action.Id, OrderId = action.OrderId, Note = action.Order.Note, PaymentType = action.Order.Payments.First().Type, ShipCost = action.Order.ShipCost, ActionType = action.OrderActionType, ActionStatus = action.Status };
                orderActionModel.Total = action.Order.Total;
                //if (orderActionModel.PaymentType == (int)PaymentEnum.Cash)
                //{
                //    orderActionModel.Total = action.Order.Total;
                //}
                if (orderActionModel.PaymentType == (int)PaymentEnum.VNPay && orderActionModel.ActionType == (int)OrderActionEnum.DeliveryCus)
                {
                    orderActionModel.Total = 0;
                }
                if (orderActionModel.ActionType == (int)OrderActionEnum.PickupStore)
                {
                    orderActionModel.Name = await context.Stores.Where(x => x.Id == action.Order.StoreId).Select(x => x.Name).FirstOrDefaultAsync();
                }
                if (orderActionModel.ActionType == (int)OrderActionEnum.PickupHub || orderActionModel.ActionType == (int)OrderActionEnum.DeliveryHub)
                {
#pragma warning disable CS8601 // Possible null reference assignment.
                    orderActionModel.Name = await context.Orders.Include(x => x.Store).ThenInclude(x => x.Building).ThenInclude(x => x.Hub)
                        .Where(x => x.Id == action.OrderId).Select(x => x.Store.Building.Hub.Name).FirstOrDefaultAsync();
#pragma warning restore CS8601 // Possible null reference assignment.
                }
                if (orderActionModel.ActionType == (int)OrderActionEnum.DeliveryCus)
                {
                    orderActionModel.Name = action.Order.FullName;
                    orderActionModel.Phone = action.Order.PhoneNumber;

                }
                orderActionModel.ServiceName = await context.Services.Where(x => x.Id == action.Order.ServiceId).Select(x => x.Name).FirstOrDefaultAsync();
                var orderDetails = await context.OrderDetails.Where(x => x.OrderId == action.OrderId)
                    .Select(x => new OrderDetailActionModel
                    {
                        Quantity = int.Parse(x.Quantity),
                        ProductName = x.ProductName,
                        Price = x.Price
                    }).ToListAsync();
                orderActionModel.OrderDetailActions = orderDetails;
                listOrderActions.Add(orderActionModel);
            }
            return listOrderActions;
        }
    }
}

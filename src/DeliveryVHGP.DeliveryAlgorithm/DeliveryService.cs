using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DeliveryVHGP.DeliveryAlgorithm
{
    public class DeliveryService : BackgroundService
    {
        private readonly ILogger<DeliveryService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DeliveryService(IServiceProvider serviceProvider, ILogger<DeliveryService> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Add order to segment
            //Load order in segment(cache in db), and run algorithm to create route

            //Load order thoa man dieu kien 
            //Add to segment with creatAt, updateAt (check mode, mode 2 check time, mode 3 check time and date; check payment, type vnpay check staus)
            //Load order from segment
            //Run algorithm
            try
            {
                while (!stoppingToken.IsCancellationRequested)//!stoppingToken.IsCancellationRequested
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var scopeRepo = scope.ServiceProvider.GetService<IRepositoryWrapper>();

                        //check and add new order to cache
                        //var listOrder = await scopeRepo.Order.CheckAvailableOrder();
                        //await scopeRepo.Cache.AddOrderToCache(listOrder); //change status -> assign(not do -> test)

                        //////load n order from cache -> segment -> run algorithm
                        //var listOrderDelivery = await scopeRepo.Cache.GetOrderFromCache(35);
                        //if (listOrderDelivery != null)
                        //{
                        //    //remove older route(do first)
                        //    await scopeRepo.RouteAction.RemoveRouteActionNotShipper();
                        //    var listSegment = await scopeRepo.Segment.GetSegmentAvaliable(listOrderDelivery);
                        //    if (listSegment.Any())
                        //    {
                        //        //_logger.LogInformation("LOGGING: " + listSegment[0].fromBuilding + " - " + listSegment[0].toBuilding);
                        //        DeliveryPickupAlgorithm algorithm = new DeliveryPickupAlgorithm(_serviceProvider);
                        //        algorithm.AlgorithsProcess(listSegment);
                        //    }
                        //}

                        //Remove route and load new route in firestore
                        var scopeFireStore = scope.ServiceProvider.GetService<IFirestoreService>();
                        await scopeFireStore.DeleteAllRoutes();
                        List<RouteModel> ListRoute = await scopeRepo.RouteAction.GetCurrentAvalableRoute();
                        if (ListRoute.Count > 0)
                            foreach (var routeModel in ListRoute)
                            {
                                await scopeFireStore.AddRoute(routeModel);
                            }
                        await Task.Delay(400000, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error: " + ex.Message);
                await Task.Delay(60000, stoppingToken).ConfigureAwait(false);
                await ExecuteAsync(stoppingToken);
            }

        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Worker STARTING");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Worker STOPPING: {time}", DateTimeOffset.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}

using DeliveryVHGP.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Load order thoa man dieu kien 
            //Add to segment with creatAt, updateAt
            //Load order from segment
            //Run algorithm

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    _logger.LogInformation("Worker running at: {time}", DateTime.Now);
                    _logger.LogInformation("Worker running at: {time}", DateTime.UtcNow.AddHours(7));
                    var scopeSev = scope.ServiceProvider.GetService<IRepositoryWrapper>();
                    await scopeSev.Account.CreateAcc();

                    await Task.Delay(3600000, stoppingToken);
                }

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

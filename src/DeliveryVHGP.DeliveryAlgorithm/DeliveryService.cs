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
            while (!stoppingToken.IsCancellationRequested)
            {
                //using (var scope = _serviceProvider.CreateScope())
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //    var scopeSev = scope.ServiceProvider.GetService<ITaskBack>();
                //    scopeSev.WriteTest("assssssssss");
                //await Task.Delay(5000, stoppingToken);
                //}                
                Console.WriteLine("Test backround service delay 250s");
                await Task.Delay(250000, stoppingToken);
            }
        }
    }
}

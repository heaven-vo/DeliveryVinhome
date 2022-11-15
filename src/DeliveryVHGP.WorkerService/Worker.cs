using DeliveryVHGP.Core.Interfaces;

namespace DeliveryVHGP.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var scopeSev = scope.ServiceProvider.GetService<IRepositoryWrapper>();
                    await scopeSev.Account.CreateAcc();
                    await Task.Delay(10000, stoppingToken);
                }

            }
        }
    }
}
using DeliveryVHGP.Core.Interfaces;

namespace DeliveryVHGP.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger) //IServiceProvider serviceProvider, 
        {
            _logger = logger;
            //_serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //using (var scope = _serviceProvider.CreateScope())
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //    var scopeSev = scope.ServiceProvider.GetService<IRepositoryWrapper>();
                //    await scopeSev.Account.CreateAcc();
                //    await Task.Delay(10000, stoppingToken);
                //}
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
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
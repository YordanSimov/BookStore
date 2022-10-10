using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProjectDK.BL.Background
{
    public class MyBackgroundService : IHostedService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
        }

        private async Task DoWork(CancellationToken stopToken = default)
        {
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(4));
            while (!stopToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stopToken))
            {
                _logger.LogInformation(DateTime.Now.ToString());
            }
            await Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Hello from {nameof(MyBackgroundService)}");
            DoWork();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("STOP");
            return Task.CompletedTask;
        }
    }
}

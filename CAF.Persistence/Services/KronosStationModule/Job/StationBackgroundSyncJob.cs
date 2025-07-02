using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CAF.Persistence.Services.KronosStationModule.Job;

public class StationBackgroundSyncJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StationBackgroundSyncJob> _logger;

    public StationBackgroundSyncJob(IServiceProvider serviceProvider, ILogger<StationBackgroundSyncJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var syncService = scope.ServiceProvider.GetRequiredService<StationSyncService>();

            try
            {
                _logger.LogInformation("Station sync started at: {Time}", DateTimeOffset.Now);
                await syncService.SyncStationsAsync();
                _logger.LogInformation("Station sync completed at: {Time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Station sync failed");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // 24 saatte bir sync
        }
    }
}

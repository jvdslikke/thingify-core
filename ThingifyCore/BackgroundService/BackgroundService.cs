using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; 

namespace ThingifyCore.BackgroundService;

public sealed class BackgroundService : IHostedService, IAsyncDisposable
{
    private readonly ILogger<BackgroundService> _logger;

    private readonly IThingsRepository _thingsRepository;

    public BackgroundService(
        ILogger<BackgroundService> logger,
        IThingsRepository thingsRepository)
    {
        _logger = logger;
        _thingsRepository = thingsRepository;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Service} is running.", nameof(BackgroundService));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{Service} is stopping.", nameof(BackgroundService));

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
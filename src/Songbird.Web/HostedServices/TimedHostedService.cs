using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Songbird.Web.HostedServices;

public abstract class TimedHostedService : IHostedService, IDisposable {
    private readonly TimeSpan _interval;
    private readonly TimeSpan _waitBeforeStart;
    private readonly ILogger<TimedHostedService> _logger;
    private Timer _timer;

    protected TimedHostedService(TimeSpan interval, TimeSpan waitBeforeStart, ILogger<TimedHostedService> logger) {
        _interval = interval;
        _waitBeforeStart = waitBeforeStart;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken) {
        _logger.LogInformation($"Starting a TimedService ({GetType().Name}) which will run in {_waitBeforeStart} and then every {_interval}.");

        _timer = new Timer(TimerCallback, stoppingToken, _waitBeforeStart, _interval);
        return Task.CompletedTask;
    }

    private async void TimerCallback(object state) {
        var cancellationToken = (CancellationToken)state;

        _logger.LogInformation($"Starting execution of TimedService ({GetType().Name}).");

        var stopwatch = Stopwatch.StartNew();
        await ExecuteAsync(cancellationToken);
        stopwatch.Stop();

        _logger.LogInformation($"Executed TimedService ({GetType().Name}) which took {stopwatch.Elapsed}.");
    }

    protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

    public Task StopAsync(CancellationToken stoppingToken) {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose() {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}

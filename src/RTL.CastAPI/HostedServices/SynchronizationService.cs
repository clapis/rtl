using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RTL.CastAPI.Application.Commands.SyncMetadata;
using RTL.CastAPI.Configuration;

namespace RTL.CastAPI.HostedServices
{
    internal class SynchronizationService : IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly SynchronizationSettings _settings;
        private readonly IServiceProvider _services;
        private readonly ILogger<SynchronizationService> _logger;

        public SynchronizationService(
            IServiceProvider services,
            ILogger<SynchronizationService> logger,
            IOptions<SynchronizationSettings> settings)
        {
            _logger = logger;
            _services = services;
            _settings = settings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Synchronization Service init ({nameof(_settings.IsEnabled)}: {_settings.IsEnabled}, {nameof(_settings.Interval)}: {_settings.Interval})");

            if (_settings.IsEnabled)
            {
                _timer = new Timer(Execute, null, TimeSpan.Zero, _settings.Interval);
            }

            return Task.CompletedTask;
        }

        private async void Execute(object state)
        {
            _logger.LogInformation($"Synchronization started.");

            try
            {
                using (var scope = _services.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var sw = Stopwatch.StartNew();

                    await mediator.Send(new SyncMetadataCommand());

                    sw.Stop();

                    _logger.LogInformation($"Synchronization completed. ({sw.ElapsedMilliseconds})ms");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Synchronization failed.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Synchronization Service shutting down.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

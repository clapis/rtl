using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.TvMaze
{
    /// <summary>
    /// Ensures a rate limit. Requests are serialized with an interval between calls.
    /// E.g. Max 20 calls every 10 seconds, means 1 call every 0.5 seconds 
    /// NB. For a more dynamic/reactive policy we could use Polly and handle
    /// HTTP Status Code 429 (Too Many Requests), but let's play it safe and stick to docs indication
    /// </summary>
    public class ThrottleHandler : DelegatingHandler
    {
        private DateTime _last;
        private readonly TimeSpan _interval;
        private readonly SemaphoreSlim _semaphore;

        public ThrottleHandler(IOptions<TvMazeSettings> settings)
        {
            _semaphore = new SemaphoreSlim(1);
            _interval = settings.Value.RateLimit;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync();

            var sinceLast = DateTime.UtcNow - _last;

            if (sinceLast < _interval)
                await Task.Delay(_interval - sinceLast);

            var response = await base.SendAsync(request, cancellationToken);

            _last = DateTime.UtcNow;

            _semaphore.Release();

            return response;
        }

    }
}

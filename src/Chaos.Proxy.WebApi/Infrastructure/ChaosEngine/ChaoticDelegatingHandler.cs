using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public class ChaoticDelegatingHandler : DelegatingHandler
    {
        private readonly IChance _chance;

        private readonly IChaosIntervalTimer _chaosTimer;

        private readonly IChaoticResponseFactory _chaoticResponseFactory;

        private readonly IHandlerSettings _handlerSettings;

        private readonly IRandomDelay _randomDelay;

        private readonly IResponseFiddler _responseFiddler;

        public ChaoticDelegatingHandler(IChance chance, IHandlerSettings handlerSettings,
            IChaoticResponseFactory responseFactory, IRandomDelay randomDelay, IChaosIntervalTimer chaosTimer,
            IResponseFiddler responseFiddler)
        {
            _chance = chance;
            _handlerSettings = handlerSettings;
            _randomDelay = randomDelay;
            _chaosTimer = chaosTimer;
            _responseFiddler = responseFiddler;
            _chaoticResponseFactory = responseFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            var delayedTime = 0;

            if (!_chaosTimer.TimeForChaos)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var currentChaosSettings = _handlerSettings.Current;

            if (currentChaosSettings == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            if (ShouldIgnoreRequest(request, currentChaosSettings))
            {
                var ignoredPattern = currentChaosSettings.IgnoreUrlPattern.FirstOrDefault(ignoredUrl =>
                    request.RequestUri.AbsoluteUri.Contains(ignoredUrl));

                return await base.SendAsync(request, cancellationToken).ContinueWith(
                    t => CreateIgnoredUrlMessageResponseHeaders(t, ignoredPattern, currentChaosSettings),
                    cancellationToken);
            }

            if (!_chance.Indicated(currentChaosSettings.PercentageOfChaos))
            {
                return await base.SendAsync(request, cancellationToken).ContinueWith(
                    t => CreateChanceMissMessageResponseHeaders(t, currentChaosSettings), cancellationToken);
            }

            if (_chance.Indicated(currentChaosSettings.PercentageOfSlowResponses))
            {
                delayedTime = await _randomDelay.DelayFor(currentChaosSettings.MinResponseDelayTime, currentChaosSettings.MaxResponseDelayTime);
            }

            if (currentChaosSettings.HttpResponses.Count == 0)
                response = await base.SendAsync(request, cancellationToken);
            else
                response = _chaoticResponseFactory.Build(request, currentChaosSettings);

            if (currentChaosSettings.ResponseFiddles.Count > 0)
            {
                response = await _responseFiddler.Fiddle(response, currentChaosSettings);
            }

            response.Headers.Add(ChaosResponseHeaders.ConfigurationName, currentChaosSettings.Name);

            if (delayedTime > 0)
            {
                response.Headers.Add(ChaosResponseHeaders.DelayTime, delayedTime.ToString());
            }

            return response;
        }

        private static HttpResponseMessage CreateChanceMissMessageResponseHeaders(Task<HttpResponseMessage> t,
            IChaosSettings currentChaosSettings)
        {
            var taskResult = t.Result;
            taskResult.Headers.Add(ChaosResponseHeaders.ChanceMiss, string.Empty);
            taskResult.Headers.Add(ChaosResponseHeaders.ConfigurationName, currentChaosSettings.Name);
            return taskResult;
        }

        private static HttpResponseMessage CreateIgnoredUrlMessageResponseHeaders(Task<HttpResponseMessage> t,
            string ignoredPattern, IChaosSettings currentChaosSettings)
        {
            var taskResult = t.Result;
            taskResult.Headers.Add(ChaosResponseHeaders.IgnoredUrl, ignoredPattern);
            taskResult.Headers.Add(ChaosResponseHeaders.ConfigurationName, currentChaosSettings.Name);
            return taskResult;
        }

        private static bool ShouldIgnoreRequest(HttpRequestMessage request, IChaosSettings currentChaosSettings)
        {
            if (currentChaosSettings.IgnoreUrlPattern == null)
            {
                return false;
            }

            return currentChaosSettings.IgnoreUrlPattern.Any(ignoredUrl => request.RequestUri.AbsoluteUri.Contains(ignoredUrl));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((DisposableChaosTimer) _chaosTimer)?.Dispose();
                _handlerSettings?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
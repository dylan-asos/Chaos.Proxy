using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Controllers
{
    public class HostsController : ApiController
    {
        private readonly IApiSettingsData _apiSettings;

        private readonly IApiChaosConfigurationSettingsData _chaosConfigurationSettings;
        private readonly ICacheInvalidator _cacheInvalidator;

        public HostsController(IApiSettingsData apiSettings,
            IApiChaosConfigurationSettingsData chaosConfigurationSettings,
            ICacheInvalidator cacheInvalidator)
        {
            _apiSettings = apiSettings;
            _chaosConfigurationSettings = chaosConfigurationSettings;
            _cacheInvalidator = cacheInvalidator;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(CreateHostMappingRequest createHostMappingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chaosUrl = _apiSettings.GenerateChaosUrl(createHostMappingRequest.ChaosSubdomainName);
            if (await _apiSettings.GetByHostAsync(chaosUrl) != null)
            {
                return Conflict();
            }

            var result =
                await _apiSettings.AddAsync(createHostMappingRequest.ChaosSubdomainName,
                    createHostMappingRequest.ForwardHostName, createHostMappingRequest.Scheme,
                    createHostMappingRequest.Port);

            return Created(createHostMappingRequest.ChaosSubdomainName, result);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string apiKey)
        {
            var hostForwardSettings = await _apiSettings.GetByApiKeyAsync(apiKey);

            var tasks = new List<Task>()
            {
                _apiSettings.DeleteAsync(apiKey),
                _chaosConfigurationSettings.DeleteAsync(apiKey),
                _cacheInvalidator.Invalidate(hostForwardSettings.ForwardApiHostName)
            };

            await Task.WhenAll(tasks);

            return new StatusCodeResult(HttpStatusCode.NoContent, Request);
        }
    }
}
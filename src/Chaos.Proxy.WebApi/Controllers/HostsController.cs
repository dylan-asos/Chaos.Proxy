using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Controllers
{
    public class HostsController : ApiController
    {
        private readonly IApiSettingsData _apiSettings;

        private readonly IApiChaosConfigurationSettingsData _chaosConfigurationSettings;

        public HostsController(IApiSettingsData apiSettings,
            IApiChaosConfigurationSettingsData chaosConfigurationSettings)
        {
            _apiSettings = apiSettings;
            _chaosConfigurationSettings = chaosConfigurationSettings;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(CreateHostMappingRequest createHostMappingRequest)
        {
            try
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
            catch (InvalidOperationException validationException)
            {
                return BadRequest(validationException.Message);
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string apiKey)
        {
            try
            {
                await _apiSettings.DeleteAsync(apiKey);
                await _chaosConfigurationSettings.DeleteAsync(apiKey);

                return new StatusCodeResult(HttpStatusCode.NoContent, Request);
            }
            catch (InvalidOperationException validationException)
            {
                return BadRequest(validationException.Message);
            }
        }
    }
}
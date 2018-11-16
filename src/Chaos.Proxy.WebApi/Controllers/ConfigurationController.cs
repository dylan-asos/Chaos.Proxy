﻿using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using Chaos.Proxy.WebApi.Infrastructure.Mapping;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Controllers
{
    public class ConfigurationController : ApiController
    {
        private readonly IApiSettingsData _apiSettingsData;

        private readonly ICacheInvalidator _cacheInvalidator;

        private readonly IApiChaosConfigurationSettingsData _chaosConfigurationSettings;

        private readonly IChaosProxyHostSettings _chaosProxyHostSettings;

        public ConfigurationController(ICacheInvalidator cacheInvalidator, IApiSettingsData apiSettingsData,
            IApiChaosConfigurationSettingsData chaosConfigurationSettings,
            IChaosProxyHostSettings chaosProxyHostSettings)
        {
            _cacheInvalidator = cacheInvalidator;
            _apiSettingsData = apiSettingsData;
            _chaosConfigurationSettings = chaosConfigurationSettings;
            _chaosProxyHostSettings = chaosProxyHostSettings;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] string apiKey)
        {
            var hostForwardSettings = await _apiSettingsData.GetByApiKeyAsync(apiKey);

            if (hostForwardSettings == null)
            {
                return BadRequest(Constants.InvalidApiKey);
            }

            var apiConfiguration = await _chaosProxyHostSettings.GetAsync(hostForwardSettings.ForwardApiHostName);

            return new OkNegotiatedContentResult<ChaosConfiguration>(apiConfiguration, this);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(UpdateConfigurationRequest updateConfiguration)
        {
            try
            {
                var apiKey = updateConfiguration.ApiKey;

                var hostForwardSettings = await _apiSettingsData.GetByApiKeyAsync(apiKey);
                if (hostForwardSettings == null)
                {
                    return BadRequest(Constants.InvalidApiKey);
                }

                var chaosConfiguration = UpdateRequestToConfigurationConverter.ToChaosConfiguration(updateConfiguration);
                chaosConfiguration.Validate();

                await _chaosConfigurationSettings.CreateOrUpdateAsync(hostForwardSettings, apiKey, chaosConfiguration);

                _cacheInvalidator.Invalidate(hostForwardSettings.ForwardApiHostName);

                return Created(hostForwardSettings.ForwardApiHostName, chaosConfiguration);
            }
            catch (InvalidOperationException validationException)
            {
                return BadRequest(validationException.Message);
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri] string apiKey)
        {
            try
            {
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
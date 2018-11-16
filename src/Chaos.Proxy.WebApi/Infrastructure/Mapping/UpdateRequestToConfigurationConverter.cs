using System;
using System.Linq;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;

namespace Chaos.Proxy.WebApi.Infrastructure.Mapping
{
    public static class UpdateRequestToConfigurationConverter
    {
        public static ChaosConfiguration ToChaosConfiguration(UpdateConfigurationRequest updateRequest)
        {
            var config = new ChaosConfiguration
            {
                ChaosInterval = new TimeSpan(0, 0, updateRequest.ChaosInterval),
                ConfigurationRotationInterval = new TimeSpan(0, 0, updateRequest.ConfigurationRotationInterval),
                Enabled = updateRequest.Enabled,
                HttpClientTimeoutInSeconds = updateRequest.HttpClientTimeoutInSeconds
            };

            foreach (var updateChaosSettings in updateRequest.ChaosSettings)
            {
                var chaosConfiguration = new ChaosSettings
                {
                    Name = updateChaosSettings.Name,
                    MaxResponseDelayTime = updateChaosSettings.MaxResponseDelayTime,
                    MinResponseDelayTime = updateChaosSettings.MinResponseDelayTime,
                    PercentageOfChaos = updateChaosSettings.PercentageOfChaos,
                    PercentageOfSlowResponses = updateChaosSettings.PercentageOfSlowResponses,
                    ResponseTypeMediaType = updateChaosSettings.ResponseTypeMediaType
                };

                if (updateChaosSettings.IgnoreUrls != null)
                {
                    foreach (var url in updateChaosSettings.IgnoreUrls)
                        chaosConfiguration.IgnoreUrlPattern.Add(url.Pattern);
                }

                foreach (var updateResponse in updateChaosSettings.HttpResponses)
                {
                    var httpResponse = new ResponseDetails {StatusCode = updateResponse.StatusCode};

                    foreach (var payload in updateResponse.Payloads.Select(chaosResponsePayload =>
                        new ChaosResponsePayload
                            {Code = chaosResponsePayload.Code, Content = chaosResponsePayload.Content}))
                        httpResponse.Payloads.Add(payload);

                    chaosConfiguration.HttpResponses.Add(httpResponse);
                }

                foreach (var responseFiddle in updateChaosSettings.ResponseFiddles)
                {
                    var fiddle = new ResponseFiddle
                    {
                        Match = responseFiddle.Match,
                        ReplaceMatchingWith = responseFiddle.ReplaceMatchingWith
                    };

                    chaosConfiguration.ResponseFiddles.Add(fiddle);
                }

                config.ChaosSettings.Add(chaosConfiguration);
            }

            return config;
        }
    }
}
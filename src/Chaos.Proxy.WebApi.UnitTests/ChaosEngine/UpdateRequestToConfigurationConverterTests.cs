using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using Chaos.Proxy.WebApi.Infrastructure.Mapping;
using FluentAssertions;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    [TestFixture]
    public sealed class UpdateRequestToConfigurationConverterTests
    {
        private IChaosApiConfiguration _apiConfiguration;

        [SetUp]
        public void Given_I_Have_Converted_A_Request_To_An_Api_Configueration()
        {
            var updateConfigurationRequest = BuildRequest();
            _apiConfiguration = UpdateRequestToConfigurationConverter.ToChaosConfiguration(updateConfigurationRequest);
        }

        private UpdateConfigurationRequest BuildRequest()
        {
            return new UpdateConfigurationRequest
            {
                Enabled = true,
                ChaosInterval = 5,
                ConfigurationRotationInterval = 5,
                ChaosSettings =
                    new Collection<UpdateChaosSettings>
                    {
                        new UpdateChaosSettings
                        {
                            MaxResponseDelayTime = 5,
                            MinResponseDelayTime = 4,
                            PercentageOfChaos = 50,
                            PercentageOfSlowResponses = 50,
                            ResponseTypeMediaType = "application/xml",
                            Name = "test",
                            HttpResponses =
                                new List<UpdateResponseDetails>
                                {
                                    new UpdateResponseDetails(
                                        new List<UpdateChaosPayload>
                                        {
                                            new UpdateChaosPayload
                                            {
                                                Code
                                                    =
                                                    "123",
                                                Content
                                                    =
                                                    "123"
                                            }
                                        })
                                    {
                                        StatusCode
                                            =
                                            123
                                    }
                                }
                        }
                    }
            };
        }

        [Test]
        public void Then_Chaos_Interval_Should_Be_Correct()
        {
            _apiConfiguration.ChaosInterval.Should().Be(new TimeSpan(0, 0, 5));
        }

        [Test]
        public void Then_Chaos_Rotation_Interval_Should_Be_Correct()
        {
            _apiConfiguration.ConfigurationRotationInterval.Should().Be(new TimeSpan(0, 0, 5));
        }

        [Test]
        public void Then_Enabled_Should_Be_True()
        {
            _apiConfiguration.Enabled.Should().BeTrue();
        }

        [Test]
        public void Then_Should_Have_The_Correct_HttpResponse_Payloads()
        {
            var settings = _apiConfiguration.ChaosSettings.FirstOrDefault();

            settings.HttpResponses.FirstOrDefault().Payloads.Count.Should().Be(1);
            settings.HttpResponses.FirstOrDefault().Payloads.FirstOrDefault().Code.Should().Be("123");
            settings.HttpResponses.FirstOrDefault().Payloads.FirstOrDefault().Content.Should().Be("123");
        }

        [Test]
        public void Then_Should_Have_The_Correct_HttpResponses()
        {
            _apiConfiguration.ChaosSettings.FirstOrDefault().HttpResponses.Count.Should().Be(1);
            _apiConfiguration.ChaosSettings.FirstOrDefault().HttpResponses.FirstOrDefault().StatusCode.Should().Be(123);
        }

        [Test]
        public void Then_Should_Have_The_Correct_MaxResponseDelayTime()
        {
            _apiConfiguration.ChaosSettings.FirstOrDefault().MaxResponseDelayTime.Should().Be(5);
        }

        [Test]
        public void Then_Should_Have_The_Correct_MinResponseDelayTime()
        {
            _apiConfiguration.ChaosSettings.FirstOrDefault().MinResponseDelayTime.Should().Be(4);
        }

        [Test]
        public void Then_Should_Have_The_Correct_Name()
        {
            _apiConfiguration.ChaosSettings.FirstOrDefault().Name.Should().Be("test");
        }

        [Test]
        public void Then_Should_Have_The_Correct_Number_Of_Settings()
        {
            _apiConfiguration.ChaosSettings.Count.Should().Be(1);
        }

        [Test]
        public void Then_Should_Have_The_Correct_PercentageOfChaos()
        {
            _apiConfiguration.ChaosSettings.FirstOrDefault().PercentageOfChaos.Should().Be(50);
        }

        [Test]
        public void Then_Should_Have_The_Correct_PercentageOfSlowResponses()
        {
            _apiConfiguration.ChaosSettings.FirstOrDefault().PercentageOfChaos.Should().Be(50);
        }

        [Test]
        public void Then_Should_Have_The_Correct_ResponseTypeMediaType()
        {
            _apiConfiguration.ChaosSettings.FirstOrDefault().ResponseTypeMediaType.Should().Be("application/xml");
        }
    }
}
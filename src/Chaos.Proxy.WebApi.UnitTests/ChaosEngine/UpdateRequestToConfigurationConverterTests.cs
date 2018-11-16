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
        [SetUp]
        public void Given_I_Have_Converted_A_Request_To_An_Api_Configueration()
        {
            var updateConfigurationRequest = BuildRequest();
            apiConfiguration = UpdateRequestToConfigurationConverter.ToChaosConfiguration(updateConfigurationRequest);
        }

        private IChaosApiConfiguration apiConfiguration;

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
            apiConfiguration.ChaosInterval.Should().Be(new TimeSpan(0, 0, 5));
        }

        [Test]
        public void Then_Chaos_Rotation_Interval_Should_Be_Correct()
        {
            apiConfiguration.ConfigurationRotationInterval.Should().Be(new TimeSpan(0, 0, 5));
        }

        [Test]
        public void Then_Enabled_Should_Be_True()
        {
            apiConfiguration.Enabled.Should().BeTrue();
        }

        [Test]
        public void Then_Should_Have_The_Correct_HttpResponse_Payloads()
        {
            apiConfiguration.ChaosSettings.FirstOrDefault().HttpResponses.FirstOrDefault().Payloads.Count.Should()
                .Be(1);
            apiConfiguration.ChaosSettings.FirstOrDefault().HttpResponses.FirstOrDefault().Payloads.FirstOrDefault()
                .Code.Should().Be("123");
            apiConfiguration.ChaosSettings.FirstOrDefault().HttpResponses.FirstOrDefault().Payloads.FirstOrDefault()
                .Content.Should().Be("123");
        }

        [Test]
        public void Then_Should_Have_The_Correct_HttpResponses()
        {
            apiConfiguration.ChaosSettings.FirstOrDefault().HttpResponses.Count.Should().Be(1);
            apiConfiguration.ChaosSettings.FirstOrDefault().HttpResponses.FirstOrDefault().StatusCode.Should().Be(123);
        }

        [Test]
        public void Then_Should_Have_The_Correct_MaxResponseDelayTime()
        {
            apiConfiguration.ChaosSettings.FirstOrDefault().MaxResponseDelayTime.Should().Be(5);
        }

        [Test]
        public void Then_Should_Have_The_Correct_MinResponseDelayTime()
        {
            apiConfiguration.ChaosSettings.FirstOrDefault().MinResponseDelayTime.Should().Be(4);
        }

        [Test]
        public void Then_Should_Have_The_Correct_Name()
        {
            apiConfiguration.ChaosSettings.FirstOrDefault().Name.Should().Be("test");
        }

        [Test]
        public void Then_Should_Have_The_Correct_Number_Of_Settings()
        {
            apiConfiguration.ChaosSettings.Count.Should().Be(1);
        }

        [Test]
        public void Then_Should_Have_The_Correct_PercentageOfChaos()
        {
            apiConfiguration.ChaosSettings.FirstOrDefault().PercentageOfChaos.Should().Be(50);
        }

        [Test]
        public void Then_Should_Have_The_Correct_PercentageOfSlowResponses()
        {
            apiConfiguration.ChaosSettings.FirstOrDefault().PercentageOfChaos.Should().Be(50);
        }

        [Test]
        public void Then_Should_Have_The_Correct_ResponseTypeMediaType()
        {
            apiConfiguration.ChaosSettings.FirstOrDefault().ResponseTypeMediaType.Should().Be("application/xml");
        }
    }
}
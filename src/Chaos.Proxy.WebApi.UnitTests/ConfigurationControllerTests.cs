using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Chaos.Proxy.WebApi.Controllers;
using Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests
{
    [TestFixture]
    public class ConfigurationControllerTests
    {
        private ConfigurationController _configurationController;

        private Mock<ICacheInvalidator> _cacheInvalidator;

        private Mock<IApiSettingsData> _apiSettingsData;

        private Mock<IApiChaosConfigurationSettingsData> _chaosConfigurationSettings;

        private Mock<IChaosProxyHostSettings> _chaosProxyHostSettings;

        [SetUp]
        public void Init()
        {
            _cacheInvalidator = new Mock<ICacheInvalidator>();
            _apiSettingsData = new Mock<IApiSettingsData>();
            _chaosConfigurationSettings = new Mock<IApiChaosConfigurationSettingsData>();
            _chaosProxyHostSettings = new Mock<IChaosProxyHostSettings>();

            _configurationController = new ConfigurationController(_cacheInvalidator.Object, _apiSettingsData.Object,
                _chaosConfigurationSettings.Object, _chaosProxyHostSettings.Object);
        }

        public class GetTests : ConfigurationControllerTests
        {
            [Test]
            public async Task Returns_Bad_Request_When_Key_Does_Not_Exist()
            {
                var result = await _configurationController.Get("invalid-key") as BadRequestErrorMessageResult;

                result.Should().NotBeNull();
            }

            [Test]
            public async Task Returns_Configuration_From_Data_Source()
            {
                _apiSettingsData.Setup(f => f.GetByApiKeyAsync("test-key")).ReturnsAsync(
                    new ApiHostForwardingSettings {ForwardApiHostName = "somedomain.test.com"});

                _chaosProxyHostSettings.Setup(f => f.GetAsync("test-key"))
                    .ReturnsAsync(new ChaosConfiguration {Enabled = true});

                var result =
                    await _configurationController.Get("test-key") as
                        OkNegotiatedContentResult<ChaosConfiguration>;

                result.Content.Enabled.Should().BeTrue();
            }
        }

        public class SuccessfulPostTests : ConfigurationControllerTests
        {
            [SetUp]
            public void When_A_Successful_Post_Had_Occurred()
            {
                _apiSettingsData.Setup(f => f.GetByApiKeyAsync("creates-entry")).ReturnsAsync(
                    new ApiHostForwardingSettings {ForwardApiHostName = "somedomain.test.com"});

                _configurationController.Request = new HttpRequestMessage();

                result = _configurationController.Post(new UpdateConfigurationRequest
                    {ApiKey = "creates-entry"}).Result as CreatedNegotiatedContentResult<ChaosConfiguration>;
            }

            private CreatedNegotiatedContentResult<ChaosConfiguration> result;

            [Test]
            public void Then_Storage_Should_Be_Updated()
            {
                _chaosConfigurationSettings.Verify(
                    f => f.CreateOrUpdateAsync(It.IsAny<ApiHostForwardingSettings>(), "creates-entry",
                        It.IsAny<ChaosConfiguration>()), Times.Once);
            }

            [Test]
            public void Then_The_Cache_Should_Be_Invalidated()
            {
                _cacheInvalidator.Verify(f => f.Invalidate("somedomain.test.com"), Times.Once);
            }

            [Test]
            public void Then_The_Location_Should_Be_Set()
            {
                result.Location.OriginalString.Should().Be("somedomain.test.com");
            }

            [Test]
            public void Then_The_Result_Should_Be_Of_Correct_Type()
            {
                result.Should().NotBeNull();
            }
        }

        public class PostTests : ConfigurationControllerTests
        {
            [Test]
            public async Task Returns_Bad_Request_On_Validation_Error()
            {
                _apiSettingsData.Setup(f => f.GetByApiKeyAsync("validation-error"))
                    .ThrowsAsync(new InvalidOperationException());

                var result =
                    await _configurationController.Post(new UpdateConfigurationRequest()) as
                        BadRequestErrorMessageResult;

                result.Should().NotBeNull();
            }

            [Test]
            public async Task Returns_Bad_Request_When_Key_Does_Not_Exist()
            {
                var result =
                    await _configurationController.Post(new UpdateConfigurationRequest()) as
                        BadRequestErrorMessageResult;

                result.Should().NotBeNull();
            }
        }

        public class DeleteTests : ConfigurationControllerTests
        {
            [Test]
            public async Task Returns_No_Conent_On_Successful_Delete()
            {
                _configurationController.Request = new HttpRequestMessage();
                _apiSettingsData.Setup(f => f.GetByApiKeyAsync("deletes-entry")).ReturnsAsync(
                    new ApiHostForwardingSettings {ForwardApiHostName = "somedomain.test.com"});
                _chaosConfigurationSettings.Setup(f => f.DeleteAsync("deletes-entry")).Returns(Task.CompletedTask);

                var result = await _configurationController.Delete("deletes-entry") as StatusCodeResult;

                result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }
    }
}
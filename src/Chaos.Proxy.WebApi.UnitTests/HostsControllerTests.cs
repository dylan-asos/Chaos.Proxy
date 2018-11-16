using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Chaos.Proxy.WebApi.Controllers;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests
{
    public class HostsControllerTests
    {
        private Mock<IApiSettingsData> _apiSettingsData;
        private Mock<IApiChaosConfigurationSettingsData> _chaosConfigurationSettings;

        private HostsController _hostsController;

        [SetUp]
        public void Init()
        {
            _apiSettingsData = new Mock<IApiSettingsData>();
            _chaosConfigurationSettings = new Mock<IApiChaosConfigurationSettingsData>();

            _hostsController = new HostsController(_apiSettingsData.Object, _chaosConfigurationSettings.Object);
        }

        public class DeleteTests : HostsControllerTests
        {
            [Test]
            public async Task Deletes_Configuration_Data()
            {
                _hostsController.Request = new HttpRequestMessage();
                _apiSettingsData.Setup(f => f.DeleteAsync("test-key")).Returns(Task.FromResult(0));
                _chaosConfigurationSettings.Setup(f => f.DeleteAsync("test-key")).Returns(Task.FromResult(0));

                var result = await _hostsController.Delete("test-key") as StatusCodeResult;

                result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }

            [Test]
            public async Task Returns_Bad_Request_When_Key_Does_Not_Exist()
            {
                _apiSettingsData.Setup(f => f.DeleteAsync("invalid-key")).Throws<InvalidOperationException>();

                var result = await _hostsController.Delete("invalid-key") as BadRequestErrorMessageResult;

                result.Should().NotBeNull();
            }
        }

        public class PostTests : HostsControllerTests
        {
            [Test]
            public async Task Returns_Bad_Request_When_Invalid_Model_State()
            {
                _hostsController.ModelState.AddModelError("invalid-data", new Exception("some-error"));

                var result = await _hostsController.Post(new CreateHostMappingRequest()) as InvalidModelStateResult;

                result.Should().NotBeNull();
            }

            [Test]
            public async Task Returns_Bad_Request_When_Invalid_Operation()
            {
                _apiSettingsData.Setup(f => f.GenerateChaosUrl("invalid")).Throws<InvalidOperationException>();

                var result =
                    await _hostsController.Post(new CreateHostMappingRequest {ChaosSubdomainName = "invalid"}) as
                        BadRequestErrorMessageResult;

                result.Should().NotBeNull();
            }

            [Test]
            public async Task Returns_Conflict_When_Host_Already_Exists()
            {
                _apiSettingsData.Setup(f => f.GenerateChaosUrl("some-chaos-host")).Returns("chaos-host");
                _apiSettingsData.Setup(f => f.GetByHostAsync("chaos-host"))
                    .Returns(Task.FromResult(new ApiHostForwardingSettings()));

                var result = await _hostsController.Post(new CreateHostMappingRequest
                    {ChaosSubdomainName = "some-chaos-host"}) as ConflictResult;

                result.Should().NotBeNull();
            }

            [Test]
            public async Task Returns_Created_When_Ok()
            {
                _apiSettingsData.Setup(f => f.GenerateChaosUrl("chaos-host")).Returns("chaos-host");

                _apiSettingsData.Setup(f => f.AddAsync("chaos-host", "forward-host", null, 0))
                    .Returns(Task.FromResult(new ApiHostForwardingSettings {RowKey = "test-success"}));

                var result =
                    await _hostsController.Post(new CreateHostMappingRequest
                            {ChaosSubdomainName = "chaos-host", ForwardHostName = "forward-host"}) as
                        CreatedNegotiatedContentResult<ApiHostForwardingSettings>;

                result.Should().NotBeNull();
                result.Content.ApiKey.Should().Be("test-success");
            }
        }
    }
}
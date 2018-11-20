using System;
using System.Collections.ObjectModel;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    public class HandlerSettingsTests
    {
        private Mock<IChaosApiConfiguration> _configuration;

        private HandlerSettings _handlerSettings;

        private Mock<IConfigurationRotatationTimer> _rotationTimer;

        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IChaosApiConfiguration>();
            _configuration.SetupGet(f => f.ConfigurationRotationInterval).Returns(new TimeSpan(0, 0, 2));
            _configuration.Setup(f => f.ChaosSettings)
                .Returns(new Collection<ChaosSettings>
                {
                    new ChaosSettings {Name = "Test1"}, new ChaosSettings {Name = "Test2"},
                    new ChaosSettings {Name = "Test3"}
                });

            _rotationTimer = new Mock<IConfigurationRotatationTimer>();

            _handlerSettings = new HandlerSettings(_rotationTimer.Object, _configuration.Object);
        }

        [TestCase(1, "Test2")]
        [TestCase(2, "Test3")]
        [TestCase(3, "Test1")]
        public void Should_Rotate_Configurations(int rotateCount, string expectedConfiguration)
        {
            RepeatAction(rotateCount, _handlerSettings.RotateConfiguration);

            _handlerSettings.Current.Name.Should().Be(expectedConfiguration);
        }

        private static void RepeatAction(int repeatCount, Action action)
        {
            for (var i = 0; i < repeatCount; i++) action();
        }
    }
}
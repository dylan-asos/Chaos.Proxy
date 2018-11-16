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
        private Mock<IChaosApiConfiguration> configuration;
        private HandlerSettings handlerSettings;

        private Mock<IConfigurationRotatationTimer> rotationTimer;

        [SetUp]
        public void Setup()
        {
            configuration = new Mock<IChaosApiConfiguration>();
            configuration.SetupGet(f => f.ConfigurationRotationInterval).Returns(new TimeSpan(0, 0, 2));
            configuration.Setup(f => f.ChaosSettings)
                .Returns(new Collection<ChaosSettings>
                {
                    new ChaosSettings {Name = "Test1"}, new ChaosSettings {Name = "Test2"},
                    new ChaosSettings {Name = "Test3"}
                });

            rotationTimer = new Mock<IConfigurationRotatationTimer>();

            handlerSettings = new HandlerSettings(rotationTimer.Object, configuration.Object);
        }

        [TestCase(1, "Test2")]
        [TestCase(2, "Test3")]
        [TestCase(3, "Test1")]
        public void Should_Rotate_Configurations(int rotateCount, string expectedConfiguration)
        {
            RepeatAction(rotateCount, handlerSettings.RotateConfiguration);

            handlerSettings.Current.Name.Should().Be(expectedConfiguration);
        }

        private static void RepeatAction(int repeatCount, Action action)
        {
            for (var i = 0; i < repeatCount; i++) action();
        }
    }
}
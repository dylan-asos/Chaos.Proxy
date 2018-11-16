﻿using System;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    [TestFixture]
    public sealed class ApiConfigurationTests
    {
        [SetUp]
        public void Init()
        {
            _apiConfiguration = new ChaosConfiguration();
        }

        private ChaosConfiguration _apiConfiguration;

        [Test]
        public void Throws_Invalid_Operation_Exception_When_Settings_Are_Invalid()
        {
            _apiConfiguration.ChaosSettings.Add(new ChaosSettings
                {MaxResponseDelayTime = 50, MinResponseDelayTime = 51, PercentageOfSlowResponses = 1});

            Assert.Throws<InvalidOperationException>(
                () => { _apiConfiguration.Validate(); });
        }
    }
}
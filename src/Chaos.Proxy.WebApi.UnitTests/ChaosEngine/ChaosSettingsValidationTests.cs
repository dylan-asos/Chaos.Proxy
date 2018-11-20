using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    public class ChaosSettingsValidationTests
    {
        private ChaosSettings _chaosSettings;

        [SetUp]
        public void Setup()
        {
            _chaosSettings = new ChaosSettings();
        }

        [TestCase(-1)]
        [TestCase(101)]
        public void Returns_Errors_When_Chaos_Percentage_Settings_Are_Invalid(int chaosPercent)
        {
            _chaosSettings.PercentageOfChaos = chaosPercent;

            var results = _chaosSettings.Validate();

            results.Count.Should().Be(1);
        }

        [TestCase(-1)]
        [TestCase(101)]
        public void Returns_Errors_When_SlowResponse_Percentage_Settings_Are_Invalid(int slowResponsePercent)
        {
            _chaosSettings.PercentageOfSlowResponses = slowResponsePercent;

            var results = _chaosSettings.Validate();

            results.Count.Should().Be(1);
        }

        [TestCase(100)]
        [TestCase(99)]
        public void Returns_Errors_When_Max_Response_Time_Is_LessThan_Or_Equal_To_Min_Response_Time(int maxResponseTime)
        {
            _chaosSettings.PercentageOfSlowResponses = 100;
            _chaosSettings.MinResponseDelayTime = 100;
            _chaosSettings.MaxResponseDelayTime = maxResponseTime;

            var results = _chaosSettings.Validate();

            results.Count.Should().Be(1);
        }
    }
}
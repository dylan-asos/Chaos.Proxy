using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    public class ChaosSettingsValidationTests
    {
        private ChaosSettings chaosSettings;

        [SetUp]
        public void Setup()
        {
            chaosSettings = new ChaosSettings();
        }

        [TestCase(-1)]
        [TestCase(101)]
        public void Returns_Errors_When_Chaos_Percentage_Settings_Are_Invalid(int chaosPercent)
        {
            chaosSettings.PercentageOfChaos = chaosPercent;

            var results = chaosSettings.Validate();

            results.Count.Should().Be(1);
        }

        [TestCase(-1)]
        [TestCase(101)]
        public void Returns_Errors_When_SlowResponse_Percentage_Settings_Are_Invalid(int slowResponsePercent)
        {
            chaosSettings.PercentageOfSlowResponses = slowResponsePercent;

            var results = chaosSettings.Validate();

            results.Count.Should().Be(1);
        }

        [TestCase(100)]
        [TestCase(99)]
        public void Returns_Errors_When_Max_Response_Time_Is_LessThan_Or_Equal_To_Min_Response_Time(int maxResponseTime)
        {
            chaosSettings.PercentageOfSlowResponses = 100;
            chaosSettings.MinResponseDelayTime = 100;
            chaosSettings.MaxResponseDelayTime = maxResponseTime;

            var results = chaosSettings.Validate();

            results.Count.Should().Be(1);
        }
    }
}
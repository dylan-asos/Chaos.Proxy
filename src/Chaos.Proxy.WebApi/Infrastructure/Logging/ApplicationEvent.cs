using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog.Events;

namespace Chaos.Proxy.WebApi.Infrastructure.Logging
{
    public class ApplicationEvent
    {
        private const string ApiCritical = "ERROR_CHAOS_PROXY_{0}";

        private const string ApiError = "ERROR_CHAOS_PROXY_{0}";

        private const string ApiInformation = "INFO_CHAOS_PROXY_{0}";

        private const string ApiVerbose = "VERBOSE_CHAOS_PROXY_{0}";

        private const string ApiWarning = "WARN_CHAOS_PROXY_{0}";

        private const int BaseEventCodeId = 15000;

        private static readonly Dictionary<string, ApplicationEvent> ApplicationEvents;

        // Service Failure
        public static readonly ApplicationEvent ServiceFailure =
            new ApplicationEvent(LogEventLevel.Error, "ServiceFailure", 0x12);

        static ApplicationEvent()
        {
            ApplicationEvents = typeof(ApplicationEvent).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(fi => (ApplicationEvent) fi.GetValue(null))
                .ToDictionary(ae => ae.Code, ae => ae, StringComparer.OrdinalIgnoreCase);
        }

        private ApplicationEvent(LogEventLevel level, string code)
        {
            Level = level;
            Code = string.Format(GetPrefixForEventLevel(level), code);
            EventCodeId = 0;
        }

        private ApplicationEvent(LogEventLevel level, string code, int eventCodeId)
            : this(level, code)
        {
            EventCodeId = BaseEventCodeId + eventCodeId;
        }

        public LogEventLevel Level { get; }

        public string Code { get; }

        public int EventCodeId { get; }

        private static string GetPrefixForEventLevel(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Fatal:
                    return ApiCritical;
                case LogEventLevel.Error:
                    return ApiError;
                case LogEventLevel.Information:
                    return ApiInformation;
                case LogEventLevel.Verbose:
                    return ApiVerbose;
                case LogEventLevel.Warning:
                    return ApiWarning;
                default:
                    return ApiCritical;
            }
        }

        public static int GetEventCodeId(string code)
        {
            return ApplicationEvents.TryGetValue(code, out var applicationEvent) ? applicationEvent.EventCodeId : 0;
        }
    }
}
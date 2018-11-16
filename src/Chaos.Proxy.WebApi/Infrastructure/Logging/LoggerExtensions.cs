using Serilog;

namespace Chaos.Proxy.WebApi.Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        public static void WriteApplicationEvent(this ILogger logger, string message, ApplicationEvent eventCode)
        {
            logger.Write(eventCode.Level, message, eventCode.Code);
        }
    }
}
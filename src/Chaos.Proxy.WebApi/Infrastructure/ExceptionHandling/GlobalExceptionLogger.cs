using System;
using System.Web.Http.ExceptionHandling;
using Chaos.Proxy.WebApi.Infrastructure.Extensions;
using Chaos.Proxy.WebApi.Infrastructure.Logging;
using Serilog;

namespace Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        private readonly ILogger _logger;

        public GlobalExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            DoLog(context, (dynamic) context.Exception);
        }

        private void DoLog(ExceptionLoggerContext context, AggregateException ex)
        {
            foreach (var innerException in ex.InnerExceptions) DoLog(context, (dynamic) innerException);
        }

        private void DoLog(ExceptionLoggerContext context, Exception ex)
        {
            var requestMethod = "Unknown";
            var requestUrl = "Unknown";

            var requestMessage = context.Request;
            if (requestMessage != null)
            {
                if (requestMessage.Method != null) requestMethod = requestMessage.Method.ToString();

                if (requestMessage.RequestUri != null) requestUrl = requestMessage.RequestUri.ToString();
            }

            _logger.WriteApplicationEvent(
                $"Unhandled exception invoking '[{requestMethod}]{requestUrl}': {ex.Serialize()}", ApplicationEvent.ServiceFailure);
        }
    }
}
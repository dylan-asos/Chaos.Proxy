using System;
using System.Net;
using System.Web.Http.ExceptionHandling;
using Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling.Errors;
using Chaos.Proxy.WebApi.Infrastructure.HttpResults;

namespace Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Result = new ErrorsResult(context.Request, HttpStatusCode.InternalServerError, ApiErrors.InternalServerError);
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
}
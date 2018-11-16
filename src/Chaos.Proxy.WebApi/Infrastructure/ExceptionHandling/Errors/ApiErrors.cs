namespace Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling.Errors
{
    public static class ApiErrors
    {
        private const string InternalServerErrorCode = "InternalServerError";

        private const string NotFoundCode = "NotFound";

        public static ErrorDetails NotFound =>
            CreateError(NotFoundCode, "No resource matches the URL and protocol specified");

        public static ErrorDetails InternalServerError => CreateError(InternalServerErrorCode,
            "Unable to process your request at the moment. Please try later");

        private static ErrorDetails CreateError(string errorCode, string message)
        {
            return new ErrorDetails(errorCode, message) {UserMessage = message};
        }
    }
}
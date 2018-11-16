using System.Runtime.Serialization;

namespace Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling.Errors
{
    /// <summary>
    ///     Defines error details returned from an HTTP Client service call
    /// </summary>
    [DataContract]
    public class HttpResultErrorDetails
    {
        /// <summary>Ctor</summary>
        public HttpResultErrorDetails()
        {
        }

        /// <summary>Ctor</summary>
        /// <param name="errorCode">The error code</param>
        /// <param name="message">The message associated with the code</param>
        public HttpResultErrorDetails(string errorCode, string message)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
            Level = ErrorLevel.Info;
        }

        /// <summary>
        ///     An error code that defines the problem, e.g. CircuitBroken, InvalidFunds, ProductNotFound
        /// </summary>
        [DataMember]
        public string ErrorCode { get; set; }

        /// <summary>The message that descibed the ErrorCode in detail</summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     The prarameter name that triggered the error code, if applicable
        /// </summary>
        [DataMember]
        public string ParameterName { get; set; }

        /// <summary>The Error level assocated with the error code</summary>
        [DataMember]
        public ErrorLevel Level { get; set; }

        /// <summary>
        ///     Any user message to return to the caller. This might be a localised message to display in script
        /// </summary>
        [DataMember]
        public string UserMessage { get; set; }

        /// <summary>
        ///     An array of strings that define the context of the error message that was triggered.
        /// </summary>
        [DataMember]
        public string[] MessageContext { get; set; }

        /// <summary>A formatted represenation of the error instance</summary>
        /// <returns>Formatted error details</returns>
        public override string ToString()
        {
            return $"{Level} - Error Code: {ErrorCode} Message: {ErrorMessage}";
        }
    }
}
using System.Runtime.Serialization;

namespace Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling.Errors
{
    [DataContract]
    public class ErrorDetails
    {
        public ErrorDetails()
        {
        }

        public ErrorDetails(string errorCode, string message)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
            Level = ErrorLevel.Info;
        }

        [DataMember] public string ErrorCode { get; set; }

        [DataMember] public string ErrorMessage { get; set; }

        [DataMember] public string ParameterName { get; set; }

        [DataMember] public ErrorLevel Level { get; set; }

        [DataMember] public string UserMessage { get; set; }

        [DataMember] public string[] MessageContext { get; set; }

        public override string ToString()
        {
            return $"{Level} - Error Code: {ErrorCode} Message: {ErrorMessage}";
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Chaos.Proxy.WebApi.Infrastructure.Contracts
{
    public class CreateHostMappingRequest
    {
        [RegularExpression("^[a-zA-Z0-9_.-]*$", ErrorMessage = "Only letters, numbers and hyphens are allowed")]
        [Required]
        public string ChaosSubdomainName { get; set; }

        [RegularExpression("^[a-zA-Z0-9_.-]*$", ErrorMessage =
            "Only letters, numbers and hyphens are allowed. This is just the host name, do not include the protocol")]
        [Required]
        public string ForwardHostName { get; set; }

        [RegularExpression(@"^(http|https)$", ErrorMessage = "Only http or https scheme supported")]
        public string Scheme { get; set; }

        public int Port { get; set; }
    }
}
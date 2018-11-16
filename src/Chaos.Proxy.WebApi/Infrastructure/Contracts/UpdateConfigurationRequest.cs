using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Chaos.Proxy.WebApi.Infrastructure.Contracts
{
    public class UpdateConfigurationRequest
    {
        public UpdateConfigurationRequest()
        {
            ChaosSettings = new Collection<UpdateChaosSettings>();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must specify an API key")]
        public string ApiKey { get; set; }

        public int HttpClientTimeoutInSeconds { get; set; }

        [Required] public bool Enabled { get; set; }

        public int ChaosInterval { get; set; }

        public int ConfigurationRotationInterval { get; set; }

        public Collection<UpdateChaosSettings> ChaosSettings { get; set; }
    }
}
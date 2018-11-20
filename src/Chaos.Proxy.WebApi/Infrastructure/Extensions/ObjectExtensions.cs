using System.Collections.Generic;
using System.Text;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Newtonsoft.Json;

namespace Chaos.Proxy.WebApi.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static string Serialize(this object instance)
        {
            return JsonConvert.SerializeObject(instance, Formatting.Indented);
        }

        public static string AsFormattingString(this List<ChaosSettingError> validationErrors)
        {
            var sb = new StringBuilder();
            foreach (var chaosSettingError in validationErrors)
            {
                sb.AppendLine(chaosSettingError.ToString());
            }

            return sb.ToString();
        }
    }
}
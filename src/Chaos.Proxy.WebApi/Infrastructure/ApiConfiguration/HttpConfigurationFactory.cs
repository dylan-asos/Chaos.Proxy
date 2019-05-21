using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json.Serialization;

namespace Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration
{
    [ExcludeFromCodeCoverage]
    public class HttpConfigurationFactory
    {
        private readonly IHttpControllerActivator _controllerActivator;

        private readonly IDependencyResolver _dependencyResolver;

        private readonly IExceptionHandler _exceptionHandler;

        private readonly IExceptionLogger _exceptionLogger;

        public HttpConfigurationFactory(
            IHttpControllerActivator controllerActivator,
            IDependencyResolver dependencyResolver,
            IExceptionHandler exceptionHandler,
            IExceptionLogger exceptionLogger)
        {
            _controllerActivator = controllerActivator;
            _dependencyResolver = dependencyResolver;
            _exceptionHandler = exceptionHandler;
            _exceptionLogger = exceptionLogger;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public HttpConfiguration Create()
        {
            var config = new HttpConfiguration
                {DependencyResolver = _dependencyResolver, IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always};

            ConfigureWebApi(config);
            ConfigureWebApiRoutes(config);
            ConfigureServicePoint();

            config.EnsureInitialized();
            return config;
        }

        private void ConfigureServicePoint()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => SupportInvalidCertificate();

            ServicePointManager.DefaultConnectionLimit = short.MaxValue;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 10000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ThreadPool.SetMinThreads(250, 250);
        }


        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), _exceptionHandler);
            config.Services.Replace(typeof(IExceptionLogger), _exceptionLogger);
            config.Services.Replace(typeof(IHttpControllerActivator), _controllerActivator);

            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            var jsonFormatter = formatters.OfType<JsonMediaTypeFormatter>().First();
            var settings = jsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private void ConfigureWebApiRoutes(HttpConfiguration config)
        {
            RouteConfiguration.Initialise(config, _dependencyResolver);
        }

        private static bool SupportInvalidCertificate()
        {
            return bool.TryParse(ConfigurationManager.AppSettings["SupportInvalidCertificates"],
                       out var supportInvalidCertificates) && supportInvalidCertificates;
        }
    }
}
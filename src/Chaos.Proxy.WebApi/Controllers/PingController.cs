using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chaos.Proxy.WebApi.Controllers
{
    public class PingController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
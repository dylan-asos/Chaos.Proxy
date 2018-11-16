using System.Collections.Generic;
using AutoMapper;
using Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling.Errors;

namespace Chaos.Proxy.WebApi.Infrastructure.Mapping
{
    public class HttpResultErrorMapper
    {
        private readonly IMapper _errorMapper;

        public HttpResultErrorMapper()
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<HttpResultErrorDetails, ErrorDetails>());

            _errorMapper = configuration.CreateMapper();
        }

        public virtual ErrorDetails[] Convert(List<HttpResultErrorDetails> errors)
        {
            return _errorMapper.Map<ErrorDetails[]>(errors);
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Net.Http.Utilits.Logging;

namespace Net.Http.Utilits.Extensions
{
    public static class HttpClientLoggingDependencyInjectionExtensions
    {
        public static IServiceCollection AddHttpClientLogging(this IServiceCollection services)
        {
             services
                .TryAddEnumerable(ServiceDescriptor
                .Singleton<IHttpMessageHandlerBuilderFilter, HttpClientMessageHandlerBuilderFilter>());

            return services;
        }
    }
}

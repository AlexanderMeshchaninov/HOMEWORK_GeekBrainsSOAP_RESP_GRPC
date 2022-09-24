using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Http.Utilits.Options;

namespace Net.Http.Utilits.Logging
{
    public class HttpClientMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        public ILoggerFactory LoggerFactory { get; }
        public IOptions<HttpClientLoggingOptions> Options { get; }

        public HttpClientMessageHandlerBuilderFilter(
            ILoggerFactory loggerFactory, 
            IOptions<HttpClientLoggingOptions> options)
        {
            LoggerFactory = loggerFactory;
            Options = options;
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            return (builder) =>
            {
                // call next handler 
                next(builder);

                // create logger 
                string loggerName = !string.IsNullOrEmpty(builder.Name) ? builder.Name : "Default";
                ILogger logger = LoggerFactory.CreateLogger($"Net.Http.Utilits.HttpClient.{loggerName}");

                // create and add handler
                LoggingHttpMessageHandler loggingHttpMessageHandler = new LoggingHttpMessageHandler(logger, Options);
                builder.AdditionalHandlers.Insert(0, loggingHttpMessageHandler);
            };
        }
    }
}

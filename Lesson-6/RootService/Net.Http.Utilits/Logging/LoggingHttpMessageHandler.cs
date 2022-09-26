using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Http.Utilits.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Net.Http.Utilits.Logging
{
    public class LoggingHttpMessageHandler : DelegatingHandler
    {
        public ILogger Logger { get; }
        public HttpClientLoggingOptions Options { get; }

        public LoggingHttpMessageHandler(
            ILogger logger,
            IOptions<HttpClientLoggingOptions> options)
        {
            Logger = logger;
            Options = options.Value;
        }

        [DebuggerStepThrough]
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // generate correlation id
            Guid correlationId = Guid.NewGuid();

            // request tracing
            if (Logger.IsEnabled(LogLevel.Information))
            {
                // tracing request
                StringBuilder requestText = new StringBuilder();
                requestText.AppendLine($"Request #{correlationId}");

                // parse URI
                Uri requestUri = request.RequestUri;
                requestText.AppendLine($"Host: {requestUri.Host}");
                requestText.AppendLine($"Path: {requestUri.AbsolutePath}");
                requestText.AppendLine($"QueryString: {requestUri.Query}");
                requestText.AppendLine($"Method: {request.Method}");
                requestText.AppendLine($"Scheme: {requestUri.Scheme}");

                // tracing request headers
                foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
                {
                    foreach (string value in header.Value)
                    {
                        requestText.AppendLine($"{header.Key}: {value}");
                    }
                }

                // tracing request contents 
                if (request.Content != null)
                {
                    if (CheckContentType(request.Content.Headers.ContentType))
                    {
                        // read body
                        string requestBody = await request.Content.ReadAsStringAsync();

                        // truncate too long bodies
                        if (requestBody != null && requestBody.Length >= Options.MaxBodyLength)
                        {
                            requestBody = requestBody.Substring(0, Options.MaxBodyLength) + "... <Truncated>";
                        }

                        // append log
                        requestText.AppendLine($"RequestBody:");
                        requestText.AppendLine(requestBody);
                    }
                    else
                    {
                        // append log 
                        requestText.AppendLine($"RequestBody: <Not Logged>");
                    }
                }

                // tracing output
                Logger.LogInformation(requestText.ToString());
            }

            // Measure execution time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // execute request
            HttpResponseMessage response;

            try
            {
                // execute request
                response = await base.SendAsync(request, cancellationToken);

                // stop measuring time
                stopwatch.Stop();
            }
            catch (Exception exception)
            {
                // stop measuring time
                stopwatch.Stop();

                // tracing response
                StringBuilder responseText = new StringBuilder();
                responseText.AppendLine($"Response #{correlationId}");
                responseText.AppendLine($"Elapsed: {stopwatch.ElapsedMilliseconds}ms");
                responseText.AppendLine($"Exception: {exception.Message}");

                // tracing output
                Logger.LogInformation(responseText.ToString());

                // re-throw
                throw;
            }

            // response tracing 
            if (Logger.IsEnabled(LogLevel.Information))
            {
                // tracing response
                StringBuilder responseText = new StringBuilder();
                responseText.AppendLine($"Response #{correlationId}");
                responseText.AppendLine($"Elapsed: {stopwatch.ElapsedMilliseconds}ms");
                responseText.AppendLine($"StatusCode: {(int)response.StatusCode} {response.ReasonPhrase}");

                // get response headers
                IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = response.Headers;

                // add content response headers
                if (response.Content != null && response.Content.Headers != null)
                    headers = headers.Concat(response.Content.Headers);

                // tracing response headers
                foreach (KeyValuePair<string, IEnumerable<string>> header in headers.OrderBy(item => item.Key))
                {
                    foreach (string value in header.Value)
                    {
                        responseText.AppendLine($"{header.Key}: {value}");
                    }
                }

                // tracing response contents
                if (response.Content != null)
                {
                    if (IsTextContent(response.Content.Headers.ContentType))
                    {
                        // fetch body
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // truncate too long bodies
                        if (responseBody != null && responseBody.Length >= Options.MaxBodyLength)
                        {
                            responseBody = responseBody.Substring(0, Options.MaxBodyLength) + "... <Truncated>";
                        }

                        // append log
                        responseText.AppendLine($"ResponseBody:");
                        responseText.AppendLine(responseBody);
                    }
                    else
                    {
                        // append log
                        responseText.AppendLine($"ResponseBody: <Not Logged>");
                    }
                }

                // tracing output
                Logger.LogInformation(responseText.ToString());
            }

            return response;
        }

        private bool CheckContentType(MediaTypeHeaderValue contentType)
        {
            // for multipart form data we allow body logging only
            // when explicitly specified
            if (contentType != null && contentType.MediaType.Contains("multipart/form-data"))
            {
                return Options.LogMultipartFormData;
            }

            // allow body logging
            return true;
        }

        private bool IsTextContent(MediaTypeHeaderValue contentType)
        {
            return contentType != null &&
                (contentType.MediaType.EndsWith("/json") ||
                 contentType.MediaType.EndsWith("/xml") ||
                 contentType.MediaType.StartsWith("text/"));
        }
    }
}

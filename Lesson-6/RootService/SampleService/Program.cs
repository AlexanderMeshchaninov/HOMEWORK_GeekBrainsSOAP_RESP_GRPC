using Microsoft.AspNetCore.HttpLogging;
using Net.Http.Utilits.Extensions;
using NLog.Web;
using Polly;
using SampleService.Services;

namespace SampleService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Custom http client and polly settings
            builder.Services
                .AddHttpClient<IRootServiceClient, RootServiceClient>("RootServiceClient")
                .AddTransientHttpErrorPolicy(x => 
                x.WaitAndRetryAsync(
                    retryCount: 4, 
                    sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2), 
                    onRetry: (response, sleepDuration, attemptNumber, context) => 
                    { 
                        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();

                        logger.LogError(response.Exception is not null ? response.Exception : 
                            new Exception($"\n {response.Result.StatusCode}: {response.Result.RequestMessage}"),
                            $"(attempt: {attemptNumber}) RootServiceClient request exception!");
                    }));

            //Nlog
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.RequestHeaders.Add("Authorization");
                logging.RequestHeaders.Add("X-Real-IP");
                logging.RequestHeaders.Add("X-Forwarded-For");
            });

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();

            }).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

            //HttpUtilits...
            builder.Services.AddHttpClientLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseHttpLogging();

            app.MapControllers();

            app.Run();
        }
    }
}
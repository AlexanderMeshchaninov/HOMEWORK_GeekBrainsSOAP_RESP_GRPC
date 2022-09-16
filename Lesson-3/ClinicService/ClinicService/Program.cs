using System.Net;
using ClinicService.Core;
using ClinicService.Data;
using ClinicService.Repository;
using ClinicService.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

namespace ClinicService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //gRPC
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5001, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });

            builder.Services.AddGrpc();
            
            //База данных
            builder.Services.AddDbContext<ClinicServiceDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });
            
            //Логгирование
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
            
            //Репозитории
            builder.Services.AddScoped<IPetRepository, PetRepository>();
            builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpLogging();

            app.UseWhen(ctx => ctx.Request.ContentType != "application/grpc", builder =>
            {
                builder.UseHttpLogging();
            });
            
            app.UseAuthorization();
            
            app.MapControllers();
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ClinicClientService>();
            });

            app.Run();
        }
    }
}
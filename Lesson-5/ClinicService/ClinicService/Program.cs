using System.Net;
using System.Text;
using ClinicService.Core;
using ClinicService.Data;
using ClinicService.Repository;
using ClinicService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
                    listenOptions.UseHttps("C:/test_certificate.pfx", "12345");
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
            
            //Аутентификация
            builder.Services.AddSingleton<IAuthentificationService, AuthentificationService>();
            
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new
                    TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthentificationService.SecretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
            });
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Сервис клиники",
                    Version = "v1",
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme(Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            //app.UseHttpLogging();

            app.UseWhen(ctx => ctx.Request.ContentType != "application/grpc", builder =>
            {
                builder.UseHttpLogging();
            });
            
            app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ClinicClientService>();
                endpoints.MapGrpcService<AuthService>();
            });

            app.Run();
        }
    }
}
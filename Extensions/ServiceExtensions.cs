
using System.Text;
using LearnAPI.Contracts;
using LearnAPI.LoggerService;
using LearnAPI.Models;
using LearnAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LearnAPI.Extensions
{
    public static class ServiceExtensions
    {
        // Existing: Configure CORS
        public static void ConfigureCors(
            this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });
        }

        // Existing: Configure IIS integration
        public static void ConfigureIISIntegration(
            this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => { });
        }

        // Register database
        public static IServiceCollection ConfigureDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? configuration.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrEmpty(connectionString) &&
                connectionString.StartsWith("postgres"))
            {
                var uri = new Uri(connectionString);
                var userInfo = uri.UserInfo.Split(':');

                connectionString =
                    $"Host={uri.Host};Port={uri.Port};" +
                    $"Database={uri.AbsolutePath.Trim('/')};" +
                    $"Username={Uri.UnescapeDataString(userInfo[0])};" +
                    $"Password={Uri.UnescapeDataString(userInfo[1])};" +
                    "SSL Mode=Require;Trust Server Certificate=true;";
            }

            services.AddDbContext<BookContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        // Register application services
        public static IServiceCollection ConfigureApplicationServices(
            this IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureCors();
            services.ConfigureIISIntegration();

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddHttpContextAccessor();
            services.AddScoped<TokenService>();
            services.AddAuthorization();

            return services;
        }

        // Configure JWT authentication
        public static IServiceCollection ConfigureJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],

                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    configuration["Jwt:Key"]!))
                        };
                });

            return services;
        }

        // Configure Swagger and JWT support
        public static IServiceCollection ConfigureSwaggerServices(
            this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "Enter 'Bearer' followed by a space and your JWT."
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            return services;
        }
    }
}

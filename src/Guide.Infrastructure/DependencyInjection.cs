using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Guide.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<GuideDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("MySqlConnection");
                var useMySql = options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    options => options.UseMicrosoftJson());

                var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

                if (isDev)
                {
                    useMySql
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                        .LogTo(Log.Logger.Information, LogLevel.Information);
                }
                else
                {
                    useMySql
                        .EnableSensitiveDataLogging(false)
                        .EnableDetailedErrors(false)
                        .LogTo(Log.Logger.Warning, LogLevel.Warning);
                }
            });

        return services;
    }
}
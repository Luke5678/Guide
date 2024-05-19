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
                var connectionString = configuration.GetConnectionString("MySql");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb") // azure
                                       ?? throw new InvalidOperationException("Connection string not found.");
                }

                var db = options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    x => x.UseMicrosoftJson()
                );

                var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
                if (isDev)
                {
                    db.EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                        .LogTo(Log.Logger.Information, LogLevel.Information);
                }
                else
                {
                    db.EnableSensitiveDataLogging(false)
                        .EnableDetailedErrors(false)
                        .LogTo(Log.Logger.Warning, LogLevel.Warning);
                }
            });

        return services;
    }
}
using System.Text.RegularExpressions;
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

                var azure = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
                if (azure != null)
                {
                    var dbhost = Regex.Match(azure, @"Data Source=(.+?);").Groups[1].Value;
                    var server = dbhost.Split(':')[0];
                    var port = dbhost.Split(':')[1];
                    var dbname = Regex.Match(azure, @"Database=(.+?);").Groups[1].Value;
                    var dbusername = Regex.Match(azure, @"User Id=(.+?);").Groups[1].Value;
                    var dbpassword = Regex.Match(azure, @"Password=(.+?)$").Groups[1].Value;

                    connectionString =
                        $@"server={server};userid={dbusername};password={dbpassword};database={dbname};port={port};pooling = false; convert zero datetime=True;";
                }

                if (connectionString == null)
                {
                    throw new InvalidOperationException("Connection string not found.");
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
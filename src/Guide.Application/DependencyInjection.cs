using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using Guide.Application.Common.Behaviors;
using Guide.Application.Common.Interfaces;
using Guide.Application.Common.Services;
using Guide.Shared.Common.Interfaces;

namespace Guide.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IAttractionService, AttractionService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUploadService, UploadService>();

        return services;
    }
}
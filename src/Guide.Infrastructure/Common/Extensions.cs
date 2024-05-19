﻿using Guide.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guide.Infrastructure.Common;

public static class Extensions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var guideDbContext = scope.ServiceProvider.GetRequiredService<GuideDbContext>();
        guideDbContext.Database.Migrate();
    }

    public static void CreateDefaultUser(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        if (userManager.Users.Any()) return;
        
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var email = configuration["DefaultUser:Email"];
        var password = configuration["DefaultUser:Password"];

        var user = new User
        {
            UserName = email, Email = email, EmailConfirmed = true
        };
        
        userManager.CreateAsync(user, password!).Wait();
    }
}
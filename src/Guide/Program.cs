using Guide.Common.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Guide.Components;
using Guide.Components.Account;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents()
        .AddInteractiveWebAssemblyComponents();

    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddScoped<IdentityUserAccessor>();
    builder.Services.AddScoped<IdentityRedirectManager>();
    builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

    builder.Services.AddInfrastructure(builder.Configuration);

    // var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    //                        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    // builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //     options.UseSqlite(connectionString));
    // builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<GuideDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

    builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(Guide.Client._Imports).Assembly);

    // Add additional endpoints required by the Identity /Account Razor components.
    app.MapAdditionalIdentityEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
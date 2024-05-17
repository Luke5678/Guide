using BitzArt.Blazor.Cookies;
using Guide.Application;
using Guide.Client.Common.Services;
using Guide.Common.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Guide.Components;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

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
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<GuideDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

    builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        options.SetDefaultCulture("pl");
        options.AddSupportedCultures(["en", "pl"]);
        options.AddSupportedUICultures(["en", "pl"]);
        options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new CookieRequestCultureProvider()
        };
    });

    builder.AddBlazorCookies();
    builder.Services.AddScoped<CultureCookieService>();

    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddControllers();

    var app = builder.Build();

    app.UseDefaultFiles();
    app.UseStaticFiles();
    // var staticFilesPath = $@"{AppDomain.CurrentDomain.BaseDirectory}/wwwroot";
    // Directory.CreateDirectory(staticFilesPath);
    // app.UseStaticFiles(new StaticFileOptions
    // {
    //     FileProvider = new PhysicalFileProvider($@"{AppDomain.CurrentDomain.BaseDirectory}/wwwroot")
    // });


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

    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(Guide.Client.Components._Imports).Assembly);

    app.MapControllers();

    // Add additional endpoints required by the Identity /Account Razor components.
    app.MapAdditionalIdentityEndpoints();

    app.UseRequestLocalization();

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
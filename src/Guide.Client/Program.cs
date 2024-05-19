using BitzArt.Blazor.Cookies;
using Guide.Client.Common.Authorization;
using Guide.Client.Common.Services;
using Guide.Shared.Common.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(_ =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:7000")
    });

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddLocalization();
builder.Services.AddScoped<ICookieService, BrowserCookieService>();
builder.Services.AddScoped<CultureCookieService>();
builder.Services.AddScoped<IAttractionService, AttractionService>();

var app = builder.Build();

app.Services.GetService<CultureCookieService>()?.SetCultureFromCookie();

await app.RunAsync();
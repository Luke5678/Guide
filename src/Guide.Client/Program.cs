using BitzArt.Blazor.Cookies;
using Guide.Client.Common.Authorization;
using Guide.Client.Common.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddLocalization();
builder.Services.AddScoped<ICookieService, BrowserCookieService>();
builder.Services.AddScoped<CultureCookieService>();

var app = builder.Build();

app.Services.GetService<CultureCookieService>()?.SetCultureFromCookie();

await app.RunAsync();
using System.Globalization;
using BitzArt.Blazor.Cookies;
using Guide.Shared.Common.Static;

namespace Guide.Client.Common.Services;

public class CultureCookieService(ICookieService cookieService)
{
    private const string CookieName = ".AspNetCore.Culture";
    private const char CookieSeparator = '|';
    private const string CulturePrefix = "c=";

    public async Task SetCulture(string language)
    {
        var culture = CultureInfo.GetCultureInfo(language);

        if (!string.IsNullOrWhiteSpace(culture.Name) &&
            CultureInfo.GetCultures(CultureTypes.AllCultures).Contains(culture))
        {
            await cookieService.SetAsync(".AspNetCore.Culture", $"c={culture.Name}|uic={culture.Name}", null);
        }
    }

    public async Task SetCultureFromCookie()
    {
        var cookie = await cookieService.GetAsync(CookieName);
        var culture = ParseCookieValue(cookie?.Value ?? "");
        
        if (culture == null)
        {
            SetDefaultCulture();
            return;
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    private static void SetDefaultCulture()
    {
        var culture = CultureInfo.GetCultureInfo(LanguageCodes.Default);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    private static CultureInfo? ParseCookieValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith(CulturePrefix))
        {
            return null;
        }

        var split = value.Split(CookieSeparator);
        if (split.Length != 2)
        {
            return null;
        }

        var cultureName = split[0][CulturePrefix.Length..];
        var culture = new CultureInfo(cultureName);

        return CultureInfo.GetCultures(CultureTypes.AllCultures).Contains(culture)
            ? culture
            : null;
    }
}
using System.Globalization;
using BitzArt.Blazor.Cookies;
using Guide.Shared.Common.Static;

namespace Guide.Client.Common.Services;

public class LocalizationService(ICookieService cookieService)
{
    private const string CookieName = ".AspNetCore.Culture";
    private const string CulturePrefix = "c=";
    private const char CookieSeparator = '|';

    public async Task InitializeCulture()
    {
        var cookie = await cookieService.GetAsync(CookieName);
        var culture = ParseCookieValue(cookie?.Value ?? "");

        if (culture == null)
        {
            var language =
                LanguageCodes.List.FirstOrDefault(x => x == CultureInfo.CurrentCulture.TwoLetterISOLanguageName) ??
                LanguageCodes.Default;
            culture = GetCultureFromLanguage(language);
        }

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    public async Task SetCultureCookie(string language)
    {
        var culture = GetCultureFromLanguage(language);
        await cookieService.SetAsync(".AspNetCore.Culture", $"c={culture.Name}|uic={culture.Name}", null);
    }

    private CultureInfo GetCultureFromLanguage(string language)
    {
        var culture = CultureInfo.GetCultureInfo(language);

        if (!string.IsNullOrWhiteSpace(culture.Name) &&
            CultureInfo.GetCultures(CultureTypes.AllCultures).Contains(culture))
        {
            return culture;
        }

        return CultureInfo.CurrentCulture;
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
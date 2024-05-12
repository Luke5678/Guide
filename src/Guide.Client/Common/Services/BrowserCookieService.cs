using BitzArt.Blazor.Cookies;
using Microsoft.JSInterop;

namespace Guide.Client.Common.Services;

public class BrowserCookieService(IJSRuntime js) : ICookieService
{
    public async Task<IEnumerable<Cookie>> GetAllAsync()
    {
        var raw = await js.InvokeAsync<string>("eval", "document.cookie");
        if (string.IsNullOrWhiteSpace(raw)) return Enumerable.Empty<Cookie>();

        return raw.Split("; ").Select(x =>
        {
            var index = x.IndexOf('=');
            if (index == -1) throw new Exception($"Invalid cookie format: '{x}'.");
            return new Cookie(x[..index], x[(index + 1)..]);
        });
    }

    public async Task<Cookie?> GetAsync(string key)
    {
        var cookies = await GetAllAsync();
        return cookies.FirstOrDefault(x => x.Key == key);
    }

    public async Task SetAsync(Cookie cookie, CancellationToken cancellationToken = default)
    {
        await SetAsync(cookie.Key, cookie.Value, cookie.Expiration, cancellationToken);
    }

    public async Task SetAsync(string key, string value, DateTimeOffset? expiration,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new Exception("Key is required when setting a cookie.");
        await js.InvokeVoidAsync("eval", cancellationToken,
            $"document.cookie = \"{key}={value}; expires={expiration:R}; path=/\"");
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new Exception("Key is required when removing a cookie.");
        await js.InvokeVoidAsync("eval", cancellationToken,
            $"document.cookie = \"{key}=; expires=Thu, 01 Jan 1970 00:00:01 GMT; path=/\"");
    }
}
using Microsoft.JSInterop;

namespace JobHuntTracker.Web;

public class AuthState(IJSRuntime js)
{
    private const string Key = "jobhunt.token";
    private bool _loaded;

    public string? Token { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrEmpty(Token);

    public async Task InitAsync()
    {
        if (_loaded) return;
        Token = await js.InvokeAsync<string?>("sessionStorage.getItem", Key);
        _loaded = true;
    }

    public async Task SignInAsync(string token)
    {
        Token = token;
        _loaded = true;
        await js.InvokeVoidAsync("sessionStorage.setItem", Key, token);
    }

    public async Task SignOutAsync()
    {
        Token = null;
        await js.InvokeVoidAsync("sessionStorage.removeItem", Key);
    }
}
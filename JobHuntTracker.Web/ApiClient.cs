using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobHuntTracker.Web;

public class ApiClient(HttpClient http, AuthState auth)
{
    private static readonly JsonSerializerOptions Json =
        new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    public Task<(bool Ok, string? Error)> LoginAsync(string email, string password) =>
        AuthenticateAsync("api/auth/login", email, password);

    public Task<(bool Ok, string? Error)> RegisterAsync(string email, string password) =>
        AuthenticateAsync("api/auth/register", email, password);

    public async Task<List<JobApplication>> GetAllAsync()
    {
        using var res = await SendAsync(HttpMethod.Get, "api/jobapplications");
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<List<JobApplication>>(Json) ?? [];
    }

    public async Task AddAsync(NewApplication application)
    {
        using var res = await SendAsync(HttpMethod.Post, "api/jobapplications", application);
        res.EnsureSuccessStatusCode();
    }

    public async Task MoveAsync(Guid id, ApplicationStatus status)
    {
        using var res = await SendAsync(HttpMethod.Patch, $"api/jobapplications/{id}/status",
                                        new StatusChange(status));
        res.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        using var res = await SendAsync(HttpMethod.Delete, $"api/jobapplications/{id}");
        res.EnsureSuccessStatusCode();
    }

    private async Task<(bool Ok, string? Error)> AuthenticateAsync(string path, string email, string password)
    {
        using var res = await http.PostAsJsonAsync(path, new Credentials(email, password), Json);
        if (!res.IsSuccessStatusCode)
            return (false, await ReadErrorAsync(res));

        var body = await res.Content.ReadFromJsonAsync<AuthResponse>(Json);
        await auth.SignInAsync(body!.Token);
        return (true, null);
    }

    // One place that attaches the token and handles expired sessions
    private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, object? body = null)
    {
        using var req = new HttpRequestMessage(method, url);
        if (auth.Token is not null)
            req.Headers.Authorization = new("Bearer", auth.Token);
        if (body is not null)
            req.Content = JsonContent.Create(body, body.GetType(), options: Json);

        var res = await http.SendAsync(req);
        if (res.StatusCode == HttpStatusCode.Unauthorized)
        {
            await auth.SignOutAsync();
            throw new UnauthorizedAccessException();
        }
        return res;
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage res)
    {
        try
        {
            var doc = await res.Content.ReadFromJsonAsync<JsonElement>(Json);
            if (doc.TryGetProperty("error", out var e) && e.GetString() is { } message)
                return message;
        }
        catch (JsonException) { }

        return res.StatusCode == HttpStatusCode.BadRequest
            ? "Please check your input (password needs 8+ characters)."
            : $"Something went wrong ({(int)res.StatusCode}).";
    }
}
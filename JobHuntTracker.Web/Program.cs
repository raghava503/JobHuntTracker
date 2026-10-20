using JobHuntTracker.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBase = builder.Configuration["ApiBaseUrl"]
              ?? throw new InvalidOperationException("ApiBaseUrl is missing in wwwroot/appsettings.json");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(apiBase) });
builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<ApiClient>();

await builder.Build().RunAsync();
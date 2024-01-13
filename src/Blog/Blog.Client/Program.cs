using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(ServiceProvider => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7145")
});

await builder.Build().RunAsync();

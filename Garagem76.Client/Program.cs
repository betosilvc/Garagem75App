using Garagem76.Client;
using Garagem76.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5244/")
});

builder.Services.AddScoped<ClienteApi>();

builder.Services.AddScoped<VeiculoApi>();

builder.Services.AddScoped<PecaApi>();

builder.Services.AddScoped<OrdemServicoApi>();


await builder.Build().RunAsync();
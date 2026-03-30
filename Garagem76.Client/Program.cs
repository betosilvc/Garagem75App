using Garagem76.Client;
using Garagem76.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");


builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7244/")
    };

    return client;
});
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<ClienteApi>();

builder.Services.AddScoped<VeiculoApi>();

builder.Services.AddScoped<PecaApi>();

builder.Services.AddScoped<OrdemServicoApi>();

builder.Services.AddScoped<TipoUsuarioApi>();

builder.Services.AddScoped<UsuarioApi>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();


await builder.Build().RunAsync();
using Garagem75.Shared;
using Garagem75.Shared.Dtos;
using System.Net.Http.Json;

public class ClienteApi
{
    private readonly HttpClient _http;

    public ClienteApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ClienteDto>> GetAll()
    {
        return await _http.GetFromJsonAsync<List<ClienteDto>>("api/cliente") ?? new();
    }

    public async Task<ClienteDto?> GetById(int id)
    {
        return await _http.GetFromJsonAsync<ClienteDto>($"api/cliente/{id}");
    }

    public async Task Create(ClienteDto cliente)
    {
        var resp = await _http.PostAsJsonAsync("api/cliente", cliente);
        resp.EnsureSuccessStatusCode();
    }
    public async Task Update(int id, ClienteDto cliente)
    {
        var resp = await _http.PutAsJsonAsync($"api/cliente/{id}", cliente);
        resp.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id)
    {
        var resp = await _http.DeleteAsync($"api/cliente/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeleteEndereco(int id)
    {
        await _http.DeleteAsync($"api/cliente/endereco/{id}");
    }
}
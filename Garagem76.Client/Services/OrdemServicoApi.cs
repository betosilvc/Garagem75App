using Garagem75.Shared.Dtos;
using System.Net.Http.Json;

public class OrdemServicoApi
{
    private readonly HttpClient _http;

    public OrdemServicoApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<OrdemServicoDto>> GetAll()
        => await _http.GetFromJsonAsync<List<OrdemServicoDto>>("api/ordemservico") ?? new();

    public async Task<OrdemServicoDto?> GetById(int id)
        => await _http.GetFromJsonAsync<OrdemServicoDto>($"api/ordemservico/{id}");

    public async Task Create(OrdemServicoDto dto)
        => await _http.PostAsJsonAsync("api/ordemservico", dto);

    public async Task Finalizar(int id)
        => await _http.PutAsync($"api/ordemservico/{id}/finalizar", null);
}
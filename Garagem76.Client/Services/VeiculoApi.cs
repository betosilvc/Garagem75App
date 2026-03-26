using Garagem75.Shared.Dtos;
using Garagem75.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

public class VeiculoApi
{
    private readonly HttpClient _http;

    public VeiculoApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<VeiculoDto>> GetAll()
        => await _http.GetFromJsonAsync<List<VeiculoDto>>("api/veiculo");

    public async Task<VeiculoDto> GetById(int id)
        => await _http.GetFromJsonAsync<VeiculoDto>($"api/veiculo/{id}");

    public async Task Create(VeiculoDto v)
        => await _http.PostAsJsonAsync("api/veiculo", v);

    public async Task Update(int id, VeiculoDto v)
        => await _http.PutAsJsonAsync($"api/veiculo/{id}", v);

    public async Task Delete(int id)
        => await _http.DeleteAsync($"api/veiculo/{id}");

    public async Task<string?> UploadFoto(int id, IBrowserFile file)
    {
        var content = new MultipartFormDataContent();

        var stream = file.OpenReadStream(5 * 1024 * 1024); // 5MB
        var fileContent = new StreamContent(stream);

        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

        content.Add(fileContent, "file", file.Name);

        var response = await _http.PostAsync($"api/veiculo/{id}/upload", content);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<UploadResult>();

        return result?.fotoUrl;
    }

    public async Task<List<VeiculoDto>> GetByCliente(int clienteId)
    {
        return await _http.GetFromJsonAsync<List<VeiculoDto>>(
            $"api/veiculo/cliente/{clienteId}"
        ) ?? new();
    }

    public class UploadResult
    {
        public string? fotoUrl { get; set; }
    }
}
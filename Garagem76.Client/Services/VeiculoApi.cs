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
        try
        {
            var content = new MultipartFormDataContent();

            // 1. Aumente o limite para suportar câmeras de alta resolução
            long maxFileSize = 1024 * 1024 * 15; // 15MB

            // 2. Leia o stream para um buffer (ajuda na estabilidade do upload via celular)
            var buffer = new byte[file.Size];
            await file.OpenReadStream(maxFileSize).ReadAsync(buffer);

            var fileContent = new ByteArrayContent(buffer);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.Name);

            // 3. Verifique se a URL absoluta da API está correta aqui
            var response = await _http.PostAsync($"api/veiculo/{id}/upload", content);

            if (!response.IsSuccessStatusCode)
            {
                // Opcional: Logar o erro para saber se é 413 (Payload too large) ou 404
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro no upload: {response.StatusCode} - {errorContent}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<UploadResult>();
            return result?.fotoUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção no upload: {ex.Message}");
            return null;
        }
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
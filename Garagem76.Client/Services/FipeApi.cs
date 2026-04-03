using Garagem75.Shared.Dtos;
using System.Net.Http.Json;

public class FipeApi
{
    private readonly HttpClient _http;

    public FipeApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<MarcaDto>> GetMarcas()
    {
        return await _http.GetFromJsonAsync<List<MarcaDto>>("carros/marcas")
               ?? new();
    }

    public async Task<List<ModeloDto>> GetModelos(string codigoMarca)
    {
        var response = await _http.GetFromJsonAsync<ModeloResponseDto>(
            $"carros/marcas/{codigoMarca}/modelos"
        );

        return response?.modelos ?? new();
    }

    public async Task<List<AnoDto>> GetAnos(string codigoMarca, string codigoModelo)
    {
        return await _http.GetFromJsonAsync<List<AnoDto>>(
            $"carros/marcas/{codigoMarca}/modelos/{codigoModelo}/anos"
        ) ?? new();
    }
}
using Garagem75.Shared.Models;

public class VeiculoApi
{
    private readonly HttpClient _http;

    public VeiculoApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Veiculo>> GetAll()
        => await _http.GetFromJsonAsync<List<Veiculo>>("api/veiculo");

    public async Task<Veiculo> Get(int id)
        => await _http.GetFromJsonAsync<Veiculo>($"api/veiculo/{id}");

    public async Task Create(Veiculo v)
        => await _http.PostAsJsonAsync("api/veiculo", v);

    public async Task Update(int id, Veiculo v)
        => await _http.PutAsJsonAsync($"api/veiculo/{id}", v);

    public async Task Delete(int id)
        => await _http.DeleteAsync($"api/veiculo/{id}");
}
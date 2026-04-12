using Garagem75.Shared.Dtos;
using System.Net.Http.Json;

public class DashboardApi
{
    private readonly HttpClient _http;

    public DashboardApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<decimal> GetFaturamentoDia()
        => await _http.GetFromJsonAsync<decimal>("api/dashboard/faturamento-dia");

    public async Task<List<ItemGrafico>> GetFabricantes()
        => await _http.GetFromJsonAsync<List<ItemGrafico>>("api/dashboard/fabricantes");

    public async Task<List<ItemGrafico>> GetMarcasPecas()
        => await _http.GetFromJsonAsync<List<ItemGrafico>>("api/dashboard/marcas-pecas");

    public async Task<DashboardDto> GetDashboard()
        => await _http.GetFromJsonAsync<DashboardDto>("api/dashboard");
}

// ── Gráficos simples ──────────────────────────────────────────────
public class ItemGrafico
{
    public string Nome { get; set; }
    public int Total { get; set; }
}


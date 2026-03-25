public class OrdemServicoPecaApi
{
    private readonly HttpClient _http;

    public OrdemServicoPecaApi(HttpClient http)
    {
        _http = http;
    }

    public async Task Add(int osId, int pecaId, int qtd)
    {
        await _http.PostAsync($"api/ordemservicopeca?ordemId={osId}&pecaId={pecaId}&quantidade={qtd}", null);
    }

    public async Task Remove(int osId, int pecaId)
    {
        await _http.DeleteAsync($"api/ordemservicopeca?ordemId={osId}&pecaId={pecaId}");
    }
}
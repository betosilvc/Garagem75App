using Garagem75.Shared.Dtos;
using System.Net.Http.Json;

namespace Garagem76.Client.Services
{
    public class PecaApi
    {
        private readonly HttpClient _http;

        public PecaApi(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PecaDto>> GetAll()
        {
            return await _http.GetFromJsonAsync<List<PecaDto>>("api/peca") ?? new();
        }

        public async Task<PecaDto?> GetById(int id)
        {
            return await _http.GetFromJsonAsync<PecaDto>($"api/peca/{id}");
        }

        public async Task Create(PecaDto peca)
        {
            var resp = await _http.PostAsJsonAsync("api/peca", peca);
            resp.EnsureSuccessStatusCode();
        }
        public async Task Update(int id, PecaDto peca)
        {
            var resp = await _http.PutAsJsonAsync($"api/peca/{id}", peca);
            resp.EnsureSuccessStatusCode();
        }

        public async Task Delete(int id)
        {
            var resp = await _http.DeleteAsync($"api/peca/{id}");
            resp.EnsureSuccessStatusCode();
        }

       
    }
}

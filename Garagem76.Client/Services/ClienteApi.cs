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

    public async Task<HttpResponseMessage> Create(ClienteDto cliente)
    {
        return await _http.PostAsJsonAsync("api/cliente", cliente);
        // 👆 removeu o EnsureSuccessStatusCode — agora quem trata é o Blazor
    }
    public async Task<HttpResponseMessage> Update(int id, ClienteDto cliente)
    {
        return await _http.PutAsJsonAsync($"api/cliente/{id}", cliente);
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
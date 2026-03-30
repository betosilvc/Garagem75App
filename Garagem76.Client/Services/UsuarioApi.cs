using Garagem75.Shared.Dtos;
using System.Net.Http.Json;

namespace Garagem76.Client.Services
{
    public class UsuarioApi
    {
        private readonly HttpClient _http;

        public UsuarioApi(HttpClient http)
        {
            _http = http;
        }

        // GET ALL
        public async Task<List<UsuarioDto>> GetAll()
        {
            return await _http.GetFromJsonAsync<List<UsuarioDto>>("api/usuario")
                   ?? new List<UsuarioDto>();
        }

        // GET BY ID
        public async Task<UsuarioDto?> GetById(int id)
        {
            return await _http.GetFromJsonAsync<UsuarioDto>($"api/usuario/{id}");
        }

        // CREATE
        public async Task Create(UsuarioDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/usuario", dto);
            response.EnsureSuccessStatusCode();
        }

        // UPDATE
        public async Task Update(int id, UsuarioDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/usuario/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        // DELETE (soft delete)
        public async Task Delete(int id)
        {
            var response = await _http.DeleteAsync($"api/usuario/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<string?> Login(string email, string senha)
        {
            var response = await _http.PostAsJsonAsync("api/usuario/login", new
            {
                Email = email,
                Senha = senha
            });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            return result?.Token;
        }
    }
}
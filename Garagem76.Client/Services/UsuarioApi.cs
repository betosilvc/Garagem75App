using Garagem75.Shared.Dtos;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Garagem76.Client.Services
{
    public class UsuarioApi
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public UsuarioApi(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task AddToken()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

            // 🔥 LIMPA antes (evita duplicação)
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<UsuarioDto>> GetAll()
        {
            await AddToken();
            return await _http.GetFromJsonAsync<List<UsuarioDto>>("api/usuario")
                   ?? new List<UsuarioDto>();
        }

        public async Task<UsuarioDto?> GetById(int id)
        {
            await AddToken();
            return await _http.GetFromJsonAsync<UsuarioDto>($"api/usuario/{id}");
        }

        public async Task Create(UsuarioDto dto)
        {
            await AddToken();
            var response = await _http.PostAsJsonAsync("api/usuario", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task Update(int id, UsuarioDto dto)
        {
            await AddToken();
            var response = await _http.PutAsJsonAsync($"api/usuario/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(int id)
        {
            await AddToken();
            var response = await _http.DeleteAsync($"api/usuario/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task Reativar(int id)
        {
            try
            {
                var response = await _http.PutAsync($"api/usuario/{id}/reativar", null);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao reativar: " + ex.Message);
                throw;
            }
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
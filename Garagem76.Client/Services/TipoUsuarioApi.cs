using Garagem75.Shared.Dtos;
using System.Net.Http.Json;

namespace Garagem76.Client.Services
{
    public class TipoUsuarioApi
    {
        private readonly HttpClient _http;

        public TipoUsuarioApi(HttpClient http)
        {
            _http = http;
        }

        // GET ALL
        public async Task<List<TipoUsuarioDto>> GetAll()
        {
            return await _http.GetFromJsonAsync<List<TipoUsuarioDto>>("api/tipousuario")
                   ?? new List<TipoUsuarioDto>();
        }

        // GET BY ID
        public async Task<TipoUsuarioDto?> GetById(int id)
        {
            return await _http.GetFromJsonAsync<TipoUsuarioDto>($"api/tipousuario/{id}");
        }
    }
}
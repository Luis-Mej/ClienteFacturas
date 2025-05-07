using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dtos;
using Dtos.UsuariosDTOS;
using ServiciosAPI.Rutas;

namespace ServiciosAPI.Servicios
{
    public class UsuarioServicio
    {
        private readonly HttpClient _client;

        public UsuarioServicio(string token)
        {
            _client = new HttpClient();
        }

        public async Task<bool> RegistrarUsuarioAsync(UsuarioDTOs usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(ApiRutas.UrlBase + "Usuarios", content);

            if (!response.IsSuccessStatusCode)
                return false;
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultado = JsonSerializer.Deserialize<ResponseBase<UsuarioLoginDTO>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (resultado?.Data == null)
                return false;
            return resultado.Data != null;
        }

        public async Task<bool> ActualizarUsuarioAsync(UsuarioDTOs usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(ApiRutas.UrlBase + $"Usuarios/{usuario.Id}", content);
            if (!response.IsSuccessStatusCode)
                return false;
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultado = JsonSerializer.Deserialize<ResponseBase<UsuarioLoginDTO>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (resultado?.Data == null)
                return false;
            return resultado.Data != null;
        }
    }
}

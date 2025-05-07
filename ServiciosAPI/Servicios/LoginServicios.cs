using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dtos;
using Dtos.UsuariosDTOS;
using Sesion;

namespace ServiciosAPI.Servicios
{
    public class LoginServicios
    {
        private readonly HttpClient _client;
        public LoginServicios()
        {
            _client = new HttpClient();
        }

        public async Task<bool> LoginAsync(UsuarioLoginDTO usuarioLoginDTO)
        {
            var json = JsonSerializer.Serialize(usuarioLoginDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://localhost:7037/api/Usuarios/Login", content);

            if (!response.IsSuccessStatusCode)
                return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultado = JsonSerializer.Deserialize<ResponseBase<string>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (resultado?.Data == null)
                return false;

            SesionActual.Token = resultado.Data;
            SesionActual.IdUsuario = JwtHelper.ObtenerIdUsuarioDesdeToken(resultado.Data);

            return SesionActual.Token != null && SesionActual.IdUsuario != 0;
        }

        public async Task<bool> RegistrarUsuarioAsync(UsuarioLoginDTO usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://localhost:7037/api/Usuarios", content);
            return response.IsSuccessStatusCode;
        }
    }
}

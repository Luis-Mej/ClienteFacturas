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
using ServiciosAPI.JwtServicio;
using ServiciosAPI.Rutas;

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

            var response = await _client.PostAsync(ApiRutas.UrlBase + "Login", content);

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
            SesionActual.IdUsuario = JwtServicio.JwtServicio.ObtenerIdUsuarioToken(resultado.Data);

            return SesionActual.Token != null && SesionActual.IdUsuario != 0;
        }
    }
}

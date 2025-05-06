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

        public async void Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            var json = JsonSerializer.Serialize(usuarioLoginDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(ApiRutas.Login.Autenticar, content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<ApiRutas>(responseBody);
                // Aquí puedes guardar el token en la sesión o en un lugar seguro
            }
            else
            {
                throw new Exception();
            }
        }
    }
}

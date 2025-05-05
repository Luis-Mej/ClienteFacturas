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

        //public async Task<List<UsuarioDTOs>> ObtenerUsuariosAsync()
        //{
        //    var response = await _client.GetAsync(ApiRutas.Usuarios.ObtenerTodos);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = await response.Content.ReadAsStringAsync();
        //        var resultado = JsonSerializer.Deserialize<ResponseBase<List<UsuarioDTOs>>>(json, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });
        //        return resultado?.Data ?? new List<UsuarioDTOs>();
        //    }
        //    return new List<UsuarioDTOs>();
        //}
    }
}

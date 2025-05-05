using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Dtos.ProductosDTOS;
using Dtos;
using System.Text.Json;
using ServiciosAPI.Rutas;

namespace ServiciosAPI.Servicios
{
    public class ProductoServicio
    {
        private readonly HttpClient _client;

        public ProductoServicio(string token)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<ProductoDTO>> ObtenerProductosAsync()
        {
            var response = await _client.GetAsync("https://localhost:7037/api/Productos");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultado = JsonSerializer.Deserialize<ResponseBase<List<ProductoDTO>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return resultado?.Data ?? new List<ProductoDTO>();
            }
            return new List<ProductoDTO>();
        }

        public async Task<bool> GuardarProductoAsync(ProductoDTO producto)
        {
            var json = JsonSerializer.Serialize(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesta = producto.Id == 0
                ? await _client.PostAsync(ApiRutas.Productos.Crear, content)
                : await _client.PutAsync(ApiRutas.Productos.Actualizar, content);

            return respuesta.IsSuccessStatusCode;
        }
    }
}

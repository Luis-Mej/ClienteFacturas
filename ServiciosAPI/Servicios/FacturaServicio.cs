using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Dtos.FacturasDTOS;
using Dtos;
using System.Threading.Tasks;
using ServiciosAPI.Rutas;
using System.Text.Json;
using Dto.FacturasDTOS;

namespace ServiciosAPI.Servicios
{
    public class FacturaServicio
    {
        private readonly HttpClient _client;
        public FacturaServicio(string token)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<FacturasDTOs>> ObtenerFacturasAsync()
        {
            var response = await _client.GetAsync(ApiRutas.Facturas.ObtenerTodos);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultado = JsonSerializer.Deserialize<ResponseBase<List<FacturasDTOs>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return resultado?.Data ?? new List<FacturasDTOs>();
            }
            return new List<FacturasDTOs>();
        }

        public async Task<bool> GuardarFacturaAsync(FacturasDTOs factura)
        {
            var json = JsonSerializer.Serialize(factura);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = factura.IdFactura == 0
                ? await _client.PostAsync(ApiRutas.Facturas.Crear, content)
                : await _client.GetAsync(ApiRutas.Facturas.ObtenerTodos);
            return respuesta.IsSuccessStatusCode;
        }
    }
}

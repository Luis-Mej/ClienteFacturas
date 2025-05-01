using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API_Web.API
{
    public class Web_API
    {
        public Web_API()
        {
            try
            {//revisar los enlaces de la API
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7037/api/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.Equals(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri("https://localhost:7037/api/");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Web API: {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosAPI.Rutas
{
    public class ApiRutas
    {
        public const string UrlBase = "https://localhost:7037/api/";

        public static class Productos
        {
            public const string ObtenerTodos = UrlBase + "Productos";
            public const string Crear = UrlBase + "Productos";
            public const string Actualizar = UrlBase + "Productos/{id}";
            public const string Eliminar = UrlBase + "Productos/{id}";
        }

        public static class Usuarios
        {
            public const string Crear = UrlBase + "Usuarios";
            public const string Actualizar = UrlBase + "Usuarios/{id}";
        }

        public static class Facturas
        {
            public const string ObtenerTodos = UrlBase + "Facturas";
            public const string ObtenerPorId = UrlBase + "Facturas/{id}";
            public const string Crear = UrlBase + "Facturas";
        }

        public static class Login
        {
            public const string Autenticar = UrlBase + "Login";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiciosAPI.JwtServicio
{
    public class JwtServicio
    {
        public static int ObtenerIdUsuarioToken(string token)
        {
            try
            {
                var partes = token.Split('.');
                if (partes.Length != 3) return 0;
                var payload = partes[1];
                var jsonBytes = Base64UrlDecode(payload);
                var jsonString = Encoding.UTF8.GetString(jsonBytes);
                var doc = JsonDocument.Parse(jsonString);
                var root = doc.RootElement;
                if (root.TryGetProperty("idUsuario", out var idUsuarioProp))
                {
                    var idUsuarioStr = idUsuarioProp.GetString();
                    if (int.TryParse(idUsuarioStr, out var idUsuario))
                        return idUsuario;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }

        private static byte[] Base64UrlDecode(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');
            switch (input.Length % 4)
            {
                case 2: input += "=="; break;
                case 3: input += "="; break;
                case 0: break;
                default: throw new FormatException("Token inválido");
            }
            return Convert.FromBase64String(input);
        }
    }
}

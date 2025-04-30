using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Windows.Forms;
using Dtos.UsuariosDTOS;
using Sesion;
using Dtos;

namespace ClientFacturas
{
    public partial class EditUsuario : Form
    {
        private UsuarioDTOs EditarUsuario;
        public EditUsuario(UsuarioDTOs usuarioDTOs)
        {
            InitializeComponent();
            EditarUsuario = usuarioDTOs;

            if (EditarUsuario != null || usuarioDTOs != null)
            {
                if (EditarUsuario == null || string.IsNullOrEmpty(EditarUsuario.Nombre))
                {
                    MessageBox.Show("Error: El usuario no tiene nombre válido para editar.");
                    this.Close();
                    return;
                }
                txtUsuario.Text = usuarioDTOs?.Nombre;
                txtContrasenaNueva.Text = usuarioDTOs?.Contrasenia;
            }
            btnActualizar.Click -= btnActualizar_Click;
            btnActualizar.Click += btnActualizar_Click;
        }
        //Revisar mas tardecito ^^
        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            if (SesionActual.IdUsuario == null)
            {
                MessageBox.Show("Error: Usuario inválido");
                return;
            }

            var actulizarDatos = new UsuarioDTOs();
            actulizarDatos.Id = SesionActual.IdUsuario;
            actulizarDatos.Nombre = txtUsuario.Text;
            actulizarDatos.Contrasenia = txtContrasenaNueva.Text;

            HttpClient client = new HttpClient();
            string Url = $"https://localhost:7037/api/Usuarios/{SesionActual.IdUsuario}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SesionActual.Token);

            var json = JsonSerializer.Serialize(actulizarDatos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var respuesta= await client.PutAsync(Url, content);

            if (respuesta.IsSuccessStatusCode)
            {
                var respuestaContent = await respuesta.Content.ReadAsStringAsync();
                var respuestaObj = JsonSerializer.Deserialize<ResponseBase<UsuarioDTOs>>(respuestaContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                MessageBox.Show("Usuario actualizado");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: No se pudo actualizar el usuario.");
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            MenuPrincipal.VolverAlMenuPrincipal(this);
        }
    }
}

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
using System.Windows.Forms;
using Dtos.UsuariosDTOS;

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

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            btnActualizar.Enabled = false;

            try
            {
                string usuario = txtUsuario.Text.Trim();
                string contrasena = txtContrasenaNueva.Text.Trim();

                if (string.IsNullOrEmpty(usuario) || contrasena == null)
                {
                    MessageBox.Show("Se deben llenar todos los campos.");
                    return;
                }

                UsuarioDTOs usuarioDTOs = new UsuarioDTOs(usuario, contrasena);

                using (HttpClient client = new HttpClient())
                {
                    var json = JsonSerializer.Serialize(usuarioDTOs);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage respuesta;

                    if (EditarUsuario == null)
                    {
                        respuesta = client.PutAsync("https://localhost:7097/api/Usuarios", content).Result;

                        if (respuesta.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Usuario actualizado exitosamente.");
                            DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Error al actualizar el usuario.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: No se puede editar el usuario.");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                btnActualizar.Enabled = true;
            }
        }
    }
}

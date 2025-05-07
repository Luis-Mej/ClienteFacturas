using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dtos.UsuariosDTOS;
using System.Text.Json;
using Dtos;
using System.Net.Http;
using ServiciosAPI.Servicios;

namespace ClientFacturas
{
    public partial class Registrar : Form
    {
        private UsuarioDTOs EditarUsuario;
        private readonly UsuarioServicio _usuarioServicio;
        public Registrar()
        {
            InitializeComponent();
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea cancelar el registro del usuario", "Cancelar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Login login = new Login();
                login.FormClosed += (s, args) => this.Close();
                this.Hide();
                login.Show();
            }
            return;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            btnGuardar.Enabled = false;
            try
            {
                int id = EditarUsuario?.Id ?? 0;
                string nombre = txtNombre.Text.Trim();
                string contrasenia = txtContrasenia.Text.Trim();
                string contraseniaConfirmar = txtConfirContrasenia.Text.Trim();

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contrasenia) || string.IsNullOrEmpty(contraseniaConfirmar))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    btnGuardar.Enabled = true;
                    return;
                }
                if (contrasenia != contraseniaConfirmar)
                {
                    MessageBox.Show("Las contraseñas no coinciden.");
                    btnGuardar.Enabled = true;
                    return;
                }

                var UsuarioDTOs = new UsuarioDTOs
                {
                    Nombre = txtNombre.Text,
                    Contrasenia = txtContrasenia.Text,
                };
                var registroExitoso = await _usuarioServicio.RegistrarUsuarioAsync(UsuarioDTOs);

                if (!registroExitoso)
                {
                    MessageBox.Show("Error al registrar el usuario");
                    btnGuardar.Enabled = true;
                    return;
                }

                MessageBox.Show("Usuario registrado exitosamente");
                Login login = new Login();
                login.FormClosed += (s, args) => this.Close();
                this.Hide();
                login.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el usuario: " + ex.Message);
                btnGuardar.Enabled = true;
                return;
            }
        }
    }
}

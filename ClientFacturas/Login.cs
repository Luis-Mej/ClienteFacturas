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
using Dtos;
using Dtos.UsuariosDTOS;
using ServiciosAPI.Rutas;
using ServiciosAPI.Servicios;
using Sesion;

namespace ClientFacturas
{
    public partial class Login : Form
    {
        private readonly LoginServicios loginServicios;
        public Login()
        {
            InitializeComponent();
            loginServicios = new LoginServicios();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var usuarioLoginDTO = new UsuarioLoginDTO
            {
                Nombre = txtLogin.Text,
                Contrasenia = txtContrasena.Text
            };

            var loginExitoso = await loginServicios.LoginAsync(usuarioLoginDTO);

            if (!loginExitoso)
            {
                MessageBox.Show("Error al iniciar sesión");
                return;
            }

            MessageBox.Show("Login exitoso");
            Hide();

            var menuPrincipal = new MenuPrincipal();
            menuPrincipal.FormClosed += (s, args) => Close();
            menuPrincipal.Show();
        }

        private void linkRegistro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registrar registrar = new Registrar();
            registrar.Show();
            this.Hide();
        }
    }
}

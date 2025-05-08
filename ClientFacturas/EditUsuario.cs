using System;
using System.Windows.Forms;
using Dtos.UsuariosDTOS;
using ServiciosAPI.Servicios;
using Sesion;

namespace ClientFacturas
{
    public partial class EditUsuario : Form
    {
        private UsuarioDTOs EditarUsuario;
        private readonly UsuarioServicio usuarioServicio;
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

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            if (SesionActual.IdUsuario == null)
            {
                MessageBox.Show("Error: Usuario inválido");
                return;
            }

            var usuarioDTO = new UsuarioDTOs
            {
                Id = SesionActual.IdUsuario,
                Nombre = txtUsuario.Text,
                Contrasenia = txtContrasenaNueva.Text,
            };

            //var actualizarDatos = new UsuarioDTOs();
            //actulizarDatos.Id = SesionActual.IdUsuario;
            //actulizarDatos.Nombre = txtUsuario.Text;
            //actulizarDatos.Contrasenia = txtContrasenaNueva.Text;

            var acualizacionExitosa = await usuarioServicio.ActualizarUsuarioAsync(usuarioDTO);

            if (!acualizacionExitosa)
            {
                MessageBox.Show("Error al actualizar el usuario");
                return;
            }

            MessageBox.Show("Datos actualizados");
            Hide();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            MenuPrincipal.VolverAlMenuPrincipal(this);
        }
    }
}

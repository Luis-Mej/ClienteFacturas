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

namespace ClientFacturas
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión?", "Confirmación", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Login login = new Login();
                login.FormClosed += (s, args) => this.Close();
                this.Hide();
                login.Show();
            }
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            Producto productos = new Producto();
            productos.FormClosed += (s, args) => this.Show();
            this.Hide();
            productos.Show();
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {
            EditUsuario usuario = new EditUsuario(null);
            usuario.FormClosed += (s, args) => this.Show();
            this.Hide();
            usuario.Show();
        }

        private void btnFacturasCab_Click(object sender, EventArgs e)
        {
            VerFacturas verFacturas = new VerFacturas();
            verFacturas.FormClosed += (s, args) => this.Show();
            this.Hide();
            verFacturas.Show();
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            Compras compras = new Compras();
            compras.FormClosed += (s, args) => this.Show();
            this.Hide();
            compras.Show();
        }
    }
}

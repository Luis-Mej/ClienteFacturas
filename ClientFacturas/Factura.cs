using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientFacturas.Reports;
using Dtos;
using Dtos.FacturasDTOS;
using Dtos.ProductosDTOS;
using Newtonsoft.Json;
using Sesion;

namespace ClientFacturas
{
    public partial class Factura : Form
    {
        private List<FacturaDetDTO> productoCarrito;
        public Factura(List<FacturaDetDTO> productoEnCarrito)
        {
            InitializeComponent();
            this.productoCarrito = productoEnCarrito;
            Load += Factura_Load;
        }
        private async void Factura_Load(object sender, EventArgs e)
        {
            await CargarProductosDelCarrito();
        }

        private bool ValidarCampos()
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtCliente.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("El campo 'Cliente' solo puede contener letras y espacios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtIdentificacion.Text, @"^\d{10}$"))
            {
                MessageBox.Show("El campo 'Identificación' solo puede contener números.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtTelefono.Text, @"^\d{10}$"))
            {
                MessageBox.Show("El campo 'Teléfono' solo puede contener números.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.(com)$"))
            {
                MessageBox.Show("El campo 'Email' no es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async Task CargarProductosDelCarrito()
        {
            dgvDetalles.Columns.Clear();
            dgvDetalles.DataSource = null;
            dgvDetalles.AutoGenerateColumns = false;

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "IdProducto",
                Name = "IdProducto",
                Visible = false
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreProducto",
                HeaderText = "Nombre del Producto",
                Name = "NombreProducto",
                ReadOnly = true
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "Precio Unitario",
                Name = "PrecioUnitario",
                ReadOnly = true
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "Cantidad",
                Name = "Cantidad",
                ReadOnly = true
            });

            dgvDetalles.DataSource = productoCarrito;

            CalcularTotales();
        }

        private void CalcularTotales()
        {
            decimal totalSubtotal = 0;
            decimal totalIVA = 0;
            decimal totalGeneral = 0;

            foreach (var item in productoCarrito)
            {

                decimal precio = item.PrecioUnitario;
                int cantidad = item.Cantidad ?? 0;

                decimal subtotal = precio * cantidad;
                decimal iva = subtotal * 0.15M;
                decimal total = subtotal + iva;

                totalSubtotal += subtotal;
                totalIVA += iva;
                totalGeneral += total;
            }

            txtSubTotal.Text = totalSubtotal.ToString("0.00");
            txtIva.Text = totalIVA.ToString("0.00");
            txtTotal.Text = totalGeneral.ToString("0.00");
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }

            if (SesionActual.IdUsuario == null)
            {
                MessageBox.Show("Error: Usuario inválido");
                return;
            }

            var guardarFactura = new FacturaDTO();

            guardarFactura.FacturaCab = new FacturaCabDTO(txtCliente.Text, txtIdentificacion.Text, txtTelefono.Text, txtEmail.Text, DateTime.Now, decimal.Parse(txtSubTotal.Text), decimal.Parse(txtIva.Text), decimal.Parse(txtTotal.Text), SesionActual.IdUsuario);

            guardarFactura.Detalles = productoCarrito;

            HttpClient client = new HttpClient();
            string Url = "https://localhost:7037/api/Facturas";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SesionActual.Token);

            var json = System.Text.Json.JsonSerializer.Serialize(guardarFactura);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Url, content);


            if (response.IsSuccessStatusCode)
            {
                FacturaVisualDTO facturaVisual = new FacturaVisualDTO();
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = System.Text.Json.JsonSerializer.Deserialize<ResponseBase<FacturaVisualDTO>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseObject != null && responseObject.Data != null)
                {
                    facturaVisual = responseObject.Data;
                }
                else
                {
                    MessageBox.Show("Error al obtener la factura");
                    return;
                }
                MessageBox.Show("Factura guardada exitosamente");
                this.DialogResult = DialogResult.OK;
                FacturaReport facturaReport = new FacturaReport(facturaVisual);
                facturaReport.Show();
            }
            else
            {
                MessageBox.Show("Error al guardar la factura");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cancelar la compra?", "Confirmación", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtIdentificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtIdentificacion_TextChanged(object sender, EventArgs e)
        {
            int numeroMax=10;

            if (txtIdentificacion.Text.Length > numeroMax)
            {
                txtIdentificacion.Text = txtIdentificacion.Text.Substring(0, numeroMax);
                txtIdentificacion.SelectionStart = txtIdentificacion.Text.Length;
            }
        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {
            int numeroMax = 10;

            if (txtTelefono.Text.Length > numeroMax)
            {
                txtTelefono.Text = txtTelefono.Text.Substring(0, numeroMax);
                txtTelefono.SelectionStart = txtTelefono.Text.Length;
            }
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '@' && e.KeyChar != '.' && e.KeyChar != '_')
            {
                e.Handled = true;
            }
        }
    }
}

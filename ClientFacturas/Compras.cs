using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dtos;
using Dtos.ProductosDTOS;
using Sesion;
using Dtos.FacturasDTOS;

namespace ClientFacturas
{
    public partial class Compras : Form
    {
        private List<ProductoDTO> listaProductos = new List<ProductoDTO>();
        public Compras()
        {
            InitializeComponent();
            dgvProductosComprar.AutoGenerateColumns = false;
            Load += Compras_Load;
        }

        private async void Compras_Load(object sender, EventArgs e)
        {
            await CargarProductos();
        }

        private async Task CargarProductos()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SesionActual.Token);
                var respuesta = await client.GetAsync("https://localhost:7037/api/Productos");

                if (respuesta.IsSuccessStatusCode)
                {
                    var json = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<ResponseBase<List<ProductoDTO>>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (resultado != null && resultado.Data != null)
                    {
                        listaProductos = resultado.Data;

                        dgvProductosComprar.Columns.Clear();

                        DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn
                        {
                            Name = "Seleccionado",
                            HeaderText = "",
                            Width = 30
                        };
                        dgvProductosComprar.Columns.Add(checkColumn);

                        dgvProductosComprar.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Nombre",
                            HeaderText = "Nombre"
                        });

                        dgvProductosComprar.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Precio",
                            HeaderText = "Precio"
                        });

                        dgvProductosComprar.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Stock",
                            HeaderText = "Stock"
                        });

                        dgvProductosComprar.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Id",
                            HeaderText = "Id",
                            Visible = false
                        });

                        dgvProductosComprar.DataSource = new BindingList<ProductoDTO>(listaProductos);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener la lista de productos.");
                    }
                }
                else
                {
                    MessageBox.Show("Error al cargar productos desde la API.");
                }
            }
        }

        private List<ProductoDTO> ObtenerProductosSeleccionados()
        {
            var seleccionados = new List<ProductoDTO>();

            foreach (DataGridViewRow fila in dgvProductosComprar.Rows)
            {
                bool marcado = Convert.ToBoolean(fila.Cells["Seleccionado"].Value);
                if (marcado)
                {
                    if (fila.DataBoundItem is ProductoDTO producto)
                    {
                        seleccionados.Add(producto);
                    }
                }
            }

            return seleccionados;
        }

        private void btnCarrito_Click(object sender, EventArgs e)
        {
            var seleccionados = ObtenerProductosSeleccionados();

            if (seleccionados.Count == 0)
            {
                MessageBox.Show("No se han seleccionado productos.");
                return;
            }
            var detallesSeleccionados = seleccionados.Select(p => new FacturaDetDTO
            {
                IdProducto = p.Id,
                NombreProducto = p.Nombre,
                PrecioUnitario = p.Precio,
                Cantidad = 1
            }).ToList();

            Carrito carritoForm = new Carrito(detallesSeleccionados);
            carritoForm.ShowDialog();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.ToLower().Trim();

            var productosFiltrados = listaProductos.Where(p => p.Nombre.ToLower().Contains(filtro) || p.Precio.ToString().Contains(filtro) || p.Stock.ToString().Contains(filtro)).ToList();

            dgvProductosComprar.DataSource = productosFiltrados;
        }
    }
}

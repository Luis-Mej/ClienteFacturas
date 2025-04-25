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
using System.Threading.Tasks;
using System.Windows.Forms;
using Dtos;
using Dtos.FacturasDTOS;
using Sesion;

namespace ClientFacturas
{
    public partial class VerFacturas : Form
    {
        private List<FactCabeceraDTO> Facturas = new List<FactCabeceraDTO>();

        public VerFacturas()
        {
            InitializeComponent();
            dgvFacturas.AutoGenerateColumns = false;
            Load += VerFacturas_Load;
        }

        private async void VerFacturas_Load(object sender, EventArgs e)
        {
            await CargarFacturas();
        }

        private async Task CargarFacturas()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SesionActual.Token);
                var respuesta = await client.GetAsync("https://localhost:7037/api/Facturas");

                if (respuesta.IsSuccessStatusCode)
                {
                    var json = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<ResponseBase<List<FactCabeceraDTO>>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (resultado != null && resultado.Data != null)
                    {
                        Facturas = resultado.Data;
                        dgvFacturas.Columns.Clear();
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "NombreCliente",
                            HeaderText = "Nombre Cliente"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Identificacion",
                            HeaderText = "Identificación"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Telefono",
                            HeaderText = "Teléfono"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Email",
                            HeaderText = "Email"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "FechaCreacion",
                            HeaderText = "Fecha Creación"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "SubTotal",
                            HeaderText = "SubTotal"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Iva",
                            HeaderText = "IVA"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Total",
                            HeaderText = "Total"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "NombreUsuario",
                            HeaderText = "Nombre Usuario"
                        });
                        dgvFacturas.DataSource = new BindingList<FactCabeceraDTO>(Facturas);
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron facturas.");
                    }
                }
                else
                {
                    MessageBox.Show("Error al cargar las facturas: " + respuesta.ReasonPhrase);
                }
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.VolverAlMenu(sender, e);
        }
    }
}

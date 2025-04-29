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
        private List<FacturaCabDTO> Facturas = new List<FacturaCabDTO>();

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
                    var resultado = JsonSerializer.Deserialize<ResponseBase<List<FacturaCabDTO>>>(json, new JsonSerializerOptions
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
                            DataPropertyName = "FechaCreacion",
                            HeaderText = "Fecha"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "Total",
                            HeaderText = "Total"
                        });
                        dgvFacturas.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = "IdUsuario",
                            HeaderText = "Nombre Usuario"
                        });
                        dgvFacturas.DataSource = new BindingList<FacturaCabDTO>(Facturas);
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
            MenuPrincipal.VolverAlMenuPrincipal(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dtos.FacturasDTOS;
using Microsoft.Reporting.WinForms;

namespace ClientFacturas.Reports
{
    public partial class FacturaReport : Form
    {
        FacturaVisualDTO _facturaVisual;
        public FacturaReport(FacturaVisualDTO facturaVisual)
        {
            this._facturaVisual = facturaVisual;
            InitializeComponent();
        }
        private void reportViewer1_Load(object sender, EventArgs e)
        {
            try
            {
                this.facturaBindingSource.DataSource = _facturaVisual.Detalles;
                ReportParameter nombreClienteParameter = new ReportParameter("NombreCliente", _facturaVisual.Cabecera.NombreCliente);
                ReportParameter identificacionParameter = new ReportParameter("Identificacion", _facturaVisual.Cabecera.Identificacion);
                ReportParameter telefonoParameter = new ReportParameter("Telefono", _facturaVisual.Cabecera.Telefono);
                ReportParameter emailParameter = new ReportParameter("Email", _facturaVisual.Cabecera.Email);
                ReportParameter fechaParameter = new ReportParameter("Fecha", _facturaVisual.Cabecera.FechaCreacion.GetValueOrDefault().ToString("dd/MM/yyyy"));
                ReportParameter subtotalParameter = new ReportParameter("SubTotal", _facturaVisual.Cabecera.SubTotal.ToString("0.00"));
                ReportParameter totalParameter = new ReportParameter("Total", _facturaVisual.Cabecera.Total.GetValueOrDefault().ToString("0.00"));
                ReportParameter ivaParameter = new ReportParameter("Iva", _facturaVisual.Cabecera.Iva.GetValueOrDefault().ToString("0.00"));

                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { nombreClienteParameter, identificacionParameter, telefonoParameter, emailParameter, fechaParameter, subtotalParameter, totalParameter, ivaParameter });
                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar el informe. Asegúrese de que los datos sean válidos.");
            }

        }
    }
}

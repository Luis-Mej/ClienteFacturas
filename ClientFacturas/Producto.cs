﻿using Dtos;
using Dtos.ProductosDTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using Dtos.FacturasDTOS;
using System.Net.Http;
using Sesion;
using ServiciosAPI.Servicios;

namespace ClientFacturas
{
    public partial class Producto : Form
    {
        private List<ProductoDTO> listaProductos = new List<ProductoDTO>();
        private readonly ProductoServicio productoServicio;
        public Producto()
        {
            InitializeComponent();
            dgvProductos.AutoGenerateColumns = false;

            string token = SesionActual.Token.Trim();
            productoServicio = new ProductoServicio(token);

            Load += Producto_Load;
        }

        private async void Producto_Load(object sender, EventArgs e)
        {
            await CargarProductos();
        }

        private async Task CargarProductos()
        {
            try
            {
                listaProductos = await productoServicio.ObtenerProductosAsync() ?? new List<ProductoDTO>();

                if (listaProductos.Any())
                {
                    ConfigurarColumnasDataGridView();
                    dgvProductos.DataSource = new BindingList<ProductoDTO>(listaProductos);
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la lista de productos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private void ConfigurarColumnasDataGridView()
        {
            dgvProductos.Columns.Clear();

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre"
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Precio",
                HeaderText = "Precio"
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = "Stock"
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Id",
                Visible = false
            });
        }


        private void txtBuscarProducto_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscarProducto.Text.ToLower().Trim();

            var productosFiltrados = listaProductos.Where(p => p.Nombre.ToLower().Contains(filtro) || p.Precio.ToString().Contains(filtro) || p.Stock.ToString().Contains(filtro)).ToList();

            dgvProductos.DataSource = productosFiltrados;
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is ProductoDTO productoElejido)
            {
                Detalles form = new Detalles(productoElejido);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await CargarProductos();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para editar.");
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is ProductoDTO productoElegido)
            {
                if (productoElegido.Id == 0)
                {
                    MessageBox.Show("No se pudo determinar el ID del producto seleccionado.");
                    return;
                }

                if (MessageBox.Show($"¿Deseas eliminar el producto '{productoElegido.Nombre}'?", "Eliminar Producto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    btnEliminar.Enabled = false;

                    try
                    {
                        var productoEliminado = await productoServicio.EliminarProductoAsync(productoElegido.Id);

                        if (!productoEliminado)
                        {
                            MessageBox.Show("Error al eliminar el producto.");
                            return;
                        }

                        await CargarProductos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error inesperado al eliminar: {ex.Message}");
                    }
                    finally
                    {
                        btnEliminar.Enabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Detalles form = new Detalles();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = CargarProductos();
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            MenuPrincipal.VolverAlMenuPrincipal(this);
        }
    }
}

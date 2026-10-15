using CadGuarida;
using ClnGuarida;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpGuarida
{
    public partial class FrmProducto : Form
    {
        private bool esNuevo;
        public FrmProducto()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var productos = ProductoCln.listarPa(txtParametro.Text.Trim());
            dgvLista.DataSource = productos;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["idCategoria"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["codigo"].HeaderText = "Código";
            dgvLista.Columns["nombre"].HeaderText = "Nombre";
            dgvLista.Columns["descripcion"].HeaderText = "Descripción";
            dgvLista.Columns["pesoKg"].HeaderText = "Peso en Kg";
            dgvLista.Columns["precioCostoUsd"].HeaderText = "Precio en Dólares";
            dgvLista.Columns["precioVentaBob"].HeaderText = "Precio en Bolivianos";
            dgvLista.Columns["stockActual"].HeaderText = "Stock Actual";
            dgvLista.Columns["stockMinimo"].HeaderText = "Stock Minimo";
            dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";
            dgvLista.Columns["fechaRegistro"].HeaderText = "Fecha Registro";

            if (productos.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["codigo"];
            btnEditar.Enabled = productos.Count > 0;
            btnEliminar.Enabled = productos.Count > 0;
        }

        private void cargarCatergorias()
        {
            cbxCategoria.DataSource = CategoriaCln.listar();
            cbxCategoria.ValueMember = "id";
            cbxCategoria.DisplayMember = "nombre";
            cbxCategoria.SelectedIndex = -1;
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {
            Size = new Size(900, 360);
            listar();
            cargarCatergorias();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void resetearCampos()
        {
            cbxCategoria.SelectedIndex = -1;
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            nudPesoKg.Value = 0;
            nudPrecioCostoUsd.Value = 0;
            nudPrecioCostoBob.Value = 0;
            nudStockActual.Value = 0;
            nudStockMinimo.Value = 0;
            resetearErrores();
        }

        private void resetearErrores()
        {
            erpCategoria.Clear();
            erpCodigo.Clear();
            erpNombre.Clear();
            erpDescripcion.Clear();
            erpPesoKg.Clear();
            erpPrecioCostoUsd.Clear();
            erpPrecioCostoBob.Clear();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(900, 520);
            pnlAcciones.Enabled = false;
            resetearCampos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(900, 520);
            pnlAcciones.Enabled = false;

            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            var producto = ProductoCln.obtenerUno(id);
            cbxCategoria.SelectedValue = producto.idCategoria;
            txtCodigo.Text = producto.codigo;
            txtNombre.Text = producto.nombre;
            txtDescripcion.Text = producto.descripcion;
            nudPesoKg.Value = producto.pesoKg ?? 0m;
            nudPrecioCostoUsd.Value = producto.precioCostoUsd.GetValueOrDefault();
            nudPrecioCostoBob.Value = producto.precioVentaBob;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(900, 360);
            pnlAcciones.Enabled = true;
        }

        private Boolean validarCampos()
        {
            bool esValido = true;
            resetearErrores();

            if(cbxCategoria.SelectedIndex == -1)
            {
                erpCategoria.SetError(cbxCategoria, "La categoria es obligatoria");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                erpCodigo.SetError(txtCodigo, "El código es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                erpNombre.SetError(txtNombre, "El nombre es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                erpDescripcion.SetError(txtDescripcion, "La descripción es obligatoria");
                esValido = false;
            }
            if (nudPesoKg.Value == 0)
            {
                erpPesoKg.SetError(nudPesoKg, "El peso debe ser mayor a 0");
                esValido = false;
            }
            if (nudPrecioCostoUsd.Value == 0)
            {
                erpPrecioCostoUsd.SetError(nudPrecioCostoUsd, "El precio en dólares debe ser mayor a 0");
                esValido = false;
            }
            if (nudPrecioCostoBob.Value == 0)
            {
                erpPrecioCostoBob.SetError(nudPrecioCostoBob, "El precio en bolivianos debe ser mayor a 0");
                esValido = false;
            }

            return esValido;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                var producto = new Producto
                {
                    idCategoria = (int)cbxCategoria.SelectedValue,
                    codigo = txtCodigo.Text.Trim(),
                    nombre = txtNombre.Text.Trim(),
                    descripcion = txtDescripcion.Text.Trim(),
                    pesoKg = nudPesoKg.Value,
                    precioCostoUsd = nudPrecioCostoUsd.Value,
                    precioVentaBob = nudPrecioCostoBob.Value,
                    stockActual = (int)nudStockActual.Value,
                    stockMinimo = (int)nudStockMinimo.Value,
                    usuarioRegistro = Util.usuario.usuario1
                };

                try
                {
                    if (esNuevo)
                    {
                        producto.fechaRegistro = DateTime.Now;
                        producto.estado = (short)Estado.Activo;
                        ProductoCln.crear(producto);
                    }
                    else
                    {
                        producto.id = (int)dgvLista.CurrentRow.Cells["id"].Value;
                        ProductoCln.actualizar(producto);
                    }
                    listar();
                    btnCancelar.PerformClick();
                    MessageBox.Show("Producto guardado correctamente", "::: Guarida Goblin :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "::: Error - Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string codigo = dgvLista.CurrentRow.Cells["codigo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"Está seguro que desea dar de baja el producto {codigo}?",
                "::: Confirmación - Guardia Goblin :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                ProductoCln.eliminar(id, Util.usuario.usuario1);
                listar();
                MessageBox.Show("Producto dado de baja correctamente", "::: Mensaje - Guarida Goblin",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

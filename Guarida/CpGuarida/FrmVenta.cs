using ClnGuarida;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CpGuarida
{
    public partial class FrmVenta : Form
    {
        public FrmVenta()
        {
            InitializeComponent();
        }
        private void listar()
        {
            var productos = ProductoCln.listarPa(txtBuscar.Text.Trim());
            dgvListaProducto.DataSource = productos;
            dgvListaProducto.Columns["id"].Visible = false;
            dgvListaProducto.Columns["idCategoria"].Visible = false;
            dgvListaProducto.Columns["estado"].Visible = false;
            dgvListaProducto.Columns["codigo"].HeaderText = "Código";
            dgvListaProducto.Columns["nombre"].HeaderText = "Nombre";
            dgvListaProducto.Columns["descripcion"].HeaderText = "Descripción";
            dgvListaProducto.Columns["pesoKg"].HeaderText = "Peso en Kg";
            dgvListaProducto.Columns["precioCostoUsd"].HeaderText = "Precio en Dólares";
            dgvListaProducto.Columns["precioVentaBob"].HeaderText = "Precio en Bolivianos";
            dgvListaProducto.Columns["stockActual"].HeaderText = "Stock Actual";
            dgvListaProducto.Columns["stockMinimo"].HeaderText = "Stock Minimo";
            dgvListaProducto.Columns["usuarioRegistro"].Visible = false;
            dgvListaProducto.Columns["fechaRegistro"].Visible = false;

            if (productos.Count > 0) dgvListaProducto.CurrentCell = dgvListaProducto.Rows[0].Cells["codigo"];
        }
        private void cargarCLientes()
        {
            cbxCliente.DataSource = ClienteCln.listar("");
            cbxCliente.ValueMember = "id";
            cbxCliente.DisplayMember = "ApellidoNombre";
            cbxCliente.SelectedIndex = -1;
            cbxCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        private void FrmVenta_Load(object sender, EventArgs e)
        {
            cargarCLientes();
            listar();
            txtUsuario.Text = "admin";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            listar();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void tmrFecha_Tick(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
           
        }
    }
}

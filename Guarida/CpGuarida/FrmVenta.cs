using CadGuarida;
using ClnGuarida;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

            string textoActual = cbxCliente.Text;
            cbxCliente.DataSource = ClienteCln.listar(textoActual);
            cbxCliente.ValueMember = "id";
            cbxCliente.DisplayMember = "ApellidoNombre";
            cbxCliente.AutoCompleteMode = AutoCompleteMode.None;
            cbxCliente.Text = textoActual;
            cbxCliente.SelectionStart = cbxCliente.Text.Length;
        }
        private void cargarMetodosPago()
        {
            cbxMetoPago.DataSource = VentaCln.metodPago();
            cbxMetoPago.SelectedIndex = -1;
        }
        private void FrmVenta_Load(object sender, EventArgs e)
        {
            listarDetalles();
            cargarCLientes();
            listar();
            txtUsuario.Text = "admin";
            calcularSubTotal();
            CalcularTotal();
            cargarMetodosPago();
        }
         
        private void listarDetalles()
        {
            dgvDatosVenta.Columns.Clear();
            dgvDatosVenta.Columns.Add("codigo", "Código");
            dgvDatosVenta.Columns.Add("nombre", "Nombre");
            dgvDatosVenta.Columns.Add("precioVentaBob", "Precio Bolivianos");
            dgvDatosVenta.Columns.Add("cantidad", "Cantidad");
            dgvDatosVenta.Columns.Add("subtotal", "Subtotal");
        }
        private void calcularSubTotal()
        {
            decimal totalsubtotal = 0;
            foreach (DataGridViewRow row in dgvDatosVenta.Rows)
            {
                if (row.Cells["subtotal"].Value != null)
                {
                    totalsubtotal += Convert.ToDecimal(row.Cells["subtotal"].Value);
                }
            }
            lblSubInfo.Text = totalsubtotal.ToString("0.00");
        }
        private void CalcularTotal()
        {
            decimal subtotal = 0;
            decimal descuento = 0;
            decimal baseImponible = 0;
            decimal iva = 0;
            decimal total = 0;
            decimal cambio = 0;
            decimal recibido = 0;

            const decimal PORCENTAJE_IVA = 0.13m;

            if (decimal.TryParse(lblSubInfo.Text, out subtotal))
            {
                decimal porcentajeDescuento = nudDescuento.Value / 100m;
                descuento = subtotal * porcentajeDescuento;
                if (descuento < 0) descuento = 0;
                if (descuento > subtotal) descuento = subtotal;
                baseImponible = subtotal - descuento;
                iva = Math.Round(baseImponible * PORCENTAJE_IVA, 2);
                total = baseImponible + iva;
                recibido = nudRecibido.Value;
                cambio = recibido - total;
                lblTotalInfo.Text = total.ToString("0.00");
                lblIvaInfo.Text = iva.ToString("0.00");
                lblCinfo.Text = cambio.ToString("0.00");
            }
            else
            {

                lblTotalInfo.Text = "0.00";
                lblIvaInfo.Text = "0.00";
                lblCinfo.Text = "0.00";
            }
        }
        private void ActualizarFilaSubtotal(DataGridViewRow fila, int nuevaCantidad)
        {
            decimal precioUnitario = Convert.ToDecimal(fila.Cells["precioVentaBob"].Value);
            fila.Cells["cantidad"].Value = nuevaCantidad;
            fila.Cells["subtotal"].Value = nuevaCantidad * precioUnitario;
        }

        private void agregarProductoAlDetalle()
        {
           if (dgvListaProducto.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto de la lista", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigo = dgvListaProducto.CurrentRow.Cells["codigo"].Value.ToString();
            string nombre = dgvListaProducto.CurrentRow.Cells["nombre"].Value.ToString();
            decimal precioVentaBob = Convert.ToDecimal(dgvListaProducto.CurrentRow.Cells["precioVentaBob"].Value);
            int stockActual = Convert.ToInt32(dgvListaProducto.CurrentRow.Cells["stockActual"].Value);
            int cantidad = 1; 

            if (stockActual <= 0)
            {
                MessageBox.Show($"El producto '{nombre}' no tiene stock disponible.", "Sin Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool yaExiste = dgvDatosVenta.Rows
                .Cast<DataGridViewRow>()
                .Any(r => r.Cells["codigo"].Value != null && r.Cells["codigo"].Value.ToString() == codigo);

            if (yaExiste)
            {
                MessageBox.Show("El producto ya está en el detalle", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal subtotal = precioVentaBob * cantidad;
            dgvDatosVenta.Rows.Add(codigo, nombre, precioVentaBob, cantidad, subtotal);
            calcularSubTotal();
            CalcularTotal();
            btnCancelar.Enabled = dgvDatosVenta.Rows.Count > 0;
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            cbxCliente.AutoCompleteMode = AutoCompleteMode.None;
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

        private void cbxCliente_KeyUp(object sender, KeyEventArgs e)
        {
            cargarCLientes();
            cbxCliente.DroppedDown = true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            agregarProductoAlDetalle();
        }

        private void btnScantidad_Click(object sender, EventArgs e)
        {
            if (dgvDatosVenta.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto del detalle de venta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int cantidadActual = Convert.ToInt32(dgvDatosVenta.CurrentRow.Cells["cantidad"].Value);
            string codigoProducto = dgvDatosVenta.CurrentRow.Cells["codigo"].Value.ToString();

            int stockDisponible = dgvListaProducto.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["codigo"].Value?.ToString() == codigoProducto)
                .Select(r => Convert.ToInt32(r.Cells["stockActual"].Value))
                .FirstOrDefault();

            if (cantidadActual + 1 > stockDisponible)
            {
                MessageBox.Show($"No hay suficiente stock. Stock disponible: {stockDisponible}", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int nuevaCantidad = cantidadActual + 1;
            ActualizarFilaSubtotal(dgvDatosVenta.CurrentRow, nuevaCantidad); // ¡Adiós duplicidad!
            calcularSubTotal();
            CalcularTotal();
            btnCancelar.Enabled = dgvDatosVenta.Rows.Count > 0;
        }

        private void btnRcantidad_Click(object sender, EventArgs e)
        {
            if (dgvDatosVenta.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto del detalle de venta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int cantidadActual = Convert.ToInt32(dgvDatosVenta.CurrentRow.Cells["cantidad"].Value);
            if (cantidadActual <= 1)
            {
                MessageBox.Show("La cantidad mínima es 1. Si desea quitar el producto, elimínelo de la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int nuevaCantidad = cantidadActual - 1;
            ActualizarFilaSubtotal(dgvDatosVenta.CurrentRow, nuevaCantidad); // ¡Adiós duplicidad!
            calcularSubTotal();
            CalcularTotal();
            btnCancelar.Enabled = dgvDatosVenta.Rows.Count > 0;
        }

        private void btnBorrarProducto_Click(object sender, EventArgs e)
        {
            if (dgvDatosVenta.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto del detalle para quitar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = dgvDatosVenta.CurrentRow.Cells["nombre"].Value.ToString();
            DialogResult producto = MessageBox.Show($"¿Está seguro que desea quitar el producto {nombre} de la venta?",
                "::: Confirmación - Guardia Goblin :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (producto == DialogResult.Yes)
            {
                dgvDatosVenta.Rows.Remove(dgvDatosVenta.CurrentRow);
                calcularSubTotal();
                CalcularTotal();

                MessageBox.Show("Producto quitado de la venta correctamente", "::: Mensaje - Guardia Goblin :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void npnDescuento_ValueChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void npnRecibido_ValueChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }
         private void resetearCampos()
        {
            resetearErrores();
            cbxMetoPago.SelectedIndex = -1;
            cbxCliente.SelectedIndex = -1;
            dgvDatosVenta.Rows.Clear();
            txtObservaciones.Clear();
            nudDescuento.Value = 0;
            nudRecibido.Value = 0;
        }
        private void resetearErrores()
        {
            erpCliente.Clear();
            erpDatosVenta.Clear();
            erpMetoPago.Clear();
            erpRecibido.Clear();
       
        }
        private Boolean validarCampos()
        {
            bool esValido = true;
            resetearErrores();

            if (string.IsNullOrWhiteSpace(cbxCliente.Text))
            {
                erpCliente.SetError(cbxCliente, "El cliente es obligatorio");
                esValido = false;
            }
            if (cbxMetoPago.SelectedIndex == -1)
            {
                erpMetoPago.SetError(cbxMetoPago, "El método de pago es obligatorio");
                esValido = false;
            }
            if (nudRecibido.Value == 0)
            {
                erpRecibido.SetError(nudRecibido, "El monto recibido debe ser mayor a 0");
                esValido = false;
            }
            if (dgvDatosVenta.Rows.Count == 0)
            {
                erpDatosVenta.SetError(dgvDatosVenta, "El detalle de la venta es obligatorio");
                esValido = false;
            }

            return esValido;
        }
        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            validarCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bool hayDatos = dgvDatosVenta.Rows.Count > 0
                 || !string.IsNullOrWhiteSpace(cbxCliente.Text)
                 || nudRecibido.Value > 0
                 || nudDescuento.Value > 0
                 || !string.IsNullOrWhiteSpace(txtObservaciones.Text)
                 || !string.IsNullOrWhiteSpace(cbxMetoPago.Text);
            if (!hayDatos)
            {
                MessageBox.Show("No hay ninguna venta en curso para cancelar.",
                    "::: Aviso - Guardia Goblin :::",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cancelar la venta actual?",
                "::: Confirmación - Guardia Goblin :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                resetearCampos();
                MessageBox.Show("Venta cancelada correctamente", "::: Mensaje - Guardia Goblin :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    
}

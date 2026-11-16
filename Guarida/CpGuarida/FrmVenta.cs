using CadGuarida;
using ClnGuarida;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CpGuarida
{
    public partial class FrmVenta : Form
    {
        private int idClienteSeleccionado = 0;
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
            dgvListaProducto.Columns["precioCostoUsd"].Visible = false;
            dgvListaProducto.Columns["usuarioRegistro"].Visible = false;
            dgvListaProducto.Columns["fechaRegistro"].Visible = false;
            dgvListaProducto.Columns["codigo"].HeaderText = "Código";
            dgvListaProducto.Columns["nombre"].HeaderText = "Nombre";
            dgvListaProducto.Columns["descripcion"].HeaderText = "Descripción";
            dgvListaProducto.Columns["pesoKg"].HeaderText = "Peso en Kg";
            dgvListaProducto.Columns["precioVentaBob"].HeaderText = "Precio(BO)";
            dgvListaProducto.Columns["stockActual"].HeaderText = "Stock";
            dgvListaProducto.Columns["stockMinimo"].HeaderText = "Stock Mínimo";
            dgvListaProducto.Columns["nombre"].DisplayIndex = 0;
            dgvListaProducto.Columns["codigo"].DisplayIndex = 1;
            dgvListaProducto.Columns["precioVentaBob"].DisplayIndex = 2;
            dgvListaProducto.Columns["stockActual"].DisplayIndex = 3;
            dgvListaProducto.Columns["descripcion"].DisplayIndex = 4;
            dgvListaProducto.Columns["pesoKg"].DisplayIndex = 5;
            dgvListaProducto.Columns["stockMinimo"].DisplayIndex = 6;

            if (productos.Count > 0) dgvListaProducto.CurrentCell = dgvListaProducto.Rows[0].Cells["codigo"];
        }

        private void cargarMetodosPago()
        {
            cbxMetoPago.DataSource = VentaCln.metodPago();
            cbxMetoPago.SelectedIndex = -1;
        }
        private void FrmVenta_Load(object sender, EventArgs e)
        {
            listarDetalles();
            listar();
            txtUsuario.Text =Util.usuario.usuario1;
            calcularSubTotal();
            CalcularTotal();
            cargarMetodosPago();
            validarEfectivo();
            nudDescuento.TextChanged += nudDescuento_TabIndexChanged;
            nudRecibido.TextChanged += nudRecibido_TabIndexChanged;
        }

        private void listarDetalles()
        {
            dgvDatosVenta.Columns.Clear();
            dgvDatosVenta.Columns.Add("id", "id");
            dgvDatosVenta.Columns.Add("codigo", "Código");
            dgvDatosVenta.Columns.Add("nombre", "Nombre");
            dgvDatosVenta.Columns.Add("precioVentaBob", "Precio Bolivianos");
            dgvDatosVenta.Columns.Add("cantidad", "Cantidad");
            dgvDatosVenta.Columns.Add("subtotal", "Subtotal");
            dgvDatosVenta.Columns["id"].Visible = false;
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
            lblSubInfo.Text = totalsubtotal.ToString("N2");
        }

        private void CalcularTotal()
        {
            decimal subtotal = 0;
            decimal descuento = 0;
            decimal total = 0;
            decimal cambio = 0;
            decimal recibido = 0;

            if (decimal.TryParse(lblSubInfo.Text, out subtotal))
            {
                decimal porcentajeDescuento = 0;
                decimal.TryParse(nudDescuento.Text, out porcentajeDescuento);
                porcentajeDescuento /= 100m;
                descuento = subtotal * porcentajeDescuento;

                if (descuento < 0) descuento = 0;
                if (descuento > subtotal) descuento = subtotal;
                total = subtotal - descuento;
                decimal.TryParse(nudRecibido.Text, out recibido);
                cambio = recibido - total;

                if (cambio < 0) cambio = 0;
                lblTotalInfo.Text = total.ToString("N2");
                lblCinfo.Text = cambio.ToString("N2");
                lblDesRe.Text = descuento.ToString("N2");
            }
            else
            {
                lblTotalInfo.Text = "0.00";
                lblCinfo.Text = "0.00";
                lblDesRe.Text = "0.00";
            }
        }
        private void validarEfectivo()
        {
            bool esEfectivo = cbxMetoPago.Text == "Efectivo";
            if (esEfectivo)
            {
                nudRecibido.Enabled = true;
                nudRecibido.ReadOnly = false;   
            }
            else
            {
                nudRecibido.Enabled = false;   
                nudRecibido.Value = 0;
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
            if (dgvListaProducto.CurrentRow == null) return;

            int idProducto = Convert.ToInt32(dgvListaProducto.CurrentRow.Cells["id"].Value);
            string codigo = dgvListaProducto.CurrentRow.Cells["codigo"].Value.ToString();
            string nombreProducto = dgvListaProducto.CurrentRow.Cells["nombre"].Value.ToString();
            decimal precioVentaBob = Convert.ToDecimal(dgvListaProducto.CurrentRow.Cells["precioVentaBob"].Value);
            int stockActual = Convert.ToInt32(dgvListaProducto.CurrentRow.Cells["stockActual"].Value);

            if (stockActual <= 0)
            {
                MessageBox.Show($"El producto '{nombreProducto}' no tiene stock disponible.", "Sin Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            int cantidad = 0;
                string inputCantidad = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Ingrese la cantidad para '{nombreProducto}'\nStock disponible: {stockActual}",
                    "Cantidad a Vender",
                    "1");

            if (string.IsNullOrWhiteSpace(inputCantidad))
            {
                    return;  
            }


            if (!int.TryParse(inputCantidad, out cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Por favor, ingrese un número entero válido mayor a 0.", "Valor Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

              
                if (cantidad > stockActual)
                {
                    MessageBox.Show($"No hay suficiente stock. Stock disponible: {stockActual}", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            

            decimal subtotal = precioVentaBob * cantidad;
            dgvDatosVenta.Rows.Add(idProducto, codigo, nombreProducto, precioVentaBob, cantidad, subtotal);

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
            ActualizarFilaSubtotal(dgvDatosVenta.CurrentRow, nuevaCantidad);
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
            ActualizarFilaSubtotal(dgvDatosVenta.CurrentRow, nuevaCantidad);
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

       

    

        private void resetearCampos()
        {
            resetearErrores();
            cbxMetoPago.SelectedIndex = -1;
            txtCliente.Clear();
            idClienteSeleccionado = 0;
            txtCliente.Clear();
            dgvDatosVenta.Rows.Clear();
            nudDescuento.Value = 0;
            nudRecibido.Value = 0;
            lblCinfo.Text = "0.00";
            lblSubInfo.Text = "0.00";
            lblTotalInfo.Text = "0.00";
            lblDesRe.Text = "0.00";
            txtObservaciones.Clear();

        }
        private void resetearErrores()
        {
            erpCliente.Clear();
            erpDatosVenta.Clear();

        }
        private Boolean validarCampos()
        {
            bool esValido = true;
            resetearErrores();

            if (string.IsNullOrWhiteSpace(txtResultadoCli.Text))
            {
                erpCliente.SetError(txtResultadoCli, "El cliente es obligatorio");
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
            if (validarCampos())
            {
                try
                {
                    var venta = new Venta
                    {
                        idCliente = idClienteSeleccionado,
                        idUsuario = Util.usuario.id,
                        transaccion = null,
                        fecha = DateTime.Now,
                        subtotal = Convert.ToDecimal(lblSubInfo.Text),
                        total = Convert.ToDecimal(lblTotalInfo.Text),
                        observaciones = txtObservaciones.Text.Trim(),
                        usuarioRegistro = Util.usuario.usuario1,
                        fechaRegistro = DateTime.Now,
                        metodoPago = cbxMetoPago.Text.Trim(),
                        descuento = Convert.ToDecimal(lblDesRe.Text),
                        estado = (short)Estado.Activo
                    };

                    var detalles = new List<VentaDetalle>();

                    foreach (DataGridViewRow row in dgvDatosVenta.Rows)
                    {
                        if (row.Cells["codigo"].Value != null)
                        {
                            var detalle = new VentaDetalle
                            {
                                idProducto = Convert.ToInt32(row.Cells["id"].Value),
                                cantidad = Convert.ToInt32(row.Cells["cantidad"].Value),
                                precioUnitario = Convert.ToDecimal(row.Cells["precioVentaBob"].Value),
                                subtotal = Convert.ToDecimal(row.Cells["subtotal"].Value),
                                usuarioRegistro = Util.usuario.usuario1,
                                fechaRegistro = DateTime.Now,
                                estado = (short)Estado.Activo,
                            };
                            detalles.Add(detalle);
                        }
                    }

                    venta.VentaDetalle = detalles;

                    int idVentaRegistrada = VentaCln.crear(venta, detalles);

                    if (idVentaRegistrada > 0)
                    {

                        MessageBox.Show($"Venta N° {venta.transaccion} registrada correctamente", "::: Guarida Goblin :::",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FrmDetalle frm = new FrmDetalle(idVentaRegistrada);
                        frm.ShowDialog();
                        resetearCampos();
                        listar();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo registrar la venta.", "::: Error - Guarida Goblin :::",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {

                    Exception inner = ex;
                    while (inner.InnerException != null) inner = inner.InnerException;

                    MessageBox.Show("Error real: " + inner.Message,
                        "::: Error - Guardia Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bool hayDatos = dgvDatosVenta.Rows.Count > 0
                 || idClienteSeleccionado != 0
                 || nudRecibido.Value > 0
                 || nudDescuento.Value > 0
                 || !string.IsNullOrWhiteSpace(txtObservaciones.Text)
                 || !string.IsNullOrWhiteSpace(cbxMetoPago.Text);
            if (!hayDatos)
            {
                Close();
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
        private void btnCliente_Click(object sender, EventArgs e)
        {

            FrmCliente frm = new FrmCliente();
            if (!string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                frm.carnetAEnviar = txtCliente.Text.Trim();
            }
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtCliente_TextChanged(null, null);
            }
        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {
            string criterio = txtCliente.Text.Trim();
            if (string.IsNullOrEmpty(criterio))
            {
                txtResultadoCli.Text = string.Empty;
                idClienteSeleccionado = 0;
                return;
            }

            try
            {
                var cliente = ClienteCln.buscar(criterio);

                if (cliente != null)
                {
                    txtResultadoCli.Text = $"{cliente.nombres}";
                    idClienteSeleccionado = cliente.id;
                }
                else
                {
                    txtResultadoCli.Text = "No encontrado";
                    idClienteSeleccionado = 0;
                }
            }
            catch (Exception)
            {
                txtResultadoCli.Text = string.Empty;
                idClienteSeleccionado = 0;
            }
        }

        private void txtCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
        }

       

        private void nudDescuento_ValueChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void nudRecibido_ValueChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void nudDescuento_TabIndexChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void nudRecibido_TabIndexChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void cbxMetoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            validarEfectivo();
        }
    }
}

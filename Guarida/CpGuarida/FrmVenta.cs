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
    public partial class FrmVenta : Form
    {
        private BindingList<ItemVenta> items = new BindingList<ItemVenta>();

        public FrmVenta()
        {
            InitializeComponent();
        }

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            lblFechaValor.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            cargarClientes();
            cargarProductos();
            configurarTabla();
            actualizarTotal();
        }

        private void cargarClientes()
        {
            var clientes = ClienteCln.listar()
                .Select(x => new OpcionCombo { id = x.id, texto = (x.nombres + " " + x.apellidos).Trim() })
                .ToList();
            cbxCliente.ValueMember = "id";
            cbxCliente.DisplayMember = "texto";
            cbxCliente.DataSource = clientes;
            cbxCliente.SelectedIndex = clientes.Count > 0 ? 0 : -1;
        }

        private void cargarProductos()
        {
            var productos = ProductoCln.listar()
                .Select(x => new OpcionCombo { id = x.id, texto = x.codigo + " - " + x.nombre })
                .ToList();
            cbxProducto.ValueMember = "id";
            cbxProducto.DisplayMember = "texto";
            cbxProducto.DataSource = productos;
            cbxProducto.SelectedIndex = -1;
            mostrarDatosProducto();
        }

        private void configurarTabla()
        {
            dgvDetalle.DataSource = items;
            dgvDetalle.Columns["idProducto"].Visible = false;
            dgvDetalle.Columns["codigo"].HeaderText = "Código";
            dgvDetalle.Columns["nombre"].HeaderText = "Producto";
            dgvDetalle.Columns["cantidad"].HeaderText = "Cantidad";
            dgvDetalle.Columns["precioUnitario"].HeaderText = "Precio Unitario (Bs)";
            dgvDetalle.Columns["subtotal"].HeaderText = "Subtotal (Bs)";
            dgvDetalle.Columns["precioUnitario"].DefaultCellStyle.Format = "N2";
            dgvDetalle.Columns["subtotal"].DefaultCellStyle.Format = "N2";
        }

        private void mostrarDatosProducto()
        {
            var opcion = cbxProducto.SelectedItem as OpcionCombo;
            if (opcion == null)
            {
                lblPrecioValor.Text = "-";
                lblStockValor.Text = "-";
                return;
            }
            var producto = ProductoCln.obtenerUno(opcion.id);
            lblPrecioValor.Text = producto.precioVentaBob.ToString("N2");
            lblStockValor.Text = producto.stockActual.ToString();
        }

        private void cbxProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            mostrarDatosProducto();
        }

        private void actualizarTotal()
        {
            decimal total = items.Sum(x => x.subtotal);
            lblTotalValor.Text = total.ToString("N2");
            btnGuardar.Enabled = items.Count > 0;
            btnQuitar.Enabled = items.Count > 0;
        }

        private void resetearErrores()
        {
            erpCliente.Clear();
            erpProducto.Clear();
            erpCantidad.Clear();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            resetearErrores();
            var opcionProducto = cbxProducto.SelectedItem as OpcionCombo;
            if (opcionProducto == null)
            {
                erpProducto.SetError(cbxProducto, "Seleccione un producto");
                return;
            }
            if (nudCantidad.Value <= 0)
            {
                erpCantidad.SetError(nudCantidad, "La cantidad debe ser mayor a 0");
                return;
            }

            try
            {
                int idProducto = opcionProducto.id;
                int cantidad = (int)nudCantidad.Value;
                var producto = ProductoCln.obtenerUno(idProducto);

                var existente = items.FirstOrDefault(x => x.idProducto == idProducto);
                int cantidadTotal = cantidad + (existente != null ? existente.cantidad : 0);

                if (cantidadTotal > producto.stockActual)
                {
                    erpCantidad.SetError(nudCantidad, $"Stock insuficiente. Disponible: {producto.stockActual}");
                    return;
                }

                if (existente != null)
                {
                    existente.cantidad = cantidadTotal;
                    existente.subtotal = existente.cantidad * existente.precioUnitario;
                    items.ResetBindings();
                }
                else
                {
                    items.Add(new ItemVenta
                    {
                        idProducto = producto.id,
                        codigo = producto.codigo,
                        nombre = producto.nombre,
                        cantidad = cantidad,
                        precioUnitario = producto.precioVentaBob,
                        subtotal = cantidad * producto.precioVentaBob
                    });
                }

                actualizarTotal();
                cbxProducto.SelectedIndex = -1;
                nudCantidad.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "::: Error - Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvDetalle.CurrentRow == null) return;
            items.RemoveAt(dgvDetalle.CurrentRow.Index);
            actualizarTotal();
        }

        private void limpiarVenta()
        {
            items.Clear();
            txtObservaciones.Clear();
            nudCantidad.Value = 1;
            resetearErrores();
            lblFechaValor.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            if (cbxCliente.Items.Count > 0) cbxCliente.SelectedIndex = 0;
            cbxProducto.SelectedIndex = -1;
            mostrarDatosProducto();
            actualizarTotal();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (items.Count > 0)
            {
                DialogResult dialog = MessageBox.Show("Está seguro que desea limpiar la venta?",
                    "::: Confirmación - Guarida Goblin :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog != DialogResult.Yes) return;
            }
            limpiarVenta();
        }

        private bool validarVenta()
        {
            bool esValido = true;
            resetearErrores();

            if (cbxCliente.SelectedItem as OpcionCombo == null)
            {
                erpCliente.SetError(cbxCliente, "El cliente es obligatorio");
                esValido = false;
            }
            if (items.Count == 0)
            {
                erpProducto.SetError(cbxProducto, "Agregue al menos un producto a la venta");
                esValido = false;
            }
            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validarVenta()) return;

            var venta = new Venta
            {
                idCliente = ((OpcionCombo)cbxCliente.SelectedItem).id,
                idUsuario = Util.usuario.id,
                observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim(),
                usuarioRegistro = Util.usuario.usuario1
            };

            var detalles = items.Select(x => new VentaDetalle
            {
                idProducto = x.idProducto,
                cantidad = x.cantidad
            }).ToList();

            try
            {
                int idVenta = VentaCln.registrar(venta, detalles);
                MessageBox.Show($"Venta registrada correctamente.\nNúmero de venta: {idVenta}\nTotal: Bs {venta.total:N2}",
                    "::: Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarVenta();
                cargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "::: Error - Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class OpcionCombo
    {
        public int id { get; set; }
        public string texto { get; set; }
    }

    public class ItemVenta
    {
        public int idProducto { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subtotal { get; set; }
    }
}

using CadGuarida;
using ClnGuarida;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CpGuarida
{
    /// <summary>
    /// Registro de compras (pedidos a proveedores).
    /// Mismo patrón que FrmVenta: el detalle se arma en una grilla en memoria y recién
    /// al presionar "Registrar compra" se guarda la cabecera (Compra) y el detalle (CompraDetalle) juntos.
    /// La interfaz se construye por código (no tiene archivo Designer).
    /// </summary>
    public class FrmCompra : Form
    {
        // ---- Pestañas
        private TabControl tabs;
        private TabPage tabNueva, tabHistorial;

        // ---- Datos de la compra
        private ComboBox cbxProveedor, cbxMoneda, cbxEstadoEnvio;
        private NumericUpDown nudTipoCambio;
        private DateTimePicker dtpFecha, dtpFechaEstimada;
        private TextBox txtTracking, txtObservaciones;

        // ---- Productos disponibles
        private TextBox txtBuscar;
        private Button btnBuscar, btnLimpiar;
        private DataGridView dgvProductos;
        private NumericUpDown nudPrecio, nudCantidad;
        private Label lblPrecio;
        private Button btnAgregar;

        // ---- Detalle de la compra (en memoria)
        private DataGridView dgvDetalle;
        private Button btnMas, btnMenos, btnQuitar;
        private Label lblTotal, lblEquivalente;
        private Button btnRegistrar, btnCancelar;
        private ErrorProvider erpDatos;

        // ---- Historial
        private DataGridView dgvHistorial, dgvHistDetalle;
        private Button btnRecibir, btnAnular, btnActualizar;

        public FrmCompra()
        {
            ConstruirInterfaz();
            Load += FrmCompra_Load;
        }

        private bool esUsd { get { return cbxMoneda.Text == CompraCln.Usd; } }

        // =====================================================================
        //  CARGA INICIAL
        // =====================================================================
        private void FrmCompra_Load(object sender, EventArgs e)
        {
            try
            {
                cbxProveedor.DisplayMember = "razonSocial";
                cbxProveedor.ValueMember = "id";
                cbxProveedor.DataSource = CompraCln.listarProveedores();
                cbxProveedor.SelectedIndex = -1;

                cbxMoneda.DataSource = CompraCln.monedas();
                cbxEstadoEnvio.DataSource = CompraCln.estadosEnvio();

                listarProductos();
                listarHistorial();
                actualizarEstadoControles();
                calcularTotal();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        // =====================================================================
        //  PRODUCTOS
        // =====================================================================
        private void listarProductos()
        {
            var productos = ProductoCln.listarPa(txtBuscar.Text.Trim());
            dgvProductos.DataSource = productos;

            var visibles = new Dictionary<string, string>
            {
                { "codigo", "Código" },
                { "nombre", "Nombre" },
                { "precioCostoUsd", "Costo (USD)" },
                { "stockActual", "Stock" },
            };
            int orden = 0;
            foreach (DataGridViewColumn col in dgvProductos.Columns)
            {
                col.Visible = visibles.ContainsKey(col.Name);
                if (col.Visible) col.HeaderText = visibles[col.Name];
            }
            foreach (var par in visibles)
                dgvProductos.Columns[par.Key].DisplayIndex = orden++;

            if (productos.Count > 0)
                dgvProductos.CurrentCell = dgvProductos.Rows[0].Cells["codigo"];
            sugerirPrecio();
        }

        /// <summary>
        /// Carga en el cuadro de precio el costo del producto seleccionado, en la moneda elegida.
        /// </summary>
        private void sugerirPrecio()
        {
            if (dgvProductos.CurrentRow == null) return;
            decimal costoUsd = Convert.ToDecimal(dgvProductos.CurrentRow.Cells["precioCostoUsd"].Value);
            decimal precio = esUsd ? costoUsd : costoUsd * nudTipoCambio.Value;
            nudPrecio.Value = limitar(Math.Round(precio, 2), nudPrecio);
        }

        private static decimal limitar(decimal valor, NumericUpDown nud)
        {
            return Math.Min(Math.Max(valor, nud.Minimum), nud.Maximum);
        }

        // =====================================================================
        //  DETALLE EN MEMORIA (guardado dinámico)
        // =====================================================================
        private void agregarProducto()
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto de la lista.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["id"].Value);
            string codigo = Convert.ToString(dgvProductos.CurrentRow.Cells["codigo"].Value);
            string nombre = Convert.ToString(dgvProductos.CurrentRow.Cells["nombre"].Value);
            decimal precio = nudPrecio.Value;
            int cantidad = (int)nudCantidad.Value;

            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a 0.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (precio <= 0)
            {
                MessageBox.Show("El precio debe ser mayor a 0.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si el producto ya está en el detalle, se suma la cantidad.
            var fila = dgvDetalle.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => Convert.ToInt32(r.Cells["id"].Value) == idProducto);
            if (fila != null)
            {
                fila.Cells["cantidad"].Value = Convert.ToInt32(fila.Cells["cantidad"].Value) + cantidad;
                actualizarSubtotalFila(fila);
            }
            else
            {
                dgvDetalle.Rows.Add(idProducto, codigo, nombre, precio, cantidad, Math.Round(precio * cantidad, 2));
            }

            nudCantidad.Value = 1;
            actualizarEstadoControles();
            calcularTotal();
        }

        private void actualizarSubtotalFila(DataGridViewRow fila)
        {
            decimal precio = Convert.ToDecimal(fila.Cells["precio"].Value);
            int cantidad = Convert.ToInt32(fila.Cells["cantidad"].Value);
            fila.Cells["subtotal"].Value = Math.Round(precio * cantidad, 2);
        }

        private void cambiarCantidad(int delta)
        {
            if (dgvDetalle.CurrentRow == null) return;
            var fila = dgvDetalle.CurrentRow;
            int nueva = Convert.ToInt32(fila.Cells["cantidad"].Value) + delta;
            if (nueva < 1)
            {
                MessageBox.Show("La cantidad mínima es 1. Si desea quitar el producto, use \"Quitar\".", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            fila.Cells["cantidad"].Value = nueva;
            actualizarSubtotalFila(fila);
            calcularTotal();
        }

        private void quitarProducto()
        {
            if (dgvDetalle.CurrentRow == null) return;
            string nombre = Convert.ToString(dgvDetalle.CurrentRow.Cells["nombre"].Value);
            if (MessageBox.Show($"¿Quitar '{nombre}' de la compra?", "::: Confirmación - Guarida Goblin :::",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            dgvDetalle.Rows.Remove(dgvDetalle.CurrentRow);
            actualizarEstadoControles();
            calcularTotal();
        }

        /// <summary>
        /// El usuario puede editar precio y cantidad directamente en la grilla.
        /// Se valida, se normaliza y se recalcula el subtotal.
        /// </summary>
        private void dgvDetalle_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var fila = dgvDetalle.Rows[e.RowIndex];
            string col = dgvDetalle.Columns[e.ColumnIndex].Name;
            if (col != "precio" && col != "cantidad") return;

            string texto = Convert.ToString(fila.Cells[col].Value);
            if (col == "cantidad")
            {
                int cantidad;
                if (!int.TryParse(texto, NumberStyles.Integer, CultureInfo.CurrentCulture, out cantidad) || cantidad < 1)
                {
                    MessageBox.Show("La cantidad debe ser un número entero mayor a 0.", "Valor inválido",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cantidad = 1;
                }
                fila.Cells["cantidad"].Value = cantidad;
            }
            else
            {
                decimal precio;
                if (!decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out precio) || precio <= 0)
                {
                    MessageBox.Show("El precio debe ser un número mayor a 0.", "Valor inválido",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    precio = 0.01m;
                }
                fila.Cells["precio"].Value = Math.Round(precio, 2);
            }
            actualizarSubtotalFila(fila);
            calcularTotal();
        }

        private decimal sumarSubtotales()
        {
            decimal total = 0;
            foreach (DataGridViewRow fila in dgvDetalle.Rows)
                total += Convert.ToDecimal(fila.Cells["subtotal"].Value);
            return total;
        }

        private void calcularTotal()
        {
            decimal total = sumarSubtotales();
            string moneda = esUsd ? "USD" : "Bs";
            lblTotal.Text = $"TOTAL ({moneda}):  {total:N2}";
            lblEquivalente.Text = esUsd
                ? $"Equivale a  Bs {total * nudTipoCambio.Value:N2}  (tipo de cambio {nudTipoCambio.Value:N2})"
                : string.Empty;
        }

        /// <summary>
        /// La moneda no se puede cambiar cuando ya hay productos cargados (los precios quedarían mezclados).
        /// El tipo de cambio solo aplica a compras en USD.
        /// </summary>
        private void actualizarEstadoControles()
        {
            bool hayDetalle = dgvDetalle.Rows.Count > 0;
            cbxMoneda.Enabled = !hayDetalle;
            nudTipoCambio.Enabled = esUsd;
            lblPrecio.Text = "Precio unitario (" + (esUsd ? "USD" : "Bs") + "):";
            btnMas.Enabled = btnMenos.Enabled = btnQuitar.Enabled = hayDetalle;
        }

        // =====================================================================
        //  REGISTRAR / CANCELAR
        // =====================================================================
        private bool validarCampos()
        {
            erpDatos.Clear();
            bool ok = true;

            if (cbxProveedor.SelectedValue == null)
            {
                erpDatos.SetError(cbxProveedor, "El proveedor es obligatorio");
                ok = false;
            }
            if (esUsd && nudTipoCambio.Value <= 0)
            {
                erpDatos.SetError(nudTipoCambio, "El tipo de cambio debe ser mayor a 0");
                ok = false;
            }
            if (dtpFechaEstimada.Checked && dtpFechaEstimada.Value.Date < dtpFecha.Value.Date)
            {
                erpDatos.SetError(dtpFechaEstimada, "La fecha estimada no puede ser anterior a la fecha de la compra");
                ok = false;
            }
            if (dgvDetalle.Rows.Count == 0)
            {
                erpDatos.SetError(dgvDetalle, "El detalle de la compra es obligatorio");
                ok = false;
            }
            return ok;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) return;

            try
            {
                var compra = new Compra
                {
                    idProveedor = Convert.ToInt32(cbxProveedor.SelectedValue),
                    fecha = dtpFecha.Value.Date,
                    moneda = cbxMoneda.Text,
                    tipoCambio = esUsd ? nudTipoCambio.Value : 1m,
                    trackingNumber = string.IsNullOrWhiteSpace(txtTracking.Text) ? null : txtTracking.Text.Trim(),
                    estadoEnvio = cbxEstadoEnvio.Text,
                    fechaEstimada = dtpFechaEstimada.Checked ? (DateTime?)dtpFechaEstimada.Value.Date : null,
                    observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim(),
                    usuarioRegistro = Util.usuario.usuario1,
                };

                var detalles = new List<CompraDetalle>();
                foreach (DataGridViewRow fila in dgvDetalle.Rows)
                {
                    detalles.Add(new CompraDetalle
                    {
                        idProducto = Convert.ToInt32(fila.Cells["id"].Value),
                        cantidad = Convert.ToInt32(fila.Cells["cantidad"].Value),
                        precioUnitario = Convert.ToDecimal(fila.Cells["precio"].Value),
                        subtotal = Convert.ToDecimal(fila.Cells["subtotal"].Value),
                    });
                }

                int idCompra = CompraCln.crear(compra, detalles);

                string aviso = compra.estadoEnvio == CompraCln.Recibido
                    ? "El stock de los productos ya fue actualizado."
                    : "El stock se actualizará cuando la compra se marque como \"Recibido\".";
                MessageBox.Show($"Compra N° {idCompra} registrada correctamente.\n{aviso}",
                    "::: Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Information);

                resetearCampos();
                listarProductos();
                listarHistorial();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bool hayDatos = dgvDetalle.Rows.Count > 0
                || cbxProveedor.SelectedIndex != -1
                || !string.IsNullOrWhiteSpace(txtTracking.Text)
                || !string.IsNullOrWhiteSpace(txtObservaciones.Text);
            if (!hayDatos)
            {
                Close();
                return;
            }
            if (MessageBox.Show("¿Está seguro que desea cancelar la compra actual?", "::: Confirmación - Guarida Goblin :::",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                resetearCampos();
        }

        private void resetearCampos()
        {
            erpDatos.Clear();
            cbxProveedor.SelectedIndex = -1;
            cbxMoneda.SelectedIndex = 0;
            cbxEstadoEnvio.SelectedIndex = 0;
            dtpFecha.Value = DateTime.Today;
            dtpFechaEstimada.Checked = false;
            txtTracking.Clear();
            txtObservaciones.Clear();
            dgvDetalle.Rows.Clear();
            nudCantidad.Value = 1;
            actualizarEstadoControles();
            calcularTotal();
        }

        // =====================================================================
        //  HISTORIAL
        // =====================================================================
        private void listarHistorial()
        {
            var compras = CompraCln.listar(null, null);
            dgvHistorial.DataSource = compras;

            var visibles = new Dictionary<string, string>
            {
                { "id", "N°" }, { "fecha", "Fecha" }, { "proveedor", "Proveedor" }, { "moneda", "Moneda" },
                { "total", "Total" }, { "estadoEnvio", "Estado de envío" }, { "trackingNumber", "Tracking" },
                { "fechaEstimada", "Llegada estimada" }, { "situacion", "Situación" },
            };
            foreach (DataGridViewColumn col in dgvHistorial.Columns)
            {
                col.Visible = visibles.ContainsKey(col.Name);
                if (col.Visible) col.HeaderText = visibles[col.Name];
            }
            dgvHistorial.Columns["fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvHistorial.Columns["fechaEstimada"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvHistorial.Columns["total"].DefaultCellStyle.Format = "N2";
            dgvHistorial.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            actualizarBotonesHistorial();
            listarDetalleHistorial();
        }

        private CompraListado compraSeleccionada()
        {
            if (dgvHistorial.CurrentRow == null) return null;
            return dgvHistorial.CurrentRow.DataBoundItem as CompraListado;
        }

        private void listarDetalleHistorial()
        {
            var compra = compraSeleccionada();
            if (compra == null)
            {
                dgvHistDetalle.DataSource = null;
                return;
            }
            dgvHistDetalle.DataSource = CompraCln.listarDetalle(compra.id);
            dgvHistDetalle.Columns["id"].Visible = false;
            dgvHistDetalle.Columns["codigo"].HeaderText = "Código";
            dgvHistDetalle.Columns["producto"].HeaderText = "Producto";
            dgvHistDetalle.Columns["cantidad"].HeaderText = "Cantidad";
            dgvHistDetalle.Columns["precioUnitario"].HeaderText = "Precio unit. (" + compra.moneda + ")";
            dgvHistDetalle.Columns["subtotal"].HeaderText = "Subtotal";
            dgvHistDetalle.Columns["precioUnitario"].DefaultCellStyle.Format = "N2";
            dgvHistDetalle.Columns["subtotal"].DefaultCellStyle.Format = "N2";
        }

        private void actualizarBotonesHistorial()
        {
            var compra = compraSeleccionada();
            bool activa = compra != null && compra.situacion == "Activa";
            btnRecibir.Enabled = activa && compra.estadoEnvio != CompraCln.Recibido;
            btnAnular.Enabled = activa;
        }

        private void btnRecibir_Click(object sender, EventArgs e)
        {
            var compra = compraSeleccionada();
            if (compra == null) return;
            if (MessageBox.Show($"¿Marcar la compra N° {compra.id} como RECIBIDA?\nEl stock de sus productos aumentará.",
                "::: Confirmación - Guarida Goblin :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                CompraCln.recibir(compra.id, Util.usuario.usuario1);
                MessageBox.Show("Compra recibida. Stock actualizado.", "::: Guarida Goblin :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                listarProductos();
                listarHistorial();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            var compra = compraSeleccionada();
            if (compra == null) return;
            string extra = compra.estadoEnvio == CompraCln.Recibido ? "\nComo ya fue recibida, se descontará el stock que había sumado." : "";
            if (MessageBox.Show($"¿Anular la compra N° {compra.id}?{extra}",
                "::: Confirmación - Guarida Goblin :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                CompraCln.anular(compra.id, Util.usuario.usuario1);
                MessageBox.Show("Compra anulada.", "::: Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listarProductos();
                listarHistorial();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        private void mostrarError(Exception ex)
        {
            Exception inner = ex;
            while (inner.InnerException != null) inner = inner.InnerException;
            MessageBox.Show(inner.Message, "::: Error - Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // =====================================================================
        //  CONSTRUCCIÓN DE LA INTERFAZ
        // =====================================================================
        private void ConstruirInterfaz()
        {
            Text = "Registro de Compras - Guarida Goblin";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 720);
            MinimumSize = new Size(1000, 680);
            Font = new Font("Segoe UI", 9F);
            erpDatos = new ErrorProvider();
            erpDatos.ContainerControl = this;

            tabs = new TabControl { Dock = DockStyle.Fill };
            tabNueva = new TabPage("Nueva compra");
            tabHistorial = new TabPage("Compras registradas");
            tabs.TabPages.Add(tabNueva);
            tabs.TabPages.Add(tabHistorial);
            Controls.Add(tabs);

            ConstruirPestanaNueva();
            ConstruirPestanaHistorial();
            tabs.SelectedIndexChanged += (s, e) => { if (tabs.SelectedTab == tabHistorial) listarHistorial(); };
        }

        private static Label etiqueta(string texto, int x, int y)
        {
            return new Label { Text = texto, Left = x, Top = y + 4, AutoSize = true };
        }

        private static DataGridView crearGrilla(bool soloLectura)
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = soloLectura,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
            };
        }

        private void ConstruirPestanaNueva()
        {
            // ---------- Datos de la compra (arriba)
            var gbxDatos = new GroupBox { Text = "Datos de la compra", Dock = DockStyle.Top, Height = 150 };

            gbxDatos.Controls.Add(etiqueta("Proveedor:", 12, 24));
            cbxProveedor = new ComboBox { Left = 100, Top = 24, Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
            gbxDatos.Controls.Add(cbxProveedor);

            gbxDatos.Controls.Add(etiqueta("Moneda:", 12, 56));
            cbxMoneda = new ComboBox { Left = 100, Top = 56, Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cbxMoneda.SelectedIndexChanged += (s, e) => { actualizarEstadoControles(); sugerirPrecio(); calcularTotal(); };
            gbxDatos.Controls.Add(cbxMoneda);

            gbxDatos.Controls.Add(etiqueta("Tipo de cambio:", 12, 88));
            nudTipoCambio = new NumericUpDown
            {
                Left = 100, Top = 88, Width = 100, DecimalPlaces = 2, Minimum = 0, Maximum = 1000, Increment = 0.05m, Value = 6.96m
            };
            nudTipoCambio.ValueChanged += (s, e) => { sugerirPrecio(); calcularTotal(); };
            gbxDatos.Controls.Add(nudTipoCambio);
            gbxDatos.Controls.Add(new Label { Text = "Bs por 1 USD", Left = 206, Top = 92, AutoSize = true, ForeColor = Color.Gray });

            gbxDatos.Controls.Add(etiqueta("Fecha compra:", 370, 24));
            dtpFecha = new DateTimePicker { Left = 470, Top = 24, Width = 120, Format = DateTimePickerFormat.Short };
            gbxDatos.Controls.Add(dtpFecha);

            gbxDatos.Controls.Add(etiqueta("Llegada estimada:", 370, 56));
            dtpFechaEstimada = new DateTimePicker { Left = 470, Top = 56, Width = 120, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false };
            gbxDatos.Controls.Add(dtpFechaEstimada);

            gbxDatos.Controls.Add(etiqueta("Estado de envío:", 370, 88));
            cbxEstadoEnvio = new ComboBox { Left = 470, Top = 88, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            gbxDatos.Controls.Add(cbxEstadoEnvio);

            gbxDatos.Controls.Add(etiqueta("N° de tracking:", 620, 24));
            txtTracking = new TextBox { Left = 720, Top = 24, Width = 220, MaxLength = 100 };
            gbxDatos.Controls.Add(txtTracking);

            gbxDatos.Controls.Add(etiqueta("Observaciones:", 620, 56));
            txtObservaciones = new TextBox { Left = 720, Top = 56, Width = 340, Height = 56, Multiline = true, MaxLength = 500 };
            gbxDatos.Controls.Add(txtObservaciones);

            // ---------- Zona central: productos (arriba) y detalle (abajo)
            var tbl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            // Productos
            var gbxProductos = new GroupBox { Text = "Productos", Dock = DockStyle.Fill };
            dgvProductos = crearGrilla(true);
            dgvProductos.SelectionChanged += (s, e) => sugerirPrecio();
            dgvProductos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) agregarProducto(); };

            var pnlBusq = new Panel { Dock = DockStyle.Top, Height = 34 };
            txtBuscar = new TextBox { Left = 6, Top = 6, Width = 300 };
            txtBuscar.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) { e.Handled = true; listarProductos(); } };
            btnBuscar = new Button { Text = "Buscar", Left = 312, Top = 5, Width = 80 };
            btnBuscar.Click += (s, e) => listarProductos();
            btnLimpiar = new Button { Text = "Limpiar", Left = 398, Top = 5, Width = 80 };
            btnLimpiar.Click += (s, e) => { txtBuscar.Clear(); listarProductos(); };
            pnlBusq.Controls.AddRange(new Control[] { txtBuscar, btnBuscar, btnLimpiar });

            var pnlAgregar = new Panel { Dock = DockStyle.Bottom, Height = 38 };
            lblPrecio = new Label { Text = "Precio unitario (USD):", Left = 6, Top = 11, AutoSize = true };
            nudPrecio = new NumericUpDown { Left = 140, Top = 7, Width = 100, DecimalPlaces = 2, Minimum = 0, Maximum = 10000000, Increment = 1 };
            var lblCant = new Label { Text = "Cantidad:", Left = 260, Top = 11, AutoSize = true };
            nudCantidad = new NumericUpDown { Left = 320, Top = 7, Width = 70, Minimum = 1, Maximum = 100000, Value = 1 };
            btnAgregar = new Button { Text = "Agregar a la compra", Left = 410, Top = 6, Width = 150 };
            btnAgregar.Click += (s, e) => agregarProducto();
            pnlAgregar.Controls.AddRange(new Control[] { lblPrecio, nudPrecio, lblCant, nudCantidad, btnAgregar });

            gbxProductos.Controls.Add(dgvProductos);   // Fill primero, luego los bordes
            gbxProductos.Controls.Add(pnlBusq);
            gbxProductos.Controls.Add(pnlAgregar);

            // Detalle
            var gbxDetalle = new GroupBox { Text = "Detalle de la compra (precio y cantidad se pueden editar en la grilla)", Dock = DockStyle.Fill };
            dgvDetalle = crearGrilla(false);
            dgvDetalle.Columns.Add("id", "id");
            dgvDetalle.Columns.Add("codigo", "Código");
            dgvDetalle.Columns.Add("nombre", "Nombre");
            dgvDetalle.Columns.Add("precio", "Precio unitario");
            dgvDetalle.Columns.Add("cantidad", "Cantidad");
            dgvDetalle.Columns.Add("subtotal", "Subtotal");
            dgvDetalle.Columns["id"].Visible = false;
            foreach (var nombreCol in new[] { "id", "codigo", "nombre", "subtotal" })
                dgvDetalle.Columns[nombreCol].ReadOnly = true;
            dgvDetalle.Columns["precio"].DefaultCellStyle.Format = "N2";
            dgvDetalle.Columns["subtotal"].DefaultCellStyle.Format = "N2";
            dgvDetalle.Columns["precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetalle.Columns["subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetalle.CellEndEdit += dgvDetalle_CellEndEdit;

            var pnlDetBtn = new Panel { Dock = DockStyle.Bottom, Height = 36 };
            btnMas = new Button { Text = "+ Cantidad", Left = 6, Top = 5, Width = 90 };
            btnMas.Click += (s, e) => cambiarCantidad(1);
            btnMenos = new Button { Text = "- Cantidad", Left = 102, Top = 5, Width = 90 };
            btnMenos.Click += (s, e) => cambiarCantidad(-1);
            btnQuitar = new Button { Text = "Quitar producto", Left = 198, Top = 5, Width = 120 };
            btnQuitar.Click += (s, e) => quitarProducto();
            pnlDetBtn.Controls.AddRange(new Control[] { btnMas, btnMenos, btnQuitar });

            gbxDetalle.Controls.Add(dgvDetalle);
            gbxDetalle.Controls.Add(pnlDetBtn);

            tbl.Controls.Add(gbxProductos, 0, 0);
            tbl.Controls.Add(gbxDetalle, 0, 1);

            // ---------- Totales y botones (abajo)
            var pnlPie = new Panel { Dock = DockStyle.Bottom, Height = 64 };
            lblTotal = new Label { Left = 12, Top = 8, AutoSize = true, Font = new Font("Segoe UI", 14F, FontStyle.Bold) };
            lblEquivalente = new Label { Left = 14, Top = 38, AutoSize = true, ForeColor = Color.Gray };
            btnRegistrar = new Button { Text = "Registrar compra", Width = 150, Height = 36, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            btnCancelar = new Button { Text = "Cancelar", Width = 100, Height = 36, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            pnlPie.Controls.AddRange(new Control[] { lblTotal, lblEquivalente, btnRegistrar, btnCancelar });
            pnlPie.Resize += (s, e) =>
            {
                btnRegistrar.Location = new Point(pnlPie.Width - btnRegistrar.Width - 12, 12);
                btnCancelar.Location = new Point(btnRegistrar.Left - btnCancelar.Width - 8, 12);
            };
            btnRegistrar.Click += btnRegistrar_Click;
            btnCancelar.Click += btnCancelar_Click;

            tabNueva.Controls.Add(tbl);          // Fill primero
            tabNueva.Controls.Add(gbxDatos);     // luego Top
            tabNueva.Controls.Add(pnlPie);       // y Bottom
        }

        private void ConstruirPestanaHistorial()
        {
            var tbl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 45));

            dgvHistorial = crearGrilla(true);
            dgvHistorial.SelectionChanged += (s, e) => { actualizarBotonesHistorial(); listarDetalleHistorial(); };

            var gbxDet = new GroupBox { Text = "Detalle de la compra seleccionada", Dock = DockStyle.Fill };
            dgvHistDetalle = crearGrilla(true);
            gbxDet.Controls.Add(dgvHistDetalle);

            tbl.Controls.Add(dgvHistorial, 0, 0);
            tbl.Controls.Add(gbxDet, 0, 1);

            var pnlBtn = new Panel { Dock = DockStyle.Top, Height = 42 };
            btnRecibir = new Button { Text = "Marcar como recibida", Left = 8, Top = 7, Width = 160 };
            btnRecibir.Click += btnRecibir_Click;
            btnAnular = new Button { Text = "Anular compra", Left = 174, Top = 7, Width = 120 };
            btnAnular.Click += btnAnular_Click;
            btnActualizar = new Button { Text = "Actualizar", Left = 300, Top = 7, Width = 90 };
            btnActualizar.Click += (s, e) => listarHistorial();
            pnlBtn.Controls.AddRange(new Control[] { btnRecibir, btnAnular, btnActualizar });

            tabHistorial.Controls.Add(tbl);
            tabHistorial.Controls.Add(pnlBtn);
        }
    }
}

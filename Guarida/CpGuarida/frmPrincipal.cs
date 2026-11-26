using System;
using System.Drawing;
using System.Windows.Forms;

namespace CpGuarida
{
    public partial class FrmPrincipal : Form
    {
        FrmAutenticacion frmAutenticaion;

        // Paleta tomada del logo de Guarida Goblin (morado y rojo)
        private readonly Color colorFondo = Color.FromArgb(28, 28, 36);
        private readonly Color colorTarjeta = Color.FromArgb(44, 44, 58);
        private readonly Color colorMorado = Color.FromArgb(91, 58, 158);
        private readonly Color colorTextoSuave = Color.FromArgb(190, 190, 205);

        private TableLayoutPanel tblCentro;
        private ToolStripStatusLabel lblFecha;
        private System.Windows.Forms.Timer tmrReloj;

        public FrmPrincipal(FrmAutenticacion frmAutenticacion)
        {
            InitializeComponent();
            this.frmAutenticaion = frmAutenticacion;
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tmrReloj != null) tmrReloj.Stop();
            frmAutenticaion.Show();
        }

        private void btnCaProductos_Click(object sender, EventArgs e)
        {
            new FrmProducto().ShowDialog();
        }

        private void btnCaClientes_Click(object sender, EventArgs e)
        {
            new FrmCliente().ShowDialog();
        }

        private void btnCaVentas_Click(object sender, EventArgs e)
        {
            new FrmVenta().ShowDialog();
        }

        private void btnVentaDetalles_Click(object sender, EventArgs e)
        {
            new FrmVentaDetalle().ShowDialog();
        }

        private void btnCaCompras_Click(object sender, EventArgs e)
        {
            new FrmCompra().ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión?", "::: Mensaje - Guarida Goblin",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close(); // FormClosing vuelve a mostrar el login
            }
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            ConfigurarDiseno();
        }

        // ------------------------------------------------------------
        // DISEÑO DEL MENÚ PRINCIPAL
        // ------------------------------------------------------------
        private void ConfigurarDiseno()
        {
            // 1. Fondo oscuro en lugar de la foto estirada del supermercado
            this.BackColor = colorFondo;
            pictureBox1.Image = null;
            pictureBox1.BackColor = colorFondo;

            // 2. El botón Clientes del ribbon no hacía nada: ahora abre FrmCliente
            btnCaClientes.Click += btnCaClientes_Click;

            // 3. Contenido central: logo, título y accesos rápidos
            ConstruirContenidoCentral();

            // 4. Barra inferior: usuario y fecha/hora
            ConstruirBarraEstado();

            pictureBox1.Resize += (s, ev) => CentrarContenido();
            CentrarContenido();
        }

        private void ConstruirContenidoCentral()
        {
            tblCentro = new TableLayoutPanel();
            tblCentro.AutoSize = true;
            tblCentro.ColumnCount = 1;
            tblCentro.BackColor = colorFondo;

            // Logo (se toma del ícono de la ventana, no hace falta agregar recursos)
            var picLogo = new PictureBox();
            picLogo.Size = new Size(128, 128);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.BackColor = colorFondo;
            picLogo.Anchor = AnchorStyles.None;
            try { picLogo.Image = new Icon(this.Icon, 128, 128).ToBitmap(); } catch { }

            var lblTitulo = new Label();
            lblTitulo.Text = "Guarida Goblin";
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Anchor = AnchorStyles.None;

            var lblSub = new Label();
            lblSub.Text = "Juegos de mesa  ·  Manuales de D&D  ·  Figuras coleccionables";
            lblSub.AutoSize = true;
            lblSub.Font = new Font("Segoe UI", 12F);
            lblSub.ForeColor = colorTextoSuave;
            lblSub.Anchor = AnchorStyles.None;

            var lblBienvenida = new Label();
            lblBienvenida.Text = "Bienvenido, " + (Util.usuario != null ? Util.usuario.usuario1 : "");
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblBienvenida.ForeColor = Color.FromArgb(170, 140, 230);
            lblBienvenida.Margin = new Padding(3, 12, 3, 12);
            lblBienvenida.Anchor = AnchorStyles.None;

            // Accesos rápidos
            var flujo = new FlowLayoutPanel();
            flujo.AutoSize = true;
            flujo.WrapContents = false;
            flujo.FlowDirection = FlowDirection.LeftToRight;
            flujo.BackColor = colorFondo;
            flujo.Anchor = AnchorStyles.None;
            flujo.Controls.Add(CrearTarjeta("Registrar Venta", Properties.Resources.carrito_de_compras, btnCaVentas_Click));
            flujo.Controls.Add(CrearTarjeta("Ventas", Properties.Resources.informe, btnVentaDetalles_Click));
            flujo.Controls.Add(CrearTarjeta("Productos", Properties.Resources.producto_1, btnCaProductos_Click));
            flujo.Controls.Add(CrearTarjeta("Compras", Properties.Resources.provider, btnCaCompras_Click));
            // En el lugar de "Clientes" va "Cerrar sesión" (Clientes sigue en el ribbon y en la venta)
            var tarjetaSalir = CrearTarjeta("Cerrar sesión", Properties.Resources.security_lock, btnCerrarSesion_Click);
            tarjetaSalir.FlatAppearance.BorderColor = Color.FromArgb(200, 40, 60);
            tarjetaSalir.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 30, 50);
            tarjetaSalir.FlatAppearance.MouseDownBackColor = Color.FromArgb(120, 20, 40);
            flujo.Controls.Add(tarjetaSalir);

            tblCentro.Controls.Add(picLogo);
            tblCentro.Controls.Add(lblTitulo);
            tblCentro.Controls.Add(lblSub);
            tblCentro.Controls.Add(lblBienvenida);
            tblCentro.Controls.Add(flujo);

            pictureBox1.Controls.Add(tblCentro);
        }

        private Button CrearTarjeta(string texto, Image icono, EventHandler accion)
        {
            var btn = new Button();
            btn.Text = texto;
            btn.Size = new Size(170, 150);
            btn.Margin = new Padding(12);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = colorTarjeta;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Image = new Bitmap(icono, new Size(64, 64));
            btn.TextImageRelation = TextImageRelation.ImageAboveText;
            btn.FlatAppearance.BorderColor = colorMorado;
            btn.FlatAppearance.MouseOverBackColor = colorMorado;
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 45, 125);
            btn.Click += accion;
            return btn;
        }

        private void ConstruirBarraEstado()
        {
            var barra = new StatusStrip();
            barra.BackColor = colorMorado;
            barra.SizingGrip = false;

            var lblUsuario = new ToolStripStatusLabel();
            lblUsuario.Text = "Usuario: " + (Util.usuario != null ? Util.usuario.usuario1 : "-");
            lblUsuario.ForeColor = Color.White;

            lblFecha = new ToolStripStatusLabel();
            lblFecha.Spring = true;
            lblFecha.TextAlign = ContentAlignment.MiddleCenter;
            lblFecha.ForeColor = Color.White;

            barra.Items.Add(lblUsuario);
            barra.Items.Add(lblFecha);
            Controls.Add(barra);

            tmrReloj = new System.Windows.Forms.Timer();
            tmrReloj.Interval = 1000;
            tmrReloj.Tick += (s, ev) => ActualizarFecha();
            tmrReloj.Start();
            ActualizarFecha();
        }

        private void ActualizarFecha()
        {
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss");
        }

        private void CentrarContenido()
        {
            if (tblCentro == null) return;
            tblCentro.PerformLayout();
            int x = Math.Max(0, (pictureBox1.ClientSize.Width - tblCentro.Width) / 2);
            int y = Math.Max(0, (pictureBox1.ClientSize.Height - tblCentro.Height) / 2);
            tblCentro.Location = new Point(x, y);
        }
    }
}

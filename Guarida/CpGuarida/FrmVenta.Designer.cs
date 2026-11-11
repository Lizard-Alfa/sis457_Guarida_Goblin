namespace CpGuarida
{
    partial class FrmVenta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVenta));
            this.label1 = new System.Windows.Forms.Label();
            this.pnlCliente = new System.Windows.Forms.Panel();
            this.btnCliente = new System.Windows.Forms.Button();
            this.pbxCliente = new System.Windows.Forms.PictureBox();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cbxCliente = new System.Windows.Forms.ComboBox();
            this.dgvDatosVenta = new System.Windows.Forms.DataGridView();
            this.gbxListaDatosVenta = new System.Windows.Forms.GroupBox();
            this.gbxListaProducto = new System.Windows.Forms.GroupBox();
            this.dgvListaProducto = new System.Windows.Forms.DataGridView();
            this.lblListaProducto = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.txtFecha = new System.Windows.Forms.TextBox();
            this.tmrFecha = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbxUsuario = new System.Windows.Forms.PictureBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbxHoraFecha = new System.Windows.Forms.PictureBox();
            this.lblfechaHora = new System.Windows.Forms.Label();
            this.pnlDatosVENTA = new System.Windows.Forms.Panel();
            this.lblCinfo = new System.Windows.Forms.Label();
            this.nudRecibido = new System.Windows.Forms.NumericUpDown();
            this.cbxMetoPago = new System.Windows.Forms.ComboBox();
            this.linea = new System.Windows.Forms.Panel();
            this.lblMetodoPago = new System.Windows.Forms.Label();
            this.lblCambio = new System.Windows.Forms.Label();
            this.lblRecibido = new System.Windows.Forms.Label();
            this.lblIvaInfo = new System.Windows.Forms.Label();
            this.lblSubInfo = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.lblProcentage = new System.Windows.Forms.Label();
            this.nudDescuento = new System.Windows.Forms.NumericUpDown();
            this.lblIva = new System.Windows.Forms.Label();
            this.lblSubToral = new System.Windows.Forms.Label();
            this.lblDescuento = new System.Windows.Forms.Label();
            this.lblOpservaciones = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRcantidad = new System.Windows.Forms.Button();
            this.btnScantidad = new System.Windows.Forms.Button();
            this.lblSumarRestar = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBorrarProducto = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnRegistrarVenta = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.pbxTitulo = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblTotalInfo = new System.Windows.Forms.Label();
            this.pnlAcciones = new System.Windows.Forms.Panel();
            this.erpCliente = new System.Windows.Forms.ErrorProvider(this.components);
            this.erpMetoPago = new System.Windows.Forms.ErrorProvider(this.components);
            this.erpRecibido = new System.Windows.Forms.ErrorProvider(this.components);
            this.erpDatosVenta = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlCliente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCliente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatosVenta)).BeginInit();
            this.gbxListaDatosVenta.SuspendLayout();
            this.gbxListaProducto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaProducto)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxUsuario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxHoraFecha)).BeginInit();
            this.pnlDatosVENTA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecibido)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDescuento)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxTitulo)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnlAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erpCliente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpMetoPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpRecibido)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpDatosVenta)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(-1, -5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1208, 98);
            this.label1.TabIndex = 0;
            this.label1.Text = "                  Punto Venta";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlCliente
            // 
            this.pnlCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCliente.Controls.Add(this.btnCliente);
            this.pnlCliente.Controls.Add(this.pbxCliente);
            this.pnlCliente.Controls.Add(this.lblCliente);
            this.pnlCliente.Controls.Add(this.cbxCliente);
            this.pnlCliente.Location = new System.Drawing.Point(2, 100);
            this.pnlCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlCliente.Name = "pnlCliente";
            this.pnlCliente.Size = new System.Drawing.Size(432, 114);
            this.pnlCliente.TabIndex = 2;
            // 
            // btnCliente
            // 
            this.btnCliente.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnCliente.Image = global::CpGuarida.Properties.Resources._new;
            this.btnCliente.Location = new System.Drawing.Point(378, 40);
            this.btnCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCliente.Name = "btnCliente";
            this.btnCliente.Size = new System.Drawing.Size(49, 61);
            this.btnCliente.TabIndex = 4;
            this.btnCliente.UseVisualStyleBackColor = false;
            // 
            // pbxCliente
            // 
            this.pbxCliente.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pbxCliente.Image = global::CpGuarida.Properties.Resources.cliente;
            this.pbxCliente.Location = new System.Drawing.Point(3, 4);
            this.pbxCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbxCliente.Name = "pbxCliente";
            this.pbxCliente.Size = new System.Drawing.Size(91, 107);
            this.pbxCliente.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxCliente.TabIndex = 3;
            this.pbxCliente.TabStop = false;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCliente.Location = new System.Drawing.Point(101, 29);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(68, 21);
            this.lblCliente.TabIndex = 1;
            this.lblCliente.Text = "Cliente:";
            // 
            // cbxCliente
            // 
            this.cbxCliente.FormattingEnabled = true;
            this.cbxCliente.Location = new System.Drawing.Point(101, 57);
            this.cbxCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbxCliente.Name = "cbxCliente";
            this.cbxCliente.Size = new System.Drawing.Size(234, 29);
            this.cbxCliente.TabIndex = 0;
            this.cbxCliente.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbxCliente_KeyUp);
            // 
            // dgvDatosVenta
            // 
            this.dgvDatosVenta.AllowUserToAddRows = false;
            this.dgvDatosVenta.AllowUserToDeleteRows = false;
            this.dgvDatosVenta.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDatosVenta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatosVenta.Location = new System.Drawing.Point(3, 26);
            this.dgvDatosVenta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvDatosVenta.MultiSelect = false;
            this.dgvDatosVenta.Name = "dgvDatosVenta";
            this.dgvDatosVenta.ReadOnly = true;
            this.dgvDatosVenta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDatosVenta.Size = new System.Drawing.Size(682, 158);
            this.dgvDatosVenta.TabIndex = 0;
            // 
            // gbxListaDatosVenta
            // 
            this.gbxListaDatosVenta.Controls.Add(this.dgvDatosVenta);
            this.gbxListaDatosVenta.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxListaDatosVenta.Location = new System.Drawing.Point(2, 450);
            this.gbxListaDatosVenta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxListaDatosVenta.Name = "gbxListaDatosVenta";
            this.gbxListaDatosVenta.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxListaDatosVenta.Size = new System.Drawing.Size(697, 188);
            this.gbxListaDatosVenta.TabIndex = 3;
            this.gbxListaDatosVenta.TabStop = false;
            this.gbxListaDatosVenta.Text = "Datos de venta:";
            // 
            // gbxListaProducto
            // 
            this.gbxListaProducto.Controls.Add(this.dgvListaProducto);
            this.gbxListaProducto.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxListaProducto.Location = new System.Drawing.Point(2, 267);
            this.gbxListaProducto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxListaProducto.Name = "gbxListaProducto";
            this.gbxListaProducto.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxListaProducto.Size = new System.Drawing.Size(861, 180);
            this.gbxListaProducto.TabIndex = 4;
            this.gbxListaProducto.TabStop = false;
            this.gbxListaProducto.Text = "Lista de producto";
            // 
            // dgvListaProducto
            // 
            this.dgvListaProducto.AllowUserToAddRows = false;
            this.dgvListaProducto.AllowUserToDeleteRows = false;
            this.dgvListaProducto.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvListaProducto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListaProducto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListaProducto.Location = new System.Drawing.Point(3, 26);
            this.dgvListaProducto.MultiSelect = false;
            this.dgvListaProducto.Name = "dgvListaProducto";
            this.dgvListaProducto.ReadOnly = true;
            this.dgvListaProducto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListaProducto.Size = new System.Drawing.Size(855, 150);
            this.dgvListaProducto.TabIndex = 0;
            // 
            // lblListaProducto
            // 
            this.lblListaProducto.AutoSize = true;
            this.lblListaProducto.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListaProducto.Location = new System.Drawing.Point(169, 243);
            this.lblListaProducto.Name = "lblListaProducto";
            this.lblListaProducto.Size = new System.Drawing.Size(134, 21);
            this.lblListaProducto.TabIndex = 1;
            this.lblListaProducto.Text = "Buscar Producto";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(326, 243);
            this.txtBuscar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(361, 29);
            this.txtBuscar.TabIndex = 5;
            this.txtBuscar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBuscar_KeyPress);
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnBuscar.Location = new System.Drawing.Point(693, 236);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(103, 41);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.ForestGreen;
            this.btnAgregar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAgregar.Location = new System.Drawing.Point(757, 450);
            this.btnAgregar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(103, 41);
            this.btnAgregar.TabIndex = 8;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // txtFecha
            // 
            this.txtFecha.Location = new System.Drawing.Point(3, 62);
            this.txtFecha.Name = "txtFecha";
            this.txtFecha.ReadOnly = true;
            this.txtFecha.Size = new System.Drawing.Size(167, 29);
            this.txtFecha.TabIndex = 9;
            // 
            // tmrFecha
            // 
            this.tmrFecha.Enabled = true;
            this.tmrFecha.Interval = 1000;
            this.tmrFecha.Tick += new System.EventHandler(this.tmrFecha_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pbxUsuario);
            this.panel1.Controls.Add(this.lblUsuario);
            this.panel1.Controls.Add(this.txtUsuario);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pbxHoraFecha);
            this.panel1.Controls.Add(this.lblfechaHora);
            this.panel1.Controls.Add(this.txtFecha);
            this.panel1.Location = new System.Drawing.Point(693, 96);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 106);
            this.panel1.TabIndex = 10;
            // 
            // pbxUsuario
            // 
            this.pbxUsuario.Image = global::CpGuarida.Properties.Resources.usuario;
            this.pbxUsuario.Location = new System.Drawing.Point(310, 9);
            this.pbxUsuario.Name = "pbxUsuario";
            this.pbxUsuario.Size = new System.Drawing.Size(47, 34);
            this.pbxUsuario.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxUsuario.TabIndex = 15;
            this.pbxUsuario.TabStop = false;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.Location = new System.Drawing.Point(363, 9);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(94, 30);
            this.lblUsuario.TabIndex = 14;
            this.lblUsuario.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(323, 62);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(167, 29);
            this.txtUsuario.TabIndex = 13;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CpGuarida.Properties.Resources.calendario;
            this.pictureBox1.Location = new System.Drawing.Point(176, 57);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(47, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // pbxHoraFecha
            // 
            this.pbxHoraFecha.Image = global::CpGuarida.Properties.Resources.fecha_limite;
            this.pbxHoraFecha.Location = new System.Drawing.Point(3, 5);
            this.pbxHoraFecha.Name = "pbxHoraFecha";
            this.pbxHoraFecha.Size = new System.Drawing.Size(47, 34);
            this.pbxHoraFecha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxHoraFecha.TabIndex = 11;
            this.pbxHoraFecha.TabStop = false;
            // 
            // lblfechaHora
            // 
            this.lblfechaHora.AutoSize = true;
            this.lblfechaHora.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblfechaHora.Location = new System.Drawing.Point(56, 5);
            this.lblfechaHora.Name = "lblfechaHora";
            this.lblfechaHora.Size = new System.Drawing.Size(167, 30);
            this.lblfechaHora.TabIndex = 10;
            this.lblfechaHora.Text = "Fecha de Venta:";
            // 
            // pnlDatosVENTA
            // 
            this.pnlDatosVENTA.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlDatosVENTA.Controls.Add(this.lblCinfo);
            this.pnlDatosVENTA.Controls.Add(this.nudRecibido);
            this.pnlDatosVENTA.Controls.Add(this.cbxMetoPago);
            this.pnlDatosVENTA.Controls.Add(this.linea);
            this.pnlDatosVENTA.Controls.Add(this.lblMetodoPago);
            this.pnlDatosVENTA.Controls.Add(this.lblCambio);
            this.pnlDatosVENTA.Controls.Add(this.lblRecibido);
            this.pnlDatosVENTA.Controls.Add(this.lblIvaInfo);
            this.pnlDatosVENTA.Controls.Add(this.lblSubInfo);
            this.pnlDatosVENTA.Controls.Add(this.txtObservaciones);
            this.pnlDatosVENTA.Controls.Add(this.lblProcentage);
            this.pnlDatosVENTA.Controls.Add(this.nudDescuento);
            this.pnlDatosVENTA.Controls.Add(this.lblIva);
            this.pnlDatosVENTA.Controls.Add(this.lblSubToral);
            this.pnlDatosVENTA.Controls.Add(this.lblDescuento);
            this.pnlDatosVENTA.Controls.Add(this.lblOpservaciones);
            this.pnlDatosVENTA.Location = new System.Drawing.Point(887, 208);
            this.pnlDatosVENTA.Name = "pnlDatosVENTA";
            this.pnlDatosVENTA.Size = new System.Drawing.Size(320, 320);
            this.pnlDatosVENTA.TabIndex = 13;
            // 
            // lblCinfo
            // 
            this.lblCinfo.AutoSize = true;
            this.lblCinfo.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCinfo.Location = new System.Drawing.Point(216, 290);
            this.lblCinfo.Name = "lblCinfo";
            this.lblCinfo.Size = new System.Drawing.Size(51, 25);
            this.lblCinfo.TabIndex = 17;
            this.lblCinfo.Text = "0.00";
            // 
            // nudRecibido
            // 
            this.nudRecibido.DecimalPlaces = 2;
            this.nudRecibido.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudRecibido.Location = new System.Drawing.Point(174, 250);
            this.nudRecibido.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudRecibido.Name = "nudRecibido";
            this.nudRecibido.Size = new System.Drawing.Size(122, 29);
            this.nudRecibido.TabIndex = 16;
            this.nudRecibido.ValueChanged += new System.EventHandler(this.npnRecibido_ValueChanged);
            // 
            // cbxMetoPago
            // 
            this.cbxMetoPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMetoPago.FormattingEnabled = true;
            this.cbxMetoPago.Location = new System.Drawing.Point(174, 199);
            this.cbxMetoPago.Name = "cbxMetoPago";
            this.cbxMetoPago.Size = new System.Drawing.Size(121, 29);
            this.cbxMetoPago.TabIndex = 15;
            // 
            // linea
            // 
            this.linea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.linea.Location = new System.Drawing.Point(10, 187);
            this.linea.Name = "linea";
            this.linea.Size = new System.Drawing.Size(284, 10);
            this.linea.TabIndex = 14;
            // 
            // lblMetodoPago
            // 
            this.lblMetodoPago.AutoSize = true;
            this.lblMetodoPago.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetodoPago.Location = new System.Drawing.Point(8, 199);
            this.lblMetodoPago.Name = "lblMetodoPago";
            this.lblMetodoPago.Size = new System.Drawing.Size(119, 50);
            this.lblMetodoPago.TabIndex = 13;
            this.lblMetodoPago.Text = "Metodo de \r\nPago:";
            // 
            // lblCambio
            // 
            this.lblCambio.AutoSize = true;
            this.lblCambio.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCambio.Location = new System.Drawing.Point(8, 290);
            this.lblCambio.Name = "lblCambio";
            this.lblCambio.Size = new System.Drawing.Size(89, 25);
            this.lblCambio.TabIndex = 12;
            this.lblCambio.Text = "Cambio:";
            // 
            // lblRecibido
            // 
            this.lblRecibido.AutoSize = true;
            this.lblRecibido.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecibido.Location = new System.Drawing.Point(8, 258);
            this.lblRecibido.Name = "lblRecibido";
            this.lblRecibido.Size = new System.Drawing.Size(99, 25);
            this.lblRecibido.TabIndex = 11;
            this.lblRecibido.Text = "Recibido:";
            // 
            // lblIvaInfo
            // 
            this.lblIvaInfo.AutoSize = true;
            this.lblIvaInfo.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIvaInfo.Location = new System.Drawing.Point(216, 159);
            this.lblIvaInfo.Name = "lblIvaInfo";
            this.lblIvaInfo.Size = new System.Drawing.Size(51, 25);
            this.lblIvaInfo.TabIndex = 10;
            this.lblIvaInfo.Text = "0.00";
            // 
            // lblSubInfo
            // 
            this.lblSubInfo.AutoSize = true;
            this.lblSubInfo.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubInfo.Location = new System.Drawing.Point(216, 96);
            this.lblSubInfo.Name = "lblSubInfo";
            this.lblSubInfo.Size = new System.Drawing.Size(51, 25);
            this.lblSubInfo.TabIndex = 9;
            this.lblSubInfo.Text = "0.00";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(8, 64);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(285, 29);
            this.txtObservaciones.TabIndex = 8;
            // 
            // lblProcentage
            // 
            this.lblProcentage.AutoSize = true;
            this.lblProcentage.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblProcentage.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcentage.Location = new System.Drawing.Point(265, 124);
            this.lblProcentage.Name = "lblProcentage";
            this.lblProcentage.Size = new System.Drawing.Size(29, 25);
            this.lblProcentage.TabIndex = 7;
            this.lblProcentage.Text = "%";
            // 
            // nudDescuento
            // 
            this.nudDescuento.DecimalPlaces = 2;
            this.nudDescuento.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudDescuento.Location = new System.Drawing.Point(203, 124);
            this.nudDescuento.Name = "nudDescuento";
            this.nudDescuento.Size = new System.Drawing.Size(60, 29);
            this.nudDescuento.TabIndex = 6;
            this.nudDescuento.ValueChanged += new System.EventHandler(this.npnDescuento_ValueChanged);
            // 
            // lblIva
            // 
            this.lblIva.AutoSize = true;
            this.lblIva.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIva.Location = new System.Drawing.Point(6, 162);
            this.lblIva.Name = "lblIva";
            this.lblIva.Size = new System.Drawing.Size(105, 25);
            this.lblIva.TabIndex = 4;
            this.lblIva.Text = "IVA(13%):";
            // 
            // lblSubToral
            // 
            this.lblSubToral.AutoSize = true;
            this.lblSubToral.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubToral.Location = new System.Drawing.Point(3, 96);
            this.lblSubToral.Name = "lblSubToral";
            this.lblSubToral.Size = new System.Drawing.Size(98, 25);
            this.lblSubToral.TabIndex = 2;
            this.lblSubToral.Text = "Subtotal:";
            // 
            // lblDescuento
            // 
            this.lblDescuento.AutoSize = true;
            this.lblDescuento.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescuento.Location = new System.Drawing.Point(3, 128);
            this.lblDescuento.Name = "lblDescuento";
            this.lblDescuento.Size = new System.Drawing.Size(116, 25);
            this.lblDescuento.TabIndex = 1;
            this.lblDescuento.Text = "Descuento:";
            // 
            // lblOpservaciones
            // 
            this.lblOpservaciones.AutoSize = true;
            this.lblOpservaciones.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpservaciones.Location = new System.Drawing.Point(3, 31);
            this.lblOpservaciones.Name = "lblOpservaciones";
            this.lblOpservaciones.Size = new System.Drawing.Size(168, 30);
            this.lblOpservaciones.TabIndex = 0;
            this.lblOpservaciones.Text = "Observaciones:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.White;
            this.lblTotal.Location = new System.Drawing.Point(3, 7);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(103, 45);
            this.lblTotal.TabIndex = 3;
            this.lblTotal.Text = "Total:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel3.Controls.Add(this.btnRcantidad);
            this.panel3.Controls.Add(this.btnScantidad);
            this.panel3.Controls.Add(this.lblSumarRestar);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(705, 498);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(155, 101);
            this.panel3.TabIndex = 14;
            // 
            // btnRcantidad
            // 
            this.btnRcantidad.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnRcantidad.FlatAppearance.BorderSize = 0;
            this.btnRcantidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRcantidad.Image = global::CpGuarida.Properties.Resources.boton_meno;
            this.btnRcantidad.Location = new System.Drawing.Point(18, 53);
            this.btnRcantidad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRcantidad.Name = "btnRcantidad";
            this.btnRcantidad.Size = new System.Drawing.Size(34, 38);
            this.btnRcantidad.TabIndex = 6;
            this.btnRcantidad.UseVisualStyleBackColor = false;
            this.btnRcantidad.Click += new System.EventHandler(this.btnRcantidad_Click);
            // 
            // btnScantidad
            // 
            this.btnScantidad.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnScantidad.FlatAppearance.BorderSize = 0;
            this.btnScantidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScantidad.Image = global::CpGuarida.Properties.Resources.agregar;
            this.btnScantidad.Location = new System.Drawing.Point(97, 49);
            this.btnScantidad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnScantidad.Name = "btnScantidad";
            this.btnScantidad.Size = new System.Drawing.Size(34, 47);
            this.btnScantidad.TabIndex = 5;
            this.btnScantidad.UseVisualStyleBackColor = false;
            this.btnScantidad.Click += new System.EventHandler(this.btnScantidad_Click);
            // 
            // lblSumarRestar
            // 
            this.lblSumarRestar.AutoSize = true;
            this.lblSumarRestar.Location = new System.Drawing.Point(0, 3);
            this.lblSumarRestar.Name = "lblSumarRestar";
            this.lblSumarRestar.Size = new System.Drawing.Size(158, 42);
            this.lblSumarRestar.TabIndex = 4;
            this.lblSumarRestar.Text = "Aumentar o\r\ndisminuir cantidad \r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(698, 602);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "Eliminar";
            // 
            // btnBorrarProducto
            // 
            this.btnBorrarProducto.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnBorrarProducto.FlatAppearance.BorderSize = 0;
            this.btnBorrarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBorrarProducto.Image = global::CpGuarida.Properties.Resources.borrar__1_;
            this.btnBorrarProducto.Location = new System.Drawing.Point(701, 623);
            this.btnBorrarProducto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBorrarProducto.Name = "btnBorrarProducto";
            this.btnBorrarProducto.Size = new System.Drawing.Size(71, 39);
            this.btnBorrarProducto.TabIndex = 15;
            this.btnBorrarProducto.UseVisualStyleBackColor = false;
            this.btnBorrarProducto.Click += new System.EventHandler(this.btnBorrarProducto_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Red;
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCancelar.FlatAppearance.BorderSize = 4;
            this.btnCancelar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCancelar.Image = global::CpGuarida.Properties.Resources.borrar;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(185, 8);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(121, 52);
            this.btnCancelar.TabIndex = 12;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnRegistrarVenta
            // 
            this.btnRegistrarVenta.BackColor = System.Drawing.Color.ForestGreen;
            this.btnRegistrarVenta.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRegistrarVenta.FlatAppearance.BorderSize = 4;
            this.btnRegistrarVenta.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnRegistrarVenta.Image = global::CpGuarida.Properties.Resources.bien;
            this.btnRegistrarVenta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRegistrarVenta.Location = new System.Drawing.Point(0, 8);
            this.btnRegistrarVenta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRegistrarVenta.Name = "btnRegistrarVenta";
            this.btnRegistrarVenta.Size = new System.Drawing.Size(163, 52);
            this.btnRegistrarVenta.TabIndex = 11;
            this.btnRegistrarVenta.Text = "Registrar Venta";
            this.btnRegistrarVenta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRegistrarVenta.UseVisualStyleBackColor = false;
            this.btnRegistrarVenta.Click += new System.EventHandler(this.btnRegistrarVenta_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnLimpiar.Image = global::CpGuarida.Properties.Resources.limpieza;
            this.btnLimpiar.Location = new System.Drawing.Point(799, 237);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(71, 39);
            this.btnLimpiar.TabIndex = 7;
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click_1);
            // 
            // pbxTitulo
            // 
            this.pbxTitulo.BackColor = System.Drawing.Color.SteelBlue;
            this.pbxTitulo.Image = global::CpGuarida.Properties.Resources.carrito_de_compras;
            this.pbxTitulo.Location = new System.Drawing.Point(24, 4);
            this.pbxTitulo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbxTitulo.Name = "pbxTitulo";
            this.pbxTitulo.Size = new System.Drawing.Size(128, 90);
            this.pbxTitulo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxTitulo.TabIndex = 1;
            this.pbxTitulo.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Navy;
            this.panel4.Controls.Add(this.lblTotalInfo);
            this.panel4.Controls.Add(this.lblTotal);
            this.panel4.Location = new System.Drawing.Point(887, 534);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(306, 63);
            this.panel4.TabIndex = 16;
            // 
            // lblTotalInfo
            // 
            this.lblTotalInfo.AutoSize = true;
            this.lblTotalInfo.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalInfo.ForeColor = System.Drawing.Color.White;
            this.lblTotalInfo.Location = new System.Drawing.Point(184, 7);
            this.lblTotalInfo.Name = "lblTotalInfo";
            this.lblTotalInfo.Size = new System.Drawing.Size(83, 45);
            this.lblTotalInfo.TabIndex = 4;
            this.lblTotalInfo.Text = "0.00";
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pnlAcciones.Controls.Add(this.btnRegistrarVenta);
            this.pnlAcciones.Controls.Add(this.btnCancelar);
            this.pnlAcciones.Location = new System.Drawing.Point(887, 602);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(306, 71);
            this.pnlAcciones.TabIndex = 17;
            // 
            // erpCliente
            // 
            this.erpCliente.ContainerControl = this;
            // 
            // erpMetoPago
            // 
            this.erpMetoPago.ContainerControl = this;
            // 
            // erpRecibido
            // 
            this.erpRecibido.ContainerControl = this;
            // 
            // erpDatosVenta
            // 
            this.erpDatosVenta.ContainerControl = this;
            // 
            // FrmVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1208, 678);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.btnBorrarProducto);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnlDatosVENTA);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.lblListaProducto);
            this.Controls.Add(this.gbxListaProducto);
            this.Controls.Add(this.gbxListaDatosVenta);
            this.Controls.Add(this.pnlCliente);
            this.Controls.Add(this.pbxTitulo);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmVenta";
            this.Padding = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmVenta";
            this.Load += new System.EventHandler(this.FrmVenta_Load);
            this.pnlCliente.ResumeLayout(false);
            this.pnlCliente.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCliente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatosVenta)).EndInit();
            this.gbxListaDatosVenta.ResumeLayout(false);
            this.gbxListaProducto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaProducto)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxUsuario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxHoraFecha)).EndInit();
            this.pnlDatosVENTA.ResumeLayout(false);
            this.pnlDatosVENTA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecibido)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDescuento)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxTitulo)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.erpCliente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpMetoPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpRecibido)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erpDatosVenta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbxTitulo;
        private System.Windows.Forms.Panel pnlCliente;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.ComboBox cbxCliente;
        private System.Windows.Forms.PictureBox pbxCliente;
        private System.Windows.Forms.Button btnCliente;
        private System.Windows.Forms.DataGridView dgvDatosVenta;
        private System.Windows.Forms.GroupBox gbxListaDatosVenta;
        private System.Windows.Forms.GroupBox gbxListaProducto;
        private System.Windows.Forms.Label lblListaProducto;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.DataGridView dgvListaProducto;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.TextBox txtFecha;
        private System.Windows.Forms.Timer tmrFecha;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblfechaHora;
        private System.Windows.Forms.PictureBox pbxHoraFecha;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbxUsuario;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Button btnRegistrarVenta;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Panel pnlDatosVENTA;
        private System.Windows.Forms.Label lblOpservaciones;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblSubToral;
        private System.Windows.Forms.Label lblDescuento;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSumarRestar;
        private System.Windows.Forms.Button btnScantidad;
        private System.Windows.Forms.Button btnRcantidad;
        private System.Windows.Forms.Button btnBorrarProducto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblIva;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.NumericUpDown nudDescuento;
        private System.Windows.Forms.Label lblProcentage;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Label lblIvaInfo;
        private System.Windows.Forms.Label lblSubInfo;
        private System.Windows.Forms.Label lblTotalInfo;
        private System.Windows.Forms.Label lblMetodoPago;
        private System.Windows.Forms.Label lblCambio;
        private System.Windows.Forms.Label lblRecibido;
        private System.Windows.Forms.Panel linea;
        private System.Windows.Forms.Label lblCinfo;
        private System.Windows.Forms.NumericUpDown nudRecibido;
        private System.Windows.Forms.ComboBox cbxMetoPago;
        private System.Windows.Forms.ErrorProvider erpCliente;
        private System.Windows.Forms.ErrorProvider erpMetoPago;
        private System.Windows.Forms.ErrorProvider erpRecibido;
        private System.Windows.Forms.ErrorProvider erpDatosVenta;
    }
}
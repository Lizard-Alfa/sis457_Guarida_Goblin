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
            this.lblfechaHora = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.pbxUsuario = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbxHoraFecha = new System.Windows.Forms.PictureBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnCliente = new System.Windows.Forms.Button();
            this.pbxCliente = new System.Windows.Forms.PictureBox();
            this.pbxTitulo = new System.Windows.Forms.PictureBox();
            this.pnlCliente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatosVenta)).BeginInit();
            this.gbxListaDatosVenta.SuspendLayout();
            this.gbxListaProducto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaProducto)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxUsuario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxHoraFecha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCliente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxTitulo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(-1, -5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1194, 98);
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
            this.pnlCliente.Size = new System.Drawing.Size(396, 114);
            this.pnlCliente.TabIndex = 2;
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
            // 
            // dgvDatosVenta
            // 
            this.dgvDatosVenta.AllowUserToAddRows = false;
            this.dgvDatosVenta.AllowUserToDeleteRows = false;
            this.dgvDatosVenta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatosVenta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDatosVenta.Location = new System.Drawing.Point(3, 26);
            this.dgvDatosVenta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvDatosVenta.Name = "dgvDatosVenta";
            this.dgvDatosVenta.ReadOnly = true;
            this.dgvDatosVenta.Size = new System.Drawing.Size(880, 158);
            this.dgvDatosVenta.TabIndex = 0;
            // 
            // gbxListaDatosVenta
            // 
            this.gbxListaDatosVenta.Controls.Add(this.dgvDatosVenta);
            this.gbxListaDatosVenta.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxListaDatosVenta.Location = new System.Drawing.Point(2, 489);
            this.gbxListaDatosVenta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxListaDatosVenta.Name = "gbxListaDatosVenta";
            this.gbxListaDatosVenta.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxListaDatosVenta.Size = new System.Drawing.Size(886, 188);
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
            this.gbxListaProducto.Size = new System.Drawing.Size(886, 180);
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
            this.dgvListaProducto.Location = new System.Drawing.Point(6, 19);
            this.dgvListaProducto.MultiSelect = false;
            this.dgvListaProducto.Name = "dgvListaProducto";
            this.dgvListaProducto.ReadOnly = true;
            this.dgvListaProducto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListaProducto.Size = new System.Drawing.Size(874, 158);
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
            this.btnAgregar.Location = new System.Drawing.Point(779, 451);
            this.btnAgregar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(103, 41);
            this.btnAgregar.TabIndex = 8;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
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
            this.panel1.Size = new System.Drawing.Size(500, 134);
            this.panel1.TabIndex = 10;
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
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(323, 62);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(167, 29);
            this.txtUsuario.TabIndex = 13;
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
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnLimpiar.Image = global::CpGuarida.Properties.Resources.limpieza;
            this.btnLimpiar.Location = new System.Drawing.Point(814, 237);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(71, 39);
            this.btnLimpiar.TabIndex = 7;
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click_1);
            // 
            // btnCliente
            // 
            this.btnCliente.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnCliente.Image = global::CpGuarida.Properties.Resources._new;
            this.btnCliente.Location = new System.Drawing.Point(341, 40);
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
            // FrmVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1197, 678);
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatosVenta)).EndInit();
            this.gbxListaDatosVenta.ResumeLayout(false);
            this.gbxListaProducto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaProducto)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxUsuario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxHoraFecha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCliente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxTitulo)).EndInit();
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
    }
}
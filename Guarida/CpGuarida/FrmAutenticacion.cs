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
    public partial class FrmAutenticacion : Form
    {
        public FrmAutenticacion()
        {
            InitializeComponent();
            ConfigurarEstiloMedieval();
        }

        private void ConfigurarEstiloMedieval()
        {
            // 1. Quitar bordes
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.Text = "";
            this.StartPosition = FormStartPosition.CenterScreen;

            // 2. Configurar TextBox
            txtUsuario.Location = new Point(475, 260);
            txtUsuario.Size = new Size(250, 24);
            txtUsuario.BackColor = Color.White;
            txtUsuario.ForeColor = Color.Black;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Font = new Font("Arial", 11);

            txtClave.Location = new Point(475, 330);
            txtClave.Size = new Size(250, 24);
            txtClave.BackColor = Color.White;
            txtClave.ForeColor = Color.Black;
            txtClave.BorderStyle = BorderStyle.FixedSingle;
            txtClave.Font = new Font("Arial", 11);
            txtClave.PasswordChar = '●';

        }

        private void frmAutenticacion_Load(object sender, EventArgs e)
        {
            ConfigurarEstiloMedieval();
        }
        // LÓGICA DE NEGOCIO 
        private bool validarCampos()
        {
            bool esValido = true;
            erpUsuario.Clear();
            erpClave.Clear();

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                erpUsuario.SetError(txtUsuario, "El usuario es obligatorio");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtClave.Text))
            {
                erpClave.SetError(txtClave, "La contraseña es obligatoria");
                esValido = false;
            }

            return esValido;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                var usuario = UsuarioCln.validar(txtUsuario.Text, Util.Encrypt(txtClave.Text));
                if (usuario == null)
                {
                    MessageBox.Show("Usuario y/o contraseña incorrectos", "::: Mensaje - Guarida Goblin",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Util.usuario = usuario;
                    txtClave.Clear();
                    txtUsuario.Focus();
                    txtUsuario.SelectAll();
                    Hide();
                    new FrmPrincipal(this).ShowDialog();
                }
            }
        }
        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) btnIngresar.PerformClick();
        }

        private void FrmAutenticacion_Load(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

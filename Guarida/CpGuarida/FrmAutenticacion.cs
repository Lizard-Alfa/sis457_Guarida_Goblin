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
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

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
            var usuario = UsuarioCln.validar(txtUsuario.Text, Util.Encrypt(txtClave.Text));
            if(usuario == null)
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

        private void frmAutenticacion_Load(object sender, EventArgs e)
        {

        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) btnIngresar.PerformClick();
        }
    }
}

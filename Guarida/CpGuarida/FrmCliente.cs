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
    public partial class FrmCliente : Form
    {
        public FrmCliente()
        {
            InitializeComponent();
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {

        }

        private void resetearCampos()
        {
            
            txtCi.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtCelular.Clear();
            txtDrireccion.Clear();
            txtEmail.Clear();
        }
        private void resetearErrores()
        {
            erpCi.Clear();
            erpNombres.Clear();
        }
        private bool validarCampos()
        {
            bool esValido = true;

            if (string.IsNullOrWhiteSpace(txtCi.Text))
            {
                erpCi.SetError(txtCi, "El cliente es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                erpNombres.SetError(txtNombre, "El nombre es obligatorio");
                esValido = false;
            }
            
            return esValido;
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                var cliente = new Cliente
                {
                    cedulaIdentidad = txtCi.Text.Trim(),
                    nombres = txtNombre.Text.Trim(),
                    apellidos = txtApellido.Text.Trim(),
                    celular = txtCelular.Text.Trim(),
                    direccion = txtDrireccion.Text.Trim(),
                    email = txtEmail.Text.Trim(),
                    usuarioRegistro = Util.usuario.usuario1
                };
                try
                {
                    
                        cliente.fechaRegistro = DateTime.Now;
                        cliente.estado =(short)Estado.Activo;
                        ClienteCln.crear(cliente);
                      resetearCampos();
                      resetearErrores();
                    btnCancelar.PerformClick();
                    MessageBox.Show("Cliente guardado correctamente", "::: Mensaje - Guarida :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "::: Error - Guarida :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bool hayDatos = !string.IsNullOrWhiteSpace(txtCi.Text)
              || !string.IsNullOrWhiteSpace(txtNombre.Text)
              || !string.IsNullOrWhiteSpace(txtApellido.Text)
              || !string.IsNullOrWhiteSpace(txtCelular.Text)
              || !string.IsNullOrWhiteSpace(txtDrireccion.Text)
              || !string.IsNullOrWhiteSpace(txtEmail.Text);

            if (hayDatos)
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cancelar? Los datos ingresados se perderán.",
                    "::: Confirmación - Guarida :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.No)
                {
                    return;
                }
            }
            Close();
        }
    }
}

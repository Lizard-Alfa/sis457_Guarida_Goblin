using CadGuarida;
using ClnGuarida;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CpGuarida
{
    public partial class FrmProveedor : Form
    {
        private bool esNuevo;

        public FrmProveedor()
        {
            InitializeComponent();
        }

        private void listar()
        {
            string tipoFiltro = cbxTipo?.Text ?? "AMBOS";
            var proveedores = ProveedorCln.listarPa(tipoFiltro, txtParametro.Text.Trim());

            dgvLista.DataSource = proveedores;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["tipo"].HeaderText = "Tipo";
            dgvLista.Columns["razonSocial"].HeaderText = "Razón Social";
            dgvLista.Columns["nit"].HeaderText = "NIT";
            dgvLista.Columns["contacto"].HeaderText = "Contacto";
            dgvLista.Columns["email"].HeaderText = "E-mail";
            dgvLista.Columns["telefono"].HeaderText = "Teléfono";
            dgvLista.Columns["pais"].HeaderText = "País";
            dgvLista.Columns["usuarioRegistro"].Visible = false;
            dgvLista.Columns["fechaRegistro"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;

            if (proveedores.Count > 0)
                dgvLista.CurrentCell = dgvLista.Rows[0].Cells["razonSocial"];

            btnEditar.Enabled = proveedores.Count > 0;
            btnEliminar.Enabled = proveedores.Count > 0;
        }
        private void cargarTipos()
        {
            cbxTipo.Items.Clear();
            cbxTipo.Items.Add("AMBOS");
            cbxTipo.Items.Add("Nacional");
            cbxTipo.Items.Add("Internacional");
            cbxTipo.SelectedIndex = 0;
        }
        private void FrmProveedor_Load(object sender, EventArgs e)
        {
            Size = new Size (800, 340);
            cargarTipos();
            gbxDatos.Visible = false;
            listar();
        }

        private void btnBuscar_Click(object sender, EventArgs e) => listar();

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        // Habilitar/deshabilitar campos
        private void cbxTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool esNacional = cbxTipo.Text == "Nacional";

            txtNIT.Enabled = esNacional;
            txtNIT.Clear();
            txtPais.Enabled = !esNacional;
            txtPais.Clear();

            // Resetear errores
            erpNIT.Clear();
            erpPais.Clear();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(800, 500);
            pnlAcciones.Enabled = false;
            resetearCampos();
            cbxTipo.SelectedIndex = -1;
            gbxDatos.Visible = true;
            txtRazonSocial.Focus();
            
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null) return;
            esNuevo = false;
            Size = new Size(800, 500);
            pnlAcciones.Enabled = false;

            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            var prov = ProveedorCln.obtenerUno(id);

            cbxTipo.Text = prov.tipo;
            txtRazonSocial.Text = prov.razonSocial;
            txtNIT.Text = prov.nit ?? "";
            txtContacto.Text = prov.contacto ?? "";
            txtEmail.Text = prov.email ?? "";
            txtTelefono.Text = prov.telefono ?? "";
            txtPais.Text = prov.pais ?? "";

            gbxDatos.Visible = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(800, 340);
            pnlAcciones.Enabled = true;
            gbxDatos.Visible = false;
        }

        private void resetearCampos()
        {
            cbxTipo.SelectedIndex = -1;
            txtRazonSocial.Clear();
            txtNIT.Clear();
            txtContacto.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtPais.Clear();
            resetearErrores();
        }

        private void resetearErrores()
        {
            erpTipo.Clear();
            erpRazonSocial.Clear();
            erpNIT.Clear();
            erpContacto.Clear();
            erpPais.Clear();
        }

        private bool validarCampos()
        {
            bool ok = true;
            resetearErrores();

            if (string.IsNullOrWhiteSpace(cbxTipo.Text))
            {
                erpTipo.SetError(cbxTipo, "Seleccione un tipo");
                ok = false;
            }
            if (string.IsNullOrWhiteSpace(txtRazonSocial.Text))
            {
                erpRazonSocial.SetError(txtRazonSocial, "La razón social es obligatoria");
                ok = false;
            }

            bool esNacional = cbxTipo.Text == "Nacional";

            // NIT obligatorio solo si es Nacional
            if (esNacional && string.IsNullOrWhiteSpace(txtNIT.Text))
            {
                erpNIT.SetError(txtNIT, "El NIT es obligatorio para proveedores nacionales");
                ok = false;
            }

            // País obligatorio solo si es Internacional
            if (!esNacional && string.IsNullOrWhiteSpace(txtPais.Text))
            {
                erpPais.SetError(txtPais, "El país es obligatorio para proveedores internacionales");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtContacto.Text))
            {
                erpContacto.SetError(txtContacto, "El contacto es obligatorio");
                ok = false;
            }

            return ok;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) return;

            try
            {
                var prov = new Proveedor
                {
                    tipo = cbxTipo.Text,
                    razonSocial = txtRazonSocial.Text.Trim(),
                    nit = string.IsNullOrWhiteSpace(txtNIT.Text) ? null : txtNIT.Text.Trim(),
                    contacto = txtContacto.Text.Trim(),
                    email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                    telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    pais = string.IsNullOrWhiteSpace(txtPais.Text) ? null : txtPais.Text.Trim(),
                    usuarioRegistro = Util.usuario.usuario1
                };

                if (esNuevo)
                {
                    prov.fechaRegistro = DateTime.Now;
                    prov.estado = (short)Estado.Activo;
                    ProveedorCln.crear(prov);
                }
                else
                {
                    prov.id = (int)dgvLista.CurrentRow.Cells["id"].Value;
                    ProveedorCln.actualizar(prov);
                }

                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Proveedor guardado correctamente", "::: Guarida Goblin :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "::: Error - Guarida Goblin :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null) return;
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string razon = dgvLista.CurrentRow.Cells["razonSocial"].Value.ToString();

            var dialog = MessageBox.Show($"¿Dar de baja al proveedor {razon}?",
                "::: Confirmación :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                ProveedorCln.eliminar(id, Util.usuario.usuario1);
                listar();
                MessageBox.Show("Proveedor dado de baja", "::: Guarida Goblin :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();
    }
}
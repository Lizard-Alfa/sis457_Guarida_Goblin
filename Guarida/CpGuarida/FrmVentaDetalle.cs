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
    public partial class FrmVentaDetalle : Form
    {
        public FrmVentaDetalle()
        {
            InitializeComponent();
        }
        private void listar()
        {
            try { 
            var ventas = VentaCln.listarPa( dtpInicio.Checked ? (DateTime?)dtpInicio.Value.Date : null,dtpFinal.Checked ? (DateTime?)dtpFinal.Value.Date : null
         );

            dgvListar.DataSource = ventas;
            dgvListar.Columns["id"].Visible = false;
            dgvListar.Columns["idCliente"].Visible = false;
            dgvListar.Columns["Cliente"].HeaderText = "Cliente";
            dgvListar.Columns["usuario"].HeaderText = "Usuario";
            dgvListar.Columns["transaccion"].HeaderText = "Transacción";
            dgvListar.Columns["fecha"].HeaderText = "Fecha";
            dgvListar.Columns["subtotal"].HeaderText = "Subtotal";
            dgvListar.Columns["descuento"].HeaderText = "Descuento";
            dgvListar.Columns["iva"].HeaderText = "Iva (13%)";
            dgvListar.Columns["metodoPago"].HeaderText = "Metodo de Pago";
            dgvListar.Columns["total"].HeaderText = "Total";
            dgvListar.Columns["observaciones"].Visible = false;
            dgvListar.Columns["usuarioRegistro"].Visible = false;
            dgvListar.Columns["fechaRegistro"].Visible = false;
            dgvListar.Columns["estado"].HeaderText = "Estado";

            if (ventas.Count > 0) dgvListar.CurrentCell = dgvListar.Rows[0].Cells["Cliente"];
        }
        catch (Exception ex)
        {
        MessageBox.Show(ex.Message, "::: Error - Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
}


        private void FrmVentaDetalle_Load(object sender, EventArgs e)
        {
            dtpInicio.Checked = false;
            dtpFinal.Checked = false;
            listar(); 
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void dgvListar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvListar.Columns[e.ColumnIndex].Name == "estado")
            {
                if (short.TryParse(e.Value.ToString(), out short estado))
                {
                    bool esActivo = estado == (short)Estado.Activo;
                    e.Value = esActivo ? "● Activo" : "● Anulado";
                    e.CellStyle.ForeColor = esActivo ? Color.Green : Color.Red;
                    e.FormattingApplied = true;
                }
            }
        }

        private void btxRegarcar_Click(object sender, EventArgs e)
        {
            dtpInicio.Checked = false;
            dtpFinal.Checked = false;
            listar();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)dgvListar.CurrentRow.Cells["id"].Value;
                string transaccion = dgvListar.CurrentRow.Cells["transaccion"].Value.ToString();
                DialogResult dialog = MessageBox.Show($"Está seguro que desea dar de baja la venta {transaccion}?",
                    "::: Confirmación - Guardia Goblin :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    VentaCln.anular(id, Util.usuario.usuario1);
                    dtpInicio.Checked = false;
                    dtpFinal.Checked = false;
                    listar();
                    MessageBox.Show("Venta dado de baja correctamente", "::: Mensaje - Guarida Goblin",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "::: Error - Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            if (dgvListar.CurrentRow == null) return;
            int id = (int)dgvListar.CurrentRow.Cells["id"].Value;

            try
            {
                DetalleCln.validar(id);          // si está anulada, lanza el error y no abre nada
                new FrmDetalle(id).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "::: Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

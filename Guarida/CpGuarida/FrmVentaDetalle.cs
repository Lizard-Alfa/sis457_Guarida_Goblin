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
        private bool todas;
        public FrmVentaDetalle()
        {
            InitializeComponent();
        }
        private void listar()
        {
            // todas = true  -> sin filtro de fechas (trae activas y anuladas)
            // todas = false -> por período
            var ventas = todas
                ? VentaCln.listarPa(null, null)
                : VentaCln.listarPa(dtpInicio.Value.Date , dtpFinal.Value.Date);
            dgvListar.DataSource = ventas;
            dgvListar.Columns["id"].Visible = false;
            dgvListar.Columns["usuario"].Visible = false;
            dgvListar.Columns["fecha"].Visible = false;
            dgvListar.Columns["subtotal"].Visible = false;
            dgvListar.Columns["fechaRegistro"].HeaderText = "Fecha de Venta";
            dgvListar.Columns["Cliente"].HeaderText = "Cliente";
            dgvListar.Columns["transaccion"].HeaderText = "Transacción";
            dgvListar.Columns["total"].HeaderText = "Total";
            dgvListar.Columns["observaciones"].HeaderText = "Observaciones";
            dgvListar.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";
            dgvListar.Columns["estado"].HeaderText = "Estado";
            if (ventas.Count > 0) dgvListar.CurrentCell = dgvListar.Rows[0].Cells["cliente"];
        }


        private void FrmVentaDetalle_Load(object sender, EventArgs e)
        {
            todas = true;// al iniciar: todas, activas y anuladas
            listar(); 
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            todas = false; // por período
            try
            {
                listar(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "::: Error - Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvListar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvListar.Columns[e.ColumnIndex].Name == "estado")
            {
                short estado = Convert.ToInt16(e.Value);
                e.Value = estado == (short)Estado.Activo ? "● Activo" : "● Anulado";
                e.CellStyle.ForeColor = estado == (short)Estado.Activo ? Color.Green : Color.Red;
                e.FormattingApplied = true;
            }
        }

        private void btxRegarcar_Click(object sender, EventArgs e)
        {
            todas = true; // trae , activas y anuladas
            listar();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            if (dgvListar.CurrentRow == null) return;

            int id = (int)dgvListar.CurrentRow.Cells["id"].Value;

            // La transacción puede ser NULL: si no hay, se muestra el id de la venta
            object valorTransaccion = dgvListar.CurrentRow.Cells["transaccion"].Value;
            string referencia = (valorTransaccion == null || valorTransaccion == DBNull.Value)
                ? $"N° {id}"
                : valorTransaccion.ToString();

            DialogResult dialog = MessageBox.Show($"¿Está seguro que desea anular la venta {referencia}?\nSe devolverá el stock de los productos.",
                "::: Confirmación - Guarida Goblin :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                try
                {
                    VentaCln.anular(id, Util.usuario.usuario1);
                    listar();
                    MessageBox.Show("Venta anulada correctamente", "::: Mensaje - Guarida Goblin :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "::: Error - Guarida Goblin :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

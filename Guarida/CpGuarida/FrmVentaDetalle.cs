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

        private void listar(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var ventas = VentaCln.listarPa(fechaInicio, fechaFin);
            dgvListar.DataSource = ventas;
            dgvListar.Columns["id"].Visible = false;
            dgvListar.Columns["usuario"].Visible = false;
            dgvListar.Columns["subtotal"].Visible = false;
            dgvListar.Columns["fecha"].Visible = false;
            dgvListar.Columns["cliente"].HeaderText = "Cliente";
            dgvListar.Columns["cliente"].HeaderText = "Cliente";
            dgvListar.Columns["transaccion"].HeaderText = "Transación";
            dgvListar.Columns["observaciones"].HeaderText = "Observaciones";
            dgvListar.Columns["UsuarioRegistro"].HeaderText = "Usuario que Registro";
            dgvListar.Columns["FechaRegistro"].HeaderText = "Fecha de Venta";
            dgvListar.Columns["total"].HeaderText = "Total";
            dgvListar.Columns["Estado"].HeaderText = "Estado";
            if (ventas.Count > 0) dgvListar.CurrentCell = dgvListar.Rows[0].Cells["cliente"];
        }

        private void FrmVentaDetalle_Load(object sender, EventArgs e)
        {
            listar(); 
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DateTime inicio = dtpInicio.Value.Date;
            DateTime fin = dtpFinal.Value.Date.AddDays(1).AddSeconds(-1);
            listar(inicio, fin);
        }
    }
}

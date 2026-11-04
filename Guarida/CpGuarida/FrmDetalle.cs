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
    public partial class FrmDetalle : Form
    {
        private readonly int idVenta;
        private const string L = "* * * * * * * * * * * * * * * * * * * *";

        public FrmDetalle(int idVenta)
        {
            InitializeComponent();
            this.idVenta = idVenta;
        }

        private void FrmDetalle_Load(object sender, EventArgs e)
        {
            var d = DetalleCln.listarPorVenta(idVenta);
            var v = d.First().Venta;

            rtxtexBo.Text =
                $"\n{"GUARIDA GOBLIN",27}\n{"Sucre, Bolivia",27}\n\n{L}\n{"COMPROBANTE DE VENTA",30}\n{L}\n" +
                $"{"Venta N°:",-20}{idVenta,20}\n" +
                $"{"Transacción:",-20}{(v.transaccion?.ToString() ?? "S/N"),20}\n" +
                $"{"Fecha:",-20}{v.fecha,20:dd/MM/yyyy HH:mm}\n" +
                $"{"Cliente:",-12}{(v.Cliente?.nombres + " " + v.Cliente?.apellidos).Trim(),28}\n{L}\n" +
                $"{"Descripción",-20}{"Subtotal",20}\n" +
                string.Concat(d.Select(x => $"{x.Producto.nombre}\n{"  " + x.cantidad + " x " + x.precioUnitario.ToString("N2"),-20}{x.subtotal,20:N2}\n")) +
                $"{L}\n{"TOTAL Bs",-20}{d.Sum(x => x.subtotal),20:N2}\n{L}\n\n{"¡GRACIAS POR SU COMPRA!",31}";

            Text = $"Detalle de la venta N° {idVenta}";
        }
    }
}

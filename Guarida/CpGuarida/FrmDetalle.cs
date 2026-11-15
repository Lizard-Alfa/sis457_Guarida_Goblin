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
        private const string L = "------------------------------------------------------------";

        public FrmDetalle(int idVenta)
        {
            InitializeComponent();
            this.idVenta = idVenta;
        }

        private void FrmDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                var detalles = DetalleCln.listarPorVenta(idVenta);
                if (detalles == null || !detalles.Any())
                {
                    MessageBox.Show("No se encontraron detalles para esta venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                var venta = detalles.First().Venta;
                rtxtexBo.Text =
                    $"\n{"GUARIDA GOBLIN",27}\n{"Sucre, Bolivia",27}\n\n{L}\n{"COMPROBANTE DE VENTA",29}\n{L}\n" +
                    $"{"Venta N°:",-20}{idVenta,26}\n" +
                    $"{"Transacción:",-20}{(venta.transaccion?.ToString() ?? "S/N"),26}\n" +
                    $"{"Fecha:",-20}{venta.fecha:dd/MM/yyyy HH:mm,26}\n" +
                    $"{"Cliente:",-20}{(venta.Cliente?.nombres + " " + venta.Cliente?.apellidos).Trim(),26}\n" +
                    $"{"Vendedor:",-20}{venta.usuarioRegistro,26}\n" +
                    $"{"Método de Pago:",-20}{venta.metodoPago,26}\n" +
                    $"{L}\n" +
                    $"{"Descripción",-20}{"Subtotal",26}\n" +
                    string.Concat(detalles.Select(d =>
                        $"{d.Producto.nombre}\n" +
                        $"{"  " + d.cantidad + " x " + d.precioUnitario.ToString("N2"),-20}{d.subtotal,26:N2}\n")) +
                    $"{L}\n" +
                    $"{"Subtotal:",-20}{venta.subtotal,26:N2}\n" +
                    $"{"Descuento:",-20}{venta.descuento,26:N2}\n" +
                    $"{"IVA (13%):",-20}{venta.iva,26:N2}\n" +
                    $"{L}\n" +
                    $"{"TOTAL Bs:",-20}{venta.total,26:N2}\n" +
                    $"{L}\n\n" +
                    $"{"¡GRACIAS POR SU COMPRA!",29}";

                Text = $"Detalle de la venta N° {idVenta} - Guardia Goblin";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar el comprobante: " + ex.Message,
                    "::: Error - Guardia Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}

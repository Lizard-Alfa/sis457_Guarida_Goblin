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
        public FrmDetalle(int idVenta)
        {
            InitializeComponent();
            this.idVenta = idVenta;
        }

        private void FrmDetalle_Load(object sender, EventArgs e)
        {
            listar();
        }
        private void listar()
        {
            try
            {
                var detalles = DetalleCln.listarPorVenta(idVenta);

                if (detalles == null || !detalles.Any())
                {
                    MessageBox.Show("No se encontraron detalles para esta venta.",
                        "::: Aviso - Guarida Goblin :::", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }
                rtxtexBo.Font = new Font("Consolas", 9.5f, FontStyle.Regular);
                rtxtexBo.ReadOnly = true;
                rtxtexBo.WordWrap = false;
                rtxtexBo.ScrollBars = RichTextBoxScrollBars.Vertical;

                float anchoChar = TextRenderer.MeasureText("WWWWWWWWWW", rtxtexBo.Font).Width / 10f;
                int totalCols = (int)((rtxtexBo.ClientSize.Width - 6) / anchoChar);
                int colDer = 18;
                int colIzq = totalCols - colDer;

                string L = new string('-', totalCols);
                Func<string, string> centrar = t => t.Length >= totalCols ? t : t.PadLeft((totalCols + t.Length) / 2);
                Func<string, string, string> fila = (izq, der) =>
                    (izq.Length > colIzq - 1 ? izq.Substring(0, colIzq - 1) : izq).PadRight(colIzq) + der.PadLeft(colDer);

                var venta = detalles.First().Venta;
                string clienteNombre = (venta.Cliente?.nombres ?? "S/N").Trim();
                string clienteCi = (venta.Cliente?.cedulaIdentidad ?? "S/N").Trim();
                string vendedor = (venta.usuarioRegistro ?? "S/N").Trim();
                string metodoPago = (venta.metodoPago ?? "S/N").Trim();

                rtxtexBo.Text =
                    $"\n{centrar("GUARIDA GOBLIN")}\n" +
                    $"{centrar("Sucre, Bolivia")}\n\n" +
                    $"{L}\n{centrar("COMPROBANTE DE VENTA")}\n{L}\n" +
                    $"{fila("Venta N°:", idVenta.ToString())}\n" +
                    $"{fila("Fecha:", venta.fecha.ToString("dd/MM/yyyy HH:mm"))}\n" +
                    $"{fila("CI/NIT:", clienteCi)}\n" +
                    $"{fila("Cliente:", clienteNombre)}\n" +
                    $"{fila("Vendedor:", vendedor)}\n" +
                    $"{fila("Método de Pago:", metodoPago)}\n" +
                    $"{L}\n" +
                    $"{fila("Descripción", "Subtotal")}\n" +
                    string.Concat(detalles.Select(d =>
                        $"{d.Producto.nombre}\n" +
                        $"{fila($"  {d.cantidad} x {d.precioUnitario:N2}", $"{d.subtotal:N2}")}\n")) +
                    $"{L}\n" +
                    $"{fila("Subtotal:", $"{venta.subtotal:N2}")}\n" +
                    $"{fila("Descuento:", $"{venta.descuento:N2}")}\n" +
                    
                    $"{L}\n" +
                    $"{fila("TOTAL Bs:", $"{venta.total:N2}")}\n" +
                    $"{L}\n\n" +
                    $"{centrar("¡GRACIAS POR SU COMPRA!")}\n\n";

                this.Text = $"Venta N° {idVenta} - Guarida Goblin";
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

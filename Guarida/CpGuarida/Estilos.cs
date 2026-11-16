using System.Drawing;
using System.Windows.Forms;

namespace CpGuarida
{
    public static class Estilos
    {
        // Paleta de colores
        public static Color Primario => Color.FromArgb(74, 44, 23);
        public static Color Secundario => Color.FromArgb(201, 169, 89);
        public static Color Fondo => Color.FromArgb(245, 230, 211);
        public static Color FondoSecundario => Color.FromArgb(250, 240, 230);
        public static Color BotonAccion => Color.FromArgb(46, 93, 58);
        public static Color BotonPeligro => Color.FromArgb(139, 46, 46);
        public static Color BotonNeutro => Color.FromArgb(107, 107, 107);
        public static Color Texto => Color.FromArgb(44, 44, 44);
        public static Color TextoSecundario => Color.FromArgb(90, 90, 90);

        // Formulario
        public static void AplicarFormulario(Form form)
        {
            form.BackColor = Fondo;
            form.Font = new Font("Segoe UI", 10);
        }
        // TextBox
        public static void AplicarTextBox(TextBox txt)
        {
            txt.BackColor = FondoSecundario;
            txt.ForeColor = Texto;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 10);
        }
        // ComboBox
        public static void AplicarComboBox(ComboBox cbo)
        {
            cbo.BackColor = FondoSecundario;
            cbo.ForeColor = Texto;
            cbo.Font = new Font("Segoe UI", 10);
        }
        // Botones
        public static void AplicarBotonAccion(Button btn)
        {
            if (btn.Image != null)
            {
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
                return;
            }

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = BotonAccion;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }
        public static void AplicarBotonPeligro(Button btn)
        {
            if (btn.Image != null)
            {
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
                return;
            }

            AplicarBotonAccion(btn);
            btn.BackColor = BotonPeligro;
        }

        public static void AplicarBotonNeutro(Button btn)
        {
            if (btn.Image != null)
            {
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
                return;
            }

            AplicarBotonAccion(btn);
            btn.BackColor = BotonNeutro;
        }
        // DataGridView
        public static void AplicarDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = FondoSecundario;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Primario;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Texto;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.GridColor = Secundario;
            dgv.RowTemplate.Height = 28;
        }
        // Labels
        public static void AplicarLabelTitulo(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lbl.ForeColor = Primario;
            lbl.BackColor = Color.Transparent;
        }

        public static void AplicarLabel(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 10);
            lbl.ForeColor = TextoSecundario;
            lbl.BackColor = Color.Transparent;
        }
    }
}
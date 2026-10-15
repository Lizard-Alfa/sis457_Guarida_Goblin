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
    public partial class FrmPrincipal : Form
    {
        FrmAutenticacion frmAutenticaion;
        public FrmPrincipal( FrmAutenticacion frmAutenticacion)
        {
            InitializeComponent();
            this.frmAutenticaion = frmAutenticacion;
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmAutenticaion.Show();
        }

        private void btnCaProductos_Click(object sender, EventArgs e)
        {
            new FrmProducto().ShowDialog();
        }
    }
}

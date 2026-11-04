namespace CpGuarida
{
    partial class FrmDetalle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtxtexBo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtxtexBo
            // 
            this.rtxtexBo.BackColor = System.Drawing.Color.White;
            this.rtxtexBo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtexBo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtexBo.Location = new System.Drawing.Point(12, 12);
            this.rtxtexBo.Name = "rtxtexBo";
            this.rtxtexBo.ReadOnly = true;
            this.rtxtexBo.Size = new System.Drawing.Size(320, 504);
            this.rtxtexBo.TabIndex = 0;
            this.rtxtexBo.Text = "";
            // 
            // FrmDetalle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 528);
            this.Controls.Add(this.rtxtexBo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmDetalle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmDetalle";
            this.Load += new System.EventHandler(this.FrmDetalle_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtexBo;
    }
}
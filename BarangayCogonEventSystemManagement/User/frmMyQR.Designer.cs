namespace BarangayCogonEventManagementSystem
{
    partial class frmMyQR
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvQRList;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvQRList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQRList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvQRList
            // 
            this.dgvQRList.AllowUserToAddRows = false;
            this.dgvQRList.AllowUserToDeleteRows = false;
            this.dgvQRList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.dgvQRList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQRList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQRList.Location = new System.Drawing.Point(30, 30);
            this.dgvQRList.Name = "dgvQRList";
            this.dgvQRList.ReadOnly = true;
            this.dgvQRList.RowHeadersVisible = false;
            this.dgvQRList.RowHeadersWidth = 51;
            this.dgvQRList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQRList.Size = new System.Drawing.Size(1060, 670);
            this.dgvQRList.TabIndex = 0;
            // 
            // frmMyQR
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(1150, 730);
            this.Controls.Add(this.dgvQRList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMyQR";
            this.Text = "My QR Codes";
            this.Load += new System.EventHandler(this.frmMyQR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQRList)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

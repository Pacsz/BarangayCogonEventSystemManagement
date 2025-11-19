namespace BarangayCogonEventManagementSystem
{
    partial class frmMyEvents
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvMyEvents;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvMyEvents = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMyEvents
            // 
            this.dgvMyEvents.AllowUserToAddRows = false;
            this.dgvMyEvents.AllowUserToDeleteRows = false;
            this.dgvMyEvents.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.dgvMyEvents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMyEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMyEvents.Location = new System.Drawing.Point(30, 30);
            this.dgvMyEvents.Name = "dgvMyEvents";
            this.dgvMyEvents.ReadOnly = true;
            this.dgvMyEvents.RowHeadersVisible = false;
            this.dgvMyEvents.RowHeadersWidth = 51;
            this.dgvMyEvents.Size = new System.Drawing.Size(1060, 670);
            this.dgvMyEvents.TabIndex = 0;
            // 
            // frmMyEvents
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(1150, 730);
            this.Controls.Add(this.dgvMyEvents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMyEvents";
            this.Text = "My Events";
            this.Load += new System.EventHandler(this.frmMyEvents_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyEvents)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

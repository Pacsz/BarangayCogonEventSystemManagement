namespace BarangayCogonEventManagementSystem
{
    partial class frmMyQR
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvQRList;
        private System.Windows.Forms.PictureBox picQR;
        private System.Windows.Forms.Label lblEvent;
        private System.Windows.Forms.Button btnSaveQR;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.topPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvQRList = new System.Windows.Forms.DataGridView();
            this.picQR = new System.Windows.Forms.PictureBox();
            this.lblEvent = new System.Windows.Forms.Label();
            this.btnSaveQR = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQRList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.topPanel.Controls.Add(this.lblTitle);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(700, 81);
            this.topPanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 49);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📱 My Approved QR Codes";
            // 
            // dgvQRList
            // 
            this.dgvQRList.AllowUserToAddRows = false;
            this.dgvQRList.AllowUserToDeleteRows = false;
            this.dgvQRList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvQRList.BackgroundColor = System.Drawing.Color.White;
            this.dgvQRList.ColumnHeadersHeight = 34;
            this.dgvQRList.Location = new System.Drawing.Point(19, 121);
            this.dgvQRList.Name = "dgvQRList";
            this.dgvQRList.ReadOnly = true;
            this.dgvQRList.RowHeadersVisible = false;
            this.dgvQRList.RowHeadersWidth = 62;
            this.dgvQRList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQRList.Size = new System.Drawing.Size(400, 362);
            this.dgvQRList.TabIndex = 1;
            this.dgvQRList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvQRList_CellClick);
            // 
            // picQR
            // 
            this.picQR.BackColor = System.Drawing.Color.WhiteSmoke;
            this.picQR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picQR.Location = new System.Drawing.Point(449, 141);
            this.picQR.Name = "picQR";
            this.picQR.Size = new System.Drawing.Size(200, 200);
            this.picQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picQR.TabIndex = 2;
            this.picQR.TabStop = false;

            // 
            // lblEvent
            // 
            this.lblEvent.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblEvent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblEvent.Location = new System.Drawing.Point(449, 361);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(230, 60);
            this.lblEvent.TabIndex = 2;
            this.lblEvent.Text = "Select an event to view QR";
            // 
            // btnSaveQR
            // 
            this.btnSaveQR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnSaveQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveQR.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSaveQR.ForeColor = System.Drawing.Color.White;
            this.btnSaveQR.Location = new System.Drawing.Point(454, 443);
            this.btnSaveQR.Name = "btnSaveQR";
            this.btnSaveQR.Size = new System.Drawing.Size(200, 40);
            this.btnSaveQR.TabIndex = 1;
            this.btnSaveQR.Text = "Save QR as Image";
            this.btnSaveQR.UseVisualStyleBackColor = false;
            this.btnSaveQR.Click += new System.EventHandler(this.btnSaveQR_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Gray;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(454, 493);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(200, 40);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmMyQR
            // 
            this.ClientSize = new System.Drawing.Size(1176, 672);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveQR);
            this.Controls.Add(this.lblEvent);
            this.Controls.Add(this.picQR);
            this.Controls.Add(this.dgvQRList);
            this.Controls.Add(this.topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMyQR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My QR Codes";
            this.Load += new System.EventHandler(this.frmMyQR_Load);
            this.topPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQRList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

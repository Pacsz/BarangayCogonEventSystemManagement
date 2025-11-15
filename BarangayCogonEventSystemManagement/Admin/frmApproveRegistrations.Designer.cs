using FontAwesome.Sharp;

namespace BarangayCogonEventManagementSystem
{
    partial class frmApproveRegistrations
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.DataGridView dgvRegistrations;
        private FontAwesome.Sharp.IconButton btnApprove;
        private FontAwesome.Sharp.IconButton btnReject;
        private FontAwesome.Sharp.IconButton btnViewQR;
        private System.Windows.Forms.PictureBox picQR;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.dgvRegistrations = new System.Windows.Forms.DataGridView();
            this.btnApprove = new FontAwesome.Sharp.IconButton();
            this.btnReject = new FontAwesome.Sharp.IconButton();
            this.btnViewQR = new FontAwesome.Sharp.IconButton();
            this.picQR = new System.Windows.Forms.PictureBox();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegistrations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1085, 60);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(1085, 60);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Approve Registrations";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvRegistrations
            // 
            this.dgvRegistrations.ColumnHeadersHeight = 34;
            this.dgvRegistrations.Location = new System.Drawing.Point(44, 97);
            this.dgvRegistrations.Name = "dgvRegistrations";
            this.dgvRegistrations.ReadOnly = true;
            this.dgvRegistrations.RowHeadersWidth = 62;
            this.dgvRegistrations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRegistrations.Size = new System.Drawing.Size(729, 497);
            this.dgvRegistrations.TabIndex = 1;
            // 
            // btnApprove
            // 
            this.btnApprove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(142)))), ((int)(((byte)(60)))));
            this.btnApprove.FlatAppearance.BorderSize = 0;
            this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprove.ForeColor = System.Drawing.Color.White;
            this.btnApprove.IconChar = FontAwesome.Sharp.IconChar.CircleCheck;
            this.btnApprove.IconColor = System.Drawing.Color.White;
            this.btnApprove.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnApprove.Location = new System.Drawing.Point(823, 113);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(166, 65);
            this.btnApprove.TabIndex = 2;
            this.btnApprove.Text = "Approve";
            this.btnApprove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnApprove.UseVisualStyleBackColor = false;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnReject
            // 
            this.btnReject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnReject.FlatAppearance.BorderSize = 0;
            this.btnReject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReject.ForeColor = System.Drawing.Color.White;
            this.btnReject.IconChar = FontAwesome.Sharp.IconChar.CircleXmark;
            this.btnReject.IconColor = System.Drawing.Color.White;
            this.btnReject.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnReject.Location = new System.Drawing.Point(823, 192);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(166, 60);
            this.btnReject.TabIndex = 3;
            this.btnReject.Text = "Reject";
            this.btnReject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReject.UseVisualStyleBackColor = false;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnViewQR
            // 
            this.btnViewQR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnViewQR.FlatAppearance.BorderSize = 0;
            this.btnViewQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewQR.ForeColor = System.Drawing.Color.White;
            this.btnViewQR.IconChar = FontAwesome.Sharp.IconChar.Qrcode;
            this.btnViewQR.IconColor = System.Drawing.Color.White;
            this.btnViewQR.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnViewQR.Location = new System.Drawing.Point(823, 268);
            this.btnViewQR.Name = "btnViewQR";
            this.btnViewQR.Size = new System.Drawing.Size(166, 68);
            this.btnViewQR.TabIndex = 4;
            this.btnViewQR.Text = "View QR";
            this.btnViewQR.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnViewQR.UseVisualStyleBackColor = false;
            this.btnViewQR.Click += new System.EventHandler(this.btnViewQR_Click);
            // 
            // picQR
            // 
            this.picQR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picQR.Location = new System.Drawing.Point(816, 400);
            this.picQR.Name = "picQR";
            this.picQR.Size = new System.Drawing.Size(173, 185);
            this.picQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picQR.TabIndex = 5;
            this.picQR.TabStop = false;
            // 
            // frmApproveRegistrations
            // 
            this.ClientSize = new System.Drawing.Size(1085, 691);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.dgvRegistrations);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnViewQR);
            this.Controls.Add(this.picQR);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmApproveRegistrations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Approve Registrations - BEMS";
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegistrations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

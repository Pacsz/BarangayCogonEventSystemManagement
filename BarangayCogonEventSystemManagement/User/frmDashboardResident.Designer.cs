namespace BarangayCogonEventManagementSystem
{
    partial class frmDashboardResident
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel sidebar;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnMyEvents;
        private System.Windows.Forms.Button btnMyQR;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnRegisterEvent;
        private System.Windows.Forms.DataGridView dgvEvents;
        private System.Windows.Forms.Label lblUpcomingEvents;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.sidebar = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnMyQR = new System.Windows.Forms.Button();
            this.btnMyEvents = new System.Windows.Forms.Button();
            this.btnRegisterEvent = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.lblUpcomingEvents = new System.Windows.Forms.Label();
            this.dgvEvents = new System.Windows.Forms.DataGridView();
            this.sidebar.SuspendLayout();
            this.topBar.SuspendLayout();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.sidebar.Controls.Add(this.btnLogout);
            this.sidebar.Controls.Add(this.btnMyQR);
            this.sidebar.Controls.Add(this.btnMyEvents);
            this.sidebar.Controls.Add(this.btnRegisterEvent);
            this.sidebar.Controls.Add(this.btnDashboard);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(220, 638);
            this.sidebar.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnLogout.ForeColor = System.Drawing.Color.Red;
            this.btnLogout.Location = new System.Drawing.Point(10, 520);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(200, 50);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnMyQR
            // 
            this.btnMyQR.FlatAppearance.BorderSize = 0;
            this.btnMyQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyQR.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnMyQR.ForeColor = System.Drawing.Color.White;
            this.btnMyQR.Location = new System.Drawing.Point(10, 376);
            this.btnMyQR.Name = "btnMyQR";
            this.btnMyQR.Size = new System.Drawing.Size(200, 50);
            this.btnMyQR.TabIndex = 3;
            this.btnMyQR.Text = "My QR Codes";
            this.btnMyQR.UseVisualStyleBackColor = true;
            this.btnMyQR.Click += new System.EventHandler(this.btnMyQR_Click);
            // 
            // btnMyEvents
            // 
            this.btnMyEvents.FlatAppearance.BorderSize = 0;
            this.btnMyEvents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyEvents.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnMyEvents.ForeColor = System.Drawing.Color.White;
            this.btnMyEvents.Location = new System.Drawing.Point(10, 291);
            this.btnMyEvents.Name = "btnMyEvents";
            this.btnMyEvents.Size = new System.Drawing.Size(200, 50);
            this.btnMyEvents.TabIndex = 2;
            this.btnMyEvents.Text = "My Events";
            this.btnMyEvents.UseVisualStyleBackColor = true;
            this.btnMyEvents.Click += new System.EventHandler(this.btnMyEvents_Click);
            // 
            // btnRegisterEvent
            // 
            this.btnRegisterEvent.FlatAppearance.BorderSize = 0;
            this.btnRegisterEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisterEvent.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRegisterEvent.ForeColor = System.Drawing.Color.White;
            this.btnRegisterEvent.Location = new System.Drawing.Point(10, 209);
            this.btnRegisterEvent.Name = "btnRegisterEvent";
            this.btnRegisterEvent.Size = new System.Drawing.Size(200, 50);
            this.btnRegisterEvent.TabIndex = 1;
            this.btnRegisterEvent.Text = "Register Event";
            this.btnRegisterEvent.UseVisualStyleBackColor = true;
            this.btnRegisterEvent.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(10, 132);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(200, 50);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = true;
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.topBar.Controls.Add(this.lblWelcome);
            this.topBar.Controls.Add(this.lblRole);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(220, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(825, 70);
            this.topBar.TabIndex = 1;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(20, 10);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(400, 40);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome, Resident!";
            // 
            // lblRole
            // 
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblRole.ForeColor = System.Drawing.Color.White;
            this.lblRole.Location = new System.Drawing.Point(593, 20);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(167, 30);
            this.lblRole.TabIndex = 1;
            this.lblRole.Text = "Role: Attendee";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.lblUpcomingEvents);
            this.mainPanel.Controls.Add(this.dgvEvents);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(220, 70);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(825, 568);
            this.mainPanel.TabIndex = 2;
            // 
            // lblUpcomingEvents
            // 
            this.lblUpcomingEvents.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblUpcomingEvents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblUpcomingEvents.Location = new System.Drawing.Point(20, 28);
            this.lblUpcomingEvents.Name = "lblUpcomingEvents";
            this.lblUpcomingEvents.Size = new System.Drawing.Size(317, 48);
            this.lblUpcomingEvents.TabIndex = 0;
            this.lblUpcomingEvents.Text = "?? Upcoming Events";
            // 
            // dgvEvents
            // 
            this.dgvEvents.AllowUserToAddRows = false;
            this.dgvEvents.AllowUserToDeleteRows = false;
            this.dgvEvents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEvents.BackgroundColor = System.Drawing.Color.White;
            this.dgvEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEvents.Location = new System.Drawing.Point(20, 76);
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.ReadOnly = true;
            this.dgvEvents.RowHeadersVisible = false;
            this.dgvEvents.RowHeadersWidth = 62;
            this.dgvEvents.RowTemplate.Height = 28;
            this.dgvEvents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEvents.Size = new System.Drawing.Size(740, 450);
            this.dgvEvents.TabIndex = 1;
            // 
            // frmDashboardResident
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 638);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.sidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmDashboardResident";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resident Dashboard - BEMS";
            this.Load += new System.EventHandler(this.frmDashboardResident_Load);
            this.sidebar.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

namespace BarangayCogonEventManagementSystem
{
    partial class frmDashboardAdmin
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel sidebar;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnManageEvents;
        private System.Windows.Forms.Button btnRegistrations;
        private System.Windows.Forms.Button btnQRScanner;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblEventsCount;
        private System.Windows.Forms.Label lblAttendeesCount;
        private System.Windows.Forms.Label lblVolunteersCount;
        private System.Windows.Forms.Label lblEvents;
        private System.Windows.Forms.Label lblAttendees;
        private System.Windows.Forms.Label lblVolunteers;
        private System.Windows.Forms.Label lblPresent;


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
            this.btnQRScanner = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnRegistrations = new System.Windows.Forms.Button();
            this.btnManageEvents = new System.Windows.Forms.Button();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.lblEventsCount = new System.Windows.Forms.Label();
            this.lblEvents = new System.Windows.Forms.Label();
            this.lblAttendeesCount = new System.Windows.Forms.Label();
            this.lblAttendees = new System.Windows.Forms.Label();
            this.lblVolunteersCount = new System.Windows.Forms.Label();
            this.lblVolunteers = new System.Windows.Forms.Label();
            this.lblPresent = new System.Windows.Forms.Label();
            this.lblPresentCount = new System.Windows.Forms.Label();
            this.sidebar.SuspendLayout();
            this.topBar.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.sidebar.Controls.Add(this.btnLogout);
            this.sidebar.Controls.Add(this.btnQRScanner);
            this.sidebar.Controls.Add(this.btnReports);
            this.sidebar.Controls.Add(this.btnRegistrations);
            this.sidebar.Controls.Add(this.btnManageEvents);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Margin = new System.Windows.Forms.Padding(4);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(257, 771);
            this.sidebar.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnLogout.ForeColor = System.Drawing.Color.Red;
            this.btnLogout.Location = new System.Drawing.Point(13, 574);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(231, 60);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnQRScanner
            // 
            this.btnQRScanner.FlatAppearance.BorderSize = 0;
            this.btnQRScanner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQRScanner.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnQRScanner.ForeColor = System.Drawing.Color.White;
            this.btnQRScanner.Location = new System.Drawing.Point(18, 360);
            this.btnQRScanner.Margin = new System.Windows.Forms.Padding(4);
            this.btnQRScanner.Name = "btnQRScanner";
            this.btnQRScanner.Size = new System.Drawing.Size(231, 60);
            this.btnQRScanner.TabIndex = 4;
            this.btnQRScanner.Text = "QR Scanner";
            this.btnQRScanner.UseVisualStyleBackColor = true;
            this.btnQRScanner.Click += new System.EventHandler(this.btnScanner_Click);
            // 
            // btnReports
            // 
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.Location = new System.Drawing.Point(13, 293);
            this.btnReports.Margin = new System.Windows.Forms.Padding(4);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(231, 60);
            this.btnReports.TabIndex = 2;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnRegistrations
            // 
            this.btnRegistrations.FlatAppearance.BorderSize = 0;
            this.btnRegistrations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrations.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRegistrations.ForeColor = System.Drawing.Color.White;
            this.btnRegistrations.Location = new System.Drawing.Point(13, 213);
            this.btnRegistrations.Margin = new System.Windows.Forms.Padding(4);
            this.btnRegistrations.Name = "btnRegistrations";
            this.btnRegistrations.Size = new System.Drawing.Size(231, 60);
            this.btnRegistrations.TabIndex = 1;
            this.btnRegistrations.Text = "Registrations";
            this.btnRegistrations.UseVisualStyleBackColor = true;
            this.btnRegistrations.Click += new System.EventHandler(this.btnRegistrations_Click);
            // 
            // btnManageEvents
            // 
            this.btnManageEvents.FlatAppearance.BorderSize = 0;
            this.btnManageEvents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageEvents.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnManageEvents.ForeColor = System.Drawing.Color.White;
            this.btnManageEvents.Location = new System.Drawing.Point(13, 133);
            this.btnManageEvents.Margin = new System.Windows.Forms.Padding(4);
            this.btnManageEvents.Name = "btnManageEvents";
            this.btnManageEvents.Size = new System.Drawing.Size(231, 60);
            this.btnManageEvents.TabIndex = 0;
            this.btnManageEvents.Text = "Manage Events";
            this.btnManageEvents.UseVisualStyleBackColor = true;
            this.btnManageEvents.Click += new System.EventHandler(this.btnManageEvents_Click);
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.topBar.Controls.Add(this.lblTitle);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(257, 0);
            this.topBar.Margin = new System.Windows.Forms.Padding(4);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(906, 80);
            this.topBar.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(906, 80);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Admin Dashboard";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.Controls.Add(this.lblEventsCount);
            this.mainPanel.Controls.Add(this.lblEvents);
            this.mainPanel.Controls.Add(this.lblAttendeesCount);
            this.mainPanel.Controls.Add(this.lblAttendees);
            this.mainPanel.Controls.Add(this.lblVolunteersCount);
            this.mainPanel.Controls.Add(this.lblVolunteers);
            this.mainPanel.Controls.Add(this.lblPresentCount);
            this.mainPanel.Controls.Add(this.lblPresent);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(257, 80);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(906, 691);
            this.mainPanel.TabIndex = 2;
            // 
            // lblEventsCount
            // 
            this.lblEventsCount.Font = new System.Drawing.Font("Segoe UI Semibold", 24F);
            this.lblEventsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblEventsCount.Location = new System.Drawing.Point(173, 120);
            this.lblEventsCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEventsCount.Name = "lblEventsCount";
            this.lblEventsCount.Size = new System.Drawing.Size(227, 123);
            this.lblEventsCount.TabIndex = 0;
            this.lblEventsCount.Text = "0";
            // 
            // lblEvents
            // 
            this.lblEvents.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblEvents.Location = new System.Drawing.Point(173, 80);
            this.lblEvents.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEvents.Name = "lblEvents";
            this.lblEvents.Size = new System.Drawing.Size(227, 93);
            this.lblEvents.TabIndex = 1;
            this.lblEvents.Text = "Total Events";
            // 
            // lblAttendeesCount
            // 
            this.lblAttendeesCount.Font = new System.Drawing.Font("Segoe UI Semibold", 24F);
            this.lblAttendeesCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblAttendeesCount.Location = new System.Drawing.Point(507, 120);
            this.lblAttendeesCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAttendeesCount.Name = "lblAttendeesCount";
            this.lblAttendeesCount.Size = new System.Drawing.Size(227, 123);
            this.lblAttendeesCount.TabIndex = 2;
            this.lblAttendeesCount.Text = "0";
            // 
            // lblAttendees
            // 
            this.lblAttendees.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblAttendees.Location = new System.Drawing.Point(507, 80);
            this.lblAttendees.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAttendees.Name = "lblAttendees";
            this.lblAttendees.Size = new System.Drawing.Size(227, 93);
            this.lblAttendees.TabIndex = 3;
            this.lblAttendees.Text = "Attendees";
            // 
            // lblVolunteersCount
            // 
            this.lblVolunteersCount.Font = new System.Drawing.Font("Segoe UI Semibold", 24F);
            this.lblVolunteersCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblVolunteersCount.Location = new System.Drawing.Point(173, 333);
            this.lblVolunteersCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVolunteersCount.Name = "lblVolunteersCount";
            this.lblVolunteersCount.Size = new System.Drawing.Size(227, 124);
            this.lblVolunteersCount.TabIndex = 4;
            this.lblVolunteersCount.Text = "0";
            // 
            // lblVolunteers
            // 
            this.lblVolunteers.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblVolunteers.Location = new System.Drawing.Point(173, 293);
            this.lblVolunteers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVolunteers.Name = "lblVolunteers";
            this.lblVolunteers.Size = new System.Drawing.Size(227, 93);
            this.lblVolunteers.TabIndex = 5;
            this.lblVolunteers.Text = "Volunteers";
            // 
            // lblPresent
            // 
            this.lblPresent.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblPresent.Location = new System.Drawing.Point(507, 293);
            this.lblPresent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPresent.Name = "lblPresent";
            this.lblPresent.Size = new System.Drawing.Size(227, 93);
            this.lblPresent.TabIndex = 7;
            this.lblPresent.Text = "Present";
            // 
            // lblPresentCount
            // 
            this.lblPresentCount.Font = new System.Drawing.Font("Segoe UI Semibold", 24F);
            this.lblPresentCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblPresentCount.Location = new System.Drawing.Point(507, 333);
            this.lblPresentCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPresentCount.Name = "lblPresentCount";
            this.lblPresentCount.Size = new System.Drawing.Size(227, 137);
            this.lblPresentCount.TabIndex = 6;
            this.lblPresentCount.Text = "0";
            // 
            // frmDashboardAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 771);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.sidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmDashboardAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard - BEMS";
            this.sidebar.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblPresentCount;
    }
}

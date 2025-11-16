using FontAwesome.Sharp;

namespace BarangayCogonEventManagementSystem
{
    partial class frmDashboardAdmin
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel sidebar;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblTitle;
        private IconButton btnManageEvents;
        private IconButton btnRegistrations;
        private IconButton btnQRScanner;
        private IconButton btnReports;
        private IconButton btnLogout;
        private IconButton btnDashboard;
        private System.Windows.Forms.Panel pnlNav;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblEventsCount;
        private System.Windows.Forms.Label lblAttendeesCount;
        private System.Windows.Forms.Label lblVolunteersCount;
        private System.Windows.Forms.Label lblEvents;
        private System.Windows.Forms.Label lblAttendees;
        private System.Windows.Forms.Label lblVolunteers;
        private System.Windows.Forms.Label lblPresent;
        private System.Windows.Forms.Label lblPresentCount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDashboardAdmin));
            this.sidebar = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pnlNav = new System.Windows.Forms.Panel();
            this.btnLogout = new FontAwesome.Sharp.IconButton();
            this.btnQRScanner = new FontAwesome.Sharp.IconButton();
            this.btnReports = new FontAwesome.Sharp.IconButton();
            this.btnRegistrations = new FontAwesome.Sharp.IconButton();
            this.btnManageEvents = new FontAwesome.Sharp.IconButton();
            this.btnDashboard = new FontAwesome.Sharp.IconButton();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.lblEventsCount = new System.Windows.Forms.Label();
            this.lblEvents = new System.Windows.Forms.Label();
            this.lblAttendeesCount = new System.Windows.Forms.Label();
            this.lblAttendees = new System.Windows.Forms.Label();
            this.lblVolunteersCount = new System.Windows.Forms.Label();
            this.lblVolunteers = new System.Windows.Forms.Label();
            this.lblPresentCount = new System.Windows.Forms.Label();
            this.lblPresent = new System.Windows.Forms.Label();
            this.sidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.topBar.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.sidebar.Controls.Add(this.pictureBox2);
            this.sidebar.Controls.Add(this.pnlNav);
            this.sidebar.Controls.Add(this.btnLogout);
            this.sidebar.Controls.Add(this.btnQRScanner);
            this.sidebar.Controls.Add(this.btnReports);
            this.sidebar.Controls.Add(this.btnRegistrations);
            this.sidebar.Controls.Add(this.btnManageEvents);
            this.sidebar.Controls.Add(this.btnDashboard);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(250, 800);
            this.sidebar.TabIndex = 0;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(34, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(173, 157);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // pnlNav
            // 
            this.pnlNav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.pnlNav.Location = new System.Drawing.Point(0, 195);
            this.pnlNav.Margin = new System.Windows.Forms.Padding(0);
            this.pnlNav.Name = "pnlNav";
            this.pnlNav.Size = new System.Drawing.Size(5, 48);
            this.pnlNav.TabIndex = 7;
            // 
            // btnLogout
            // 
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnLogout.ForeColor = System.Drawing.Color.Red;
            this.btnLogout.IconChar = FontAwesome.Sharp.IconChar.RightFromBracket;
            this.btnLogout.IconColor = System.Drawing.Color.Red;
            this.btnLogout.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnLogout.IconSize = 32;
            this.btnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Location = new System.Drawing.Point(12, 730);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnLogout.Size = new System.Drawing.Size(226, 48);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "  Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnQRScanner
            // 
            this.btnQRScanner.FlatAppearance.BorderSize = 0;
            this.btnQRScanner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQRScanner.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnQRScanner.ForeColor = System.Drawing.Color.White;
            this.btnQRScanner.IconChar = FontAwesome.Sharp.IconChar.Qrcode;
            this.btnQRScanner.IconColor = System.Drawing.Color.White;
            this.btnQRScanner.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnQRScanner.IconSize = 32;
            this.btnQRScanner.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQRScanner.Location = new System.Drawing.Point(12, 411);
            this.btnQRScanner.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnQRScanner.Name = "btnQRScanner";
            this.btnQRScanner.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnQRScanner.Size = new System.Drawing.Size(226, 48);
            this.btnQRScanner.TabIndex = 5;
            this.btnQRScanner.Text = "  QR Scanner";
            this.btnQRScanner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQRScanner.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnQRScanner.UseVisualStyleBackColor = true;
            this.btnQRScanner.Click += new System.EventHandler(this.btnScanner_Click);
            // 
            // btnReports
            // 
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.IconChar = FontAwesome.Sharp.IconChar.ChartLine;
            this.btnReports.IconColor = System.Drawing.Color.White;
            this.btnReports.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnReports.IconSize = 32;
            this.btnReports.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.Location = new System.Drawing.Point(12, 357);
            this.btnReports.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnReports.Name = "btnReports";
            this.btnReports.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnReports.Size = new System.Drawing.Size(226, 48);
            this.btnReports.TabIndex = 4;
            this.btnReports.Text = "  Reports";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnRegistrations
            // 
            this.btnRegistrations.FlatAppearance.BorderSize = 0;
            this.btnRegistrations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrations.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRegistrations.ForeColor = System.Drawing.Color.White;
            this.btnRegistrations.IconChar = FontAwesome.Sharp.IconChar.ClipboardList;
            this.btnRegistrations.IconColor = System.Drawing.Color.White;
            this.btnRegistrations.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnRegistrations.IconSize = 32;
            this.btnRegistrations.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRegistrations.Location = new System.Drawing.Point(12, 303);
            this.btnRegistrations.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRegistrations.Name = "btnRegistrations";
            this.btnRegistrations.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnRegistrations.Size = new System.Drawing.Size(226, 48);
            this.btnRegistrations.TabIndex = 3;
            this.btnRegistrations.Text = "  Registrations";
            this.btnRegistrations.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRegistrations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRegistrations.UseVisualStyleBackColor = true;
            this.btnRegistrations.Click += new System.EventHandler(this.btnRegistrations_Click);
            // 
            // btnManageEvents
            // 
            this.btnManageEvents.FlatAppearance.BorderSize = 0;
            this.btnManageEvents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageEvents.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnManageEvents.ForeColor = System.Drawing.Color.White;
            this.btnManageEvents.IconChar = FontAwesome.Sharp.IconChar.CalendarAlt;
            this.btnManageEvents.IconColor = System.Drawing.Color.White;
            this.btnManageEvents.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnManageEvents.IconSize = 32;
            this.btnManageEvents.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageEvents.Location = new System.Drawing.Point(12, 249);
            this.btnManageEvents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnManageEvents.Name = "btnManageEvents";
            this.btnManageEvents.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnManageEvents.Size = new System.Drawing.Size(226, 48);
            this.btnManageEvents.TabIndex = 2;
            this.btnManageEvents.Text = "  Manage Events";
            this.btnManageEvents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageEvents.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnManageEvents.UseVisualStyleBackColor = true;
            this.btnManageEvents.Click += new System.EventHandler(this.btnManageEvents_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.IconChar = FontAwesome.Sharp.IconChar.PieChart;
            this.btnDashboard.IconColor = System.Drawing.Color.White;
            this.btnDashboard.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnDashboard.IconSize = 32;
            this.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.Location = new System.Drawing.Point(12, 195);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(226, 48);
            this.btnDashboard.TabIndex = 1;
            this.btnDashboard.Text = "  Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.topBar.Controls.Add(this.lblTitle);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(250, 0);
            this.topBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1150, 70);
            this.topBar.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1150, 70);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Admin Dashboard";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.mainPanel.Controls.Add(this.lblEventsCount);
            this.mainPanel.Controls.Add(this.lblEvents);
            this.mainPanel.Controls.Add(this.lblAttendeesCount);
            this.mainPanel.Controls.Add(this.lblAttendees);
            this.mainPanel.Controls.Add(this.lblVolunteersCount);
            this.mainPanel.Controls.Add(this.lblVolunteers);
            this.mainPanel.Controls.Add(this.lblPresentCount);
            this.mainPanel.Controls.Add(this.lblPresent);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mainPanel.Location = new System.Drawing.Point(250, 70);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1150, 730);
            this.mainPanel.TabIndex = 2;
            // 
            // lblEventsCount
            // 
            this.lblEventsCount.Font = new System.Drawing.Font("Segoe UI Semibold", 28F, System.Drawing.FontStyle.Bold);
            this.lblEventsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblEventsCount.Location = new System.Drawing.Point(300, 180);
            this.lblEventsCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEventsCount.Name = "lblEventsCount";
            this.lblEventsCount.Size = new System.Drawing.Size(250, 120);
            this.lblEventsCount.TabIndex = 0;
            this.lblEventsCount.Text = "0";
            // 
            // lblEvents
            // 
            this.lblEvents.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblEvents.ForeColor = System.Drawing.Color.White;
            this.lblEvents.Location = new System.Drawing.Point(300, 140);
            this.lblEvents.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEvents.Name = "lblEvents";
            this.lblEvents.Size = new System.Drawing.Size(250, 80);
            this.lblEvents.TabIndex = 1;
            this.lblEvents.Text = "Total Events";
            // 
            // lblAttendeesCount
            // 
            this.lblAttendeesCount.Font = new System.Drawing.Font("Segoe UI Semibold", 28F, System.Drawing.FontStyle.Bold);
            this.lblAttendeesCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblAttendeesCount.Location = new System.Drawing.Point(650, 180);
            this.lblAttendeesCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAttendeesCount.Name = "lblAttendeesCount";
            this.lblAttendeesCount.Size = new System.Drawing.Size(250, 120);
            this.lblAttendeesCount.TabIndex = 2;
            this.lblAttendeesCount.Text = "0";
            // 
            // lblAttendees
            // 
            this.lblAttendees.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblAttendees.ForeColor = System.Drawing.Color.White;
            this.lblAttendees.Location = new System.Drawing.Point(650, 140);
            this.lblAttendees.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAttendees.Name = "lblAttendees";
            this.lblAttendees.Size = new System.Drawing.Size(250, 80);
            this.lblAttendees.TabIndex = 3;
            this.lblAttendees.Text = "Attendees";
            // 
            // lblVolunteersCount
            // 
            this.lblVolunteersCount.Font = new System.Drawing.Font("Segoe UI Semibold", 28F, System.Drawing.FontStyle.Bold);
            this.lblVolunteersCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblVolunteersCount.Location = new System.Drawing.Point(300, 400);
            this.lblVolunteersCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVolunteersCount.Name = "lblVolunteersCount";
            this.lblVolunteersCount.Size = new System.Drawing.Size(250, 120);
            this.lblVolunteersCount.TabIndex = 4;
            this.lblVolunteersCount.Text = "0";
            // 
            // lblVolunteers
            // 
            this.lblVolunteers.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblVolunteers.ForeColor = System.Drawing.Color.White;
            this.lblVolunteers.Location = new System.Drawing.Point(300, 360);
            this.lblVolunteers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVolunteers.Name = "lblVolunteers";
            this.lblVolunteers.Size = new System.Drawing.Size(250, 80);
            this.lblVolunteers.TabIndex = 5;
            this.lblVolunteers.Text = "Volunteers";
            // 
            // lblPresentCount
            // 
            this.lblPresentCount.Font = new System.Drawing.Font("Segoe UI Semibold", 28F, System.Drawing.FontStyle.Bold);
            this.lblPresentCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.lblPresentCount.Location = new System.Drawing.Point(650, 400);
            this.lblPresentCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPresentCount.Name = "lblPresentCount";
            this.lblPresentCount.Size = new System.Drawing.Size(250, 120);
            this.lblPresentCount.TabIndex = 6;
            this.lblPresentCount.Text = "0";
            // 
            // lblPresent
            // 
            this.lblPresent.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblPresent.ForeColor = System.Drawing.Color.White;
            this.lblPresent.Location = new System.Drawing.Point(650, 360);
            this.lblPresent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPresent.Name = "lblPresent";
            this.lblPresent.Size = new System.Drawing.Size(250, 80);
            this.lblPresent.TabIndex = 7;
            this.lblPresent.Text = "Present";
            // 
            // frmDashboardAdmin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.sidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "frmDashboardAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard - BEMS";
            this.Load += new System.EventHandler(this.frmDashboardAdmin_Load);
            this.sidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.topBar.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

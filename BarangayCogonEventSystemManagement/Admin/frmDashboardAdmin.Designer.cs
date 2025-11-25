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
        
        // Stat Cards
        private System.Windows.Forms.Panel pnlEventsCard;
        private System.Windows.Forms.Panel pnlAttendeesCard;
        private System.Windows.Forms.Panel pnlVolunteersCard;
        private System.Windows.Forms.Panel pnlPresentCard;
        
        private System.Windows.Forms.Label lblEventsCount;
        private System.Windows.Forms.Label lblEvents;
        private IconPictureBox iconEvents;
        
        private System.Windows.Forms.Label lblAttendeesCount;
        private System.Windows.Forms.Label lblAttendees;
        private IconPictureBox iconAttendees;
        
        private System.Windows.Forms.Label lblVolunteersCount;
        private System.Windows.Forms.Label lblVolunteers;
        private IconPictureBox iconVolunteers;
        
        private System.Windows.Forms.Label lblPresentCount;
        private System.Windows.Forms.Label lblPresent;
        private IconPictureBox iconPresent;
        
        // Recent Registrations
        private System.Windows.Forms.Panel pnlRecentRegistrations;
        private System.Windows.Forms.Label lblRecentRegistrations;
        private System.Windows.Forms.DataGridView dgvRecentRegistrations;
        private System.Windows.Forms.Button btnViewAllRegistrations;

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
            this.pnlRecentRegistrations = new System.Windows.Forms.Panel();
            this.btnViewAllRegistrations = new System.Windows.Forms.Button();
            this.dgvRecentRegistrations = new System.Windows.Forms.DataGridView();
            this.lblRecentRegistrations = new System.Windows.Forms.Label();
            this.pnlPresentCard = new System.Windows.Forms.Panel();
            this.iconPresent = new FontAwesome.Sharp.IconPictureBox();
            this.lblPresentCount = new System.Windows.Forms.Label();
            this.lblPresent = new System.Windows.Forms.Label();
            this.pnlVolunteersCard = new System.Windows.Forms.Panel();
            this.iconVolunteers = new FontAwesome.Sharp.IconPictureBox();
            this.lblVolunteersCount = new System.Windows.Forms.Label();
            this.lblVolunteers = new System.Windows.Forms.Label();
            this.pnlAttendeesCard = new System.Windows.Forms.Panel();
            this.iconAttendees = new FontAwesome.Sharp.IconPictureBox();
            this.lblAttendeesCount = new System.Windows.Forms.Label();
            this.lblAttendees = new System.Windows.Forms.Label();
            this.pnlEventsCard = new System.Windows.Forms.Panel();
            this.iconEvents = new FontAwesome.Sharp.IconPictureBox();
            this.lblEventsCount = new System.Windows.Forms.Label();
            this.lblEvents = new System.Windows.Forms.Label();
            this.sidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.topBar.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.pnlRecentRegistrations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentRegistrations)).BeginInit();
            this.pnlPresentCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPresent)).BeginInit();
            this.pnlVolunteersCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconVolunteers)).BeginInit();
            this.pnlAttendeesCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconAttendees)).BeginInit();
            this.pnlEventsCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconEvents)).BeginInit();
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
            this.mainPanel.Controls.Add(this.pnlRecentRegistrations);
            this.mainPanel.Controls.Add(this.pnlPresentCard);
            this.mainPanel.Controls.Add(this.pnlVolunteersCard);
            this.mainPanel.Controls.Add(this.pnlAttendeesCard);
            this.mainPanel.Controls.Add(this.pnlEventsCard);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mainPanel.Location = new System.Drawing.Point(250, 70);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1150, 730);
            this.mainPanel.TabIndex = 2;
            // 
            // pnlRecentRegistrations
            // 
            this.pnlRecentRegistrations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlRecentRegistrations.Controls.Add(this.btnViewAllRegistrations);
            this.pnlRecentRegistrations.Controls.Add(this.dgvRecentRegistrations);
            this.pnlRecentRegistrations.Controls.Add(this.lblRecentRegistrations);
            this.pnlRecentRegistrations.Location = new System.Drawing.Point(30, 180);
            this.pnlRecentRegistrations.Name = "pnlRecentRegistrations";
            this.pnlRecentRegistrations.Size = new System.Drawing.Size(1100, 520);
            this.pnlRecentRegistrations.TabIndex = 4;
            // 
            // btnViewAllRegistrations
            // 
            this.btnViewAllRegistrations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnViewAllRegistrations.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewAllRegistrations.FlatAppearance.BorderSize = 0;
            this.btnViewAllRegistrations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewAllRegistrations.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewAllRegistrations.ForeColor = System.Drawing.Color.White;
            this.btnViewAllRegistrations.Location = new System.Drawing.Point(900, 460);
            this.btnViewAllRegistrations.Name = "btnViewAllRegistrations";
            this.btnViewAllRegistrations.Size = new System.Drawing.Size(180, 45);
            this.btnViewAllRegistrations.TabIndex = 2;
            this.btnViewAllRegistrations.Text = "View All →";
            this.btnViewAllRegistrations.UseVisualStyleBackColor = false;
            this.btnViewAllRegistrations.Click += new System.EventHandler(this.btnViewAllRegistrations_Click);
            // 
            // dgvRecentRegistrations
            // 
            this.dgvRecentRegistrations.AllowUserToAddRows = false;
            this.dgvRecentRegistrations.AllowUserToDeleteRows = false;
            this.dgvRecentRegistrations.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.dgvRecentRegistrations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecentRegistrations.Location = new System.Drawing.Point(20, 55);
            this.dgvRecentRegistrations.Name = "dgvRecentRegistrations";
            this.dgvRecentRegistrations.ReadOnly = true;
            this.dgvRecentRegistrations.RowHeadersVisible = false;
            this.dgvRecentRegistrations.RowHeadersWidth = 51;
            this.dgvRecentRegistrations.Size = new System.Drawing.Size(1060, 390);
            this.dgvRecentRegistrations.TabIndex = 1;
            // 
            // lblRecentRegistrations
            // 
            this.lblRecentRegistrations.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblRecentRegistrations.ForeColor = System.Drawing.Color.White;
            this.lblRecentRegistrations.Location = new System.Drawing.Point(20, 15);
            this.lblRecentRegistrations.Name = "lblRecentRegistrations";
            this.lblRecentRegistrations.Size = new System.Drawing.Size(500, 30);
            this.lblRecentRegistrations.TabIndex = 0;
            this.lblRecentRegistrations.Text = "📋 Recent Registrations (Last 2 Days)";
            this.lblRecentRegistrations.Click += new System.EventHandler(this.lblRecentRegistrations_Click);
            // 
            // pnlPresentCard
            // 
            this.pnlPresentCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlPresentCard.Controls.Add(this.iconPresent);
            this.pnlPresentCard.Controls.Add(this.lblPresentCount);
            this.pnlPresentCard.Controls.Add(this.lblPresent);
            this.pnlPresentCard.Location = new System.Drawing.Point(870, 30);
            this.pnlPresentCard.Name = "pnlPresentCard";
            this.pnlPresentCard.Size = new System.Drawing.Size(260, 120);
            this.pnlPresentCard.TabIndex = 3;
            // 
            // iconPresent
            // 
            this.iconPresent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.iconPresent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.iconPresent.IconChar = FontAwesome.Sharp.IconChar.UserClock;
            this.iconPresent.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.iconPresent.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPresent.IconSize = 48;
            this.iconPresent.Location = new System.Drawing.Point(20, 35);
            this.iconPresent.Name = "iconPresent";
            this.iconPresent.Size = new System.Drawing.Size(48, 48);
            this.iconPresent.TabIndex = 0;
            this.iconPresent.TabStop = false;
            // 
            // lblPresentCount
            // 
            this.lblPresentCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblPresentCount.ForeColor = System.Drawing.Color.White;
            this.lblPresentCount.Location = new System.Drawing.Point(75, 25);
            this.lblPresentCount.Name = "lblPresentCount";
            this.lblPresentCount.Size = new System.Drawing.Size(170, 55);
            this.lblPresentCount.TabIndex = 1;
            this.lblPresentCount.Text = "0";
            this.lblPresentCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPresent
            // 
            this.lblPresent.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPresent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(161)))), ((int)(((byte)(178)))));
            this.lblPresent.Location = new System.Drawing.Point(75, 75);
            this.lblPresent.Name = "lblPresent";
            this.lblPresent.Size = new System.Drawing.Size(170, 25);
            this.lblPresent.TabIndex = 2;
            this.lblPresent.Text = "Active Attendance";
            // 
            // pnlVolunteersCard
            // 
            this.pnlVolunteersCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlVolunteersCard.Controls.Add(this.iconVolunteers);
            this.pnlVolunteersCard.Controls.Add(this.lblVolunteersCount);
            this.pnlVolunteersCard.Controls.Add(this.lblVolunteers);
            this.pnlVolunteersCard.Location = new System.Drawing.Point(590, 30);
            this.pnlVolunteersCard.Name = "pnlVolunteersCard";
            this.pnlVolunteersCard.Size = new System.Drawing.Size(260, 120);
            this.pnlVolunteersCard.TabIndex = 2;
            // 
            // iconVolunteers
            // 
            this.iconVolunteers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.iconVolunteers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.iconVolunteers.IconChar = FontAwesome.Sharp.IconChar.ClockFour;
            this.iconVolunteers.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.iconVolunteers.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconVolunteers.IconSize = 48;
            this.iconVolunteers.Location = new System.Drawing.Point(20, 35);
            this.iconVolunteers.Name = "iconVolunteers";
            this.iconVolunteers.Size = new System.Drawing.Size(48, 48);
            this.iconVolunteers.TabIndex = 0;
            this.iconVolunteers.TabStop = false;
            // 
            // lblVolunteersCount
            // 
            this.lblVolunteersCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblVolunteersCount.ForeColor = System.Drawing.Color.White;
            this.lblVolunteersCount.Location = new System.Drawing.Point(75, 25);
            this.lblVolunteersCount.Name = "lblVolunteersCount";
            this.lblVolunteersCount.Size = new System.Drawing.Size(170, 55);
            this.lblVolunteersCount.TabIndex = 1;
            this.lblVolunteersCount.Text = "0";
            this.lblVolunteersCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVolunteers
            // 
            this.lblVolunteers.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblVolunteers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(161)))), ((int)(((byte)(178)))));
            this.lblVolunteers.Location = new System.Drawing.Point(75, 75);
            this.lblVolunteers.Name = "lblVolunteers";
            this.lblVolunteers.Size = new System.Drawing.Size(170, 25);
            this.lblVolunteers.TabIndex = 2;
            this.lblVolunteers.Text = "Pending Approvals";
            // 
            // pnlAttendeesCard
            // 
            this.pnlAttendeesCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlAttendeesCard.Controls.Add(this.iconAttendees);
            this.pnlAttendeesCard.Controls.Add(this.lblAttendeesCount);
            this.pnlAttendeesCard.Controls.Add(this.lblAttendees);
            this.pnlAttendeesCard.Location = new System.Drawing.Point(310, 30);
            this.pnlAttendeesCard.Name = "pnlAttendeesCard";
            this.pnlAttendeesCard.Size = new System.Drawing.Size(260, 120);
            this.pnlAttendeesCard.TabIndex = 1;
            // 
            // iconAttendees
            // 
            this.iconAttendees.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.iconAttendees.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.iconAttendees.IconChar = FontAwesome.Sharp.IconChar.ClipboardList;
            this.iconAttendees.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.iconAttendees.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconAttendees.IconSize = 48;
            this.iconAttendees.Location = new System.Drawing.Point(20, 35);
            this.iconAttendees.Name = "iconAttendees";
            this.iconAttendees.Size = new System.Drawing.Size(48, 48);
            this.iconAttendees.TabIndex = 0;
            this.iconAttendees.TabStop = false;
            // 
            // lblAttendeesCount
            // 
            this.lblAttendeesCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblAttendeesCount.ForeColor = System.Drawing.Color.White;
            this.lblAttendeesCount.Location = new System.Drawing.Point(75, 25);
            this.lblAttendeesCount.Name = "lblAttendeesCount";
            this.lblAttendeesCount.Size = new System.Drawing.Size(170, 55);
            this.lblAttendeesCount.TabIndex = 1;
            this.lblAttendeesCount.Text = "0";
            this.lblAttendeesCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAttendees
            // 
            this.lblAttendees.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblAttendees.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(161)))), ((int)(((byte)(178)))));
            this.lblAttendees.Location = new System.Drawing.Point(75, 75);
            this.lblAttendees.Name = "lblAttendees";
            this.lblAttendees.Size = new System.Drawing.Size(170, 25);
            this.lblAttendees.TabIndex = 2;
            this.lblAttendees.Text = "Total Registrations";
            // 
            // pnlEventsCard
            // 
            this.pnlEventsCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlEventsCard.Controls.Add(this.iconEvents);
            this.pnlEventsCard.Controls.Add(this.lblEventsCount);
            this.pnlEventsCard.Controls.Add(this.lblEvents);
            this.pnlEventsCard.Location = new System.Drawing.Point(30, 30);
            this.pnlEventsCard.Name = "pnlEventsCard";
            this.pnlEventsCard.Size = new System.Drawing.Size(260, 120);
            this.pnlEventsCard.TabIndex = 0;
            // 
            // iconEvents
            // 
            this.iconEvents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.iconEvents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.iconEvents.IconChar = FontAwesome.Sharp.IconChar.CalendarAlt;
            this.iconEvents.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.iconEvents.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconEvents.IconSize = 48;
            this.iconEvents.Location = new System.Drawing.Point(20, 35);
            this.iconEvents.Name = "iconEvents";
            this.iconEvents.Size = new System.Drawing.Size(48, 48);
            this.iconEvents.TabIndex = 0;
            this.iconEvents.TabStop = false;
            // 
            // lblEventsCount
            // 
            this.lblEventsCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblEventsCount.ForeColor = System.Drawing.Color.White;
            this.lblEventsCount.Location = new System.Drawing.Point(75, 25);
            this.lblEventsCount.Name = "lblEventsCount";
            this.lblEventsCount.Size = new System.Drawing.Size(170, 55);
            this.lblEventsCount.TabIndex = 1;
            this.lblEventsCount.Text = "0";
            this.lblEventsCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEvents
            // 
            this.lblEvents.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblEvents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(161)))), ((int)(((byte)(178)))));
            this.lblEvents.Location = new System.Drawing.Point(75, 75);
            this.lblEvents.Name = "lblEvents";
            this.lblEvents.Size = new System.Drawing.Size(170, 25);
            this.lblEvents.TabIndex = 2;
            this.lblEvents.Text = "Total Events";
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
            this.pnlRecentRegistrations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentRegistrations)).EndInit();
            this.pnlPresentCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPresent)).EndInit();
            this.pnlVolunteersCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconVolunteers)).EndInit();
            this.pnlAttendeesCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconAttendees)).EndInit();
            this.pnlEventsCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconEvents)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

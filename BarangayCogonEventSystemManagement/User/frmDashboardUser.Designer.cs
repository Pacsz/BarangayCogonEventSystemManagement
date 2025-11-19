using FontAwesome.Sharp;

namespace BarangayCogonEventManagementSystem
{
    partial class frmDashboardUser
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel sidebar;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblTitle;
        private IconButton btnDashboard;
        private IconButton btnBrowseEvents;
        private IconButton btnMyEvents;
        private IconButton btnMyQR;
        private IconButton btnLogout;
        private System.Windows.Forms.Panel pnlNav;
        private System.Windows.Forms.Panel mainPanel;
        
        // Stat Cards
        private System.Windows.Forms.Panel pnlMyEventsCard;
        private System.Windows.Forms.Panel pnlPendingCard;
        private System.Windows.Forms.Panel pnlApprovedCard;
        
        private System.Windows.Forms.Label lblMyEventsCount;
        private System.Windows.Forms.Label lblMyEventsLabel;
        private IconPictureBox iconMyEvents;
        
        private System.Windows.Forms.Label lblPendingCount;
        private System.Windows.Forms.Label lblPendingLabel;
        private IconPictureBox iconPending;
        
        private System.Windows.Forms.Label lblApprovedCount;
        private System.Windows.Forms.Label lblApprovedLabel;
        private IconPictureBox iconApproved;
        
        // Upcoming Events
        private System.Windows.Forms.Panel pnlUpcomingEvents;
        private System.Windows.Forms.Label lblUpcomingEvents;
        private System.Windows.Forms.DataGridView dgvUpcomingEvents;
        private System.Windows.Forms.Button btnViewAllEvents;
        private System.Windows.Forms.PictureBox pictureBox1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.sidebar = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlNav = new System.Windows.Forms.Panel();
            this.btnLogout = new FontAwesome.Sharp.IconButton();
            this.btnMyQR = new FontAwesome.Sharp.IconButton();
            this.btnMyEvents = new FontAwesome.Sharp.IconButton();
            this.btnBrowseEvents = new FontAwesome.Sharp.IconButton();
            this.btnDashboard = new FontAwesome.Sharp.IconButton();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.pnlUpcomingEvents = new System.Windows.Forms.Panel();
            this.btnViewAllEvents = new System.Windows.Forms.Button();
            this.dgvUpcomingEvents = new System.Windows.Forms.DataGridView();
            this.lblUpcomingEvents = new System.Windows.Forms.Label();
            this.pnlApprovedCard = new System.Windows.Forms.Panel();
            this.iconApproved = new FontAwesome.Sharp.IconPictureBox();
            this.lblApprovedCount = new System.Windows.Forms.Label();
            this.lblApprovedLabel = new System.Windows.Forms.Label();
            this.pnlPendingCard = new System.Windows.Forms.Panel();
            this.iconPending = new FontAwesome.Sharp.IconPictureBox();
            this.lblPendingCount = new System.Windows.Forms.Label();
            this.lblPendingLabel = new System.Windows.Forms.Label();
            this.pnlMyEventsCard = new System.Windows.Forms.Panel();
            this.iconMyEvents = new FontAwesome.Sharp.IconPictureBox();
            this.lblMyEventsCount = new System.Windows.Forms.Label();
            this.lblMyEventsLabel = new System.Windows.Forms.Label();
            this.sidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.topBar.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.pnlUpcomingEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpcomingEvents)).BeginInit();
            this.pnlApprovedCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconApproved)).BeginInit();
            this.pnlPendingCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPending)).BeginInit();
            this.pnlMyEventsCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconMyEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.sidebar.Controls.Add(this.pictureBox1);
            this.sidebar.Controls.Add(this.pnlNav);
            this.sidebar.Controls.Add(this.btnLogout);
            this.sidebar.Controls.Add(this.btnMyQR);
            this.sidebar.Controls.Add(this.btnMyEvents);
            this.sidebar.Controls.Add(this.btnBrowseEvents);
            this.sidebar.Controls.Add(this.btnDashboard);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(250, 800);
            this.sidebar.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::BarangayCogonEventSystemManagement.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(34, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(173, 157);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
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
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "  Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnMyQR
            // 
            this.btnMyQR.FlatAppearance.BorderSize = 0;
            this.btnMyQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyQR.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnMyQR.ForeColor = System.Drawing.Color.White;
            this.btnMyQR.IconChar = FontAwesome.Sharp.IconChar.Qrcode;
            this.btnMyQR.IconColor = System.Drawing.Color.White;
            this.btnMyQR.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMyQR.IconSize = 32;
            this.btnMyQR.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMyQR.Location = new System.Drawing.Point(12, 357);
            this.btnMyQR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMyQR.Name = "btnMyQR";
            this.btnMyQR.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnMyQR.Size = new System.Drawing.Size(226, 48);
            this.btnMyQR.TabIndex = 4;
            this.btnMyQR.Text = "  My QR Codes";
            this.btnMyQR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMyQR.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMyQR.UseVisualStyleBackColor = true;
            this.btnMyQR.Click += new System.EventHandler(this.btnMyQR_Click);
            // 
            // btnMyEvents
            // 
            this.btnMyEvents.FlatAppearance.BorderSize = 0;
            this.btnMyEvents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyEvents.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnMyEvents.ForeColor = System.Drawing.Color.White;
            this.btnMyEvents.IconChar = FontAwesome.Sharp.IconChar.ClipboardList;
            this.btnMyEvents.IconColor = System.Drawing.Color.White;
            this.btnMyEvents.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMyEvents.IconSize = 32;
            this.btnMyEvents.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMyEvents.Location = new System.Drawing.Point(12, 303);
            this.btnMyEvents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMyEvents.Name = "btnMyEvents";
            this.btnMyEvents.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnMyEvents.Size = new System.Drawing.Size(226, 48);
            this.btnMyEvents.TabIndex = 3;
            this.btnMyEvents.Text = "  My Events";
            this.btnMyEvents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMyEvents.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMyEvents.UseVisualStyleBackColor = true;
            this.btnMyEvents.Click += new System.EventHandler(this.btnMyEvents_Click);
            // 
            // btnBrowseEvents
            // 
            this.btnBrowseEvents.FlatAppearance.BorderSize = 0;
            this.btnBrowseEvents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseEvents.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnBrowseEvents.ForeColor = System.Drawing.Color.White;
            this.btnBrowseEvents.IconChar = FontAwesome.Sharp.IconChar.CalendarAlt;
            this.btnBrowseEvents.IconColor = System.Drawing.Color.White;
            this.btnBrowseEvents.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBrowseEvents.IconSize = 32;
            this.btnBrowseEvents.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowseEvents.Location = new System.Drawing.Point(12, 249);
            this.btnBrowseEvents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBrowseEvents.Name = "btnBrowseEvents";
            this.btnBrowseEvents.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnBrowseEvents.Size = new System.Drawing.Size(226, 48);
            this.btnBrowseEvents.TabIndex = 2;
            this.btnBrowseEvents.Text = "  Browse Events";
            this.btnBrowseEvents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowseEvents.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowseEvents.UseVisualStyleBackColor = true;
            this.btnBrowseEvents.Click += new System.EventHandler(this.btnBrowseEvents_Click);
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
            this.lblTitle.Text = "Resident Dashboard";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.mainPanel.Controls.Add(this.pnlUpcomingEvents);
            this.mainPanel.Controls.Add(this.pnlApprovedCard);
            this.mainPanel.Controls.Add(this.pnlPendingCard);
            this.mainPanel.Controls.Add(this.pnlMyEventsCard);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mainPanel.Location = new System.Drawing.Point(250, 70);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1150, 730);
            this.mainPanel.TabIndex = 2;
            // 
            // pnlUpcomingEvents
            // 
            this.pnlUpcomingEvents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlUpcomingEvents.Controls.Add(this.btnViewAllEvents);
            this.pnlUpcomingEvents.Controls.Add(this.dgvUpcomingEvents);
            this.pnlUpcomingEvents.Controls.Add(this.lblUpcomingEvents);
            this.pnlUpcomingEvents.Location = new System.Drawing.Point(30, 180);
            this.pnlUpcomingEvents.Name = "pnlUpcomingEvents";
            this.pnlUpcomingEvents.Size = new System.Drawing.Size(1100, 520);
            this.pnlUpcomingEvents.TabIndex = 3;
            // 
            // btnViewAllEvents
            // 
            this.btnViewAllEvents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnViewAllEvents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewAllEvents.FlatAppearance.BorderSize = 0;
            this.btnViewAllEvents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewAllEvents.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewAllEvents.ForeColor = System.Drawing.Color.White;
            this.btnViewAllEvents.Location = new System.Drawing.Point(900, 460);
            this.btnViewAllEvents.Name = "btnViewAllEvents";
            this.btnViewAllEvents.Size = new System.Drawing.Size(180, 45);
            this.btnViewAllEvents.TabIndex = 2;
            this.btnViewAllEvents.Text = "Browse All →";
            this.btnViewAllEvents.UseVisualStyleBackColor = false;
            this.btnViewAllEvents.Click += new System.EventHandler(this.btnViewAllEvents_Click);
            // 
            // dgvUpcomingEvents
            // 
            this.dgvUpcomingEvents.AllowUserToAddRows = false;
            this.dgvUpcomingEvents.AllowUserToDeleteRows = false;
            this.dgvUpcomingEvents.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.dgvUpcomingEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUpcomingEvents.Location = new System.Drawing.Point(20, 55);
            this.dgvUpcomingEvents.Name = "dgvUpcomingEvents";
            this.dgvUpcomingEvents.ReadOnly = true;
            this.dgvUpcomingEvents.RowHeadersVisible = false;
            this.dgvUpcomingEvents.RowHeadersWidth = 51;
            this.dgvUpcomingEvents.Size = new System.Drawing.Size(1060, 390);
            this.dgvUpcomingEvents.TabIndex = 1;
            // 
            // lblUpcomingEvents
            // 
            this.lblUpcomingEvents.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblUpcomingEvents.ForeColor = System.Drawing.Color.White;
            this.lblUpcomingEvents.Location = new System.Drawing.Point(20, 15);
            this.lblUpcomingEvents.Name = "lblUpcomingEvents";
            this.lblUpcomingEvents.Size = new System.Drawing.Size(500, 30);
            this.lblUpcomingEvents.TabIndex = 0;
            this.lblUpcomingEvents.Text = "📅 Upcoming Events";
            // 
            // pnlApprovedCard
            // 
            this.pnlApprovedCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlApprovedCard.Controls.Add(this.iconApproved);
            this.pnlApprovedCard.Controls.Add(this.lblApprovedCount);
            this.pnlApprovedCard.Controls.Add(this.lblApprovedLabel);
            this.pnlApprovedCard.Location = new System.Drawing.Point(750, 30);
            this.pnlApprovedCard.Name = "pnlApprovedCard";
            this.pnlApprovedCard.Size = new System.Drawing.Size(340, 120);
            this.pnlApprovedCard.TabIndex = 2;
            // 
            // iconApproved
            // 
            this.iconApproved.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.iconApproved.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.iconApproved.IconChar = FontAwesome.Sharp.IconChar.CircleCheck;
            this.iconApproved.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.iconApproved.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconApproved.IconSize = 48;
            this.iconApproved.Location = new System.Drawing.Point(20, 35);
            this.iconApproved.Name = "iconApproved";
            this.iconApproved.Size = new System.Drawing.Size(48, 48);
            this.iconApproved.TabIndex = 0;
            this.iconApproved.TabStop = false;
            // 
            // lblApprovedCount
            // 
            this.lblApprovedCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblApprovedCount.ForeColor = System.Drawing.Color.White;
            this.lblApprovedCount.Location = new System.Drawing.Point(75, 25);
            this.lblApprovedCount.Name = "lblApprovedCount";
            this.lblApprovedCount.Size = new System.Drawing.Size(250, 55);
            this.lblApprovedCount.TabIndex = 1;
            this.lblApprovedCount.Text = "0";
            this.lblApprovedCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblApprovedLabel
            // 
            this.lblApprovedLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblApprovedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(161)))), ((int)(((byte)(178)))));
            this.lblApprovedLabel.Location = new System.Drawing.Point(75, 75);
            this.lblApprovedLabel.Name = "lblApprovedLabel";
            this.lblApprovedLabel.Size = new System.Drawing.Size(250, 25);
            this.lblApprovedLabel.TabIndex = 2;
            this.lblApprovedLabel.Text = "Approved Registrations";
            // 
            // pnlPendingCard
            // 
            this.pnlPendingCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlPendingCard.Controls.Add(this.iconPending);
            this.pnlPendingCard.Controls.Add(this.lblPendingCount);
            this.pnlPendingCard.Controls.Add(this.lblPendingLabel);
            this.pnlPendingCard.Location = new System.Drawing.Point(390, 30);
            this.pnlPendingCard.Name = "pnlPendingCard";
            this.pnlPendingCard.Size = new System.Drawing.Size(340, 120);
            this.pnlPendingCard.TabIndex = 1;
            // 
            // iconPending
            // 
            this.iconPending.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.iconPending.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.iconPending.IconChar = FontAwesome.Sharp.IconChar.ClockFour;
            this.iconPending.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.iconPending.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPending.IconSize = 48;
            this.iconPending.Location = new System.Drawing.Point(20, 35);
            this.iconPending.Name = "iconPending";
            this.iconPending.Size = new System.Drawing.Size(48, 48);
            this.iconPending.TabIndex = 0;
            this.iconPending.TabStop = false;
            // 
            // lblPendingCount
            // 
            this.lblPendingCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblPendingCount.ForeColor = System.Drawing.Color.White;
            this.lblPendingCount.Location = new System.Drawing.Point(75, 25);
            this.lblPendingCount.Name = "lblPendingCount";
            this.lblPendingCount.Size = new System.Drawing.Size(250, 55);
            this.lblPendingCount.TabIndex = 1;
            this.lblPendingCount.Text = "0";
            this.lblPendingCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPendingLabel
            // 
            this.lblPendingLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPendingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(161)))), ((int)(((byte)(178)))));
            this.lblPendingLabel.Location = new System.Drawing.Point(75, 75);
            this.lblPendingLabel.Name = "lblPendingLabel";
            this.lblPendingLabel.Size = new System.Drawing.Size(250, 25);
            this.lblPendingLabel.TabIndex = 2;
            this.lblPendingLabel.Text = "Pending Approvals";
            // 
            // pnlMyEventsCard
            // 
            this.pnlMyEventsCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.pnlMyEventsCard.Controls.Add(this.iconMyEvents);
            this.pnlMyEventsCard.Controls.Add(this.lblMyEventsCount);
            this.pnlMyEventsCard.Controls.Add(this.lblMyEventsLabel);
            this.pnlMyEventsCard.Location = new System.Drawing.Point(30, 30);
            this.pnlMyEventsCard.Name = "pnlMyEventsCard";
            this.pnlMyEventsCard.Size = new System.Drawing.Size(340, 120);
            this.pnlMyEventsCard.TabIndex = 0;
            // 
            // iconMyEvents
            // 
            this.iconMyEvents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.iconMyEvents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.iconMyEvents.IconChar = FontAwesome.Sharp.IconChar.CalendarCheck;
            this.iconMyEvents.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.iconMyEvents.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconMyEvents.IconSize = 48;
            this.iconMyEvents.Location = new System.Drawing.Point(20, 35);
            this.iconMyEvents.Name = "iconMyEvents";
            this.iconMyEvents.Size = new System.Drawing.Size(48, 48);
            this.iconMyEvents.TabIndex = 0;
            this.iconMyEvents.TabStop = false;
            // 
            // lblMyEventsCount
            // 
            this.lblMyEventsCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblMyEventsCount.ForeColor = System.Drawing.Color.White;
            this.lblMyEventsCount.Location = new System.Drawing.Point(75, 25);
            this.lblMyEventsCount.Name = "lblMyEventsCount";
            this.lblMyEventsCount.Size = new System.Drawing.Size(250, 55);
            this.lblMyEventsCount.TabIndex = 1;
            this.lblMyEventsCount.Text = "0";
            this.lblMyEventsCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMyEventsLabel
            // 
            this.lblMyEventsLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblMyEventsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(161)))), ((int)(((byte)(178)))));
            this.lblMyEventsLabel.Location = new System.Drawing.Point(75, 75);
            this.lblMyEventsLabel.Name = "lblMyEventsLabel";
            this.lblMyEventsLabel.Size = new System.Drawing.Size(250, 25);
            this.lblMyEventsLabel.TabIndex = 2;
            this.lblMyEventsLabel.Text = "My Registered Events";
            // 
            // frmDashboardUser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.sidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "frmDashboardUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resident Dashboard - BEMS";
            this.Load += new System.EventHandler(this.frmDashboardResident_Load);
            this.sidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.topBar.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.pnlUpcomingEvents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpcomingEvents)).EndInit();
            this.pnlApprovedCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconApproved)).EndInit();
            this.pnlPendingCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPending)).EndInit();
            this.pnlMyEventsCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconMyEvents)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

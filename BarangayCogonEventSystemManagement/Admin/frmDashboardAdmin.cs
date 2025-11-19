using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using MySql.Data.MySqlClient;
using QRCoder;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmDashboardAdmin : Form
    {
        private IconButton currentActiveButton;
        private readonly Color activeOrHoverColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultColor = Color.Transparent;
        private Form currentChildForm;
        private Control[] dashboardControls;
        private ContextMenuStrip contextMenuQuickActions;

        public frmDashboardAdmin()
        {
            InitializeComponent();
            dashboardControls = new Control[]
            {
                pnlEventsCard, pnlAttendeesCard, pnlVolunteersCard, pnlPresentCard,
                pnlRecentRegistrations
            };
            InitializeContextMenuStyling();
            CustomizeRecentRegistrationsTable();
            StyleViewAllButton();
            LoadDashboardData();
            AttachHoverHandlers();
            if (btnDashboard != null)
                HighlightNav(btnDashboard);
        }

        private void InitializeContextMenuStyling()
        {
            // Create and style context menu for quick actions
            contextMenuQuickActions = new ContextMenuStrip();
            contextMenuQuickActions.BackColor = Color.FromArgb(37, 42, 64);
            contextMenuQuickActions.ForeColor = Color.White;
            contextMenuQuickActions.ShowImageMargin = false;
            contextMenuQuickActions.Renderer = new ToolStripProfessionalRenderer(new CustomContextMenuColorTable());
        }

        // Custom color table for context menu styling
        private class CustomContextMenuColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(46, 51, 73); }
            }

            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(37, 42, 64); }
            }

            public override Color MenuBorder
            {
                get { return Color.FromArgb(60, 65, 90); }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(46, 51, 73); }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(46, 51, 73); }
            }

            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.FromArgb(46, 51, 73); }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.FromArgb(46, 51, 73); }
            }
        }

        private void CustomizeRecentRegistrationsTable()
        {
            // Remove existing event handlers to prevent duplicates
            dgvRecentRegistrations.CellPainting -= dgvRecentRegistrations_CellPainting;
            dgvRecentRegistrations.CellClick -= dgvRecentRegistrations_CellClick;

            dgvRecentRegistrations.Columns.Clear();
            dgvRecentRegistrations.AllowUserToAddRows = false;
            dgvRecentRegistrations.ReadOnly = true;

            // GENERAL GRID SETTINGS - Match user dashboard style
            dgvRecentRegistrations.BackgroundColor = Color.FromArgb(37, 42, 64);
            dgvRecentRegistrations.BorderStyle = BorderStyle.None;
            dgvRecentRegistrations.GridColor = Color.FromArgb(60, 65, 90);
            dgvRecentRegistrations.EnableHeadersVisualStyles = false;
            dgvRecentRegistrations.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE
            dgvRecentRegistrations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvRecentRegistrations.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvRecentRegistrations.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRecentRegistrations.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvRecentRegistrations.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvRecentRegistrations.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvRecentRegistrations.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvRecentRegistrations.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvRecentRegistrations.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvRecentRegistrations.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvRecentRegistrations.DefaultCellStyle.ForeColor = Color.White;
            dgvRecentRegistrations.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvRecentRegistrations.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvRecentRegistrations.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvRecentRegistrations.RowTemplate.Height = 55;
            dgvRecentRegistrations.RowHeadersVisible = false;
            dgvRecentRegistrations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - SAME color as default cells for consistency
            dgvRecentRegistrations.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvRecentRegistrations.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvRecentRegistrations.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvRecentRegistrations.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering to reduce flicker
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvRecentRegistrations, new object[] { true });

            // Add columns with hidden ID and QR code
            dgvRecentRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvRecentRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_name",
                HeaderText = "Event",
                ReadOnly = true,
                FillWeight = 25
            });

            dgvRecentRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "user_name",
                HeaderText = "User",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvRecentRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "role",
                HeaderText = "Role",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvRecentRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "registration_date",
                HeaderText = "Date",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvRecentRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                ReadOnly = true,
                FillWeight = 10
            });

            dgvRecentRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "qr_code",
                HeaderText = "QR Code",
                ReadOnly = true,
                Visible = false
            });

            dgvRecentRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Quick Action",
                ReadOnly = true,
                FillWeight = 13
            });

            // Wire up event handlers
            dgvRecentRegistrations.CellPainting += dgvRecentRegistrations_CellPainting;
            dgvRecentRegistrations.CellClick += dgvRecentRegistrations_CellClick;
        }

        private void StyleViewAllButton()
        {
            // Style the View All button with rounded corners
            btnViewAllRegistrations.FlatStyle = FlatStyle.Flat;
            btnViewAllRegistrations.FlatAppearance.BorderSize = 0;
            btnViewAllRegistrations.Cursor = Cursors.Hand;
            btnViewAllRegistrations.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                using (GraphicsPath path = GetRoundPath(rect, 10))
                {
                    btn.Region = new Region(path);
                    using (SolidBrush brush = new SolidBrush(btn.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, rect,
                        btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private void dgvRecentRegistrations_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return; // Skip header

            var actionColumn = dgvRecentRegistrations.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                // Paint all parts except content to ensure consistent borders
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Check if this is a placeholder row (id will be 0 or null)
                var idValue = dgvRecentRegistrations.Rows[e.RowIndex].Cells["id"].Value;
                if (idValue == null || Convert.ToInt32(idValue) == 0)
                {
                    // This is the placeholder row, don't draw the action button
                    e.Handled = true;
                    return;
                }

                Rectangle cellBounds = e.CellBounds;

                int buttonWidth = 70;
                int buttonHeight = 30;

                // Center the button in the cell
                int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;

                Rectangle viewRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                int radius = 10;

                using (GraphicsPath viewPath = GetRoundPath(viewRect, radius))
                using (SolidBrush viewBrush = new SolidBrush(Color.FromArgb(0, 126, 249))) // Accent blue
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                using (Font btnFont = new Font("Segoe UI", 12F, FontStyle.Bold))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(viewBrush, viewPath);
                    e.Graphics.DrawString("...", btnFont, textBrush, viewRect, sf);
                }

                e.Handled = true;
            }
        }

        private void dgvRecentRegistrations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the ActionColumn was clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvRecentRegistrations.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                // Get the current row data
                DataGridViewRow row = dgvRecentRegistrations.Rows[e.RowIndex];
                
                // Check if this is a placeholder row (id will be 0 or null)
                var idValue = row.Cells["id"].Value;
                if (idValue == null || Convert.ToInt32(idValue) == 0)
                {
                    // This is the placeholder row, do nothing
                    return;
                }

                string status = row.Cells["status"].Value?.ToString();
                int registrationId = Convert.ToInt32(row.Cells["id"].Value);
                string eventName = row.Cells["event_name"].Value?.ToString();
                string userName = row.Cells["user_name"].Value?.ToString();
                string qrCode = row.Cells["qr_code"].Value?.ToString();

                // Clear existing menu items
                contextMenuQuickActions.Items.Clear();

                // Add menu items based on status
                if (status == "Pending")
                {
                    // Show Approve and Reject for pending registrations
                    ToolStripMenuItem approveItem = new ToolStripMenuItem("✓ Approve");
                    approveItem.Font = new Font("Segoe UI", 10F);
                    approveItem.Click += (s, ev) => QuickApproveRegistration(registrationId, eventName, userName);
                    contextMenuQuickActions.Items.Add(approveItem);

                    ToolStripMenuItem rejectItem = new ToolStripMenuItem("✗ Reject");
                    rejectItem.Font = new Font("Segoe UI", 10F);
                    rejectItem.Click += (s, ev) => QuickRejectRegistration(registrationId);
                    contextMenuQuickActions.Items.Add(rejectItem);
                }
                else if (status == "Approved")
                {
                    // Show View QR for approved registrations
                    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                    viewQRItem.Font = new Font("Segoe UI", 10F);
                    viewQRItem.Click += (s, ev) => ViewQRCode(eventName, userName, qrCode);
                    contextMenuQuickActions.Items.Add(viewQRItem);

                    // Option to reject approved registration
                    ToolStripMenuItem rejectItem = new ToolStripMenuItem("✗ Reject");
                    rejectItem.Font = new Font("Segoe UI", 10F);
                    rejectItem.Click += (s, ev) => QuickRejectRegistration(registrationId);
                    contextMenuQuickActions.Items.Add(rejectItem);
                }
                else if (status == "Rejected")
                {
                    // Show Approve for rejected registrations
                    ToolStripMenuItem approveItem = new ToolStripMenuItem("✓ Approve");
                    approveItem.Font = new Font("Segoe UI", 10F);
                    approveItem.Click += (s, ev) => QuickApproveRegistration(registrationId, eventName, userName);
                    contextMenuQuickActions.Items.Add(approveItem);
                }

                // Add separator
                contextMenuQuickActions.Items.Add(new ToolStripSeparator());

                // Add "View All Registrations" option
                ToolStripMenuItem viewAllItem = new ToolStripMenuItem("📋 View All Registrations");
                viewAllItem.Font = new Font("Segoe UI", 10F);
                viewAllItem.Click += (s, ev) => btnRegistrations_Click(s, ev);
                contextMenuQuickActions.Items.Add(viewAllItem);

                // Get cell rectangle
                Rectangle rect = dgvRecentRegistrations.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                // Calculate the button position (centered in cell, same as in CellPainting)
                int buttonWidth = 70;
                int buttonHeight = 30;
                int buttonX = rect.Left + (rect.Width - buttonWidth) / 2;
                int buttonY = rect.Top + (rect.Height - buttonHeight) / 2;

                // Position context menu just below and to the right of the button
                Point pt = new Point(buttonX + buttonWidth + 5, buttonY);

                // Show the context menu right next to the action button
                contextMenuQuickActions.Show(dgvRecentRegistrations, pt);
            }
        }

        private void QuickApproveRegistration(int registrationId, string eventName, string userName)
        {
            try
            {
                // Show confirmation dialog before approving
                DialogResult confirmResult = MessageBox.Show(
                    $"Do you want to approve this registration?\n\n" +
                    $"Event: {eventName}\n" +
                    $"User: {userName}\n\n" +
                    $"A QR code will be generated for this registration.",
                    "Confirm Approval",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Generate QR code
                    string qrText = $"{eventName}_{userName}_{Guid.NewGuid()}";
                    string folderPath = Path.Combine(Application.StartupPath, "Assets", "QR_Codes");
                    Directory.CreateDirectory(folderPath);
                    string fileName = $"{eventName}_{userName}.png".Replace(" ", "_");
                    string fullPath = Path.Combine(folderPath, fileName);

                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q))
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    using (Bitmap qrImage = qrCode.GetGraphic(6))
                    {
                        qrImage.Save(fullPath);
                    }

                    // Update database
                    string query = @"UPDATE registrations 
                                     SET status='Approved', qr_code=@qr 
                                     WHERE id=@id";
                    MySqlParameter[] parameters = {
                        new MySqlParameter("@qr", qrText),
                        new MySqlParameter("@id", registrationId)
                    };

                    int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (result > 0)
                    {
                        MessageBox.Show($"✓ Registration approved successfully!\n\nUser: {userName}\nEvent: {eventName}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDashboardData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error approving registration: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QuickRejectRegistration(int registrationId)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to reject this registration?",
                    "Confirm Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "UPDATE registrations SET status='Rejected' WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", registrationId) };

                    int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registration rejected successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDashboardData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rejecting registration: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewQRCode(string eventName, string userName, string qrCodeData)
        {
            try
            {
                // Check if QR code data exists
                if (string.IsNullOrEmpty(qrCodeData))
                {
                    MessageBox.Show("No QR code data available for this registration.", "Missing QR Code",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Generate QR code image from data
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrData = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q))
                using (QRCode qrCode = new QRCode(qrData))
                using (Bitmap qrImage = qrCode.GetGraphic(6))
                {
                    // Create a form to display the QR code
                    Form qrForm = new Form
                    {
                        Text = $"QR Code - {userName}",
                        Size = new Size(400, 450),
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        MaximizeBox = false,
                        MinimizeBox = false,
                        BackColor = Color.White
                    };

                    PictureBox picQR = new PictureBox
                    {
                        Image = (Bitmap)qrImage.Clone(),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Fill
                    };

                    Label lblInfo = new Label
                    {
                        Text = $"Event: {eventName}\nUser: {userName}",
                        Font = new Font("Segoe UI", 10F),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Top,
                        Height = 60,
                        BackColor = Color.FromArgb(0, 126, 249),
                        ForeColor = Color.White
                    };

                    qrForm.Controls.Add(picQR);
                    qrForm.Controls.Add(lblInfo);
                    qrForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing QR code: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenChild(Form child)
        {
            try
            {
                foreach (var frm in mainPanel.Controls.OfType<Form>().ToList())
                {
                    mainPanel.Controls.Remove(frm);
                    frm.Dispose();
                }
                currentChildForm = null;

                if (child == null)
                {
                    foreach (var ctrl in dashboardControls)
                        ctrl.Visible = true;
                    LoadDashboardData();
                    return;
                }

                foreach (var ctrl in dashboardControls)
                    ctrl.Visible = false;

                currentChildForm = child;
                child.TopLevel = false;
                child.FormBorderStyle = FormBorderStyle.None;
                child.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(child);
                child.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading view: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AttachHoverHandlers()
        {
            foreach (Control c in sidebar.Controls)
            {
                if (c is IconButton b)
                {
                    b.MouseEnter -= SidebarButton_MouseEnter;
                    b.MouseLeave -= SidebarButton_MouseLeave;
                    b.MouseEnter += SidebarButton_MouseEnter;
                    b.MouseLeave += SidebarButton_MouseLeave;
                }
            }
        }

        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is IconButton b)
                b.BackColor = activeOrHoverColor;
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is IconButton b && b != currentActiveButton)
                b.BackColor = defaultColor;
        }

        private void HighlightNav(IconButton btn)
        {
            if (pnlNav == null || sidebar == null || btn == null) return;
            pnlNav.Top = btn.Top;
            pnlNav.Height = btn.Height;
            currentActiveButton = btn;

            foreach (Control c in sidebar.Controls)
            {
                if (c is IconButton b && b != currentActiveButton)
                    b.BackColor = defaultColor;
            }
            btn.BackColor = activeOrHoverColor;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            HighlightNav(btnDashboard);
            lblTitle.Text = "Admin Dashboard";
            OpenChild(null);
        }

        private void LoadDashboardData()
        {
            try
            {
                // Load statistics
                string statsQuery = @"SELECT 
                                    (SELECT COUNT(*) FROM events) AS total_events,
                                    (SELECT COUNT(*) FROM registrations WHERE role='attendee') AS total_attendees,
                                    (SELECT COUNT(*) FROM registrations WHERE role='volunteer') AS total_volunteers,
                                    (SELECT COUNT(*) FROM attendance) AS total_present";

                DataTable dtStats = DatabaseHelper.ExecuteQuery(statsQuery);

                if (dtStats.Rows.Count > 0)
                {
                    lblEventsCount.Text = dtStats.Rows[0]["total_events"].ToString();
                    lblAttendeesCount.Text = dtStats.Rows[0]["total_attendees"].ToString();
                    lblVolunteersCount.Text = dtStats.Rows[0]["total_volunteers"].ToString();
                    lblPresentCount.Text = dtStats.Rows[0]["total_present"].ToString();
                }

                // Load recent registrations (last 2 days) with ID and QR code
                string recentQuery = @"SELECT 
                                        r.id,
                                        e.name AS event_name,
                                        u.name AS user_name,
                                        r.role,
                                        DATE_FORMAT(r.created_at, '%b %d, %Y %h:%i %p') AS registration_date,
                                        r.status,
                                        r.qr_code
                                    FROM registrations r
                                    INNER JOIN events e ON r.event_id = e.id
                                    INNER JOIN users u ON r.user_id = u.id
                                    WHERE r.created_at >= DATE_SUB(NOW(), INTERVAL 2 DAY)
                                    ORDER BY r.created_at DESC
                                    LIMIT 10";

                DataTable dtRecent = DatabaseHelper.ExecuteQuery(recentQuery);

                // Clear existing rows
                dgvRecentRegistrations.Rows.Clear();

                // Check if there's data
                if (dtRecent.Rows.Count == 0)
                {
                    // Add placeholder row when no data
                    int placeholderIndex = dgvRecentRegistrations.Rows.Add(
                        0, // id
                        "", // event_name
                        "No recent registrations in the last 2 days", // user_name (placeholder message)
                        "", // role
                        "", // registration_date
                        "", // status
                        "", // qr_code
                        ""  // ActionColumn
                    );

                    // Style the placeholder row
                    DataGridViewRow placeholderRow = dgvRecentRegistrations.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178); // Muted gray
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    
                    // Merge appearance by centering across all visible columns
                    foreach (DataGridViewCell cell in placeholderRow.Cells)
                    {
                        if (dgvRecentRegistrations.Columns[cell.ColumnIndex].Visible)
                        {
                            cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
                else
                {
                    // Populate rows with data
                    foreach (DataRow dr in dtRecent.Rows)
                    {
                        // Capitalize the first letter of the role
                        string role = dr["role"].ToString();
                        string capitalizedRole = string.IsNullOrEmpty(role) ? role : char.ToUpper(role[0]) + role.Substring(1).ToLower();

                        dgvRecentRegistrations.Rows.Add(
                            dr["id"],
                            dr["event_name"],
                            dr["user_name"],
                            capitalizedRole,
                            dr["registration_date"],
                            dr["status"],
                            dr["qr_code"],
                            "" // ActionColumn (will be custom painted)
                        );
                    }
                }

                dgvRecentRegistrations.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnManageEvents_Click(object sender, EventArgs e)
        {
            HighlightNav(btnManageEvents);
            lblTitle.Text = "Manage Events";
            OpenChild(new frmManageEvents());
        }

        private void btnRegistrations_Click(object sender, EventArgs e)
        {
            HighlightNav(btnRegistrations);
            lblTitle.Text = "Registrations";
            OpenChild(new frmRegistrations());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            HighlightNav(btnReports);
            lblTitle.Text = "Reports";
            OpenChild(new frmReports());
        }

        private void btnScanner_Click(object sender, EventArgs e)
        {
            HighlightNav(btnQRScanner);
            lblTitle.Text = "QR Scanner";
            OpenChild(new frmAttendanceScanner());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                frmAdminLogin login = new frmAdminLogin();
                login.ShowDialog();
                this.Close();
            }
        }

        private void btnViewAllRegistrations_Click(object sender, EventArgs e)
        {
            btnRegistrations_Click(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void frmDashboardAdmin_Load(object sender, EventArgs e)
        {

        }

        private void lblRecentRegistrations_Click(object sender, EventArgs e)
        {

        }
    }
}

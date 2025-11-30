using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using MySql.Data.MySqlClient;
using BarangayCogonEventSystemManagement.User;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmDashboardUser : Form
    {
        private int userId;
        private string userName;
        private IconButton currentActiveButton;
        private readonly Color activeOrHoverColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultColor = Color.Transparent;
        private Form currentChildForm;
        private Control[] dashboardControls;
        private ContextMenuStrip contextMenuActions;

        public frmDashboardUser(int userId, string userName)
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;
            dashboardControls = new Control[]
            {
                pnlMyEventsCard, pnlPendingCard, pnlApprovedCard, pnlUpcomingEvents
            };
            InitializeContextMenu();
        }

        private void InitializeContextMenu()
        {
            // Create and style context menu for actions
            contextMenuActions = new ContextMenuStrip();
            contextMenuActions.BackColor = Color.FromArgb(37, 42, 64);
            contextMenuActions.ForeColor = Color.White;
            contextMenuActions.ShowImageMargin = false;
            contextMenuActions.Renderer = new ToolStripProfessionalRenderer(new CustomContextMenuColorTable());
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

        private void frmDashboardResident_Load(object sender, EventArgs e)
        {
            CustomizeUpcomingEventsTable();
            StyleViewAllButton();
            LoadDashboardData();
            AttachHoverHandlers();
            if (btnDashboard != null)
                HighlightNav(btnDashboard);
        }

        private void CustomizeUpcomingEventsTable()
        {
            // Remove existing event handlers to prevent duplicates
            dgvUpcomingEvents.CellPainting -= dgvUpcomingEvents_CellPainting;
            dgvUpcomingEvents.CellClick -= dgvUpcomingEvents_CellClick;

            dgvUpcomingEvents.Columns.Clear();
            dgvUpcomingEvents.AllowUserToAddRows = false;
            dgvUpcomingEvents.ReadOnly = true;

            // GENERAL GRID SETTINGS
            dgvUpcomingEvents.BackgroundColor = Color.FromArgb(37, 42, 64);
            dgvUpcomingEvents.BorderStyle = BorderStyle.None;
            dgvUpcomingEvents.GridColor = Color.FromArgb(60, 65, 90);
            dgvUpcomingEvents.EnableHeadersVisualStyles = false;
            dgvUpcomingEvents.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE
            dgvUpcomingEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvUpcomingEvents.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvUpcomingEvents.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvUpcomingEvents.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvUpcomingEvents.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvUpcomingEvents.DefaultCellStyle.ForeColor = Color.White;
            dgvUpcomingEvents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvUpcomingEvents.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUpcomingEvents.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvUpcomingEvents.RowTemplate.Height = 55;
            dgvUpcomingEvents.RowHeadersVisible = false;
            dgvUpcomingEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - SAME color as default cells for consistency
            dgvUpcomingEvents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvUpcomingEvents.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvUpcomingEvents.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvUpcomingEvents.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvUpcomingEvents, new object[] { true });

            // Add columns
            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "registration_id",
                HeaderText = "Registration ID",
                ReadOnly = true,
                Visible = false
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "is_registered",
                HeaderText = "Is Registered",
                ReadOnly = true,
                Visible = false
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "registration_status",
                HeaderText = "Registration Status",
                ReadOnly = true,
                Visible = false
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_name",
                HeaderText = "Event Name",
                ReadOnly = true,
                FillWeight = 25
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_date",
                HeaderText = "Event Date",
                ReadOnly = true,
                FillWeight = 17
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_time",
                HeaderText = "Event Schedule",
                ReadOnly = true,
                FillWeight = 17
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_venue",
                HeaderText = "Venue",
                ReadOnly = true,
                FillWeight = 18
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_type",
                HeaderText = "Type",
                ReadOnly = true,
                FillWeight = 13
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_description",
                HeaderText = "Description",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_end_datetime",
                HeaderText = "Event End",
                ReadOnly = true,
                Visible = false
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 13
            });

            // Wire up event handlers
            dgvUpcomingEvents.CellPainting += dgvUpcomingEvents_CellPainting;
            dgvUpcomingEvents.CellClick += dgvUpcomingEvents_CellClick;
        }

        private void StyleViewAllButton()
        {
            btnViewAllEvents.FlatStyle = FlatStyle.Flat;
            btnViewAllEvents.FlatAppearance.BorderSize = 0;
            btnViewAllEvents.Cursor = Cursors.Hand;
            btnViewAllEvents.Paint += (s, e) =>
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

        private void LoadDashboardData()
        {
            try
            {
                // Load statistics for this user
                string statsQuery = @"SELECT 
                                    (SELECT COUNT(*) FROM registrations WHERE user_id=@user_id) AS my_events,
                                    (SELECT COUNT(*) FROM registrations WHERE user_id=@user_id AND status='Pending') AS pending,
                                    (SELECT COUNT(*) FROM registrations WHERE user_id=@user_id AND status='Approved') AS approved";

                MySqlParameter[] statsParams = { new MySqlParameter("@user_id", userId) };
                DataTable dtStats = DatabaseHelper.ExecuteQuery(statsQuery, statsParams);

                if (dtStats.Rows.Count > 0)
                {
                    lblMyEventsCount.Text = dtStats.Rows[0]["my_events"].ToString();
                    lblPendingCount.Text = dtStats.Rows[0]["pending"].ToString();
                    lblApprovedCount.Text = dtStats.Rows[0]["approved"].ToString();
                }

                // Load upcoming events with registration status
                string eventsQuery = @"SELECT 
                                        e.id AS event_id,
                                        e.name AS event_name,
                                        CASE 
                                            WHEN DATE(e.start_datetime) = DATE(e.end_datetime) THEN DATE_FORMAT(e.start_datetime, '%b %d, %Y')
                                            ELSE CONCAT(DATE_FORMAT(e.start_datetime, '%b %d'), ' - ', DATE_FORMAT(e.end_datetime, '%b %d, %Y'))
                                        END AS event_date,
                                        CONCAT(DATE_FORMAT(e.start_datetime, '%h:%i %p'), ' - ', DATE_FORMAT(e.end_datetime, '%h:%i %p')) AS event_time,
                                        e.venue AS event_venue,
                                        e.type AS event_type,
                                        e.description AS event_description,
                                        e.end_datetime AS event_end_datetime,
                                        r.id AS registration_id,
                                        r.status AS registration_status,
                                        CASE WHEN r.id IS NOT NULL THEN 1 ELSE 0 END AS is_registered
                                    FROM events e
                                    LEFT JOIN registrations r ON e.id = r.event_id AND r.user_id = @user_id
                                    WHERE e.start_datetime >= NOW()
                                    ORDER BY e.start_datetime ASC
                                    LIMIT 10";

                MySqlParameter[] eventsParams = { new MySqlParameter("@user_id", userId) };
                DataTable dtEvents = DatabaseHelper.ExecuteQuery(eventsQuery, eventsParams);

                // Clear existing rows
                dgvUpcomingEvents.Rows.Clear();

                // Check if there's data
                if (dtEvents.Rows.Count == 0)
                {
                    // Add placeholder row when no data
                    int placeholderIndex = dgvUpcomingEvents.Rows.Add(
                        0, // event_id
                        null, // registration_id
                        0, // is_registered
                        null, // registration_status
                        "No upcoming events available", // event_name (placeholder message)
                        "", // event_date
                        "", // event_time
                        "", // event_venue
                        "", // event_type
                        "", // event_description
                        DBNull.Value, // event_end_datetime
                        ""  // ActionColumn
                    );

                    // Style the placeholder row
                    DataGridViewRow placeholderRow = dgvUpcomingEvents.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    // Populate rows with data
                    foreach (DataRow dr in dtEvents.Rows)
                    {
                        dgvUpcomingEvents.Rows.Add(
                            dr["event_id"],
                            dr["registration_id"] == DBNull.Value ? (object)null : dr["registration_id"],
                            dr["is_registered"],
                            dr["registration_status"] == DBNull.Value ? (object)null : dr["registration_status"],
                            dr["event_name"],
                            dr["event_date"],
                            dr["event_time"],
                            dr["event_venue"],
                            dr["event_type"],
                            dr["event_description"],
                            dr["event_end_datetime"],
                            "" // ActionColumn (will be custom painted)
                        );
                    }
                }

                dgvUpcomingEvents.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data: " + ex.Message,
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
            lblTitle.Text = "Resident Dashboard";
            OpenChild(null);
        }

        private void btnBrowseEvents_Click(object sender, EventArgs e)
        {
            HighlightNav(btnBrowseEvents);
            lblTitle.Text = "Browse Events";
            OpenChild(new frmBrowseEvents(userId));
        }

        private void btnMyEvents_Click(object sender, EventArgs e)
        {
            HighlightNav(btnMyEvents);
            lblTitle.Text = "My Events";
            OpenChild(new frmMyEvents(userId));
        }

        private void btnMyQR_Click(object sender, EventArgs e)
        {
            HighlightNav(btnMyQR);
            lblTitle.Text = "My QR Codes";
            OpenChild(new frmMyQR(userId));
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
                frmUserLogin login = new frmUserLogin();
                login.ShowDialog();
                this.Close();
            }
        }

        private void btnViewAllEvents_Click(object sender, EventArgs e)
        {
            btnBrowseEvents_Click(sender, e);
        }

        private void dgvUpcomingEvents_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var actionColumn = dgvUpcomingEvents.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                // Paint all parts except content to ensure consistent borders
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Check if this is a placeholder row
                var eventIdValue = dgvUpcomingEvents.Rows[e.RowIndex].Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0)
                {
                    e.Handled = true;
                    return;
                }

                Rectangle cellBounds = e.CellBounds;
                
                // Check if user is registered for this event
                DataGridViewRow row = dgvUpcomingEvents.Rows[e.RowIndex];
                bool isRegistered = row.Cells["is_registered"].Value != null && 
                                    Convert.ToBoolean(row.Cells["is_registered"].Value);
                
                string registrationStatus = row.Cells["registration_status"].Value?.ToString();
                
                // Show N/A only for Rejected status (matching frmMyEvents)
                bool showNA = isRegistered && registrationStatus == "Rejected";

                if (showNA)
                {
                    // Draw "N/A" text for rejected registrations
                    using (Font naFont = new Font("Segoe UI", 10F, FontStyle.Regular))
                    using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(158, 161, 178)))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        e.Graphics.DrawString("N/A", naFont, textBrush, cellBounds, sf);
                    }
                }
                else
                {
                    // Draw action button for non-registered events or non-rejected registrations
                    int buttonWidth = isRegistered ? 70 : 90;
                    int buttonHeight = 30;
                    int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                    int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;
                    Rectangle buttonRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                    int radius = 10;

                    using (GraphicsPath path = GetRoundPath(buttonRect, radius))
                    using (SolidBrush buttonBrush = new SolidBrush(isRegistered ? 
                        Color.FromArgb(0, 126, 249) : Color.FromArgb(0, 126, 249)))
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    using (Font btnFont = new Font("Segoe UI", isRegistered ? 12F : 9F, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.FillPath(buttonBrush, path);
                        e.Graphics.DrawString(isRegistered ? "..." : "Register", btnFont, textBrush, buttonRect, sf);
                    }
                }

                e.Handled = true;
            }
        }

        private void dgvUpcomingEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvUpcomingEvents.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                DataGridViewRow row = dgvUpcomingEvents.Rows[e.RowIndex];

                var eventIdValue = row.Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0)
                {
                    return;
                }

                int eventId = Convert.ToInt32(row.Cells["event_id"].Value);
                string eventName = row.Cells["event_name"].Value?.ToString();
                string eventDate = row.Cells["event_date"].Value?.ToString();
                string eventVenue = row.Cells["event_venue"].Value?.ToString();
                bool isRegistered = row.Cells["is_registered"].Value != null && 
                                    Convert.ToBoolean(row.Cells["is_registered"].Value);
                
                string registrationStatus = row.Cells["registration_status"].Value?.ToString();
                
                // Don't allow action if user is registered and status is Rejected
                bool showNA = isRegistered && registrationStatus == "Rejected";
                
                if (showNA)
                {
                    // No action for rejected registrations
                    return;
                }

                if (isRegistered)
                {
                    // Show context menu with options based on status
                    int registrationId = Convert.ToInt32(row.Cells["registration_id"].Value);
                    
                    // Clear existing menu items
                    contextMenuActions.Items.Clear();

                    // Add menu items based on status
                    if (registrationStatus == "Pending")
                    {
                        // Show Unregister for pending registrations
                        ToolStripMenuItem unregisterItem = new ToolStripMenuItem("✗ Unregister");
                        unregisterItem.Font = new Font("Segoe UI", 10F);
                        unregisterItem.Click += (s, ev) => UnregisterFromEvent(registrationId, eventName);
                        contextMenuActions.Items.Add(unregisterItem);
                    }
                    else
                    {
                        // Show View QR for approved/checked-in/attended registrations
                        ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                        viewQRItem.Font = new Font("Segoe UI", 10F);
                        viewQRItem.Click += (s, ev) => ViewQRCode(eventName, registrationId);
                        contextMenuActions.Items.Add(viewQRItem);
                    }

                    // Get cell rectangle
                    Rectangle rect = dgvUpcomingEvents.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                    // Calculate the button position (centered in cell, same as in CellPainting)
                    int buttonWidth = 70;
                    int buttonHeight = 30;
                    int buttonX = rect.Left + (rect.Width - buttonWidth) / 2;
                    int buttonY = rect.Top + (rect.Height - buttonHeight) / 2;

                    // Position context menu just below and to the right of the button
                    Point pt = new Point(buttonX + buttonWidth + 5, buttonY);

                    // Show the context menu right next to the action button
                    contextMenuActions.Show(dgvUpcomingEvents, pt);
                }
                else
                {
                    // Register
                    RegisterForEvent(eventId, eventName, eventDate, eventVenue);
                }
            }
        }

        private void ViewQRCode(string eventName, int registrationId)
        {
            try
            {
                // Fetch QR code data, event end datetime, and registration status
                string query = @"SELECT r.qr_code, r.status, e.end_datetime 
                                FROM registrations r
                                INNER JOIN events e ON r.event_id = e.id
                                WHERE r.id=@id";
                MySqlParameter[] parameters = { new MySqlParameter("@id", registrationId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count == 0 || dt.Rows[0]["qr_code"] == DBNull.Value)
                {
                    MessageBox.Show("No QR code available for this event.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string qrCodeData = dt.Rows[0]["qr_code"].ToString();
                string registrationStatus = dt.Rows[0]["status"].ToString();
                DateTime eventEndDateTime = Convert.ToDateTime(dt.Rows[0]["end_datetime"]);

                // Use the modular QR viewer form with status
                frmQRCodeViewer qrViewer = new frmQRCodeViewer(eventName, qrCodeData, eventEndDateTime, registrationStatus);
                qrViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading QR code: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegisterForEvent(int eventId, string eventName, string eventDate, string eventVenue)
        {
            try
            {
                // Prompt user to select their role for this event
                string selectedRole = ShowRoleSelectionDialog(eventName);
                
                if (string.IsNullOrEmpty(selectedRole))
                {
                    // User cancelled the role selection
                    return;
                }

                // Check for exact start/end datetime conflicts with any existing registration for this user
                try
                {
                    string eventTimeQuery = "SELECT start_datetime, end_datetime FROM events WHERE id = @id";
                    MySqlParameter[] timeParams = { new MySqlParameter("@id", eventId) };
                    DataTable dtTime = DatabaseHelper.ExecuteQuery(eventTimeQuery, timeParams);

                    if (dtTime.Rows.Count > 0)
                    {
                        DateTime start = Convert.ToDateTime(dtTime.Rows[0]["start_datetime"]);
                        DateTime end = Convert.ToDateTime(dtTime.Rows[0]["end_datetime"]);

                        string conflictQuery = @"SELECT e.name, e.start_datetime, e.end_datetime, r.status FROM registrations r
                                                 INNER JOIN events e ON r.event_id = e.id
                                                 WHERE r.user_id = @user_id
                                                 AND DATE(e.start_datetime) = DATE(@start)
                                                 AND DATE(e.end_datetime) = DATE(@end)
                                                 AND DATE_FORMAT(e.start_datetime, '%H:%i') = DATE_FORMAT(@start, '%H:%i')
                                                 AND DATE_FORMAT(e.end_datetime, '%H:%i') = DATE_FORMAT(@end, '%H:%i')";

                        MySqlParameter[] conflictParams = {
                            new MySqlParameter("@user_id", userId),
                            new MySqlParameter("@start", start),
                            new MySqlParameter("@end", end)
                        };

                        DataTable dtConflicts = DatabaseHelper.ExecuteQuery(conflictQuery, conflictParams);
                        if (dtConflicts.Rows.Count > 0)
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            foreach (DataRow dr in dtConflicts.Rows)
                            {
                                DateTime cs = Convert.ToDateTime(dr["start_datetime"]);
                                DateTime ce = Convert.ToDateTime(dr["end_datetime"]);
                                string status = dr["status"] == DBNull.Value ? "" : dr["status"].ToString();
                                sb.AppendLine($"- {dr["name"].ToString()} ({cs.ToString("MMM dd, yyyy hh:mm tt")} - {ce.ToString("hh:mm tt")}) {(!string.IsNullOrEmpty(status) ? "- Status: " + status : "")} ");
                            }

                            DialogResult conflictResult = MessageBox.Show(
                                $"Warning: The event you're trying to register has the same start and end time as one or more of your registered events:\n\n{sb.ToString()}\nDo you still want to continue?",
                                "Time Conflict Detected",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (conflictResult == DialogResult.No) return;
                        }
                    }
                }
                catch (Exception exTime)
                {
                    // Non-fatal: allow registration if time-check fails
                    Console.WriteLine("Error checking event time conflicts: " + exTime.Message);
                }

                // Show confirmation dialog before registration
                DialogResult confirmResult = MessageBox.Show(
                    $"Do you want to register for this event?\n\n" +
                    $"Event: {eventName}\n" +
                    $"Date: {eventDate}\n" +
                    $"Venue: {eventVenue}\n" +
                    $"Role: {selectedRole}\n\n" +
                    $"Your registration will be pending until approved by an administrator.",
                    "Confirm Registration",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // Check if user is already registered (safety check)
                        string checkQuery = "SELECT id FROM registrations WHERE event_id=@event_id AND user_id=@user_id";
                        MySqlParameter[] checkParams = {
                            new MySqlParameter("@event_id", eventId),
                            new MySqlParameter("@user_id", userId)
                        };
                        
                        DataTable dtCheck = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);
                        if (dtCheck.Rows.Count > 0)
                        {
                            MessageBox.Show("You are already registered for this event.", "Already Registered",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            LoadDashboardData();
                            return;
                        }

                        string insertQuery = @"INSERT INTO registrations (event_id, user_id, role, status, qr_code, created_at)
                                              VALUES (@event_id, @user_id, @role, 'Pending', NULL, NOW())";

                        MySqlParameter[] insertParams = {
                            new MySqlParameter("@event_id", eventId),
                            new MySqlParameter("@user_id", userId),
                            new MySqlParameter("@role", selectedRole.ToLower())
                        };

                        int result = DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams);
                        
                        if (result > 0)
                        {
                            MessageBox.Show($"Successfully registered for '{eventName}' as {selectedRole}!\n\nPlease wait for admin approval.",
                                "Registered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Reload the dashboard data
                            LoadDashboardData();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (MySqlException mysqlEx)
                    {
                        // Handle MySQL specific errors
                        if (mysqlEx.Number == 1062) // Duplicate entry error
                        {
                            MessageBox.Show("You are already registered for this event.", "Duplicate Registration",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            LoadDashboardData();
                        }
                        else if (mysqlEx.Number == 1452) // Foreign key constraint fails
                        {
                            MessageBox.Show("Event or user not found. Please refresh and try again.", "Database Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show($"Database error: {mysqlEx.Message}\n\nError Code: {mysqlEx.Number}", 
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error registering for event: {ex.Message}\n\nPlease contact the administrator if this problem persists.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during registration: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UnregisterFromEvent(int registrationId, string eventName)
        {
            try
            {
                // Show confirmation dialog before unregistering
                DialogResult confirmResult = MessageBox.Show(
                    $"Are you sure you want to unregister from this event?\n\n" +
                    $"Event: {eventName}\n\n" +
                    $"This action cannot be undone.",
                    "Confirm Unregister",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    string query = "DELETE FROM registrations WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", registrationId) };

                    int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (result > 0)
                    {
                        MessageBox.Show($"Successfully unregistered from '{eventName}'.", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDashboardData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unregistering: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ShowRoleSelectionDialog(string eventName)
        {
            // Create a custom dialog for role selection
            Form roleDialog = new Form
            {
                Text = "Select Your Role",
                Size = new Size(400, 250),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(46, 51, 73)
            };

            Label lblMessage = new Label
            {
                Text = $"Please select your role for:\n{eventName}",
                Location = new Point(20, 20),
                Size = new Size(360, 50),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };

            ComboBox cboRole = new ComboBox
            {
                Location = new Point(80, 90),
                Size = new Size(240, 30),
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboRole.Items.AddRange(new object[] { "Attendee", "Volunteer", "Speaker" });
            cboRole.SelectedIndex = 0;

            Button btnOk = new Button
            {
                Text = "OK",
                Location = new Point(100, 140),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(0, 126, 249),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                DialogResult = DialogResult.OK,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;

            // Add Paint event for rounded OK button
            btnOk.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                using (GraphicsPath path = GetRoundPath(rect, 8))
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

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(210, 140),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                DialogResult = DialogResult.Cancel,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            // Add Paint event for rounded Cancel button
            btnCancel.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                using (GraphicsPath path = GetRoundPath(rect, 8))
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

            roleDialog.Controls.Add(lblMessage);
            roleDialog.Controls.Add(cboRole);
            roleDialog.Controls.Add(btnOk);
            roleDialog.Controls.Add(btnCancel);

            roleDialog.AcceptButton = btnOk;
            roleDialog.CancelButton = btnCancel;

            if (roleDialog.ShowDialog() == DialogResult.OK)
            {
                return cboRole.SelectedItem?.ToString();
            }

            return null;
        }
    }
}

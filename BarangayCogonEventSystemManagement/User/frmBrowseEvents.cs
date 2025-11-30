using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventSystemManagement.User
{
    public partial class frmBrowseEvents : Form
    {
        private int userId;
        private Panel contentPanel;
        private TextBox txtSearch;
        private ComboBox cboTypeFilter;
        private Label lblFilter;
        private DataGridView dgvBrowse;
        private ContextMenuStrip contextMenuActions;

        public frmBrowseEvents(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.BackColor = Color.FromArgb(46, 51, 73);
            InitializeControls();
            InitializeContextMenu();
            LoadBrowseEvents();
        }

        private void InitializeContextMenu()
        {
            contextMenuActions = new ContextMenuStrip();
            contextMenuActions.BackColor = Color.FromArgb(37, 42, 64);
            contextMenuActions.ForeColor = Color.White;
            contextMenuActions.ShowImageMargin = false;
            contextMenuActions.Renderer = new ToolStripProfessionalRenderer(new CustomContextMenuColorTable());
        }

        private class CustomContextMenuColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected { get { return Color.FromArgb(46, 51, 73); } }
            public override Color MenuItemBorder { get { return Color.FromArgb(37, 42, 64); } }
            public override Color MenuBorder { get { return Color.FromArgb(60, 65, 90); } }
            public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(46, 51, 73); } }
            public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(46, 51, 73); } }
            public override Color MenuItemPressedGradientBegin { get { return Color.FromArgb(46, 51, 73); } }
            public override Color MenuItemPressedGradientEnd { get { return Color.FromArgb(46, 51, 73); } }
        }

        private void InitializeControls()
        {
            // Main container panel
            contentPanel = new Panel
            {
                Location = new Point(30, 20),
                Size = new Size(1090, 690),
                BackColor = Color.FromArgb(46, 51, 73)
            };

            // Search bar
            txtSearch = new TextBox
            {
                Location = new Point(0, 0),
                Size = new Size(350, 35),
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.Text = "🔍 Search events...";
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Enter += (s, ev) => { if (txtSearch.Text == "🔍 Search events...") { txtSearch.Text = ""; txtSearch.ForeColor = Color.White; } };
            txtSearch.Leave += (s, ev) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Search events..."; txtSearch.ForeColor = Color.Gray; } };

            // Type filter label
            lblFilter = new Label
            {
                Text = "Filter by Type:",
                Location = new Point(370, 5),
                Size = new Size(100, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            // Type filter dropdown
            cboTypeFilter = new ComboBox
            {
                Location = new Point(475, 0),
                Size = new Size(200, 35),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTypeFilter.Items.AddRange(new object[] { "All Types", "Community Service", "Health Drive", "Cleanup Drive", "Barangay Assembly", "Training / Workshop" });
            cboTypeFilter.SelectedIndex = 0;

            // Browse events table
            dgvBrowse = new DataGridView
            {
                Location = new Point(0, 50),
                Size = new Size(1090, 640),
                BackgroundColor = Color.FromArgb(46, 51, 73),
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(60, 65, 90),
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                CellBorderStyle = DataGridViewCellBorderStyle.Single
            };

            CustomizeDataGridView();

            // Search and filter handlers
            txtSearch.TextChanged += (s, ev) => LoadBrowseEvents();
            cboTypeFilter.SelectedIndexChanged += (s, ev) => LoadBrowseEvents();

            // Add controls to content panel
            contentPanel.Controls.Add(txtSearch);
            contentPanel.Controls.Add(lblFilter);
            contentPanel.Controls.Add(cboTypeFilter);
            contentPanel.Controls.Add(dgvBrowse);

            // Add content panel to form
            this.Controls.Add(contentPanel);
        }

        private void CustomizeDataGridView()
        {
            dgvBrowse.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvBrowse.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvBrowse.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBrowse.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvBrowse.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvBrowse.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvBrowse.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvBrowse.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvBrowse.ColumnHeadersHeight = 45;

            dgvBrowse.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvBrowse.DefaultCellStyle.ForeColor = Color.White;
            dgvBrowse.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvBrowse.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvBrowse.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvBrowse.RowTemplate.Height = 55;

            dgvBrowse.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvBrowse.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvBrowse.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvBrowse.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvBrowse, new object[] { true });

            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_id", HeaderText = "ID", ReadOnly = true, Visible = false });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "registration_id", HeaderText = "Registration ID", ReadOnly = true, Visible = false });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "is_registered", HeaderText = "Is Registered", ReadOnly = true, Visible = false });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "registration_status", HeaderText = "Registration Status", ReadOnly = true, Visible = false });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_name", HeaderText = "Event Name", ReadOnly = true, FillWeight = 25 });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_date", HeaderText = "Event Date", ReadOnly = true, FillWeight = 17 });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_time", HeaderText = "Event Schedule", ReadOnly = true, FillWeight = 17 });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_venue", HeaderText = "Venue", ReadOnly = true, FillWeight = 18 });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_type", HeaderText = "Type", ReadOnly = true, FillWeight = 13 });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_description", HeaderText = "Description", ReadOnly = true, FillWeight = 15 });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_end_datetime", HeaderText = "Event End", ReadOnly = true, Visible = false });
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn { Name = "ActionColumn", HeaderText = "Action", ReadOnly = true, FillWeight = 13 });

            dgvBrowse.CellPainting += dgvBrowse_CellPainting;
            dgvBrowse.CellClick += dgvBrowse_CellClick;
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

        private void dgvBrowse_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var actionColumn = dgvBrowse.Columns["ActionColumn"];
            if (actionColumn == null) return;
            if (e.ColumnIndex == actionColumn.Index)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                var eventIdValue = dgvBrowse.Rows[e.RowIndex].Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0) { e.Handled = true; return; }

                Rectangle cellBounds = e.CellBounds;
                DataGridViewRow row = dgvBrowse.Rows[e.RowIndex];
                bool isRegistered = row.Cells["is_registered"].Value != null && Convert.ToBoolean(row.Cells["is_registered"].Value);
                string registrationStatus = row.Cells["registration_status"].Value?.ToString();
                bool showNA = isRegistered && registrationStatus == "Rejected";

                if (showNA)
                {
                    using (Font naFont = new Font("Segoe UI", 10F, FontStyle.Regular))
                    using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(158, 161, 178)))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        e.Graphics.DrawString("N/A", naFont, textBrush, cellBounds, sf);
                    }
                }
                else
                {
                    int buttonWidth = isRegistered ? 70 : 90;
                    int buttonHeight = 30;
                    int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                    int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;
                    Rectangle buttonRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                    int radius = 10;
                    using (GraphicsPath path = GetRoundPath(buttonRect, radius))
                    using (SolidBrush buttonBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
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

        private void dgvBrowse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvBrowse.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                DataGridViewRow row = dgvBrowse.Rows[e.RowIndex];
                var eventIdValue = row.Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0) return;

                int eventId = Convert.ToInt32(row.Cells["event_id"].Value);
                string eventName = row.Cells["event_name"].Value?.ToString();
                string eventDate = row.Cells["event_date"].Value?.ToString();
                string eventVenue = row.Cells["event_venue"].Value?.ToString();
                bool isRegistered = row.Cells["is_registered"].Value != null && Convert.ToBoolean(row.Cells["is_registered"].Value);
                string registrationStatus = row.Cells["registration_status"].Value?.ToString();
                bool showNA = isRegistered && registrationStatus == "Rejected";
                if (showNA) return;

                if (isRegistered)
                {
                    int registrationId = Convert.ToInt32(row.Cells["registration_id"].Value);
                    contextMenuActions.Items.Clear();
                    if (registrationStatus == "Pending")
                    {
                        ToolStripMenuItem unregisterItem = new ToolStripMenuItem("✗ Unregister");
                        unregisterItem.Font = new Font("Segoe UI", 10F);
                        unregisterItem.Click += (s, ev) => UnregisterFromEvent(registrationId, eventName);
                        contextMenuActions.Items.Add(unregisterItem);
                    }
                    else
                    {
                        ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                        viewQRItem.Font = new Font("Segoe UI", 10F);
                        viewQRItem.Click += (s, ev) => ViewQRCode(eventName, registrationId);
                        contextMenuActions.Items.Add(viewQRItem);
                    }

                    Rectangle rect = dgvBrowse.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    int buttonWidth = 70; int buttonHeight = 30;
                    int buttonX = rect.Left + (rect.Width - buttonWidth) / 2;
                    int buttonY = rect.Top + (rect.Height - buttonHeight) / 2;
                    Point pt = new Point(buttonX + buttonWidth + 5, buttonY);
                    contextMenuActions.Show(dgvBrowse, pt);
                }
                else
                {
                    RegisterForEvent(eventId, eventName, eventDate, eventVenue);
                }
            }
        }

        private void ViewQRCode(string eventName, int registrationId)
        {
            try
            {
                string query = @"SELECT r.qr_code, r.status, e.end_datetime 
                                FROM registrations r
                                INNER JOIN events e ON r.event_id = e.id
                                WHERE r.id=@id";
                MySqlParameter[] parameters = { new MySqlParameter("@id", registrationId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count == 0 || dt.Rows[0]["qr_code"] == DBNull.Value)
                {
                    MessageBox.Show("No QR code available for this event.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string qrCodeData = dt.Rows[0]["qr_code"].ToString();
                string registrationStatus = dt.Rows[0]["status"].ToString();
                DateTime eventEndDateTime = Convert.ToDateTime(dt.Rows[0]["end_datetime"]);

                // fully qualify namespace because frmQRCodeViewer lives in a different project namespace
                var qrViewer = new BarangayCogonEventManagementSystem.frmQRCodeViewer(eventName, qrCodeData, eventEndDateTime, registrationStatus);
                qrViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading QR code: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegisterForEvent(int eventId, string eventName, string eventDate, string eventVenue)
        {
            try
            {
                string selectedRole = ShowRoleSelectionDialog(eventName);
                if (string.IsNullOrEmpty(selectedRole)) return;

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
                            // Build a detailed list of conflicting events with formatted times and registration status
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            foreach (DataRow dr in dtConflicts.Rows)
                            {
                                DateTime cs = Convert.ToDateTime(dr["start_datetime"]);
                                DateTime ce = Convert.ToDateTime(dr["end_datetime"]);
                                string status = dr.Table.Columns.Contains("status") && dr["status"] != DBNull.Value ? dr["status"].ToString() : "";
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
                    // If time check fails, we allow registration to continue but log/show a non-fatal message
                    // (Keep behavior safe: not blocking registration due to time-check failure)
                    Console.WriteLine("Error checking event time conflicts: " + exTime.Message);
                }

                DialogResult confirmResult = MessageBox.Show(
                    $"Do you want to register for this event?\n\n" +
                    $"Event: {eventName}\n" +
                    $"Date: {eventDate}\n" +
                    $"Venue: {eventVenue}\n" +
                    $"Role: {selectedRole}\n\n" +
                    $"Your registration will be pending until approved by an administrator.",
                    "Confirm Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    string checkQuery = "SELECT id FROM registrations WHERE event_id=@event_id AND user_id=@user_id";
                    MySqlParameter[] checkParams = { new MySqlParameter("@event_id", eventId), new MySqlParameter("@user_id", userId) };
                    DataTable dtCheck = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);
                    if (dtCheck.Rows.Count > 0)
                    {
                        MessageBox.Show("You are already registered for this event.", "Already Registered", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadBrowseEvents();
                        return;
                    }

                    string insertQuery = @"INSERT INTO registrations (event_id, user_id, role, status, qr_code, created_at)
                                              VALUES (@event_id, @user_id, @role, 'Pending', NULL, NOW())";
                    MySqlParameter[] insertParams = { new MySqlParameter("@event_id", eventId), new MySqlParameter("@user_id", userId), new MySqlParameter("@role", selectedRole.ToLower()) };
                    int result = DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams);
                    if (result > 0)
                    {
                        MessageBox.Show($"Successfully registered for '{eventName}' as {selectedRole}!\n\nPlease wait for admin approval.", "Registered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBrowseEvents();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                if (mysqlEx.Number == 1062) { MessageBox.Show("You are already registered for this event.", "Duplicate Registration", MessageBoxButtons.OK, MessageBoxIcon.Warning); LoadBrowseEvents(); }
                else if (mysqlEx.Number == 1452) { MessageBox.Show("Event or user not found. Please refresh and try again.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else { MessageBox.Show($"Database error: {mysqlEx.Message}\n\nError Code: {mysqlEx.Number}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering for event: {ex.Message}\n\nPlease contact the administrator if this problem persists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UnregisterFromEvent(int registrationId, string eventName)
        {
            try
            {
                DialogResult confirmResult = MessageBox.Show($"Are you sure you want to unregister from this event?\n\nEvent: {eventName}\n\nThis action cannot be undone.", "Confirm Unregister", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    string query = "DELETE FROM registrations WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", registrationId) };
                    int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (result > 0) { MessageBox.Show($"Successfully unregistered from '{eventName}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadBrowseEvents(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unregistering: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void LoadBrowseEvents()
        {
            try
            {
                string query = @"SELECT 
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
                                WHERE e.start_datetime >= NOW()";

                if (cboTypeFilter.SelectedIndex > 0) { query += " AND e.type = @type"; }

                string searchText = txtSearch.Text;
                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search events...")
                {
                    query += @" AND (e.name LIKE @search OR e.venue LIKE @search OR e.type LIKE @search OR e.organizer LIKE @search)";
                }

                query += " ORDER BY e.start_datetime ASC";

                var paramsList = new System.Collections.Generic.List<MySqlParameter>();
                paramsList.Add(new MySqlParameter("@user_id", userId));
                if (cboTypeFilter.SelectedIndex > 0) { paramsList.Add(new MySqlParameter("@type", cboTypeFilter.SelectedItem.ToString())); }
                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search events...") { paramsList.Add(new MySqlParameter("@search", "%" + searchText + "%")); }

                DataTable dtEvents = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());

                dgvBrowse.Rows.Clear();

                if (dtEvents.Rows.Count == 0)
                {
                    int placeholderIndex = dgvBrowse.Rows.Add(0, null, 0, null, "No available events to register", "", "", "", "", "", DBNull.Value, "");
                    DataGridViewRow placeholderRow = dgvBrowse.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    foreach (DataRow dr in dtEvents.Rows)
                    {
                        dgvBrowse.Rows.Add(
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
                            ""
                        );
                    }
                }

                dgvBrowse.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

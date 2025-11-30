using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRCoder;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmMyEvents : Form
    {
        private int userId;
        private ContextMenuStrip contextMenuActions;
        private TextBox txtSearch;
        private ComboBox cboStatusFilter;

        public frmMyEvents(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.BackColor = Color.FromArgb(46, 51, 73); // Match main panel background
            InitializeContextMenu();
            InitializeFilters();
        }

        private void InitializeFilters()
        {
            // Search box
            txtSearch = new TextBox
            {
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.Text = "🔍 Search events...";
            txtSearch.ForeColor = Color.Gray;
            
            txtSearch.Enter += (s, ev) => {
                if (txtSearch.Text == "🔍 Search events...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.White;
                }
            };
            
            txtSearch.Leave += (s, ev) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Search events...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            txtSearch.TextChanged += (s, ev) => LoadMyEvents();

            // Status filter
            Label lblFilter = new Label
            {
                Text = "Status:",
                Location = new Point(340, 25),
                Size = new Size(60, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            cboStatusFilter = new ComboBox
            {
                Location = new Point(405, 20),
                Size = new Size(180, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatusFilter.Items.AddRange(new object[] { "All Status", "Pending", "Approved", "Checked-in", "Attended", "Rejected", "Didn't Attend" });
            cboStatusFilter.SelectedIndex = 0;
            cboStatusFilter.SelectedIndexChanged += (s, ev) => LoadMyEvents();

            this.Controls.Add(txtSearch);
            this.Controls.Add(lblFilter);
            this.Controls.Add(cboStatusFilter);

            // Adjust dgvMyEvents position
            if (dgvMyEvents != null)
            {
                dgvMyEvents.Location = new Point(20, 60);
                dgvMyEvents.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 80);
            }
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

        private void frmMyEvents_Load(object sender, EventArgs e)
        {
            CustomizeDataGridView();
            LoadMyEvents();
        }

        private void CustomizeDataGridView()
        {
            // Remove existing event handlers to prevent duplicates
            dgvMyEvents.CellPainting -= dgvMyEvents_CellPainting;
            dgvMyEvents.CellClick -= dgvMyEvents_CellClick;

            dgvMyEvents.Columns.Clear();
            dgvMyEvents.AllowUserToAddRows = false;
            dgvMyEvents.ReadOnly = true;

            // GENERAL GRID SETTINGS - Match user dashboard style
            dgvMyEvents.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvMyEvents.BorderStyle = BorderStyle.None;
            dgvMyEvents.GridColor = Color.FromArgb(60, 65, 90);
            dgvMyEvents.EnableHeadersVisualStyles = false;
            dgvMyEvents.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE
            dgvMyEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvMyEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvMyEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMyEvents.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvMyEvents.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvMyEvents.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvMyEvents.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvMyEvents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvMyEvents.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvMyEvents.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvMyEvents.DefaultCellStyle.ForeColor = Color.White;
            dgvMyEvents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvMyEvents.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvMyEvents.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvMyEvents.RowTemplate.Height = 55;
            dgvMyEvents.RowHeadersVisible = false;
            dgvMyEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - SAME color as default cells for consistency
            dgvMyEvents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvMyEvents.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvMyEvents.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvMyEvents.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvMyEvents, new object[] { true });

            // Add columns
            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "registration_id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_id",
                HeaderText = "Event ID",
                ReadOnly = true,
                Visible = false
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_name",
                HeaderText = "Event",
                ReadOnly = true,
                FillWeight = 25
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_date",
                HeaderText = "Event Date",
                ReadOnly = true,
                FillWeight = 17
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_time",
                HeaderText = "Event Schedule",
                ReadOnly = true,
                FillWeight = 17
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_venue",
                HeaderText = "Venue",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_type",
                HeaderText = "Type",
                ReadOnly = true,
                FillWeight = 13
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "role",
                HeaderText = "Role",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "rejection_reason",
                HeaderText = "Rejection Reason",
                ReadOnly = true,
                Visible = false
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_description",
                HeaderText = "Description",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_end_datetime",
                HeaderText = "Event End",
                ReadOnly = true,
                Visible = false
            });

            dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 13
            });

            // Wire up event handlers
            dgvMyEvents.CellPainting += dgvMyEvents_CellPainting;
            dgvMyEvents.CellClick += dgvMyEvents_CellClick;
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

        private void dgvMyEvents_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return; // Skip header

            var actionColumn = dgvMyEvents.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                // Paint all parts except content to ensure consistent borders
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Check if this is a placeholder row
                var eventIdValue = dgvMyEvents.Rows[e.RowIndex].Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0)
                {
                    e.Handled = true;
                    return;
                }

                // Check if status is rejected
                DataGridViewRow row = dgvMyEvents.Rows[e.RowIndex];
                
                string status = row.Cells["status"].Value?.ToString();
                bool isRejected = status == "Rejected";

                Rectangle cellBounds = e.CellBounds;

                if (isRejected)
                {
                    // Draw info icon for rejected registrations
                    int iconSize = 24;
                    int iconX = cellBounds.X + (cellBounds.Width - iconSize) / 2;
                    int iconY = cellBounds.Y + (cellBounds.Height - iconSize) / 2;
                    Rectangle iconRect = new Rectangle(iconX, iconY, iconSize, iconSize);

                    using (SolidBrush iconBrush = new SolidBrush(Color.FromArgb(244, 67, 54))) // Red color
                    using (Font iconFont = new Font("Segoe UI", 14F, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        
                        // Draw circle background
                        e.Graphics.FillEllipse(iconBrush, iconRect);
                        
                        // Draw 'i' text in white
                        using (SolidBrush textBrush = new SolidBrush(Color.White))
                        {
                            e.Graphics.DrawString("i", iconFont, textBrush, iconRect, sf);
                        }
                    }
                }
                else
                {
                    // Draw action button for active registrations
                    int buttonWidth = 70;
                    int buttonHeight = 30;
                    int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                    int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;
                    Rectangle buttonRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                    int radius = 10;

                    using (GraphicsPath path = GetRoundPath(buttonRect, radius))
                    using (SolidBrush buttonBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    using (Font btnFont = new Font("Segoe UI", 12F, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.FillPath(buttonBrush, path);
                        e.Graphics.DrawString("...", btnFont, textBrush, buttonRect, sf);
                    }
                }

                e.Handled = true;
            }
        }

        private void dgvMyEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvMyEvents.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                DataGridViewRow row = dgvMyEvents.Rows[e.RowIndex];

                var eventIdValue = row.Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0)
                {
                    return;
                }

                string status = row.Cells["status"].Value?.ToString();
                bool isRejected = status == "Rejected";

                if (isRejected)
                {
                    // Show rejection reason popup
                    string rejectionReason = row.Cells["rejection_reason"].Value?.ToString();
                    string eventName = row.Cells["event_name"].Value?.ToString();
                    ShowRejectionReasonPopup(eventName, rejectionReason);
                    return;
                }

                int registrationId = Convert.ToInt32(row.Cells["registration_id"].Value);
                int eventId = Convert.ToInt32(row.Cells["event_id"].Value);
                string eventName2 = row.Cells["event_name"].Value?.ToString();

                // Clear existing menu items
                contextMenuActions.Items.Clear();

                // Add menu items based on status
                if (status == "Pending")
                {
                    // Show Unregister for pending registrations
                    ToolStripMenuItem unregisterItem = new ToolStripMenuItem("✗ Unregister");
                    unregisterItem.Font = new Font("Segoe UI", 10F);
                    unregisterItem.Click += (s, ev) => UnregisterFromEvent(registrationId, eventName2);
                    contextMenuActions.Items.Add(unregisterItem);
                }
                else
                {
                    // Show View QR for approved/checked-in/attended registrations
                    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                    viewQRItem.Font = new Font("Segoe UI", 10F);
                    viewQRItem.Click += (s, ev) => ViewQRCode(eventName2, registrationId);
                    contextMenuActions.Items.Add(viewQRItem);
                }

                 // Get cell rectangle
                 Rectangle rect = dgvMyEvents.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                // Calculate the button position (centered in cell, same as in CellPainting)
                int buttonWidth = 70;
                int buttonHeight = 30;
                int buttonX = rect.Left + (rect.Width - buttonWidth) / 2;
                int buttonY = rect.Top + (rect.Height - buttonHeight) / 2;

                // Position context menu just below and to the right of the button
                Point pt = new Point(buttonX + buttonWidth + 5, buttonY);

                // Show the context menu right next to the action button
                contextMenuActions.Show(dgvMyEvents, pt);
            }
        }

        private void ShowRejectionReasonPopup(string eventName, string rejectionReason)
        {
            // Create a custom dialog to display rejection reason
            Form reasonForm = new Form
            {
                Text = "Rejection Reason",
                Size = new Size(450, 360),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(46, 51, 73)
            };

            Label lblTitle = new Label
            {
                Text = "Your registration was rejected",
                Location = new Point(20, 20),
                Size = new Size(410, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(244, 67, 54), // Red color
                BackColor = Color.Transparent
            };

            Label lblEvent = new Label
            {
                Text = $"Event: {eventName}",
                Location = new Point(20, 55),
                Size = new Size(410, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(158, 161, 178),
                BackColor = Color.Transparent
            };

            Label lblReasonLabel = new Label
            {
                Text = "Reason:",
                Location = new Point(20, 85),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            TextBox txtReason = new TextBox
            {
                Location = new Point(20, 110),
                Size = new Size(410, 80),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Text = string.IsNullOrEmpty(rejectionReason) ? "No reason provided." : rejectionReason
            };

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(175, 205),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(60, 65, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                DialogResult = DialogResult.OK,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;

            // Add Paint event for rounded button
            btnClose.Paint += (s, e) =>
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

            reasonForm.Controls.Add(lblTitle);
            reasonForm.Controls.Add(lblEvent);
            reasonForm.Controls.Add(lblReasonLabel);
            reasonForm.Controls.Add(txtReason);
            reasonForm.Controls.Add(btnClose);

            reasonForm.AcceptButton = btnClose;

            reasonForm.ShowDialog();
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
                        LoadMyEvents();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unregistering: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegisterForEvent(int eventId, string eventName)
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

                // Show confirmation dialog before re-registering
                DialogResult confirmResult = MessageBox.Show(
                    $"Do you want to re-register for this event?\n\n" +
                    $"Event: {eventName}\n" +
                    $"Role: {selectedRole}\n\n" +
                    $"Your registration will be pending until approved by an administrator.",
                    "Confirm Re-Registration",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // First, delete the rejected registration
                        string deleteQuery = "DELETE FROM registrations WHERE event_id=@event_id AND user_id=@user_id";
                        MySqlParameter[] deleteParams = {
                            new MySqlParameter("@event_id", eventId),
                            new MySqlParameter("@user_id", userId)
                        };
                        DatabaseHelper.ExecuteNonQuery(deleteQuery, deleteParams);

                        // Before inserting, check for time conflicts (hours and minutes only)
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
                                        $"Warning: The event you're trying to register has the same start and end time (hours and minutes) as one or more of your registered events:\n\n{sb.ToString()}\nDo you still want to continue?",
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

                        // Insert new registration with qr_code explicitly set to NULL
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
                            MessageBox.Show($"Successfully re-registered for '{eventName}' as {selectedRole}!\n\nPlease wait for admin approval.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadMyEvents();
                        }
                        else
                        {
                            MessageBox.Show("Re-registration failed. Please try again.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (MySqlException mysqlEx)
                    {
                        MessageBox.Show($"Database error: {mysqlEx.Message}\n\nError Code: {mysqlEx.Number}", 
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error registering: {ex.Message}\n\nPlease contact the administrator if this problem persists.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering: " + ex.Message,
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

        private void LoadMyEvents()
        {
            try
            {
                // Build base query
                string query = @"SELECT 
                                    r.id AS registration_id,
                                    e.id AS event_id,
                                    e.name AS event_name,
                                    CASE 
                                        WHEN DATE(e.start_datetime) = DATE(e.end_datetime) THEN DATE_FORMAT(e.start_datetime, '%b %d, %Y')
                                        ELSE CONCAT(DATE_FORMAT(e.start_datetime, '%b %d'), ' - ', DATE_FORMAT(e.end_datetime, '%b %d, %Y'))
                                    END AS event_date,
                                    CONCAT(DATE_FORMAT(e.start_datetime, '%h:%i %p'), ' - ', DATE_FORMAT(e.end_datetime, '%h:%i %p')) AS event_time,
                                    e.venue AS event_venue,
                                    e.type AS event_type,
                                    r.role,
                                    r.status,
                                    r.rejection_reason,
                                    r.qr_code,
                                    e.end_datetime,
                                    e.description AS event_description
                                FROM registrations r
                                INNER JOIN events e ON r.event_id = e.id
                                WHERE r.user_id = @user_id";

                var paramsList = new System.Collections.Generic.List<MySqlParameter>();
                paramsList.Add(new MySqlParameter("@user_id", userId));

                // Add status filter
                if (cboStatusFilter != null && cboStatusFilter.SelectedIndex > 0)
                {
                    query += " AND r.status = @status";
                    paramsList.Add(new MySqlParameter("@status", cboStatusFilter.SelectedItem.ToString()));
                }

                // Add search filter
                if (txtSearch != null)
                {
                    string searchText = txtSearch.Text;
                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search events...")
                    {
                        query += @" AND (e.name LIKE @search 
                                    OR e.venue LIKE @search 
                                    OR e.type LIKE @search
                                    OR r.role LIKE @search)";
                        paramsList.Add(new MySqlParameter("@search", "%" + searchText + "%"));
                    }
                }

                query += " ORDER BY e.start_datetime DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());

                // Clear existing rows
                dgvMyEvents.Rows.Clear();

                if (dt.Rows.Count == 0)
                {
                    // Add placeholder row - match the column count (13 columns total)
                    int placeholderIndex = dgvMyEvents.Rows.Add(
                        0, // registration_id
                        0, // event_id
                        "No events found matching your criteria", // event_name
                        "", // event_date
                        "", // event_time
                        "", // event_venue
                        "", // event_type
                        "", // role
                        "", // status
                        "", // rejection_reason
                        "", // event_description
                        DBNull.Value, // event_end_datetime
                        "" // ActionColumn
                    );

                    DataGridViewRow placeholderRow = dgvMyEvents.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string role = dr["role"].ToString();
                        string capitalizedRole = string.IsNullOrEmpty(role) ? role :
                            char.ToUpper(role[0]) + role.Substring(1).ToLower();

                        dgvMyEvents.Rows.Add(
                            dr["registration_id"],
                            dr["event_id"],
                            dr["event_name"],
                            dr["event_date"],
                            dr["event_time"],
                            dr["event_venue"],
                            dr["event_type"],
                            capitalizedRole,
                            dr["status"],
                            dr["rejection_reason"],
                            dr["event_description"],
                            dr["end_datetime"], // This will be properly converted to DateTime
                            "" // ActionColumn (will be custom painted)
                        );
                    }
                }

                dgvMyEvents.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

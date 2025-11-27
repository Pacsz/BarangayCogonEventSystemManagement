using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRCoder;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmEventAttendees : Form
    {
        private int eventId;
        private string eventName;
        private DataGridView dgvAttendees;
        private ContextMenuStrip contextMenuActions;
        private TextBox txtSearch;
        private ComboBox cboStatusFilter;
        private ComboBox cboRoleFilter;

        public frmEventAttendees(int eventId, string eventName)
        {
            InitializeComponent();
            this.eventId = eventId;
            this.eventName = eventName;
            this.Text = $"Attendees - {eventName}";
            this.Size = new Size(1000, 630);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(46, 51, 73);
            
            InitializeContextMenu();
            InitializeControls();
            LoadAttendees();
        }

        private void InitializeContextMenu()
        {
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

        private void InitializeControls()
        {
            // Title label
            Label lblTitle = new Label
            {
                Text = $"Attendees for: {eventName}",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(950, 35),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblTitle);

            // Search box
            txtSearch = new TextBox
            {
                Location = new Point(20, 65),
                Size = new Size(280, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.Text = "🔍 Search attendees...";
            txtSearch.ForeColor = Color.Gray;
            
            txtSearch.Enter += (s, ev) => {
                if (txtSearch.Text == "🔍 Search attendees...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.White;
                }
            };
            
            txtSearch.Leave += (s, ev) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Search attendees...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            txtSearch.TextChanged += (s, ev) => LoadAttendees();
            this.Controls.Add(txtSearch);

            // Status filter
            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(320, 70),
                Size = new Size(60, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(lblStatus);

            cboStatusFilter = new ComboBox
            {
                Location = new Point(385, 65),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatusFilter.Items.AddRange(new object[] { "All Status", "Pending", "Approved", "Checked-in", "Attended", "Rejected", "Didn't Attend" });
            cboStatusFilter.SelectedIndex = 0;
            cboStatusFilter.SelectedIndexChanged += (s, ev) => LoadAttendees();
            this.Controls.Add(cboStatusFilter);

            // Role filter
            Label lblRole = new Label
            {
                Text = "Role:",
                Location = new Point(555, 70),
                Size = new Size(50, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(lblRole);

            cboRoleFilter = new ComboBox
            {
                Location = new Point(610, 65),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboRoleFilter.Items.AddRange(new object[] { "All Roles", "Attendee", "Volunteer", "Speaker" });
            cboRoleFilter.SelectedIndex = 0;
            cboRoleFilter.SelectedIndexChanged += (s, ev) => LoadAttendees();
            this.Controls.Add(cboRoleFilter);

            // DataGridView for attendees
            dgvAttendees = new DataGridView
            {
                Location = new Point(20, 110),
                Size = new Size(950, 410),
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
            this.Controls.Add(dgvAttendees);

            // Close button
            Button btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 40),
                Location = new Point(850, 530),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            
            // Add rounded corners to close button
            btnClose.Paint += (s, e) =>
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
            
            this.Controls.Add(btnClose);
        }

        private void CustomizeDataGridView()
        {
            // Remove existing event handlers to prevent duplicates
            dgvAttendees.CellPainting -= dgvAttendees_CellPainting;
            dgvAttendees.CellClick -= dgvAttendees_CellClick;

            dgvAttendees.Columns.Clear();

            // HEADER STYLE
            dgvAttendees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvAttendees.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvAttendees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAttendees.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvAttendees.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvAttendees.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvAttendees.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvAttendees.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvAttendees.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvAttendees.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvAttendees.DefaultCellStyle.ForeColor = Color.White;
            dgvAttendees.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvAttendees.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvAttendees.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvAttendees.RowTemplate.Height = 60;

            // Alternating rows
            dgvAttendees.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvAttendees.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvAttendees.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvAttendees.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvAttendees, new object[] { true });

            // Add columns
            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "registration_id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "full_name",
                HeaderText = "Full Name",
                ReadOnly = true,
                FillWeight = 30
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "email",
                HeaderText = "Email",
                ReadOnly = true,
                FillWeight = 25
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "contact",
                HeaderText = "Contact",
                ReadOnly = true,
                FillWeight = 18
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "role",
                HeaderText = "Role",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "qr_code",
                HeaderText = "QR Code",
                ReadOnly = true,
                Visible = false
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_end_datetime",
                HeaderText = "Event End",
                ReadOnly = true,
                Visible = false
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 15
            });

            // Wire up event handlers
            dgvAttendees.CellPainting += dgvAttendees_CellPainting;
            dgvAttendees.CellClick += dgvAttendees_CellClick;
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

        private void dgvAttendees_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var actionColumn = dgvAttendees.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Check if this is a placeholder row
                var regIdValue = dgvAttendees.Rows[e.RowIndex].Cells["registration_id"].Value;
                if (regIdValue == null || Convert.ToInt32(regIdValue) == 0)
                {
                    e.Handled = true;
                    return;
                }

                Rectangle cellBounds = e.CellBounds;

                // Draw action button for all events (ongoing, upcoming, or ended)
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

                e.Handled = true;
            }
        }

        private void dgvAttendees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvAttendees.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                DataGridViewRow row = dgvAttendees.Rows[e.RowIndex];

                var regIdValue = row.Cells["registration_id"].Value;
                if (regIdValue == null || Convert.ToInt32(regIdValue) == 0)
                {
                    return;
                }

                string status = row.Cells["status"].Value?.ToString();
                int registrationId = Convert.ToInt32(row.Cells["registration_id"].Value);
                string fullName = row.Cells["full_name"].Value?.ToString();
                string qrCodeData = row.Cells["qr_code"].Value?.ToString();

                // Clear existing menu items
                contextMenuActions.Items.Clear();

                // Add menu items based on status
                if (status == "Pending")
                {
                    // Show Approve and Reject for pending registrations
                    ToolStripMenuItem approveItem = new ToolStripMenuItem("✓ Approve");
                    approveItem.Font = new Font("Segoe UI", 10F);
                    approveItem.Click += (s, ev) => ApproveRegistration(registrationId, fullName);
                    contextMenuActions.Items.Add(approveItem);

                    ToolStripMenuItem rejectItem = new ToolStripMenuItem("✗ Reject");
                    rejectItem.Font = new Font("Segoe UI", 10F);
                    rejectItem.Click += (s, ev) => RejectRegistration(registrationId);
                    contextMenuActions.Items.Add(rejectItem);
                }
                else if (status == "Rejected")
                {
                    // Show Approve for rejected registrations (allow re-approval)
                    ToolStripMenuItem approveItem = new ToolStripMenuItem("✓ Approve");
                    approveItem.Font = new Font("Segoe UI", 10F);
                    approveItem.Click += (s, ev) => ApproveRegistration(registrationId, fullName);
                    contextMenuActions.Items.Add(approveItem);
                }
                else
                {
                    // For all other statuses (Approved, Checked-in, Attended, Didn't Attend)
                    // Show View QR option
                    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                    viewQRItem.Font = new Font("Segoe UI", 10F);
                    viewQRItem.Click += (s, ev) => ViewQRCode(fullName, qrCodeData);
                    contextMenuActions.Items.Add(viewQRItem);

                    // For Approved status only, also show option to reject
                    if (status == "Approved")
                    {
                        ToolStripMenuItem rejectItem = new ToolStripMenuItem("✗ Reject");
                        rejectItem.Font = new Font("Segoe UI", 10F);
                        rejectItem.Click += (s, ev) => RejectRegistration(registrationId);
                        contextMenuActions.Items.Add(rejectItem);
                    }
                }

                // Get cell rectangle
                Rectangle rect = dgvAttendees.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                
                // Calculate the button position (centered in cell, same as in CellPainting)
                int buttonWidth = 70;
                int buttonHeight = 30;
                int buttonX = rect.Left + (rect.Width - buttonWidth) / 2;
                int buttonY = rect.Top + (rect.Height - buttonHeight) / 2;
                
                // Position context menu just below and to the right of the button
                Point pt = new Point(buttonX + buttonWidth + 5, buttonY);
                
                // Show the context menu right next to the action button
                contextMenuActions.Show(dgvAttendees, pt);
            }
        }

        private void ApproveRegistration(int registrationId, string userName)
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
                        MessageBox.Show($"Registration approved!", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAttendees();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error approving registration: " + ex.Message, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RejectRegistration(int registrationId)
        {
            try
            {
                // First, get the registration details including event name and user name for QR file deletion
                string getDetailsQuery = @"SELECT e.name AS event_name, 
                                                 CONCAT(u.first_name, ' ', u.last_name) AS user_name,
                                                 r.qr_code
                                         FROM registrations r
                                         INNER JOIN events e ON r.event_id = e.id
                                         INNER JOIN users u ON r.user_id = u.id
                                         WHERE r.id = @id";
                MySqlParameter[] getDetailsParams = { new MySqlParameter("@id", registrationId) };
                DataTable dtDetails = DatabaseHelper.ExecuteQuery(getDetailsQuery, getDetailsParams);

                if (dtDetails.Rows.Count == 0)
                {
                    MessageBox.Show("Registration not found.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string eventName = dtDetails.Rows[0]["event_name"].ToString();
                string userName = dtDetails.Rows[0]["user_name"].ToString();
                string qrCode = dtDetails.Rows[0]["qr_code"]?.ToString();

                DialogResult result = MessageBox.Show("Are you sure you want to reject this registration?", 
                    "Confirm Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Delete QR code file if it exists
                    if (!string.IsNullOrEmpty(qrCode))
                    {
                        try
                        {
                            string fileName = $"{eventName}_{userName}.png".Replace(" ", "_");
                            string folderPath = Path.Combine(Application.StartupPath, "Assets", "QR_Codes");
                            string fullPath = Path.Combine(folderPath, fileName);

                            if (File.Exists(fullPath))
                            {
                                File.Delete(fullPath);
                                System.Diagnostics.Debug.WriteLine($"Deleted QR code file: {fullPath}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error deleting QR code file: {ex.Message}");
                            // Continue with rejection even if file deletion fails
                        }
                    }

                    // Update database - set status to Rejected and qr_code to NULL
                    string query = "UPDATE registrations SET status='Rejected', qr_code=NULL WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", registrationId) };

                    int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registration rejected.", "Information", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAttendees();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rejecting registration: " + ex.Message, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewQRCode(string userName, string qrCodeData)
        {
            try
            {
                if (string.IsNullOrEmpty(qrCodeData))
                {
                    MessageBox.Show("No QR code available for this registration.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Get event end datetime and registration status for notice
                string eventQuery = @"SELECT e.end_datetime, r.status 
                                     FROM events e
                                     INNER JOIN registrations r ON r.event_id = e.id
                                     WHERE e.id = @event_id AND r.qr_code = @qr_code";
                MySqlParameter[] eventParams = { 
                    new MySqlParameter("@event_id", eventId),
                    new MySqlParameter("@qr_code", qrCodeData)
                };
                DataTable eventDt = DatabaseHelper.ExecuteQuery(eventQuery, eventParams);
                
                DateTime eventEndDateTime = DateTime.Now;
                DateTime currentTime = DateTime.Now;
                const int GRACE_PERIOD_HOURS = 2;
                bool isEventEnded = false;
                bool isGracePeriodExpired = false;
                string registrationStatus = "Approved";
                
                if (eventDt.Rows.Count > 0)
                {
                    eventEndDateTime = Convert.ToDateTime(eventDt.Rows[0]["end_datetime"]);
                    registrationStatus = eventDt.Rows[0]["status"].ToString();
                    DateTime attendanceDeadline = eventEndDateTime.AddHours(GRACE_PERIOD_HOURS);
                    isEventEnded = currentTime > eventEndDateTime;
                    isGracePeriodExpired = currentTime > attendanceDeadline;
                }

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrData = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q))
                using (QRCode qrCode = new QRCode(qrData))
                using (Bitmap qrImage = qrCode.GetGraphic(6))
                {
                    Form qrForm = new Form
                    {
                        Text = $"QR Code - {userName}",
                        Size = new Size(400, 540),
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        MaximizeBox = false,
                        MinimizeBox = false,
                        BackColor = Color.FromArgb(46, 51, 73)
                    };

                    // Event and User info header
                    Label lblInfo = new Label
                    {
                        Text = $"Event: {eventName}\nUser: {userName}",
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(20, 20),
                        Size = new Size(360, 50),
                        BackColor = Color.Transparent,
                        ForeColor = Color.White
                    };

                    // QR Code picture box
                    PictureBox picQR = new PictureBox
                    {
                        Image = (Bitmap)qrImage.Clone(),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = new Point(90, 80),
                        Size = new Size(220, 220),
                        BackColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    // Notice label (with event status information)
                    Label lblNotice = new Label
                    {
                        Font = new Font("Segoe UI", 9F),
                        ForeColor = Color.FromArgb(158, 161, 178),
                        Location = new Point(20, 315),
                        Size = new Size(360, 60),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.Transparent
                    };

                    if (registrationStatus == "Attended")
                    {
                        // User has completed attendance - show admin confirmation message
                        lblNotice.Text = "✅ Attendance completed for this user\n" +
                                       "Both check-in and check-out have been recorded\n" +
                                       "User successfully attended this event";
                        lblNotice.ForeColor = Color.FromArgb(76, 175, 80); // Green color
                        lblNotice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }
                    else if (registrationStatus == "Checked-in")
                    {
                        // User has checked in but not checked out yet
                        if (!isEventEnded)
                        {
                            // Event is still ongoing
                            lblNotice.Text = "✅ User has checked in successfully\n" +
                                           "Event is still in progress\n" +
                                           "Check-out will be available after event ends";
                            lblNotice.ForeColor = Color.FromArgb(76, 175, 80); // Green color
                            lblNotice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        }
                        else if (!isGracePeriodExpired)
                        {
                            // Event has ended but within check-out grace period
                            DateTime checkoutDeadline = eventEndDateTime.AddHours(GRACE_PERIOD_HOURS);
                            TimeSpan timeRemaining = checkoutDeadline - DateTime.Now;
                            string gracePeriodInfo = "";
                            
                            if (timeRemaining.TotalHours >= 1)
                            {
                                int hours = (int)timeRemaining.TotalHours;
                                int minutes = timeRemaining.Minutes;
                                gracePeriodInfo = $"{hours}h {minutes}m";
                            }
                            else
                            {
                                int minutes = (int)timeRemaining.TotalMinutes;
                                gracePeriodInfo = $"{minutes} minute{(minutes > 1 ? "s" : "")}";
                            }
                            
                            lblNotice.Text = $"⏰ User checked in - Awaiting check-out\n" +
                                           $"Check-out grace period: {gracePeriodInfo} remaining\n" +
                                           $"User can still check out to complete attendance";
                            lblNotice.ForeColor = Color.FromArgb(255, 193, 7); // Yellow/amber color
                            lblNotice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        }
                        else
                        {
                            // Check-out grace period has expired
                            lblNotice.Text = "⚠️ Check-out period closed\n" +
                                           "User checked in but didn't check out in time\n" +
                                           $"Grace period expired {GRACE_PERIOD_HOURS} hours after event";
                            lblNotice.ForeColor = Color.FromArgb(255, 152, 0); // Orange color
                            lblNotice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        }
                    }
                    else if (isEventEnded)
                    {
                        // Event has ended and user hasn't checked in
                        lblNotice.Text = "❌ Event has ended - Check-in closed\n" +
                                       "QR code no longer valid for check-in\n" +
                                       "Check-in only allowed during event period";
                        lblNotice.ForeColor = Color.FromArgb(211, 47, 47); // Red color
                        lblNotice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }
                    else
                    {
                        // Event is ongoing or upcoming - QR is valid
                        lblNotice.Text = "This QR code is for attendance verification\n" +
                                       "at the event.\n" +
                                       "Check-in available during event period";
                    }

                    // Close button with rounded corners
                    Button btnClose = new Button
                    {
                        Text = "Close",
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        BackColor = Color.Gray,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Size = new Size(200, 45),
                        Location = new Point(100, 395),
                        Cursor = Cursors.Hand
                    };
                    btnClose.FlatAppearance.BorderSize = 0;

                    // Add rounded corners to button
                    btnClose.Paint += (s, e) =>
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

                    btnClose.Click += (s, e) => qrForm.Close();

                    qrForm.Controls.Add(lblInfo);
                    qrForm.Controls.Add(picQR);
                    qrForm.Controls.Add(lblNotice);
                    qrForm.Controls.Add(btnClose);
                    qrForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing QR code: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAttendees()
        {
            try
            {
                string query = @"SELECT 
                                    r.id AS registration_id,
                                    CONCAT(u.first_name, ' ', u.last_name) AS full_name,
                                    u.email,
                                    u.contact_number AS contact,
                                    r.role,
                                    r.status,
                                    r.qr_code,
                                    e.end_datetime,
                                    a.check_in_time,
                                    a.check_out_time
                                FROM registrations r
                                INNER JOIN users u ON r.user_id = u.id
                                INNER JOIN events e ON r.event_id = e.id
                                LEFT JOIN attendance a ON a.registration_id = r.id
                                WHERE r.event_id = @event_id";

                var paramsList = new System.Collections.Generic.List<MySqlParameter>();
                paramsList.Add(new MySqlParameter("@event_id", eventId));

                // Add status filter
                if (cboStatusFilter != null && cboStatusFilter.SelectedIndex > 0)
                {
                    query += " AND r.status = @status";
                    paramsList.Add(new MySqlParameter("@status", cboStatusFilter.SelectedItem.ToString()));
                }

                // Add role filter
                if (cboRoleFilter != null && cboRoleFilter.SelectedIndex > 0)
                {
                    query += " AND r.role = @role";
                    paramsList.Add(new MySqlParameter("@role", cboRoleFilter.SelectedItem.ToString().ToLower()));
                }

                // Add search filter
                if (txtSearch != null)
                {
                    string searchText = txtSearch.Text;
                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search attendees...")
                    {
                        query += @" AND (u.first_name LIKE @search 
                                    OR u.last_name LIKE @search 
                                    OR u.email LIKE @search
                                    OR u.contact_number LIKE @search)";
                        paramsList.Add(new MySqlParameter("@search", "%" + searchText + "%"));
                    }
                }

                query += " ORDER BY r.status DESC, u.first_name ASC, u.last_name ASC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());

                dgvAttendees.Rows.Clear();

                if (dt.Rows.Count == 0)
                {
                    int placeholderIndex = dgvAttendees.Rows.Add(
                        0, "No registrations found matching your criteria", "", "", "", "", "", ""
                    );

                    DataGridViewRow placeholderRow = dgvAttendees.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int registrationId = Convert.ToInt32(dr["registration_id"]);
                        string role = dr["role"].ToString();
                        string capitalizedRole = string.IsNullOrEmpty(role) ? role :
                            char.ToUpper(role[0]) + role.Substring(1).ToLower();

                        string currentStatus = dr["status"].ToString();
                        
                        // Determine the new status based on check_in_time and check_out_time
                        string newStatus;
                        bool hasCheckIn = dr["check_in_time"] != DBNull.Value;
                        bool hasCheckOut = dr["check_out_time"] != DBNull.Value;
                        DateTime eventEndDateTime = Convert.ToDateTime(dr["end_datetime"]);
                        bool eventHasEnded = DateTime.Now > eventEndDateTime;

                        if (hasCheckIn && hasCheckOut)
                        {
                            // Both check-in and check-out recorded
                            newStatus = "Attended";
                        }
                        else if (hasCheckIn && !hasCheckOut)
                        {
                            // Only check-in recorded
                            newStatus = "Checked-in";
                        }
                        else if (!hasCheckIn && !hasCheckOut)
                        {
                            // No attendance record
                            if (eventHasEnded && currentStatus == "Approved")
                            {
                                // Event has ended and no attendance
                                newStatus = "Didn't Attend";
                            }
                            else
                            {
                                // Keep current status (Pending, Approved, Rejected)
                                newStatus = currentStatus;
                            }
                        }
                        else
                        {
                            // This shouldn't happen (check-out without check-in), but keep current status
                            newStatus = currentStatus;
                        }

                        // Update the status in the database if it has changed
                        if (newStatus != currentStatus)
                        {
                            UpdateRegistrationStatus(registrationId, newStatus);
                        }

                        dgvAttendees.Rows.Add(
                            dr["registration_id"],
                            dr["full_name"],
                            dr["email"],
                            dr["contact"],
                            capitalizedRole,
                            newStatus,
                            dr["qr_code"],
                            dr["end_datetime"],
                            "" // ActionColumn (will be custom painted)
                        );
                    }
                }

                dgvAttendees.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading attendees: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRegistrationStatus(int registrationId, string newStatus)
        {
            try
            {
                string updateQuery = "UPDATE registrations SET status = @status WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@status", newStatus),
                    new MySqlParameter("@id", registrationId)
                };
                DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
            }
            catch (Exception ex)
            {
                // Log error silently, don't interrupt the UI
                System.Diagnostics.Debug.WriteLine($"Error updating status for registration {registrationId}: {ex.Message}");
            }
        }

        private void ApplyFilters()
        {
            try
            {
                string filterText = txtSearch.Text.Trim().ToLower();
                string selectedStatus = cboStatusFilter.SelectedItem?.ToString() ?? "All";
                string selectedRole = cboRoleFilter.SelectedItem?.ToString() ?? "All";

                foreach (DataGridViewRow row in dgvAttendees.Rows)
                {
                    // Skip placeholder row
                    if (row.Cells["registration_id"].Value == null)
                        continue;

                    bool visible = true;

                    // Filter by search text (name or email)
                    if (!string.IsNullOrEmpty(filterText))
                    {
                        string fullName = row.Cells["full_name"].Value?.ToString().ToLower() ?? "";
                        string email = row.Cells["email"].Value?.ToString().ToLower() ?? "";
                        visible = fullName.Contains(filterText) || email.Contains(filterText);
                    }

                    // Filter by status
                    if (visible && selectedStatus != "All")
                    {
                        string status = row.Cells["status"].Value?.ToString() ?? "";
                        visible = status == selectedStatus;
                    }

                    // Filter by role
                    if (visible && selectedRole != "All")
                    {
                        string role = row.Cells["role"].Value?.ToString() ?? "";
                        visible = role.Equals(selectedRole, StringComparison.OrdinalIgnoreCase);
                    }

                    row.Visible = visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying filters: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

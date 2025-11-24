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
    public partial class frmRegistrations : Form
    {
        private TextBox txtSearch;
        private ComboBox cboStatusFilter;
        private ComboBox cboRoleFilter;

        public frmRegistrations()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(46, 51, 73); // Match main panel background
            InitializeContextMenuStyling();
            InitializeFilters();
            CustomizeDataGridView();
            LoadPendingRegistrations();
        }

        private void InitializeContextMenuStyling()
        {
            // Apply dark theme styling to context menu
            contextMenuActions.BackColor = Color.FromArgb(37, 42, 64);
            contextMenuActions.ForeColor = Color.White;
            contextMenuActions.ShowImageMargin = false;  // Remove the left white margin
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

        private void InitializeFilters()
        {
            // Search box
            txtSearch = new TextBox
            {
                Location = new Point(20, 20),
                Size = new Size(280, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.Text = "🔍 Search registrations...";
            txtSearch.ForeColor = Color.Gray;
            
            txtSearch.Enter += (s, ev) => {
                if (txtSearch.Text == "🔍 Search registrations...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.White;
                }
            };
            
            txtSearch.Leave += (s, ev) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Search registrations...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            txtSearch.TextChanged += (s, ev) => LoadPendingRegistrations();

            // Status filter
            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(320, 25),
                Size = new Size(60, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            cboStatusFilter = new ComboBox
            {
                Location = new Point(385, 20),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatusFilter.Items.AddRange(new object[] { "All Status", "Pending", "Approved", "Checked-in", "Attended", "Rejected", "Didn't Attend" });
            cboStatusFilter.SelectedIndex = 0;
            cboStatusFilter.SelectedIndexChanged += (s, ev) => LoadPendingRegistrations();

            // Role filter
            Label lblRole = new Label
            {
                Text = "Role:",
                Location = new Point(555, 25),
                Size = new Size(50, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            cboRoleFilter = new ComboBox
            {
                Location = new Point(610, 20),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboRoleFilter.Items.AddRange(new object[] { "All Roles", "Attendee", "Volunteer", "Speaker" });
            cboRoleFilter.SelectedIndex = 0;
            cboRoleFilter.SelectedIndexChanged += (s, ev) => LoadPendingRegistrations();

            this.Controls.Add(txtSearch);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cboStatusFilter);
            this.Controls.Add(lblRole);
            this.Controls.Add(cboRoleFilter);

            // Adjust dgvRegistrations position
            if (dgvRegistrations != null)
            {
                dgvRegistrations.Location = new Point(20, 60);
                dgvRegistrations.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 80);
            }
        }

        private void CustomizeDataGridView()
        {
            // Remove existing event handlers to prevent duplicates
            dgvRegistrations.CellPainting -= dgvRegistrations_CellPainting;
            dgvRegistrations.CellClick -= dgvRegistrations_CellClick;

            dgvRegistrations.Columns.Clear();
            dgvRegistrations.AllowUserToAddRows = false;
            dgvRegistrations.ReadOnly = true;

            // GENERAL GRID SETTINGS - Match user dashboard style
            dgvRegistrations.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvRegistrations.BorderStyle = BorderStyle.None;
            dgvRegistrations.GridColor = Color.FromArgb(60, 65, 90);
            dgvRegistrations.EnableHeadersVisualStyles = false;
            dgvRegistrations.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE
            dgvRegistrations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvRegistrations.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvRegistrations.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRegistrations.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvRegistrations.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvRegistrations.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvRegistrations.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvRegistrations.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvRegistrations.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvRegistrations.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvRegistrations.DefaultCellStyle.ForeColor = Color.White;
            dgvRegistrations.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvRegistrations.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvRegistrations.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvRegistrations.RowTemplate.Height = 60;
            dgvRegistrations.RowHeadersVisible = false;
            dgvRegistrations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - SAME color as default cells for consistency
            dgvRegistrations.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvRegistrations.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvRegistrations.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvRegistrations.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering to reduce flicker
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvRegistrations, new object[] { true });

            // Add columns
            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_name",
                HeaderText = "Event Name",
                ReadOnly = true,
                FillWeight = 30
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "user_name",
                HeaderText = "User Name",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "email",
                HeaderText = "Email",
                ReadOnly = true,
                FillWeight = 22
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_date",
                HeaderText = "Event Date",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "role",
                HeaderText = "Role",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "qr_code",
                HeaderText = "QR Code",
                ReadOnly = true,
                Visible = false
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_end_datetime",
                HeaderText = "Event End",
                ReadOnly = true,
                Visible = false
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 12
            });

            // Wire up event handlers
            dgvRegistrations.CellPainting += dgvRegistrations_CellPainting;
            dgvRegistrations.CellClick += dgvRegistrations_CellClick;
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

        private void dgvRegistrations_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return; // Skip header

            var actionColumn = dgvRegistrations.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                // Paint all parts except content to ensure consistent borders
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Check if this is a placeholder row (id will be 0 or null)
                var idValue = dgvRegistrations.Rows[e.RowIndex].Cells["id"].Value;
                if (idValue == null || Convert.ToInt32(idValue) == 0)
                {
                    // This is the placeholder row, don't draw the action button
                    e.Handled = true;
                    return;
                }

                Rectangle cellBounds = e.CellBounds;

                // Draw action button for all events (ongoing, upcoming, or ended)
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

        private void LoadPendingRegistrations()
        {
            try
            {
                string query = @"SELECT 
                                    r.id, 
                                    e.name AS event_name, 
                                    CONCAT(u.first_name, ' ', u.last_name) AS user_name,
                                    u.email,
                                    DATE_FORMAT(e.start_datetime, '%b %d, %Y') AS event_date,
                                    e.end_datetime,
                                    r.role, 
                                    r.status, 
                                    r.qr_code 
                                 FROM registrations r
                                 INNER JOIN events e ON r.event_id = e.id
                                 INNER JOIN users u ON r.user_id = u.id
                                 WHERE 1=1";

                var paramsList = new System.Collections.Generic.List<MySqlParameter>();

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
                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search registrations...")
                    {
                        query += @" AND (e.name LIKE @search 
                                    OR u.first_name LIKE @search 
                                    OR u.last_name LIKE @search
                                    OR u.email LIKE @search)";
                        paramsList.Add(new MySqlParameter("@search", "%" + searchText + "%"));
                    }
                }

                query += " ORDER BY r.status DESC, e.start_datetime DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());

                // Clear existing rows
                dgvRegistrations.Rows.Clear();

                // Check if there's data
                if (dt.Rows.Count == 0)
                {
                    // Add placeholder row when no data
                    int placeholderIndex = dgvRegistrations.Rows.Add(
                        0, // id
                        "", // event_name
                        "No registrations found matching your criteria", // user_name (placeholder message)
                        "", // email
                        "", // event_date
                        "", // role
                        "", // status
                        "", // qr_code
                        DBNull.Value, // event_end_datetime
                        ""  // ActionColumn
                    );

                    // Style the placeholder row
                    DataGridViewRow placeholderRow = dgvRegistrations.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    // Populate rows manually to maintain custom styling
                    foreach (DataRow dr in dt.Rows)
                    {
                        // Capitalize the first letter of the role
                        string role = dr["role"].ToString();
                        string capitalizedRole = string.IsNullOrEmpty(role) ? role : char.ToUpper(role[0]) + role.Substring(1).ToLower();

                        int rowIndex = dgvRegistrations.Rows.Add(
                            dr["id"],
                            dr["event_name"],
                            dr["user_name"],
                            dr["email"],
                            dr["event_date"],
                            capitalizedRole,
                            dr["status"],
                            dr["qr_code"],
                            dr["end_datetime"],
                            "" // ActionColumn (will be custom painted)
                        );
                    }
                }

                // Clear selection to show the proper background color
                dgvRegistrations.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading registrations: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRegistrations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the ActionColumn was clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && 
                dgvRegistrations.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                // Get the current row data
                DataGridViewRow row = dgvRegistrations.Rows[e.RowIndex];
                
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
                contextMenuActions.Items.Clear();

                // Add menu items based on status
                if (status == "Pending")
                {
                    // Show Approve and Reject for pending registrations
                    ToolStripMenuItem approveItem = new ToolStripMenuItem("✓ Approve");
                    approveItem.Font = new Font("Segoe UI", 10F);
                    approveItem.Click += (s, ev) => ApproveRegistration(registrationId, eventName, userName);
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
                    approveItem.Click += (s, ev) => ApproveRegistration(registrationId, eventName, userName);
                    contextMenuActions.Items.Add(approveItem);
                }
                else
                {
                    // For all other statuses (Approved, Checked-in, Attended, Didn't Attend)
                    // Show View QR option
                    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                    viewQRItem.Font = new Font("Segoe UI", 10F);
                    viewQRItem.Click += (s, ev) => ViewQRCode(eventName, userName, qrCode);
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
                Rectangle rect = dgvRegistrations.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                
                // Calculate the button position (centered in cell, same as in CellPainting)
                int buttonWidth = 70;
                int buttonHeight = 30;
                int buttonX = rect.Left + (rect.Width - buttonWidth) / 2;
                int buttonY = rect.Top + (rect.Height - buttonHeight) / 2;
                
                // Position context menu just below and to the right of the button
                Point pt = new Point(buttonX + buttonWidth + 5, buttonY);
                
                // Show the context menu right next to the action button
                contextMenuActions.Show(dgvRegistrations, pt);
            }
        }

        private void ApproveRegistration(int registrationId, string eventName, string userName)
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
                        LoadPendingRegistrations();
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
                DialogResult result = MessageBox.Show("Are you sure you want to reject this registration?", 
                    "Confirm Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "UPDATE registrations SET status='Rejected' WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", registrationId) };

                    int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registration rejected.", "Information", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPendingRegistrations();
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

                // Get event end datetime and registration status for notice
                string eventQuery = @"SELECT e.end_datetime, r.status 
                                     FROM registrations r
                                     INNER JOIN events e ON r.event_id = e.id
                                     WHERE r.qr_code = @qr_code";
                MySqlParameter[] eventParams = { new MySqlParameter("@qr_code", qrCodeData) };
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
                        Image = (Bitmap)qrImage.Clone(), // Clone the image to avoid disposal issues
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
                    else if (isGracePeriodExpired)
                    {
                        // Grace period has expired - QR code is no longer valid
                        lblNotice.Text = "❌ Attendance period has closed.\n" +
                                       "QR code is no longer valid.\n" +
                                       $"Grace period ended {GRACE_PERIOD_HOURS} hours after event.";
                        lblNotice.ForeColor = Color.FromArgb(211, 47, 47); // Red color
                        lblNotice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }
                    else if (isEventEnded)
                    {
                        // Event has ended but still within grace period
                        DateTime attendanceDeadline = eventEndDateTime.AddHours(GRACE_PERIOD_HOURS);
                        TimeSpan timeRemaining = attendanceDeadline - currentTime;
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
                        
                        lblNotice.Text = $"⚠️ Event has ended - Grace period active\n" +
                                       $"QR code valid for {gracePeriodInfo} more\n" +
                                       $"Attendance can still be recorded!";
                        lblNotice.ForeColor = Color.FromArgb(255, 193, 7); // Yellow/amber color
                        lblNotice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }
                    else
                    {
                        lblNotice.Text = "This QR code is for attendance verification\nat the event.";
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
    }
}

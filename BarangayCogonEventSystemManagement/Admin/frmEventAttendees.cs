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

            // DataGridView for attendees
            dgvAttendees = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(950, 450),
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
                FillWeight = 25
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "email",
                HeaderText = "Email",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "contact",
                HeaderText = "Contact",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "role",
                HeaderText = "Role",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "attendance_status",
                HeaderText = "Attendance",
                ReadOnly = true,
                FillWeight = 12
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
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 12
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
                else if (status == "Approved")
                {
                    // Show View QR for approved registrations
                    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                    viewQRItem.Font = new Font("Segoe UI", 10F);
                    viewQRItem.Click += (s, ev) => ViewQRCode(fullName, qrCodeData);
                    contextMenuActions.Items.Add(viewQRItem);

                    // Option to reject approved registration
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
                        MessageBox.Show($"Registration approved!\nQR code saved at:\n{fullPath}", 
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

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrData = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q))
                using (QRCode qrCode = new QRCode(qrData))
                using (Bitmap qrImage = qrCode.GetGraphic(6))
                {
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

        private void LoadAttendees()
        {
            try
            {
                string query = @"SELECT 
                                    r.id AS registration_id,
                                    u.name AS full_name,
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
                                WHERE r.event_id = @event_id
                                ORDER BY r.status DESC, u.name ASC";

                MySqlParameter[] parameters = { new MySqlParameter("@event_id", eventId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                dgvAttendees.Rows.Clear();

                if (dt.Rows.Count == 0)
                {
                    int placeholderIndex = dgvAttendees.Rows.Add(
                        0, "No registrations for this event yet", "", "", "", "", "", "", ""
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
                        string role = dr["role"].ToString();
                        string capitalizedRole = string.IsNullOrEmpty(role) ? role :
                            char.ToUpper(role[0]) + role.Substring(1).ToLower();

                        string statusDisplay = dr["status"].ToString();
                        
                        // Determine attendance status based on check_in_time and check_out_time
                        string attendanceStatus;
                        bool hasCheckIn = dr["check_in_time"] != DBNull.Value;
                        bool hasCheckOut = dr["check_out_time"] != DBNull.Value;
                        DateTime eventEndDateTime = Convert.ToDateTime(dr["end_datetime"]);
                        bool eventHasEnded = DateTime.Now > eventEndDateTime;

                        if (hasCheckIn && hasCheckOut)
                        {
                            // Both check-in and check-out recorded
                            attendanceStatus = "Attended";
                        }
                        else if (hasCheckIn && !hasCheckOut)
                        {
                            // Only check-in recorded
                            attendanceStatus = "Checked-in";
                        }
                        else if (!hasCheckIn && !hasCheckOut)
                        {
                            // No attendance record
                            if (eventHasEnded)
                            {
                                // Event has ended and no attendance
                                attendanceStatus = "Didn't Attend";
                            }
                            else
                            {
                                // Event is ongoing or upcoming
                                attendanceStatus = "N/A";
                            }
                        }
                        else
                        {
                            // This shouldn't happen (check-out without check-in), but handle it
                            attendanceStatus = "N/A";
                        }

                        dgvAttendees.Rows.Add(
                            dr["registration_id"],
                            dr["full_name"],
                            dr["email"],
                            dr["contact"],
                            capitalizedRole,
                            statusDisplay,
                            attendanceStatus,
                            dr["qr_code"],
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
    }
}

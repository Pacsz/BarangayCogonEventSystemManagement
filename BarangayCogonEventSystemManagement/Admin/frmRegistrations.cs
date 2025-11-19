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
        public frmRegistrations()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(46, 51, 73); // Match main panel background
            InitializeContextMenuStyling();
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
                FillWeight = 25
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "role",
                HeaderText = "Role",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvRegistrations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                ReadOnly = true,
                FillWeight = 15
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
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 15
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

        private void LoadPendingRegistrations()
        {
            try
            {
                string query = @"SELECT r.id, e.name AS event_name, u.name AS user_name, r.role, r.status, r.qr_code 
                                 FROM registrations r
                                 INNER JOIN events e ON r.event_id = e.id
                                 INNER JOIN users u ON r.user_id = u.id
                                 ORDER BY r.status DESC, e.date DESC";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                // Clear existing rows
                dgvRegistrations.Rows.Clear();

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
                        capitalizedRole,
                        dr["status"],
                        dr["qr_code"],
                        "" // ActionColumn (will be custom painted)
                    );
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
                else if (status == "Approved")
                {
                    // Show View QR for approved registrations
                    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                    viewQRItem.Font = new Font("Segoe UI", 10F);
                    viewQRItem.Click += (s, ev) => ViewQRCode(eventName, userName, qrCode);
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
                    approveItem.Click += (s, ev) => ApproveRegistration(registrationId, eventName, userName);
                    contextMenuActions.Items.Add(approveItem);
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
                        MessageBox.Show($"Registration approved!\nQR code saved at:\n{fullPath}", 
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
                        Image = (Bitmap)qrImage.Clone(), // Clone the image to avoid disposal issues
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
                        BackColor = Color.FromArgb(25, 118, 210),
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
    }
}

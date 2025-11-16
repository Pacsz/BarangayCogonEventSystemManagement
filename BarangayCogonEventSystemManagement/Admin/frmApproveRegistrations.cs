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
    public partial class frmApproveRegistrations : Form
    {
        public frmApproveRegistrations()
        {
            InitializeComponent();
            CustomizeDataGridView();
            LoadPendingRegistrations();
        }

        private void CustomizeDataGridView()
        {
            // Remove existing event handlers to prevent duplicates
            dgvRegistrations.CellPainting -= dgvRegistrations_CellPainting;
            dgvRegistrations.CellClick -= dgvRegistrations_CellClick;

            dgvRegistrations.Columns.Clear();
            dgvRegistrations.AllowUserToAddRows = false;
            dgvRegistrations.ReadOnly = true;

            // GENERAL GRID SETTINGS - Match mainPanel background
            dgvRegistrations.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvRegistrations.BorderStyle = BorderStyle.None;
            dgvRegistrations.GridColor = Color.FromArgb(60, 65, 90);
            dgvRegistrations.EnableHeadersVisualStyles = false;
            dgvRegistrations.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE - Match sidebar color (same color when selected)
            dgvRegistrations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvRegistrations.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvRegistrations.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRegistrations.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54); // Same as normal background
            dgvRegistrations.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvRegistrations.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvRegistrations.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvRegistrations.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvRegistrations.ColumnHeadersHeight = 45;

            // CELL STYLE - Match mainPanel background (keep same color when selected)
            dgvRegistrations.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvRegistrations.DefaultCellStyle.ForeColor = Color.White;
            dgvRegistrations.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73); // Same as normal background
            dgvRegistrations.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvRegistrations.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvRegistrations.RowTemplate.Height = 60;
            dgvRegistrations.RowHeadersVisible = false;
            dgvRegistrations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - slightly darker for subtle contrast (same when selected)
            dgvRegistrations.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(37, 42, 64);
            dgvRegistrations.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvRegistrations.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 42, 64); // Same as normal background
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
                e.PaintBackground(e.ClipBounds, true);
                e.Handled = true;

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
                    int rowIndex = dgvRegistrations.Rows.Add(
                        dr["id"],
                        dr["event_name"],
                        dr["user_name"],
                        dr["role"],
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

                // Clear existing menu items
                contextMenuActions.Items.Clear();

                // Add menu items based on status
                if (status == "Pending")
                {
                    // Show Approve and Reject for pending registrations
                    ToolStripMenuItem approveItem = new ToolStripMenuItem("✓ Approve");
                    approveItem.Click += (s, ev) => ApproveRegistration(registrationId, eventName, userName);
                    contextMenuActions.Items.Add(approveItem);

                    ToolStripMenuItem rejectItem = new ToolStripMenuItem("✗ Reject");
                    rejectItem.Click += (s, ev) => RejectRegistration(registrationId);
                    contextMenuActions.Items.Add(rejectItem);
                }
                else if (status == "Approved")
                {
                    // Show View QR for approved registrations
                    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("🔲 View QR");
                    viewQRItem.Click += (s, ev) => ViewQRCode(eventName, userName);
                    contextMenuActions.Items.Add(viewQRItem);

                    // Option to reject approved registration
                    ToolStripMenuItem rejectItem = new ToolStripMenuItem("✗ Reject");
                    rejectItem.Click += (s, ev) => RejectRegistration(registrationId);
                    contextMenuActions.Items.Add(rejectItem);
                }
                else if (status == "Rejected")
                {
                    // Show Approve for rejected registrations (allow re-approval)
                    ToolStripMenuItem approveItem = new ToolStripMenuItem("✓ Approve");
                    approveItem.Click += (s, ev) => ApproveRegistration(registrationId, eventName, userName);
                    contextMenuActions.Items.Add(approveItem);
                }

                // Show the context menu at the cursor position
                Rectangle rect = dgvRegistrations.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                Point pt = new Point(rect.Left + rect.Width, rect.Top + rect.Height);
                contextMenuActions.Show(dgvRegistrations, pt);
            }
        }

        private void ApproveRegistration(int registrationId, string eventName, string userName)
        {
            try
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

        private void ViewQRCode(string eventName, string userName)
        {
            try
            {
                string filePath = Path.Combine(Application.StartupPath, "Assets", "QR_Codes", 
                    $"{eventName}_{userName}.png".Replace(" ", "_"));

                if (File.Exists(filePath))
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
                        Image = Image.FromFile(filePath),
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
                else
                {
                    MessageBox.Show("QR code file not found for this user.", "Missing QR", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

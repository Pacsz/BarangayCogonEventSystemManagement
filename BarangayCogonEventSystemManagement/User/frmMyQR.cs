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
    public partial class frmMyQR : Form
    {
        private int userId;

        public frmMyQR(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.BackColor = Color.FromArgb(46, 51, 73); // Match main panel background
        }

        private void frmMyQR_Load(object sender, EventArgs e)
        {
            CustomizeDataGridView();
            LoadApprovedEvents();
        }

        private void CustomizeDataGridView()
        {
            // Remove existing event handlers to prevent duplicates
            dgvQRList.CellPainting -= dgvQRList_CellPainting;
            dgvQRList.CellClick -= dgvQRList_CellClick;

            dgvQRList.Columns.Clear();
            dgvQRList.AllowUserToAddRows = false;
            dgvQRList.ReadOnly = true;

            // GENERAL GRID SETTINGS - Match user dashboard style
            dgvQRList.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvQRList.BorderStyle = BorderStyle.None;
            dgvQRList.GridColor = Color.FromArgb(60, 65, 90);
            dgvQRList.EnableHeadersVisualStyles = false;
            dgvQRList.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE
            dgvQRList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvQRList.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvQRList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvQRList.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvQRList.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvQRList.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvQRList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvQRList.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvQRList.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvQRList.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvQRList.DefaultCellStyle.ForeColor = Color.White;
            dgvQRList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvQRList.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvQRList.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvQRList.RowTemplate.Height = 55;
            dgvQRList.RowHeadersVisible = false;
            dgvQRList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - SAME color as default cells for consistency
            dgvQRList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvQRList.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvQRList.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvQRList.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvQRList, new object[] { true });

            // Add columns
            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_id",
                HeaderText = "Event ID",
                ReadOnly = true,
                Visible = false
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "qr_code_data",
                HeaderText = "QR Data",
                ReadOnly = true,
                Visible = false
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_name",
                HeaderText = "Event",
                ReadOnly = true,
                FillWeight = 30
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_date",
                HeaderText = "Event Date",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_time",
                HeaderText = "Event Schedule",
                ReadOnly = true,
                FillWeight = 19
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_venue",
                HeaderText = "Venue",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 13
            });

            // Wire up event handlers
            dgvQRList.CellPainting += dgvQRList_CellPainting;
            dgvQRList.CellClick += dgvQRList_CellClick;
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

        private void dgvQRList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return; // Skip header

            var actionColumn = dgvQRList.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                // Paint default cell background and borders
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                // Check if this is a placeholder row
                var eventIdValue = dgvQRList.Rows[e.RowIndex].Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0)
                {
                    e.Handled = true;
                    return;
                }

                Rectangle cellBounds = e.CellBounds;
                int buttonWidth = 90;
                int buttonHeight = 30;
                int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;
                Rectangle buttonRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                int radius = 10;

                using (GraphicsPath path = GetRoundPath(buttonRect, radius))
                using (SolidBrush buttonBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                using (Font btnFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(buttonBrush, path);
                    e.Graphics.DrawString("View QR", btnFont, textBrush, buttonRect, sf);
                }

                e.Handled = true;
            }
        }

        private void dgvQRList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvQRList.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                DataGridViewRow row = dgvQRList.Rows[e.RowIndex];

                var eventIdValue = row.Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0)
                {
                    return;
                }

                string eventName = row.Cells["event_name"].Value?.ToString();
                string qrCodeData = row.Cells["qr_code_data"].Value?.ToString();

                if (string.IsNullOrEmpty(qrCodeData))
                {
                    MessageBox.Show("No QR code available for this event.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Show QR code in a popup
                ShowQRCodePopup(eventName, qrCodeData);
            }
        }

        private void ShowQRCodePopup(string eventName, string qrCodeData)
        {
            // Create popup form
            Form qrPopup = new Form
            {
                Text = "QR Code",
                Size = new Size(400, 550),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(46, 51, 73)
            };

            // Event name label
            Label lblEventName = new Label
            {
                Text = eventName,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(340, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // QR Code picture box
            PictureBox picQR = new PictureBox
            {
                Location = new Point(90, 70),
                Size = new Size(220, 220),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Generate QR code
            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrData = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q))
                using (QRCode qr = new QRCode(qrData))
                {
                    picQR.Image = qr.GetGraphic(6);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating QR code: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                qrPopup.Close();
                return;
            }

            // QR Code info label
            Label lblInfo = new Label
            {
                Text = "Scan this QR code at the event for attendance",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(158, 161, 178),
                Location = new Point(20, 305),
                Size = new Size(340, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Save QR button
            Button btnSave = new Button
            {
                Text = "Save QR as Image",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 126, 249),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Location = new Point(90, 360),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, ev) =>
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "PNG Image|*.png",
                    FileName = $"QR_{eventName.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.png"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    picQR.Image.Save(saveDialog.FileName);
                    MessageBox.Show("QR Code saved successfully!", "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Close button
            Button btnClosePopup = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Location = new Point(90, 420),
                Cursor = Cursors.Hand
            };
            btnClosePopup.FlatAppearance.BorderSize = 0;
            btnClosePopup.Click += (s, ev) => qrPopup.Close();

            qrPopup.Controls.Add(lblEventName);
            qrPopup.Controls.Add(picQR);
            qrPopup.Controls.Add(lblInfo);
            qrPopup.Controls.Add(btnSave);
            qrPopup.Controls.Add(btnClosePopup);

            qrPopup.ShowDialog();
        }

        private void LoadApprovedEvents()
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
                                    r.status,
                                    r.qr_code AS qr_code_data
                                FROM registrations r
                                INNER JOIN events e ON r.event_id = e.id
                                WHERE r.user_id = @user_id AND r.status = 'Approved'
                                ORDER BY e.start_datetime ASC";

                MySqlParameter[] param = { new MySqlParameter("@user_id", userId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, param);

                // Clear existing rows
                dgvQRList.Rows.Clear();

                if (dt.Rows.Count == 0)
                {
                    // Add placeholder row
                    int placeholderIndex = dgvQRList.Rows.Add(
                        0, "", "No approved events with QR codes yet", "", "", "", "", ""
                    );

                    DataGridViewRow placeholderRow = dgvQRList.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dgvQRList.Rows.Add(
                            dr["event_id"],
                            dr["qr_code_data"],
                            dr["event_name"],
                            dr["event_date"],
                            dr["event_time"],
                            dr["event_venue"],
                            dr["status"],
                            "" // ActionColumn (will be custom painted)
                        );
                    }
                }

                dgvQRList.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading approved events: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

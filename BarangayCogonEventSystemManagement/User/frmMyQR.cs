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
        private TextBox txtSearch;

        public frmMyQR(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.BackColor = Color.FromArgb(46, 51, 73); // Match main panel background
            InitializeFilters();
        }

        private void InitializeFilters()
        {
            // Search box
            txtSearch = new TextBox
            {
                Location = new Point(20, 20),
                Size = new Size(350, 30),
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
            txtSearch.TextChanged += (s, ev) => LoadApprovedEvents();

            this.Controls.Add(txtSearch);

            // Adjust dgvQRList position
            if (dgvQRList != null)
            {
                dgvQRList.Location = new Point(20, 60);
                dgvQRList.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 80);
            }
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
                FillWeight = 22
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_time",
                HeaderText = "Event Schedule",
                ReadOnly = true,
                FillWeight = 22
            });

            dgvQRList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_venue",
                HeaderText = "Venue",
                ReadOnly = true,
                FillWeight = 22
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

                int eventId = Convert.ToInt32(row.Cells["event_id"].Value);
                string eventName = row.Cells["event_name"].Value?.ToString();
                string qrCodeData = row.Cells["qr_code_data"].Value?.ToString();

                if (string.IsNullOrEmpty(qrCodeData))
                {
                    MessageBox.Show("No QR code available for this event.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Fetch event end datetime and registration status
                try
                {
                    string query = @"SELECT e.end_datetime, r.status 
                                   FROM events e
                                   INNER JOIN registrations r ON e.id = r.event_id
                                   WHERE e.id=@id AND r.user_id=@user_id";
                    MySqlParameter[] parameters = { 
                        new MySqlParameter("@id", eventId),
                        new MySqlParameter("@user_id", userId)
                    };
                    DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Event information not found.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DateTime eventEndDateTime = Convert.ToDateTime(dt.Rows[0]["end_datetime"]);
                    string registrationStatus = dt.Rows[0]["status"].ToString();

                    // Use the modular QR viewer form with status
                    frmQRCodeViewer qrViewer = new frmQRCodeViewer(eventName, qrCodeData, eventEndDateTime, registrationStatus);
                    qrViewer.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading event details: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
                                    r.qr_code AS qr_code_data
                                FROM registrations r
                                INNER JOIN events e ON r.event_id = e.id
                                WHERE r.user_id = @user_id AND r.status IN ('Approved', 'Checked-in', 'Attended', 'Didn''t Attend')";

                var paramsList = new System.Collections.Generic.List<MySqlParameter>();
                paramsList.Add(new MySqlParameter("@user_id", userId));

                // Add search filter
                if (txtSearch != null)
                {
                    string searchText = txtSearch.Text;
                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search events...")
                    {
                        query += @" AND (e.name LIKE @search 
                                    OR e.venue LIKE @search)";
                        paramsList.Add(new MySqlParameter("@search", "%" + searchText + "%"));
                    }
                }

                query += " ORDER BY e.start_datetime ASC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());

                // Clear existing rows
                dgvQRList.Rows.Clear();

                if (dt.Rows.Count == 0)
                {
                    // Add placeholder row
                    int placeholderIndex = dgvQRList.Rows.Add(
                        0, "", "No approved events found matching your criteria", "", "", "", ""
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

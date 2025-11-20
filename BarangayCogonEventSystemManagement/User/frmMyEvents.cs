using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmMyEvents : Form
    {
        private int userId;

        public frmMyEvents(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.BackColor = Color.FromArgb(46, 51, 73); // Match main panel background
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
                FillWeight = 18
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

                string status = dgvMyEvents.Rows[e.RowIndex].Cells["status"].Value?.ToString();

                Rectangle cellBounds = e.CellBounds;
                int buttonWidth = 100;
                int buttonHeight = 30;
                int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;
                Rectangle buttonRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                int radius = 10;

                using (GraphicsPath path = GetRoundPath(buttonRect, radius))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                using (Font btnFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    string buttonText = "";
                    Color buttonColor = Color.Gray;

                    if (status == "Pending")
                    {
                        buttonText = "Unregister";
                        buttonColor = Color.FromArgb(244, 67, 54); // Red
                    }
                    else if (status == "Approved" || status == "Attended")
                    {
                        buttonText = "N/A";
                        buttonColor = Color.Gray;
                    }
                    else if (status == "Rejected")
                    {
                        buttonText = "Register";
                        buttonColor = Color.FromArgb(0, 126, 249); // Blue
                    }

                    using (SolidBrush buttonBrush = new SolidBrush(buttonColor))
                    {
                        e.Graphics.FillPath(buttonBrush, path);
                        e.Graphics.DrawString(buttonText, btnFont, textBrush, buttonRect, sf);
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
                int registrationId = Convert.ToInt32(row.Cells["registration_id"].Value);
                int eventId = Convert.ToInt32(row.Cells["event_id"].Value);
                string eventName = row.Cells["event_name"].Value?.ToString();

                if (status == "Pending")
                {
                    // Unregister action
                    UnregisterFromEvent(registrationId, eventName);
                }
                else if (status == "Rejected")
                {
                    // Re-register action
                    RegisterForEvent(eventId, eventName);
                }
                // N/A for Approved or Attended - do nothing
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
                // Show confirmation dialog before re-registering
                DialogResult confirmResult = MessageBox.Show(
                    $"Do you want to re-register for this event?\n\n" +
                    $"Event: {eventName}\n\n" +
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

                        // Get user role
                        string roleQuery = "SELECT role FROM users WHERE id=@user_id";
                        MySqlParameter[] roleParams = { new MySqlParameter("@user_id", userId) };
                        DataTable dtRole = DatabaseHelper.ExecuteQuery(roleQuery, roleParams);
                        string userRole = dtRole.Rows.Count > 0 ? dtRole.Rows[0]["role"].ToString() : "attendee";

                        // Insert new registration with qr_code explicitly set to NULL
                        string insertQuery = @"INSERT INTO registrations (event_id, user_id, role, status, qr_code, created_at)
                                              VALUES (@event_id, @user_id, @role, 'Pending', NULL, NOW())";
                        MySqlParameter[] insertParams = {
                            new MySqlParameter("@event_id", eventId),
                            new MySqlParameter("@user_id", userId),
                            new MySqlParameter("@role", userRole)
                        };

                        int result = DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams);
                        if (result > 0)
                        {
                            MessageBox.Show($"Successfully re-registered for '{eventName}'!\n\nPlease wait for admin approval.",
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

        private void LoadMyEvents()
        {
            try
            {
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
                                    r.role,
                                    r.status
                                FROM registrations r
                                INNER JOIN events e ON r.event_id = e.id
                                WHERE r.user_id = @user_id
                                ORDER BY e.start_datetime DESC";

                MySqlParameter[] param = { new MySqlParameter("@user_id", userId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, param);

                // Clear existing rows
                dgvMyEvents.Rows.Clear();

                if (dt.Rows.Count == 0)
                {
                    // Add placeholder row
                    int placeholderIndex = dgvMyEvents.Rows.Add(
                        0, 0, "You haven't registered for any events yet", "", "", "", "", "", ""
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
                            capitalizedRole,
                            dr["status"],
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

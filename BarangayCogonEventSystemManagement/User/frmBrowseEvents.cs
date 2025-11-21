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

        public frmBrowseEvents(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.BackColor = Color.FromArgb(46, 51, 73);
            InitializeControls();
            LoadBrowseEvents();
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
            // Style the DataGridView header
            dgvBrowse.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvBrowse.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvBrowse.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBrowse.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvBrowse.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvBrowse.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvBrowse.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvBrowse.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvBrowse.ColumnHeadersHeight = 45;

            // Cell style
            dgvBrowse.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvBrowse.DefaultCellStyle.ForeColor = Color.White;
            dgvBrowse.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvBrowse.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvBrowse.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvBrowse.RowTemplate.Height = 55;

            // Alternating rows - SAME color as default cells for consistency
            dgvBrowse.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvBrowse.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvBrowse.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvBrowse.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvBrowse, new object[] { true });

            // Add columns
            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_name",
                HeaderText = "Event Name",
                ReadOnly = true,
                FillWeight = 25
            });

            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_date",
                HeaderText = "Event Date",
                ReadOnly = true,
                FillWeight = 17
            });

            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_time",
                HeaderText = "Event Schedule",
                ReadOnly = true,
                FillWeight = 17
            });

            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_venue",
                HeaderText = "Venue",
                ReadOnly = true,
                FillWeight = 18
            });

            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_type",
                HeaderText = "Type",
                ReadOnly = true,
                FillWeight = 17
            });

            dgvBrowse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 13
            });

            // Wire up event handlers
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
                // Paint all parts except content to ensure consistent borders
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Check if this is a placeholder row
                var eventIdValue = dgvBrowse.Rows[e.RowIndex].Cells["event_id"].Value;
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
                    e.Graphics.DrawString("Register", btnFont, textBrush, buttonRect, sf);
                }

                e.Handled = true;
            }
        }

        private void dgvBrowse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvBrowse.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                DataGridViewRow row = dgvBrowse.Rows[e.RowIndex];

                var eventIdValue = row.Cells["event_id"].Value;
                if (eventIdValue == null || Convert.ToInt32(eventIdValue) == 0)
                {
                    return;
                }

                int eventId = Convert.ToInt32(row.Cells["event_id"].Value);
                string eventName = row.Cells["event_name"].Value?.ToString();
                string eventDate = row.Cells["event_date"].Value?.ToString();
                string eventVenue = row.Cells["event_venue"].Value?.ToString();

                // Prompt user to select their role for this event
                string selectedRole = ShowRoleSelectionDialog(eventName);
                
                if (string.IsNullOrEmpty(selectedRole))
                {
                    // User cancelled the role selection
                    return;
                }

                // Show confirmation dialog before registration
                DialogResult confirmResult = MessageBox.Show(
                    $"Do you want to register for this event?\n\n" +
                    $"Event: {eventName}\n" +
                    $"Date: {eventDate}\n" +
                    $"Venue: {eventVenue}\n" +
                    $"Role: {selectedRole}\n\n" +
                    $"Your registration will be pending until approved by an administrator.",
                    "Confirm Registration",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // Check if user is already registered (safety check)
                        string checkQuery = "SELECT id FROM registrations WHERE event_id=@event_id AND user_id=@user_id";
                        MySqlParameter[] checkParams = {
                            new MySqlParameter("@event_id", eventId),
                            new MySqlParameter("@user_id", userId)
                        };
                        
                        DataTable dtCheck = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);
                        if (dtCheck.Rows.Count > 0)
                        {
                            MessageBox.Show("You are already registered for this event.", "Already Registered",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            LoadBrowseEvents();
                            return;
                        }

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
                            MessageBox.Show($"Successfully registered for '{eventName}' as {selectedRole}!\n\nPlease wait for admin approval.",
                                "Registered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Reload the events to remove the just-registered event
                            LoadBrowseEvents();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (MySqlException mysqlEx)
                    {
                        // Handle MySQL specific errors
                        if (mysqlEx.Number == 1062) // Duplicate entry error
                        {
                            MessageBox.Show("You are already registered for this event.", "Duplicate Registration",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            LoadBrowseEvents();
                        }
                        else if (mysqlEx.Number == 1452) // Foreign key constraint fails
                        {
                            MessageBox.Show("Event or user not found. Please refresh and try again.", "Database Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show($"Database error: {mysqlEx.Message}\n\nError Code: {mysqlEx.Number}", 
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error registering for event: {ex.Message}\n\nPlease contact the administrator if this problem persists.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
                // Build query to get events the user hasn't registered for
                string query = @"SELECT 
                                    e.id AS event_id,
                                    e.name AS event_name,
                                    CASE 
                                        WHEN DATE(e.start_datetime) = DATE(e.end_datetime) THEN DATE_FORMAT(e.start_datetime, '%b %d, %Y')
                                        ELSE CONCAT(DATE_FORMAT(e.start_datetime, '%b %d'), ' - ', DATE_FORMAT(e.end_datetime, '%b %d, %Y'))
                                    END AS event_date,
                                    CONCAT(DATE_FORMAT(e.start_datetime, '%h:%i %p'), ' - ', DATE_FORMAT(e.end_datetime, '%h:%i %p')) AS event_time,
                                    e.venue AS event_venue,
                                    e.type AS event_type
                                FROM events e
                                WHERE e.start_datetime >= NOW()
                                AND e.id NOT IN (
                                    SELECT event_id FROM registrations WHERE user_id = @user_id
                                )";

                // Add type filter
                if (cboTypeFilter.SelectedIndex > 0)
                {
                    query += " AND e.type = @type";
                }

                // Add search filter
                string searchText = txtSearch.Text;
                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search events...")
                {
                    query += @" AND (e.name LIKE @search 
                                OR e.venue LIKE @search 
                                OR e.type LIKE @search
                                OR e.organizer LIKE @search)";
                }

                query += " ORDER BY e.start_datetime ASC";

                // Prepare parameters
                var paramsList = new System.Collections.Generic.List<MySqlParameter>();
                paramsList.Add(new MySqlParameter("@user_id", userId));

                if (cboTypeFilter.SelectedIndex > 0)
                {
                    paramsList.Add(new MySqlParameter("@type", cboTypeFilter.SelectedItem.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search events...")
                {
                    paramsList.Add(new MySqlParameter("@search", "%" + searchText + "%"));
                }

                DataTable dtEvents = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());

                // Clear existing rows
                dgvBrowse.Rows.Clear();

                if (dtEvents.Rows.Count == 0)
                {
                    // Add placeholder row
                    int placeholderIndex = dgvBrowse.Rows.Add(
                        0, "No available events to register", "", "", "", "", ""
                    );

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
                            dr["event_name"],
                            dr["event_date"],
                            dr["event_time"],
                            dr["event_venue"],
                            dr["event_type"],
                            "" // ActionColumn (will be custom painted)
                        );
                    }
                }

                dgvBrowse.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

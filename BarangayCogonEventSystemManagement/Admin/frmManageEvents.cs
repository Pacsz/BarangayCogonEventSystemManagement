using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventManagementSystem
{
    // Custom RoundedButton class with painted rounded corners
    public class RoundedButton : Button
    {
        private int borderRadius = 10;

        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Create rounded rectangle path
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = GetRoundPath(rect, borderRadius))
            {
                // Set the button's region to the rounded rectangle (removes square corners)
                this.Region = new Region(path);

                // Fill button background
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Draw text
                TextRenderer.DrawText(e.Graphics, this.Text, this.Font, rect, 
                    this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
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

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Cursor = Cursors.Hand;
        }
    }

    public partial class frmManageEvents : Form
    {
        private ContextMenuStrip contextMenuActions;

        public frmManageEvents()
        {
            InitializeComponent();
            InitializeContextMenu();
            CustomizeDataGridView();
            LoadEvents();
            StyleAddButton();
        }

        private void InitializeContextMenu()
        {
            contextMenuActions = new ContextMenuStrip();
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
            dgvEvents.CellPainting -= dgvEvents_CellPainting;
            dgvEvents.CellClick -= dgvEvents_CellClick;

            dgvEvents.Columns.Clear();
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.ReadOnly = true;

            // GENERAL GRID SETTINGS - Match mainPanel background
            dgvEvents.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvEvents.BorderStyle = BorderStyle.None;
            dgvEvents.GridColor = Color.FromArgb(60, 65, 90);
            dgvEvents.EnableHeadersVisualStyles = false;
            dgvEvents.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE - Match sidebar color
            dgvEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEvents.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvEvents.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvEvents.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvEvents.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvEvents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvEvents.ColumnHeadersHeight = 45;

            // CELL STYLE - Match mainPanel background
            dgvEvents.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvEvents.DefaultCellStyle.ForeColor = Color.White;
            dgvEvents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvEvents.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEvents.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvEvents.RowTemplate.Height = 60;
            dgvEvents.RowHeadersVisible = false;
            dgvEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows
            dgvEvents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(37, 42, 64);
            dgvEvents.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvEvents.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 42, 64);
            dgvEvents.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvEvents, new object[] { true });

            // Add columns
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "name",
                HeaderText = "Event Name",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "date",
                HeaderText = "Date",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "time",
                HeaderText = "Time",
                ReadOnly = true,
                FillWeight = 10
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "venue",
                HeaderText = "Venue",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "type",
                HeaderText = "Type",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "organizer",
                HeaderText = "Organizer",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "description",
                HeaderText = "Description",
                ReadOnly = true,
                Visible = false
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 13
            });

            // Wire up event handlers
            dgvEvents.CellPainting += dgvEvents_CellPainting;
            dgvEvents.CellClick += dgvEvents_CellClick;
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

        private void dgvEvents_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var actionColumn = dgvEvents.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                e.PaintBackground(e.ClipBounds, true);
                e.Handled = true;

                Rectangle cellBounds = e.CellBounds;
                int buttonWidth = 70;
                int buttonHeight = 30;

                int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;

                Rectangle viewRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                int radius = 10;

                using (GraphicsPath viewPath = GetRoundPath(viewRect, radius))
                using (SolidBrush viewBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
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

        private void LoadEvents()
        {
            try
            {
                string query = "SELECT id, name, description, date, time, venue, type, organizer FROM events ORDER BY date DESC";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                dgvEvents.Rows.Clear();

                foreach (DataRow dr in dt.Rows)
                {
                    // Format time properly
                    TimeSpan timeValue = (TimeSpan)dr["time"];
                    DateTime timeDisplay = DateTime.Today.Add(timeValue);
                    string formattedTime = timeDisplay.ToString("h:mm tt");

                    dgvEvents.Rows.Add(
                        dr["id"],
                        dr["name"],
                        Convert.ToDateTime(dr["date"]).ToString("MMM dd, yyyy"),
                        formattedTime,
                        dr["venue"],
                        dr["type"],
                        dr["organizer"],
                        dr["description"],
                        ""
                    );
                }

                dgvEvents.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvEvents.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                DataGridViewRow row = dgvEvents.Rows[e.RowIndex];
                int eventId = Convert.ToInt32(row.Cells["id"].Value);

                contextMenuActions.Items.Clear();

                // Use universally supported Unicode characters
                ToolStripMenuItem editItem = new ToolStripMenuItem("✏ Edit");
                editItem.Font = new Font("Segoe UI", 10F);
                editItem.Click += (s, ev) => ShowEditEventForm(eventId);
                contextMenuActions.Items.Add(editItem);

                ToolStripMenuItem deleteItem = new ToolStripMenuItem("✗ Delete");
                deleteItem.Font = new Font("Segoe UI", 10F);
                deleteItem.Click += (s, ev) => DeleteEvent(eventId);
                contextMenuActions.Items.Add(deleteItem);

                Rectangle rect = dgvEvents.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                int buttonWidth = 70;
                int buttonHeight = 30;
                int buttonX = rect.Left + (rect.Width - buttonWidth) / 2;
                int buttonY = rect.Top + (rect.Height - buttonHeight) / 2;

                Point pt = new Point(buttonX + buttonWidth + 5, buttonY);
                contextMenuActions.Show(dgvEvents, pt);
            }
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            ShowAddEventForm();
        }

        private void ShowAddEventForm()
        {
            Form addForm = CreateEventForm("Add Event", null);
            addForm.ShowDialog();
        }

        private void ShowEditEventForm(int eventId)
        {
            try
            {
                string query = "SELECT * FROM events WHERE id = @id";
                MySqlParameter[] parameters = { new MySqlParameter("@id", eventId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    Form editForm = CreateEventForm("Edit Event", dt.Rows[0]);
                    editForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading event: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Form CreateEventForm(string title, DataRow eventData)
        {
            Form eventForm = new Form
            {
                Text = title,
                Size = new Size(500, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(46, 51, 73)
            };

            // Title Header
            Label lblHeader = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,  // Remove background color
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Event Name
            Label lblName = new Label { Text = "Event Name:", Location = new Point(30, 70), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtName = new TextBox { Location = new Point(30, 100), Size = new Size(420, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            // Description
            Label lblDesc = new Label { Text = "Description:", Location = new Point(30, 140), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtDesc = new TextBox { Location = new Point(30, 170), Size = new Size(420, 60), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Multiline = true };

            // Date
            // Note: DateTimePicker doesn't support BackColor/ForeColor styling on the main control
            // Only the calendar dropdown can be customized
            Label lblDate = new Label { Text = "Date:", Location = new Point(30, 240), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            
            // Create a panel to wrap the DateTimePicker for better visual integration
            Panel pnlDate = new Panel
            {
                Location = new Point(30, 270),
                Size = new Size(200, 32),
                BackColor = Color.FromArgb(37, 42, 69),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };
            
            DateTimePicker dtpDate = new DateTimePicker
            {
                Location = new Point(1, 1),
                Size = new Size(196, 28),
                Font = new Font("Segoe UI", 9F),  // Reduced font size to match other inputs
                CalendarMonthBackground = Color.FromArgb(37, 42, 69),
                CalendarForeColor = Color.White,
                CalendarTitleBackColor = Color.FromArgb(24, 30, 54),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray
            };
            pnlDate.Controls.Add(dtpDate);
            
            // Time
            Label lblTime = new Label { Text = "Time:", Location = new Point(250, 240), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            
            // Create a panel to wrap the DateTimePicker for better visual integration
            Panel pnlTime = new Panel
            {
                Location = new Point(250, 270),
                Size = new Size(200, 32),
                BackColor = Color.FromArgb(37, 42, 69),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };
            
            DateTimePicker dtpTime = new DateTimePicker
            {
                Location = new Point(1, 1),
                Size = new Size(196, 28),
                Font = new Font("Segoe UI", 9F),  // Reduced font size to match other inputs
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                CalendarMonthBackground = Color.FromArgb(37, 42, 69),
                CalendarForeColor = Color.White,
                CalendarTitleBackColor = Color.FromArgb(24, 30, 54),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray
            };
            pnlTime.Controls.Add(dtpTime);

            // Venue
            Label lblVenue = new Label { Text = "Venue:", Location = new Point(30, 310), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtVenue = new TextBox { Location = new Point(30, 340), Size = new Size(420, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            // Type
            Label lblType = new Label { Text = "Type:", Location = new Point(30, 380), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            ComboBox cboType = new ComboBox
            {
                Location = new Point(30, 410),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(37, 42, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cboType.Items.AddRange(new object[] { "Community Service", "Health Drive", "Cleanup Drive", "Barangay Assembly", "Training / Workshop" });
            
            // Set default value for Type when adding new event
            if (eventData == null)
            {
                cboType.SelectedIndex = 0; // Default to "Community Service"
            }

            // Organizer
            Label lblOrganizer = new Label { Text = "Organizer:", Location = new Point(250, 380), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtOrganizer = new TextBox { Location = new Point(250, 410), Size = new Size(200, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            // Buttons with rounded corners
            RoundedButton btnSave = new RoundedButton
            {
                Text = eventData == null ? "Add Event" : "Update Event",
                Location = new Point(150, 480),
                Size = new Size(125, 45),
                BackColor = Color.FromArgb(25, 118, 210),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderRadius = 10,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;

            RoundedButton btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(280, 480),
                Size = new Size(100, 45),
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderRadius = 10,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) => eventForm.Close();

            // Populate data if editing
            if (eventData != null)
            {
                txtName.Text = eventData["name"].ToString();
                txtDesc.Text = eventData["description"].ToString();
                dtpDate.Value = Convert.ToDateTime(eventData["date"]);
                dtpTime.Value = DateTime.Today.Add((TimeSpan)eventData["time"]);
                txtVenue.Text = eventData["venue"].ToString();
                cboType.Text = eventData["type"].ToString();
                txtOrganizer.Text = eventData["organizer"].ToString();
            }

            btnSave.Click += (s, ev) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Please enter an event name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (eventData == null)
                    {
                        // Add new event
                        string query = @"INSERT INTO events (name, description, date, time, venue, type, organizer)
                                         VALUES (@name, @description, @date, @time, @venue, @type, @organizer)";
                        MySqlParameter[] parameters = {
                            new MySqlParameter("@name", txtName.Text),
                            new MySqlParameter("@description", txtDesc.Text),
                            new MySqlParameter("@date", dtpDate.Value.Date),
                            new MySqlParameter("@time", dtpTime.Value.TimeOfDay),
                            new MySqlParameter("@venue", txtVenue.Text),
                            new MySqlParameter("@type", cboType.Text),
                            new MySqlParameter("@organizer", txtOrganizer.Text)
                        };
                        DatabaseHelper.ExecuteNonQuery(query, parameters);
                        MessageBox.Show("Event added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Update existing event
                        string query = @"UPDATE events SET name=@name, description=@description, date=@date, 
                                         time=@time, venue=@venue, type=@type, organizer=@organizer WHERE id=@id";
                        MySqlParameter[] parameters = {
                            new MySqlParameter("@id", eventData["id"]),
                            new MySqlParameter("@name", txtName.Text),
                            new MySqlParameter("@description", txtDesc.Text),
                            new MySqlParameter("@date", dtpDate.Value.Date),
                            new MySqlParameter("@time", dtpTime.Value.TimeOfDay),
                            new MySqlParameter("@venue", txtVenue.Text),
                            new MySqlParameter("@type", cboType.Text),
                            new MySqlParameter("@organizer", txtOrganizer.Text)
                        };
                        DatabaseHelper.ExecuteNonQuery(query, parameters);
                        MessageBox.Show("Event updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadEvents();
                    eventForm.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving event: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            eventForm.Controls.Add(lblHeader);
            eventForm.Controls.Add(lblName);
            eventForm.Controls.Add(txtName);
            eventForm.Controls.Add(lblDesc);
            eventForm.Controls.Add(txtDesc);
            eventForm.Controls.Add(lblDate);
            eventForm.Controls.Add(pnlDate);  // Add panel instead of dtpDate
            eventForm.Controls.Add(lblTime);
            eventForm.Controls.Add(pnlTime);  // Add panel instead of dtpTime
            eventForm.Controls.Add(lblVenue);
            eventForm.Controls.Add(txtVenue);
            eventForm.Controls.Add(lblType);
            eventForm.Controls.Add(cboType);
            eventForm.Controls.Add(lblOrganizer);
            eventForm.Controls.Add(txtOrganizer);
            eventForm.Controls.Add(btnSave);
            eventForm.Controls.Add(btnCancel);

            return eventForm;
        }

        private void DeleteEvent(int eventId)
        {
            if (MessageBox.Show("Are you sure you want to delete this event?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM events WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", eventId) };
                    DatabaseHelper.ExecuteNonQuery(query, parameters);
                    MessageBox.Show("Event deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEvents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting event: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void StyleAddButton()
        {
            // Apply rounded corners to the Add Event button
            btnAddEvent.FlatStyle = FlatStyle.Flat;
            btnAddEvent.FlatAppearance.BorderSize = 0;
            btnAddEvent.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                using (GraphicsPath path = GetRoundPath(rect, 10))
                {
                    // Set the button's region to clip to rounded shape
                    btn.Region = new Region(path);

                    using (SolidBrush brush = new SolidBrush(btn.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    
                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, rect,
                        btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
        }
    }
}

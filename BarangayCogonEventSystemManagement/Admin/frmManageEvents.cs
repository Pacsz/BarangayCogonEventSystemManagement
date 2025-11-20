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
            this.BackColor = Color.FromArgb(46, 51, 73); // Match main panel background
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

            // GENERAL GRID SETTINGS - Match user dashboard style
            dgvEvents.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvEvents.BorderStyle = BorderStyle.None;
            dgvEvents.GridColor = Color.FromArgb(60, 65, 90);
            dgvEvents.EnableHeadersVisualStyles = false;
            dgvEvents.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE
            dgvEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEvents.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvEvents.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvEvents.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvEvents.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvEvents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvEvents.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvEvents.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvEvents.DefaultCellStyle.ForeColor = Color.White;
            dgvEvents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvEvents.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEvents.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvEvents.RowTemplate.Height = 60;
            dgvEvents.RowHeadersVisible = false;
            dgvEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - SAME color as default cells for consistency
            dgvEvents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvEvents.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvEvents.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
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
                HeaderText = "Event Date",
                ReadOnly = true,
                FillWeight = 14
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "time",
                HeaderText = "Event Schedule",
                ReadOnly = true,
                FillWeight = 15
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
                // Paint all parts except content to ensure consistent borders
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Check if this is a placeholder row (id will be 0 or null)
                var idValue = dgvEvents.Rows[e.RowIndex].Cells["id"].Value;
                if (idValue == null || Convert.ToInt32(idValue) == 0)
                {
                    // This is the placeholder row, don't draw the action button
                    e.Handled = true;
                    return;
                }

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
                
                e.Handled = true;
            }
        }

        private void LoadEvents()
        {
            try
            {
                string query = @"SELECT 
                                    id, 
                                    name, 
                                    description, 
                                    start_datetime, 
                                    end_datetime, 
                                    CASE 
                                        WHEN DATE(start_datetime) = DATE(end_datetime) THEN DATE_FORMAT(start_datetime, '%b %d, %Y')
                                        ELSE CONCAT(DATE_FORMAT(start_datetime, '%b %d'), ' - ', DATE_FORMAT(end_datetime, '%b %d, %Y'))
                                    END AS date_display,
                                    CONCAT(DATE_FORMAT(start_datetime, '%h:%i %p'), ' - ', DATE_FORMAT(end_datetime, '%h:%i %p')) AS time_display,
                                    venue, 
                                    type, 
                                    organizer 
                                FROM events 
                                ORDER BY start_datetime DESC";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                dgvEvents.Rows.Clear();

                if (dt.Rows.Count == 0)
                {
                    // Add placeholder row when no data
                    int placeholderIndex = dgvEvents.Rows.Add(
                        0, // id
                        "No events available yet", // name (placeholder message)
                        "", // date
                        "", // time
                        "", // venue
                        "", // type
                        "", // organizer
                        "", // description
                        ""  // ActionColumn
                    );

                    // Style the placeholder row
                    DataGridViewRow placeholderRow = dgvEvents.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dgvEvents.Rows.Add(
                            dr["id"],
                            dr["name"],
                            dr["date_display"],
                            dr["time_display"],
                            dr["venue"],
                            dr["type"],
                            dr["organizer"],
                            dr["description"],
                            ""
                        );
                    }
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
                
                // Check if this is a placeholder row (id will be 0 or null)
                var idValue = row.Cells["id"].Value;
                if (idValue == null || Convert.ToInt32(idValue) == 0)
                {
                    // This is the placeholder row, do nothing
                    return;
                }
                
                int eventId = Convert.ToInt32(row.Cells["id"].Value);
                string eventName = row.Cells["name"].Value?.ToString();

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

                // Add View Attendees menu item
                ToolStripMenuItem viewAttendeesItem = new ToolStripMenuItem("👥 View Attendees");
                viewAttendeesItem.Font = new Font("Segoe UI", 10F);
                viewAttendeesItem.Click += (s, ev) => ViewEventAttendees(eventId, eventName);
                contextMenuActions.Items.Add(viewAttendeesItem);

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
                Size = new Size(500, 650),
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
                BackColor = Color.Transparent,
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

            // Start Date & Time
            Label lblStartDateTime = new Label { Text = "Start Date & Time:", Location = new Point(30, 240), Size = new Size(140, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            
            Panel pnlStartDate = new Panel
            {
                Location = new Point(30, 270),
                Size = new Size(200, 32),
                BackColor = Color.FromArgb(37, 42, 69),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };
            
            DateTimePicker dtpStartDate = new DateTimePicker
            {
                Location = new Point(1, 1),
                Size = new Size(196, 28),
                Font = new Font("Segoe UI", 9F),
                CalendarMonthBackground = Color.FromArgb(37, 42, 69),
                CalendarForeColor = Color.White,
                CalendarTitleBackColor = Color.FromArgb(24, 30, 54),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray
            };
            pnlStartDate.Controls.Add(dtpStartDate);
            
            Panel pnlStartTime = new Panel
            {
                Location = new Point(250, 270),
                Size = new Size(200, 32),
                BackColor = Color.FromArgb(37, 42, 69),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };
            
            DateTimePicker dtpStartTime = new DateTimePicker
            {
                Location = new Point(1, 1),
                Size = new Size(196, 28),
                Font = new Font("Segoe UI", 9F),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                CalendarMonthBackground = Color.FromArgb(37, 42, 69),
                CalendarForeColor = Color.White,
                CalendarTitleBackColor = Color.FromArgb(24, 30, 54),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray
            };
            pnlStartTime.Controls.Add(dtpStartTime);

            // End Date & Time
            Label lblEndDateTime = new Label { Text = "End Date & Time:", Location = new Point(30, 310), Size = new Size(140, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            
            Panel pnlEndDate = new Panel
            {
                Location = new Point(30, 340),
                Size = new Size(200, 32),
                BackColor = Color.FromArgb(37, 42, 69),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };
            
            DateTimePicker dtpEndDate = new DateTimePicker
            {
                Location = new Point(1, 1),
                Size = new Size(196, 28),
                Font = new Font("Segoe UI", 9F),
                CalendarMonthBackground = Color.FromArgb(37, 42, 69),
                CalendarForeColor = Color.White,
                CalendarTitleBackColor = Color.FromArgb(24, 30, 54),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray
            };
            pnlEndDate.Controls.Add(dtpEndDate);
            
            Panel pnlEndTime = new Panel
            {
                Location = new Point(250, 340),
                Size = new Size(200, 32),
                BackColor = Color.FromArgb(37, 42, 69),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };
            
            DateTimePicker dtpEndTime = new DateTimePicker
            {
                Location = new Point(1, 1),
                Size = new Size(196, 28),
                Font = new Font("Segoe UI", 9F),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                CalendarMonthBackground = Color.FromArgb(37, 42, 69),
                CalendarForeColor = Color.White,
                CalendarTitleBackColor = Color.FromArgb(24, 30, 54),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray
            };
            pnlEndTime.Controls.Add(dtpEndTime);

            // Venue
            Label lblVenue = new Label { Text = "Venue:", Location = new Point(30, 380), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtVenue = new TextBox { Location = new Point(30, 410), Size = new Size(420, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            // Type
            Label lblType = new Label { Text = "Type:", Location = new Point(30, 450), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            ComboBox cboType = new ComboBox
            {
                Location = new Point(30, 480),
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
                cboType.SelectedIndex = 0;
            }

            // Organizer
            Label lblOrganizer = new Label { Text = "Organizer:", Location = new Point(250, 450), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtOrganizer = new TextBox { Location = new Point(250, 480), Size = new Size(200, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            // Buttons with rounded corners
            RoundedButton btnSave = new RoundedButton
            {
                Text = eventData == null ? "Add Event" : "Update Event",
                Location = new Point(150, 540),
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
                Location = new Point(280, 540),
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
                
                DateTime startDateTime = Convert.ToDateTime(eventData["start_datetime"]);
                DateTime endDateTime = Convert.ToDateTime(eventData["end_datetime"]);
                
                dtpStartDate.Value = startDateTime.Date;
                dtpStartTime.Value = startDateTime;
                dtpEndDate.Value = endDateTime.Date;
                dtpEndTime.Value = endDateTime;
                
                txtVenue.Text = eventData["venue"].ToString();
                cboType.Text = eventData["type"].ToString();
                txtOrganizer.Text = eventData["organizer"].ToString();
            }
            else
            {
                // Set default end time to 2 hours after start time
                dtpStartTime.ValueChanged += (s, ev) =>
                {
                    dtpEndDate.Value = dtpStartDate.Value;
                    dtpEndTime.Value = dtpStartTime.Value.AddHours(2);
                };
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

                    // Combine date and time
                    DateTime startDateTime = dtpStartDate.Value.Date.Add(dtpStartTime.Value.TimeOfDay);
                    DateTime endDateTime = dtpEndDate.Value.Date.Add(dtpEndTime.Value.TimeOfDay);

                    // Validate that end datetime is after start datetime
                    if (endDateTime <= startDateTime)
                    {
                        MessageBox.Show("End date/time must be after start date/time.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (eventData == null)
                    {
                        // Show confirmation dialog before adding event
                        DialogResult confirmResult = MessageBox.Show(
                            $"Do you want to add this event?\n\n" +
                            $"Event Name: {txtName.Text}\n" +
                            $"Start: {startDateTime.ToString("MMM dd, yyyy h:mm tt")}\n" +
                            $"End: {endDateTime.ToString("MMM dd, yyyy h:mm tt")}\n" +
                            $"Venue: {txtVenue.Text}",
                            "Confirm Add Event",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            // Add new event
                            string query = @"INSERT INTO events (name, description, start_datetime, end_datetime, venue, type, organizer)
                                             VALUES (@name, @description, @start_datetime, @end_datetime, @venue, @type, @organizer)";
                            MySqlParameter[] parameters = {
                                new MySqlParameter("@name", txtName.Text),
                                new MySqlParameter("@description", txtDesc.Text),
                                new MySqlParameter("@start_datetime", startDateTime),
                                new MySqlParameter("@end_datetime", endDateTime),
                                new MySqlParameter("@venue", txtVenue.Text),
                                new MySqlParameter("@type", cboType.Text),
                                new MySqlParameter("@organizer", txtOrganizer.Text)
                            };
                            DatabaseHelper.ExecuteNonQuery(query, parameters);
                            MessageBox.Show("Event added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEvents();
                            eventForm.Close();
                        }
                    }
                    else
                    {
                        // Show confirmation dialog before updating event
                        DialogResult confirmResult = MessageBox.Show(
                            $"Do you want to update this event?\n\n" +
                            $"Event Name: {txtName.Text}\n" +
                            $"Start: {startDateTime.ToString("MMM dd, yyyy h:mm tt")}\n" +
                            $"End: {endDateTime.ToString("MMM dd, yyyy h:mm tt")}\n" +
                            $"Venue: {txtVenue.Text}",
                            "Confirm Update Event",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            // Update existing event
                            string query = @"UPDATE events SET name=@name, description=@description, start_datetime=@start_datetime, 
                                             end_datetime=@end_datetime, venue=@venue, type=@type, organizer=@organizer WHERE id=@id";
                            MySqlParameter[] parameters = {
                                new MySqlParameter("@id", eventData["id"]),
                                new MySqlParameter("@name", txtName.Text),
                                new MySqlParameter("@description", txtDesc.Text),
                                new MySqlParameter("@start_datetime", startDateTime),
                                new MySqlParameter("@end_datetime", endDateTime),
                                new MySqlParameter("@venue", txtVenue.Text),
                                new MySqlParameter("@type", cboType.Text),
                                new MySqlParameter("@organizer", txtOrganizer.Text)
                            };
                            DatabaseHelper.ExecuteNonQuery(query, parameters);
                            MessageBox.Show("Event updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEvents();
                            eventForm.Close();
                        }
                    }
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
            eventForm.Controls.Add(lblStartDateTime);
            eventForm.Controls.Add(pnlStartDate);
            eventForm.Controls.Add(pnlStartTime);
            eventForm.Controls.Add(lblEndDateTime);
            eventForm.Controls.Add(pnlEndDate);
            eventForm.Controls.Add(pnlEndTime);
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

        private void ViewEventAttendees(int eventId, string eventName)
        {
            try
            {
                frmEventAttendees attendeesForm = new frmEventAttendees(eventId, eventName);
                attendeesForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening attendees view: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

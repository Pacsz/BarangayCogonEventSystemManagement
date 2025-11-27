using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using FontAwesome.Sharp;

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
        private TextBox txtSearch;
        private ComboBox cboTypeFilter;
        
        // Status cards
        private Panel pnlUpcomingCard;
        private Panel pnlOngoingCard;
        private Panel pnlEndedCard;

        public frmManageEvents()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(46, 51, 73); // Match main panel background
            InitializeContextMenu();
            CreateStatusCards();
            InitializeFilters();
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

        private void CreateStatusCards()
        {
            // Calculate card positions to maximize width across the form
            // Position cards below the filters (Y=55 to be on same row as dgvEvents starts)
            int spacing = 15;
            int cardY = 55;
            int startX = 20;
            
            // Calculate card width to maximize space: (form width - left margin - right margin - 2 spacings) / 3 cards
            int availableWidth = this.ClientSize.Width - 40; // Total width minus left and right margins
            int cardWidth = (availableWidth - (spacing * 2)) / 3; // Divide by 3 cards, subtract spacing
            int cardHeight = 100;

            // Upcoming Events Card (Blue)
            pnlUpcomingCard = CreateStatCard(startX, cardY, cardWidth, cardHeight, 
                Color.FromArgb(0, 126, 249), IconChar.CalendarPlus, "Upcoming", "0");
            
            // Ongoing Events Card (Orange)
            pnlOngoingCard = CreateStatCard(startX + cardWidth + spacing, cardY, cardWidth, cardHeight,
                Color.FromArgb(255, 152, 0), IconChar.Clock, "Ongoing", "0");
            
            // Ended Events Card (Green)
            pnlEndedCard = CreateStatCard(startX + (cardWidth + spacing) * 2, cardY, cardWidth, cardHeight,
                Color.FromArgb(76, 175, 80), IconChar.CheckCircle, "Ended", "0");
            
            this.Controls.Add(pnlUpcomingCard);
            this.Controls.Add(pnlOngoingCard);
            this.Controls.Add(pnlEndedCard);
            
            pnlUpcomingCard.BringToFront();
            pnlOngoingCard.BringToFront();
            pnlEndedCard.BringToFront();
        }

        private Panel CreateStatCard(int x, int y, int width, int height, Color iconColor, IconChar icon, string labelText, string countText)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.FromArgb(37, 42, 64)
            };

            IconPictureBox iconBox = new IconPictureBox
            {
                Location = new Point(15, 25),
                Size = new Size(35, 35),
                IconChar = icon,
                IconColor = iconColor,
                IconSize = 35,
                BackColor = Color.Transparent
            };

            Label lblCount = new Label
            {
                Location = new Point(60, 15),
                Size = new Size(width - 75, 40), // Adjust width based on card width
                Text = countText,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Tag = "count"
            };

            Label lblLabel = new Label
            {
                Location = new Point(60, 55),
                Size = new Size(width - 75, 25), // Adjust width based on card width
                Text = labelText,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(158, 161, 178),
                BackColor = Color.Transparent
            };

            card.Controls.Add(iconBox);
            card.Controls.Add(lblCount);
            card.Controls.Add(lblLabel);

            return card;
        }

        private void UpdateStatusCards(int upcoming, int ongoing, int ended)
        {
            foreach (Control c in pnlUpcomingCard.Controls)
                if (c is Label l && l.Tag?.ToString() == "count") l.Text = upcoming.ToString();
            
            foreach (Control c in pnlOngoingCard.Controls)
                if (c is Label l && l.Tag?.ToString() == "count") l.Text = ongoing.ToString();
            
            foreach (Control c in pnlEndedCard.Controls)
                if (c is Label l && l.Tag?.ToString() == "count") l.Text = ended.ToString();
        }

        private void InitializeFilters()
        {
            // Search box
            txtSearch = new TextBox
            {
                Location = new Point(20, 15),
                Size = new Size(300, 30),
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
            txtSearch.TextChanged += (s, ev) => LoadEvents();

            // Type filter label
            Label lblFilter = new Label
            {
                Text = "Type:",
                Location = new Point(340, 18),
                Size = new Size(50, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Type filter dropdown
            cboTypeFilter = new ComboBox
            {
                Location = new Point(395, 15),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTypeFilter.Items.AddRange(new object[] { "All Types", "Community Service", "Health Drive", "Cleanup Drive", "Barangay Assembly", "Training / Workshop" });
            cboTypeFilter.SelectedIndex = 0;
            cboTypeFilter.SelectedIndexChanged += (s, ev) => LoadEvents();

            this.Controls.Add(txtSearch);
            this.Controls.Add(lblFilter);
            this.Controls.Add(cboTypeFilter);

            // Ensure controls are brought to front
            txtSearch.BringToFront();
            lblFilter.BringToFront();
            cboTypeFilter.BringToFront();

            // Adjust dgvEvents position to accommodate status cards below filters
            if (dgvEvents != null)
            {
                dgvEvents.Location = new Point(20, 170);  // Moved down to be below status cards (55 + 100 + 15 spacing)
                dgvEvents.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 230);
            }

            // btnAddEvent position is now controlled by Designer (anchored to bottom-right)
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
                Name = "start_datetime",
                HeaderText = "Start DateTime",
                ReadOnly = true,
                Visible = false
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "end_datetime",
                HeaderText = "End DateTime",
                ReadOnly = true,
                Visible = false
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "name",
                HeaderText = "Event Name",
                ReadOnly = true,
                FillWeight = 18
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "date",
                HeaderText = "Event Date",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "time",
                HeaderText = "Schedule",
                ReadOnly = true,
                FillWeight = 13
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "venue",
                HeaderText = "Venue",
                ReadOnly = true,
                FillWeight = 13
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "type",
                HeaderText = "Type",
                ReadOnly = true,
                FillWeight = 13
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "organizer",
                HeaderText = "Organizer",
                ReadOnly = true,
                FillWeight = 13
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                ReadOnly = true,
                FillWeight = 10
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "description",
                HeaderText = "Description",
                ReadOnly = true,
                FillWeight = 13
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 10
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
                                WHERE 1=1";

                var paramsList = new System.Collections.Generic.List<MySqlParameter>();

                // Add type filter
                if (cboTypeFilter != null && cboTypeFilter.SelectedIndex > 0)
                {
                    query += " AND type = @type";
                    paramsList.Add(new MySqlParameter("@type", cboTypeFilter.SelectedItem.ToString()));
                }

                // Add search filter
                if (txtSearch != null)
                {
                    string searchText = txtSearch.Text;
                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search events...")
                    {
                        query += @" AND (name LIKE @search 
                                    OR venue LIKE @search 
                                    OR type LIKE @search
                                    OR organizer LIKE @search)";
                        paramsList.Add(new MySqlParameter("@search", "%" + searchText + "%"));
                    }
                }

                query += " ORDER BY start_datetime DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());

                dgvEvents.Rows.Clear();

                // Calculate status counts
                int upcomingCount = 0;
                int ongoingCount = 0;
                int endedCount = 0;
                DateTime now = DateTime.Now;

                if (dt.Rows.Count == 0)
                {
                    // Update status cards with zeros
                    UpdateStatusCards(0, 0, 0);

                    // Add placeholder row when no data
                    int placeholderIndex = dgvEvents.Rows.Add(
                        0, // id
                        null, // start_datetime
                        null, // end_datetime
                        "No events found matching your criteria", // name (placeholder message)
                        "", // date
                        "", // time
                        "", // venue
                        "", // type
                        "", // organizer
                        "", // status
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
                        DateTime startDateTime = Convert.ToDateTime(dr["start_datetime"]);
                        DateTime endDateTime = Convert.ToDateTime(dr["end_datetime"]);
                        
                        // Determine status
                        string status;
                        Color statusColor;
                        
                        if (now < startDateTime)
                        {
                            status = "🔵 Upcoming";
                            statusColor = Color.FromArgb(0, 126, 249); // Blue
                            upcomingCount++;
                        }
                        else if (now >= startDateTime && now <= endDateTime)
                        {
                            status = "🟠 Ongoing";
                            statusColor = Color.FromArgb(255, 152, 0); // Orange
                            ongoingCount++;
                        }
                        else
                        {
                            status = "🟢 Ended";
                            statusColor = Color.FromArgb(76, 175, 80); // Green
                            endedCount++;
                        }

                        int rowIndex = dgvEvents.Rows.Add(
                            dr["id"],
                            dr["start_datetime"],
                            dr["end_datetime"],
                            dr["name"],
                            dr["date_display"],
                            dr["time_display"],
                            dr["venue"],
                            dr["type"],
                            dr["organizer"],
                            status,
                            dr["description"],
                            ""
                        );

                        // Style the status cell with color
                        dgvEvents.Rows[rowIndex].Cells["status"].Style.ForeColor = statusColor;
                        dgvEvents.Rows[rowIndex].Cells["status"].Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    }

                    // Update status cards
                    UpdateStatusCards(upcomingCount, ongoingCount, endedCount);
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

            // ✅ Current Date & Time (single reference point)
            DateTime now = DateTime.Now;

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
            Label lblName = new Label
            {
                Text = "Event Name:",
                Location = new Point(30, 70),
                Size = new Size(120, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            TextBox txtName = new TextBox
            {
                Location = new Point(30, 100),
                Size = new Size(420, 30),
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(37, 42, 69),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Description
            Label lblDesc = new Label
            {
                Text = "Description:",
                Location = new Point(30, 140),
                Size = new Size(120, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            TextBox txtDesc = new TextBox
            {
                Location = new Point(30, 170),
                Size = new Size(420, 60),
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(37, 42, 69),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true
            };

            // Start Date & Time
            Label lblStartDateTime = new Label
            {
                Text = "Start Date & Time:",
                Location = new Point(30, 240),
                Size = new Size(140, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

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
                Value = now.Date
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
                Value = now
            };
            pnlStartTime.Controls.Add(dtpStartTime);

            // End Date & Time
            Label lblEndDateTime = new Label
            {
                Text = "End Date & Time:",
                Location = new Point(30, 310),
                Size = new Size(140, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

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
                Value = now.Date
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
                Value = now.AddHours(2)
            };
            pnlEndTime.Controls.Add(dtpEndTime);

            // Venue
            Label lblVenue = new Label
            {
                Text = "Venue:",
                Location = new Point(30, 380),
                Size = new Size(120, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            TextBox txtVenue = new TextBox
            {
                Location = new Point(30, 410),
                Size = new Size(420, 30),
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(37, 42, 69),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Type
            Label lblType = new Label
            {
                Text = "Type:",
                Location = new Point(30, 450),
                Size = new Size(120, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

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

            cboType.Items.AddRange(new object[]
            {
        "Community Service",
        "Health Drive",
        "Cleanup Drive",
        "Barangay Assembly",
        "Training / Workshop"
            });

            if (eventData == null)
                cboType.SelectedIndex = 0;

            // Organizer
            Label lblOrganizer = new Label
            {
                Text = "Organizer:",
                Location = new Point(250, 450),
                Size = new Size(120, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            TextBox txtOrganizer = new TextBox
            {
                Location = new Point(250, 480),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(37, 42, 69),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Buttons
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
                dtpStartTime.ValueChanged += (s, ev) =>
                {
                    dtpEndDate.Value = dtpStartDate.Value;
                    dtpEndTime.Value = dtpStartTime.Value.AddHours(2);
                };
            }

            // SAVE LOGIC
            btnSave.Click += (s, ev) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Event name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtDesc.Text))
                    {
                        MessageBox.Show("Event description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtVenue.Text))
                    {
                        MessageBox.Show("Event venue is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtOrganizer.Text))
                    {
                        MessageBox.Show("Event organizer is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DateTime startDateTime = dtpStartDate.Value.Date.Add(dtpStartTime.Value.TimeOfDay);
                    DateTime endDateTime = dtpEndDate.Value.Date.Add(dtpEndTime.Value.TimeOfDay);

                    // ❌ PAST VALIDATION
                    if (startDateTime < now)
                    {
                        MessageBox.Show("You cannot schedule an event in the past.\n\nPlease choose a future date and time.",
                                        "Invalid Start Time",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }

                    if (endDateTime < now)
                    {
                        MessageBox.Show("The event end date/time has already passed.\n\nPlease choose a future end date/time.",
                                        "Invalid End Time",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }

                    // ❌ LOGICAL ORDER VALIDATION
                    if (endDateTime <= startDateTime)
                    {
                        MessageBox.Show("End date/time must be later than the start date/time.",
                                        "Invalid Time Range",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }

                    if (eventData == null)
                    {
                        DialogResult confirmResult = MessageBox.Show(
                            $"Confirm Event Creation:\n\n" +
                            $"Name: {txtName.Text}\n" +
                            $"Start: {startDateTime:MMM dd, yyyy h:mm tt}\n" +
                            $"End: {endDateTime:MMM dd, yyyy h:mm tt}\n" +
                            $"Venue: {txtVenue.Text}",
                            "Confirm Add",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            string query = @"INSERT INTO events 
                                    (name, description, start_datetime, end_datetime, venue, type, organizer)
                                    VALUES (@name, @description, @start_datetime, @end_datetime, @venue, @type, @organizer)";

                            MySqlParameter[] parameters =
                            {
                        new MySqlParameter("@name", txtName.Text),
                        new MySqlParameter("@description", txtDesc.Text),
                        new MySqlParameter("@start_datetime", startDateTime),
                        new MySqlParameter("@end_datetime", endDateTime),
                        new MySqlParameter("@venue", txtVenue.Text),
                        new MySqlParameter("@type", cboType.Text),
                        new MySqlParameter("@organizer", txtOrganizer.Text)
                    };

                            DatabaseHelper.ExecuteNonQuery(query, parameters);

                            MessageBox.Show("Event added successfully!",
                                            "Success",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                            LoadEvents();
                            eventForm.Close();
                        }
                    }
                    else
                    {
                        DialogResult confirmResult = MessageBox.Show(
                            $"Confirm Event Update:\n\n" +
                            $"Name: {txtName.Text}\n" +
                            $"Start: {startDateTime:MMM dd, yyyy h:mm tt}\n" +
                            $"End: {endDateTime:MMM dd, yyyy h:mm tt}\n" +
                            $"Venue: {txtVenue.Text}",
                            "Confirm Update",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            string query = @"UPDATE events 
                                    SET name=@name, description=@description,
                                        start_datetime=@start_datetime,
                                        end_datetime=@end_datetime,
                                        venue=@venue, type=@type, organizer=@organizer
                                    WHERE id=@id";

                            MySqlParameter[] parameters =
                            {
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

                            MessageBox.Show("Event updated successfully!",
                                            "Success",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                            LoadEvents();
                            eventForm.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving event: " + ex.Message,
                                    "System Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
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
            try
            {
                // Get event details and count of related records before deletion
                string countQuery = @"SELECT 
                                        e.name AS event_name,
                                        (SELECT COUNT(*) FROM registrations WHERE event_id = @id) AS registration_count,
                                        (SELECT COUNT(*) FROM attendance a 
                                         INNER JOIN registrations r ON a.registration_id = r.id 
                                         WHERE r.event_id = @id) AS attendance_count,
                                        (SELECT COUNT(*) FROM reports WHERE event_id = @id) AS report_count
                                      FROM events e 
                                      WHERE e.id = @id";
                
                MySqlParameter[] countParams = { new MySqlParameter("@id", eventId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(countQuery, countParams);
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Event not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                string eventName = dt.Rows[0]["event_name"].ToString();
                int registrationCount = Convert.ToInt32(dt.Rows[0]["registration_count"]);
                int attendanceCount = Convert.ToInt32(dt.Rows[0]["attendance_count"]);
                int reportCount = Convert.ToInt32(dt.Rows[0]["report_count"]);
                
                // Build warning message
                string warningMessage = $"Are you sure you want to delete this event?\n\n" +
                                       $"Event: {eventName}\n\n" +
                                       $"⚠️ WARNING: This will also permanently delete:\n" +
                                       $"  • {registrationCount} registration(s)\n" +
                                       $"  • {attendanceCount} attendance record(s)\n" +
                                       $"  • {reportCount} report(s)\n\n" +
                                       $"This action cannot be undone!";
                
                DialogResult result = MessageBox.Show(warningMessage, "Confirm Delete", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM events WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", eventId) };
                    DatabaseHelper.ExecuteNonQuery(query, parameters);
                    
                    MessageBox.Show(
                        $"Event '{eventName}' and all related records deleted successfully!\n\n" +
                        $"Deleted:\n" +
                        $"  • 1 event\n" +
                        $"  • {registrationCount} registration(s)\n" +
                        $"  • {attendanceCount} attendance record(s)\n" +
                        $"  • {reportCount} report(s)", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting event: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

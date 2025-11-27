using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using FontAwesome.Sharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmReports : Form
    {
        // Stat Cards at top
        private Panel pnlTotalEventsCard;
        private Panel pnlTotalRegisteredCard;
        private Panel pnlAttendanceRateCard;
        private Panel pnlPendingCard;
        
        // Summary section
        private Panel pnlSummaryContainer;
        private Label lblSummaryData;

        // Filters
        private ComboBox cboTypeFilter;
        
        public frmReports()
        {
            InitializeComponent();
            CreateBeautifulUI();
            CustomizeDataGridView();
            StyleButtons();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void CreateBeautifulUI()
        {
            // Hide designer labels
            if (lblSummary != null) lblSummary.Visible = false;
            if (lblTotals != null) lblTotals.Visible = false;

            // === LEFT CONTAINER (70% width) - Cards + Table ===
            Panel pnlLeftContainer = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(800, 680),
                BackColor = Color.Transparent
            };

            // Initialize the filter at the top (BEFORE the cards)
            InitializeFilters(pnlLeftContainer);

            // === 4 STAT CARDS - MOVED DOWN TO Y=45 ===
            pnlTotalEventsCard = CreateStatCard(0, 45, Color.FromArgb(0, 126, 249), 
                IconChar.CalendarAlt, "Total Events", "0");
            pnlTotalRegisteredCard = CreateStatCard(205, 45, Color.FromArgb(76, 175, 80), 
                IconChar.Users, "Registered", "0");
            pnlAttendanceRateCard = CreateStatCard(410, 45, Color.FromArgb(255, 152, 0), 
                IconChar.ChartLine, "Attendance", "0%");
            pnlPendingCard = CreateStatCard(615, 45, Color.FromArgb(244, 67, 54), 
                IconChar.Clock, "Pending", "0");

            pnlLeftContainer.Controls.Add(pnlTotalEventsCard);
            pnlLeftContainer.Controls.Add(pnlTotalRegisteredCard);
            pnlLeftContainer.Controls.Add(pnlAttendanceRateCard);
            pnlLeftContainer.Controls.Add(pnlPendingCard);

            // Table - adjusted position to be below the cards (45 + 110 + 15 spacing = 170)
            if (dgvReports != null)
            {
                dgvReports.Parent = pnlLeftContainer;
                dgvReports.Location = new Point(0, 170);
                dgvReports.Size = new Size(800, 450);
            }

            // Export PDF button - adjusted position
            if (btnExportPDF != null)
            {
                btnExportPDF.Parent = pnlLeftContainer;
                btnExportPDF.Location = new Point(640, 630);
                btnExportPDF.Size = new Size(160, 50);
            }

            // Hide refresh button
            if (btnRefresh != null)
            {
                btnRefresh.Visible = false;
            }

            this.Controls.Add(pnlLeftContainer);
            pnlLeftContainer.BringToFront();

            // === RIGHT CONTAINER - MAXIMIZED HEIGHT + BIGGER FONTS ===
            pnlSummaryContainer = new Panel
            {
                Location = new Point(840, 20),
                Size = new Size(290, 680),
                BackColor = Color.FromArgb(37, 42, 64)
            };

            // Title with icon - bigger
            IconPictureBox iconSummary = new IconPictureBox
            {
                Location = new Point(15, 18),
                Size = new Size(32, 32),
                IconChar = IconChar.ChartPie,
                IconColor = Color.FromArgb(0, 126, 249),
                IconSize = 32,
                BackColor = Color.Transparent
            };

            Label lblSummaryTitle = new Label
            {
                Location = new Point(55, 18),
                Size = new Size(220, 35),
                Text = "DETAILED BREAKDOWN",
                Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 126, 249),
                BackColor = Color.Transparent
            };

            // Separator line
            Panel separator = new Panel
            {
                Location = new Point(15, 58),
                Size = new Size(260, 2),
                BackColor = Color.FromArgb(60, 65, 90)
            };

            // Summary data label - BIGGER FONTS
            lblSummaryData = new Label
            {
                Location = new Point(15, 75),
                Size = new Size(260, 590),
                BackColor = Color.Transparent,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                ForeColor = Color.White,
                Text = "Loading...",
                AutoSize = false
            };

            pnlSummaryContainer.Controls.Add(iconSummary);
            pnlSummaryContainer.Controls.Add(lblSummaryTitle);
            pnlSummaryContainer.Controls.Add(separator);
            pnlSummaryContainer.Controls.Add(lblSummaryData);

            this.Controls.Add(pnlSummaryContainer);
            pnlSummaryContainer.BringToFront();
        }

        private Panel CreateStatCard(int x, int y, Color iconColor, IconChar icon, string labelText, string countText)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(195, 110),
                BackColor = Color.FromArgb(37, 42, 64)
            };

            IconPictureBox iconBox = new IconPictureBox
            {
                Location = new Point(15, 30),
                Size = new Size(40, 40),
                IconChar = icon,
                IconColor = iconColor,
                IconSize = 40,
                BackColor = Color.Transparent
            };

            Label lblCount = new Label
            {
                Location = new Point(60, 20),
                Size = new Size(125, 45),
                Text = countText,
                Font = new System.Drawing.Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Tag = "count"
            };

            Label lblLabel = new Label
            {
                Location = new Point(60, 65),
                Size = new Size(125, 25),
                Text = labelText,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(158, 161, 178),
                BackColor = Color.Transparent
            };

            card.Controls.Add(iconBox);
            card.Controls.Add(lblCount);
            card.Controls.Add(lblLabel);

            return card;
        }

        private void UpdateStatCards(int events, int registered, double rate, int pending)
        {
            foreach (Control c in pnlTotalEventsCard.Controls)
                if (c is Label l && l.Tag?.ToString() == "count") l.Text = events.ToString();
            
            foreach (Control c in pnlTotalRegisteredCard.Controls)
                if (c is Label l && l.Tag?.ToString() == "count") l.Text = registered.ToString();
            
            foreach (Control c in pnlAttendanceRateCard.Controls)
                if (c is Label l && l.Tag?.ToString() == "count") l.Text = rate.ToString("F1") + "%";
            
            foreach (Control c in pnlPendingCard.Controls)
                if (c is Label l && l.Tag?.ToString() == "count") l.Text = pending.ToString();
        }

        private void CustomizeDataGridView()
        {
            dgvReports.Columns.Clear();
            dgvReports.AllowUserToAddRows = false;
            dgvReports.ReadOnly = true;

            // GENERAL GRID SETTINGS - Match user dashboard style
            dgvReports.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvReports.BorderStyle = BorderStyle.None;
            dgvReports.GridColor = Color.FromArgb(60, 65, 90);
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE
            dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReports.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvReports.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvReports.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvReports.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvReports.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvReports.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvReports.DefaultCellStyle.ForeColor = Color.White;
            dgvReports.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvReports.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReports.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            dgvReports.RowTemplate.Height = 60;
            dgvReports.RowHeadersVisible = false;
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - SAME color as default cells for consistency
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvReports.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvReports.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvReports.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering to reduce flicker
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvReports, new object[] { true });

            // Ensure any existing handlers are not duplicated
            dgvReports.CellPainting -= dgvReports_CellPainting;
            dgvReports.CellClick -= dgvReports_ActionCellClick;
            dgvReports.CellPainting += dgvReports_CellPainting;
            dgvReports.CellClick += dgvReports_ActionCellClick;
       }

        private void StyleButtons()
        {
            // Style Export PDF button
            btnExportPDF.FlatStyle = FlatStyle.Flat;
            btnExportPDF.FlatAppearance.BorderSize = 0;
            btnExportPDF.BackColor = Color.FromArgb(0, 126, 249); // Accent blue
            btnExportPDF.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportPDF.Cursor = Cursors.Hand;
            btnExportPDF.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
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

            // Style Refresh button
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.BackColor = Color.FromArgb(60, 65, 90); // Subtle gray-blue
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
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
        }

        private GraphicsPath GetRoundPath(System.Drawing.Rectangle rect, int radius)
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

        private void LoadReports()
        {
            try
            {
                string query = @"
                SELECT 
                    e.id, 
                    e.name AS 'Event Name', 
                    e.start_datetime AS 'Start DateTime', 
                    e.end_datetime AS 'End DateTime',
                    e.type AS 'Type',
                    CASE 
                        WHEN DATE(e.start_datetime) = DATE(e.end_datetime) THEN DATE_FORMAT(e.start_datetime, '%b %d, %Y')
                        ELSE CONCAT(DATE_FORMAT(e.start_datetime, '%b %d'), ' - ', DATE_FORMAT(e.end_datetime, '%b %d, %Y'))
                    END AS 'date_display',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.role = 'attendee' AND r.status IN ('Approved', 'Checked-in', 'Attended', ""Didn't Attend"")) AS 'Attendees',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.role = 'volunteer' AND r.status IN ('Approved', 'Checked-in', 'Attended', ""Didn't Attend"")) AS 'Volunteers',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.role = 'speaker' AND r.status IN ('Approved', 'Checked-in', 'Attended', ""Didn't Attend"")) AS 'Speakers',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.status IN ('Approved', 'Checked-in', 'Attended', ""Didn't Attend"")) AS 'Registered',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.status = 'Attended') AS 'Attended',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.status = ""Didn't Attend"") AS 'DidntAttend',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.status = 'Pending') AS 'Pending'
                FROM events e 
                WHERE 1=1";

                var paramsList = new System.Collections.Generic.List<MySqlParameter>();

                // Add type filter
                if (cboTypeFilter != null && cboTypeFilter.SelectedIndex > 0)
                {
                    query += " AND e.type = @type";
                    paramsList.Add(new MySqlParameter("@type", cboTypeFilter.SelectedItem.ToString()));
                }

                query += " ORDER BY e.start_datetime DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());
                dgvReports.Rows.Clear();

                // Table with clean headers (no icons)
                if (dgvReports.Columns.Count == 0)
                {
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn { Name = "id", Visible = false });
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn { Name = "event_name", HeaderText = "Event Name", FillWeight = 26 });
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn { Name = "date", HeaderText = "Event Date", FillWeight = 15 });
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn { Name = "type", HeaderText = "Type", FillWeight = 18 });
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn { Name = "registered", HeaderText = "Registered", FillWeight = 12 });
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn { Name = "attended", HeaderText = "Attended", FillWeight = 12 });
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn { Name = "rate", HeaderText = "Attendance %", FillWeight = 17 });

                    // Add Action column for opening attendee details (painted button)
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn { Name = "ActionColumn", HeaderText = "Action", FillWeight = 12 });
                }

                // Ensure painted action button and click handler are attached (already in CustomizeDataGridView but ensure here as well)
                dgvReports.CellPainting -= dgvReports_CellPainting;
                dgvReports.CellPainting += dgvReports_CellPainting;
                dgvReports.CellClick -= dgvReports_ActionCellClick;
                dgvReports.CellClick += dgvReports_ActionCellClick;

                int totalEvents = dt.Rows.Count;
                int totalAttendees = 0, totalVolunteers = 0, totalSpeakers = 0, totalRegistered = 0, totalAttended = 0, totalDidntAttend = 0, totalPending = 0, completed = 0;

                // Check if there's data
                if (dt.Rows.Count == 0)
                {
                    // Add placeholder row when no data
                    int placeholderIndex = dgvReports.Rows.Add(
                        0, // id
                        "No events found matching your criteria", // event_name (placeholder message)
                        "", // date
                        "", // type
                        "", // registered
                        "", // attended
                        "",  // rate
                        ""   // action
                    );

                    // Style the placeholder row
                    DataGridViewRow placeholderRow = dgvReports.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // Update stat cards with zeros
                    UpdateStatCards(0, 0, 0, 0);

                    // Show empty summary
                    lblSummaryData.Text = 
                        $"📅 EVENTS\n" +
                        $"  • Total: 0\n" +
                        $"  • Completed: 0\n" +
                        $"  • Upcoming: 0\n\n" +
                        $"👥 PARTICIPANTS\n" +
                        $"  • Attendees: 0\n" +
                        $"  • Volunteers: 0\n" +
                        $"  • Speakers: 0\n" +
                        $"  • Total Registered: 0\n\n" +
                        $"✓ ATTENDANCE\n" +
                        $"  • Attended: 0\n" +
                        $"  • Didn't Attend: 0\n" +
                        $"  • Overall Rate: 0.0%\n\n" +
                        $"⏳ PENDING\n" +
                        $"  • Awaiting Approval: 0\n\n" +
                        $"📈 AVERAGES\n" +
                        $"  • Attendees/Event: 0.0\n" +
                        $"  • Volunteers/Event: 0.0\n" +
                        $"  • Speakers/Event: 0.0\n" +
                        $"  • Registered/Event: 0.0";
                }
                else
                {
                    // Calculate totals from the FILTERED event results
                    foreach (DataRow dr in dt.Rows)
                    {
                        int attendees = Convert.ToInt32(dr["Attendees"]);
                        int volunteers = Convert.ToInt32(dr["Volunteers"]);
                        int speakers = Convert.ToInt32(dr["Speakers"]);
                        int registered = Convert.ToInt32(dr["Registered"]);
                        int attended = Convert.ToInt32(dr["Attended"]);
                        int didntAttend = Convert.ToInt32(dr["DidntAttend"]);
                        int pending = Convert.ToInt32(dr["Pending"]);

                        totalAttendees += attendees;
                        totalVolunteers += volunteers;
                        totalSpeakers += speakers;
                        totalRegistered += registered;
                        totalAttended += attended;
                        totalDidntAttend += didntAttend;
                        totalPending += pending;

                        DateTime eventDate = Convert.ToDateTime(dr["Start DateTime"]);
                        if (eventDate < DateTime.Now) completed++;

                        // Calculate attendance rate based on Attended / Registered
                        double rate = registered > 0 ? ((double)attended / registered) * 100 : 0;
                        string rateStr = rate > 0 ? (rate >= 90 ? "🟢 " : rate >= 70 ? "🟡 " : "🔴 ") + rate.ToString("F1") + "%" : "N/A";

                        int idx = dgvReports.Rows.Add(dr["id"], dr["Event Name"], dr["date_display"], 
                            dr["Type"], registered, attended, rateStr, "");

                        // Color rows by performance
                        if (rate >= 90)
                        {
                            dgvReports.Rows[idx].DefaultCellStyle.BackColor = Color.FromArgb(30, 50, 40);
                            dgvReports.Rows[idx].DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 50, 40);
                        }
                        else if (rate > 0 && rate < 70)
                        {
                            dgvReports.Rows[idx].DefaultCellStyle.BackColor = Color.FromArgb(50, 30, 35);
                            dgvReports.Rows[idx].DefaultCellStyle.SelectionBackColor = Color.FromArgb(50, 30, 35);
                        }
                    }

                    // Calculate overall rate based on Attended / Registered
                    double overallRate = totalRegistered > 0 ? ((double)totalAttended / totalRegistered) * 100 : 0;
                    UpdateStatCards(totalEvents, totalRegistered, overallRate, totalPending);

                    // Beautiful formatted summary with icons - UPDATED WITH SPEAKERS
                    lblSummaryData.Text = 
                        $"📅 EVENTS\n" +
                        $"  • Total: {totalEvents}\n" +
                        $"  • Completed: {completed}\n" +
                        $"  • Upcoming: {totalEvents - completed}\n\n" +
                        $"👥 PARTICIPANTS\n" +
                        $"  • Attendees: {totalAttendees}\n" +
                        $"  • Volunteers: {totalVolunteers}\n" +
                        $"  • Speakers: {totalSpeakers}\n" +
                        $"  • Total Registered: {totalRegistered}\n\n" +
                        $"✓ ATTENDANCE\n" +
                        $"  • Attended: {totalAttended}\n" +
                        $"  • Didn't Attend: {totalDidntAttend}\n" +
                        $"  • Overall Rate: {overallRate:F1}%\n\n" +
                        $"⏳ PENDING\n" +
                        $"  • Awaiting Approval: {totalPending}\n\n" +
                        $"📈 AVERAGES\n" +
                        $"  • Attendees/Event: {(totalEvents > 0 ? (double)totalAttendees / totalEvents : 0):F1}\n" +
                        $"  • Volunteers/Event: {(totalEvents > 0 ? (double)totalVolunteers / totalEvents : 0):F1}\n" +
                        $"  • Speakers/Event: {(totalEvents > 0 ? (double)totalSpeakers / totalEvents : 0):F1}\n" +
                        $"  • Registered/Event: {(totalEvents > 0 ? (double)totalRegistered / totalEvents : 0):F1}\n\n" +
                        // Instruction updated to use the action button
                        $"Tip: Use the 'View' action button on the right to open the attendee list for an event.";
                }

                dgvReports.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // New: paint action button in the ActionColumn
        private void dgvReports_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var actionColumn = dgvReports.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                // Check for placeholder row (id==0)
                var idVal = dgvReports.Rows[e.RowIndex].Cells["id"].Value;
                if (idVal == null || Convert.ToInt32(idVal) == 0)
                {
                    e.Handled = true;
                    return;
                }

                System.Drawing.Rectangle cellBounds = e.CellBounds;

                int buttonWidth = 70;
                int buttonHeight = 30;
                int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;
                System.Drawing.Rectangle buttonRect = new System.Drawing.Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                int radius = 10;

                using (GraphicsPath path = GetRoundPath(buttonRect, radius))
                using (SolidBrush buttonBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                using (System.Drawing.Font btnFont = new System.Drawing.Font("Segoe UI Symbol", 12F, FontStyle.Bold))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(buttonBrush, path);
                    // Use an attendees icon to indicate viewing registered users
                    e.Graphics.DrawString("👥", btnFont, textBrush, buttonRect, sf);
                }

                e.Handled = true;
            }
        }

        // New: handle clicks on the action button to open the attendee form
        private void dgvReports_ActionCellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                var actionColumn = dgvReports.Columns["ActionColumn"];
                if (actionColumn == null) return;

                if (e.ColumnIndex == actionColumn.Index)
                {
                    DataGridViewRow row = dgvReports.Rows[e.RowIndex];
                    var idVal = row.Cells["id"].Value;
                    if (idVal == null) return;

                    int eventId = 0;
                    if (!int.TryParse(idVal.ToString(), out eventId) || eventId == 0) return;

                    string eventName = row.Cells["event_name"].Value?.ToString() ?? "(Event)";

                    // Open frmEventAttendees as a dialog, hide its internal action column for a cleaner view
                    using (var frm = new frmEventAttendees(eventId, eventName, false))
                    {
                        frm.ShowDialog(this);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening attendees: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReports();
            MessageBox.Show("Reports refreshed successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvReports.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = "EventReports_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToPDF(saveFileDialog.FileName);
                    MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting PDF: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToPDF(string filePath)
        {
            // Changed from Landscape to Portrait
            Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            iTextSharp.text.Font subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE);
            iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            iTextSharp.text.Font summaryTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            iTextSharp.text.Font summaryHeadingFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            iTextSharp.text.Font summaryFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            // Title section
            Paragraph title = new Paragraph("BARANGAY COGON", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 5
            };
            doc.Add(title);

            Paragraph subtitle = new Paragraph("Event Management & Attendance Report", subtitleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 10
            };
            doc.Add(subtitle);

            doc.Add(new Paragraph($"Generated on: {DateTime.Now.ToString("MMMM dd, yyyy 'at' hh:mm tt")}", normalFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 15
            });

            // Count visible columns (exclude ActionColumn so PDF layout remains unchanged)
            int visibleColumns = 0;
            foreach (DataGridViewColumn col in dgvReports.Columns)
            {
                if (col.Visible && col.Name != "ActionColumn") visibleColumns++;
            }

            // Create table with portrait orientation
            PdfPTable table = new PdfPTable(visibleColumns)
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
                SpacingAfter = 10
            };

            // Set column widths for better layout in portrait
            float[] widths = new float[] { 2.5f, 1.2f, 1.5f, 1f, 0.8f, 1f };
            if (visibleColumns == widths.Length)
            {
                table.SetWidths(widths);
            }
            else
            {
                // fallback: distribute evenly
                float[] fallback = new float[visibleColumns];
                for (int i = 0; i < visibleColumns; i++) fallback[i] = 1f;
                table.SetWidths(fallback);
            }

            // Add headers with WHITE text (skip ActionColumn)
            foreach (DataGridViewColumn column in dgvReports.Columns)
            {
                if (column.Visible && column.Name != "ActionColumn")
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(column.HeaderText, headerFont))
                    {
                        BackgroundColor = new BaseColor(24, 30, 54),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 5,
                        BorderWidth = 1
                    };
                    table.AddCell(headerCell);
                }
            }

            // Add data rows (skip ActionColumn)
            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    var col = dgvReports.Columns[cell.ColumnIndex];
                    if (col.Visible && col.Name != "ActionColumn")
                    {
                        PdfPCell dataCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? "", normalFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 4,
                            BorderWidth = 0.5f
                        };
                        table.AddCell(dataCell);
                    }
                }
            }

            doc.Add(table);
            doc.Add(new Paragraph("\n"));

            // Summary section - 2x2 GRID LAYOUT WITH BOLD HEADINGS
            Paragraph summaryTitle = new Paragraph("SUMMARY STATISTICS", summaryTitleFont)
            {
                SpacingBefore = 10,
                SpacingAfter = 10,
                Alignment = Element.ALIGN_LEFT
            };
            doc.Add(summaryTitle);

            // Parse summary data from lblSummaryData
            string summaryText = lblSummaryData?.Text ?? "";
            
            // Extract statistics from summary text
            int totalEvents = 0, completed = 0, upcoming = 0;
            int totalAttendees = 0, totalVolunteers = 0, totalSpeakers = 0, totalRegistered = 0;
            int totalFullyAttended = 0, totalDidntAttend = 0;
            double overallRate = 0;
            int totalPending = 0;
            double avgAttendeesPerEvent = 0, avgVolunteersPerEvent = 0, avgSpeakersPerEvent = 0;

            // Parse the summary text
            string[] lines = summaryText.Split('\n');
            foreach (string line in lines)
            {
                if (line.Contains("Total:") && line.Contains("•")) totalEvents = ExtractNumber(line);
                else if (line.Contains("Completed:")) completed = ExtractNumber(line);
                else if (line.Contains("Upcoming:")) upcoming = ExtractNumber(line);
                else if (line.Contains("Attendees:") && !line.Contains("/")) totalAttendees = ExtractNumber(line);
                else if (line.Contains("Volunteers:") && !line.Contains("/")) totalVolunteers = ExtractNumber(line);
                else if (line.Contains("Speakers:") && !line.Contains("/")) totalSpeakers = ExtractNumber(line);
                else if (line.Contains("Total Registered:")) totalRegistered = ExtractNumber(line);
                else if (line.Contains("Attended:")) totalFullyAttended = ExtractNumber(line);
                else if (line.Contains("Didn't Attend:")) totalDidntAttend = ExtractNumber(line);
                else if (line.Contains("Overall Rate:")) overallRate = ExtractDouble(line);
                else if (line.Contains("Awaiting Approval:")) totalPending = ExtractNumber(line);
                else if (line.Contains("Attendees/Event:")) avgAttendeesPerEvent = ExtractDouble(line);
                else if (line.Contains("Volunteers/Event:")) avgVolunteersPerEvent = ExtractDouble(line);
                else if (line.Contains("Speakers/Event:")) avgSpeakersPerEvent = ExtractDouble(line);
            }

            double avgRegisteredPerEvent = 0;
            
            // Extract Registered/Event from summary
            foreach (string line in lines)
            {
                if (line.Contains("Registered/Event:")) avgRegisteredPerEvent = ExtractDouble(line);
            }

            // Create 2x2 grid for summary statistics
            PdfPTable summaryGrid = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 5,
                SpacingAfter = 10
            };
            summaryGrid.SetWidths(new float[] { 1f, 1f });

            // Cell 1: EVENTS
            PdfPCell eventsCell = new PdfPCell();
            eventsCell.Border = iTextSharp.text.Rectangle.BOX;
            eventsCell.Padding = 10;
            eventsCell.BorderWidth = 1f;
            eventsCell.BorderColor = new BaseColor(200, 200, 200);
            
            Paragraph eventsHeading = new Paragraph("EVENTS", summaryHeadingFont);
            eventsHeading.SpacingAfter = 8;
            eventsCell.AddElement(eventsHeading);
            eventsCell.AddElement(new Paragraph($"  • Total: {totalEvents}", summaryFont));
            eventsCell.AddElement(new Paragraph($"  • Completed: {completed}", summaryFont));
            eventsCell.AddElement(new Paragraph($"  • Upcoming: {upcoming}", summaryFont));
            summaryGrid.AddCell(eventsCell);

            // Cell 2: PARTICIPANTS
            PdfPCell participantsCell = new PdfPCell();
            participantsCell.Border = iTextSharp.text.Rectangle.BOX;
            participantsCell.Padding = 10;
            participantsCell.BorderWidth = 1f;
            participantsCell.BorderColor = new BaseColor(200, 200, 200);
            
            Paragraph participantsHeading = new Paragraph("PARTICIPANTS", summaryHeadingFont);
            participantsHeading.SpacingAfter = 8;
            participantsCell.AddElement(participantsHeading);
            participantsCell.AddElement(new Paragraph($"  • Attendees: {totalAttendees}", summaryFont));
            participantsCell.AddElement(new Paragraph($"  • Volunteers: {totalVolunteers}", summaryFont));
            participantsCell.AddElement(new Paragraph($"  • Speakers: {totalSpeakers}", summaryFont));
            participantsCell.AddElement(new Paragraph($"  • Total Registered: {totalRegistered}", summaryFont));
            summaryGrid.AddCell(participantsCell);

            // Cell 3: ATTENDANCE - UPDATED
            PdfPCell attendanceCell = new PdfPCell();
            attendanceCell.Border = iTextSharp.text.Rectangle.BOX;
            attendanceCell.Padding = 10;
            attendanceCell.BorderWidth = 1f;
            attendanceCell.BorderColor = new BaseColor(200, 200, 200);
            
            Paragraph attendanceHeading = new Paragraph("ATTENDANCE", summaryHeadingFont);
            attendanceHeading.SpacingAfter = 8;
            attendanceCell.AddElement(attendanceHeading);
            attendanceCell.AddElement(new Paragraph($"  • Attended: {totalFullyAttended}", summaryFont));
            attendanceCell.AddElement(new Paragraph($"  • Didn't Attend: {totalDidntAttend}", summaryFont));
            attendanceCell.AddElement(new Paragraph($"  • Overall Rate: {overallRate:F1}%", summaryFont));
            summaryGrid.AddCell(attendanceCell);

            // Cell 4: PENDING (without AVERAGES)
            PdfPCell pendingCell = new PdfPCell();
            pendingCell.Border = iTextSharp.text.Rectangle.BOX;
            pendingCell.Padding = 10;
            pendingCell.BorderWidth = 1f;
            pendingCell.BorderColor = new BaseColor(200, 200, 200);
            
            Paragraph pendingHeading = new Paragraph("PENDING", summaryHeadingFont);
            pendingHeading.SpacingAfter = 8;
            pendingCell.AddElement(pendingHeading);
            pendingCell.AddElement(new Paragraph($"  • Awaiting Approval: {totalPending}", summaryFont));
            pendingCell.AddElement(new Paragraph(" ", summaryFont)); // Empty lines for spacing
            pendingCell.AddElement(new Paragraph(" ", summaryFont));
            summaryGrid.AddCell(pendingCell);

            doc.Add(summaryGrid);

            // AVERAGES section - separate below the 2x2 grid
            PdfPTable averagesTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                SpacingBefore = 0,
                SpacingAfter = 10
            };

            PdfPCell averagesCell = new PdfPCell();
            averagesCell.Border = iTextSharp.text.Rectangle.BOX;
            averagesCell.Padding = 10;
            averagesCell.BorderWidth = 1f;
            averagesCell.BorderColor = new BaseColor(200, 200, 200);
            
            Paragraph averagesHeading = new Paragraph("AVERAGES", summaryHeadingFont);
            averagesHeading.SpacingAfter = 8;
            averagesCell.AddElement(averagesHeading);
            averagesCell.AddElement(new Paragraph($"  • Attendees/Event: {avgAttendeesPerEvent:F1}", summaryFont));
            averagesCell.AddElement(new Paragraph($"  • Volunteers/Event: {avgVolunteersPerEvent:F1}", summaryFont));
            averagesCell.AddElement(new Paragraph($"  • Speakers/Event: {avgSpeakersPerEvent:F1}", summaryFont));
            averagesCell.AddElement(new Paragraph($"  • Registered/Event: {avgRegisteredPerEvent:F1}", summaryFont));
            averagesTable.AddCell(averagesCell);

            doc.Add(averagesTable);

            // Add detailed breakdown (participant names) if available
            // We'll generate a table per event (Name | Role | Status) on a new page
            bool hasAnyEventDetails = false;
            // Collect event IDs and names from dgvReports (skip placeholder rows)
            var eventsList = new System.Collections.Generic.List<System.Tuple<int, string>>();
            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["id"] == null) continue;
                var idVal = row.Cells["id"].Value;
                if (idVal == null) continue;
                int id = 0;
                if (!int.TryParse(idVal.ToString(), out id)) continue;
                if (id == 0) continue; // skip placeholder or invalid
                string ename = row.Cells["event_name"]?.Value?.ToString() ?? "(Event)";
                eventsList.Add(System.Tuple.Create(id, ename));
            }

            if (eventsList.Count > 0)
            {
                // Start detailed breakdown on a new page
                doc.NewPage();
                Paragraph detailTitle = new Paragraph("PARTICIPANTS BREAKDOWN", summaryHeadingFont)
                {
                    SpacingBefore = 8,
                    SpacingAfter = 6,
                    Alignment = Element.ALIGN_LEFT
                };
                doc.Add(detailTitle);

                foreach (var ev in eventsList)
                {
                    int evId = ev.Item1;
                    string evName = ev.Item2;

                    // Query participants for this event
                    string partQuery = @"SELECT CONCAT(u.first_name, ' ', u.last_name) AS full_name, r.role, r.status
                                         FROM registrations r
                                         INNER JOIN users u ON r.user_id = u.id
                                         WHERE r.event_id = @event_id
                                         ORDER BY r.role ASC, u.first_name ASC, u.last_name ASC";
                    MySqlParameter[] partParams = { new MySqlParameter("@event_id", evId) };
                    DataTable partDt = DatabaseHelper.ExecuteQuery(partQuery, partParams);

                    // Add event header
                    Paragraph evHeader = new Paragraph("Participants for: " + evName, summaryFont)
                    {
                        SpacingBefore = 6,
                        SpacingAfter = 4,
                        Alignment = Element.ALIGN_LEFT
                    };
                    doc.Add(evHeader);

                    if (partDt.Rows.Count == 0)
                    {
                        doc.Add(new Paragraph("  (No participants)", summaryFont));
                        doc.Add(new Paragraph("\n"));
                        continue;
                    }

                    // Create table with 3 columns: Name, Role, Status
                    PdfPTable partTable = new PdfPTable(3)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 2,
                        SpacingAfter = 8
                    };
                    partTable.SetWidths(new float[] { 3f, 1f, 1f });

                    // Header cells
                    PdfPCell hn = new PdfPCell(new Phrase("Name", headerFont)) { BackgroundColor = new BaseColor(24, 30, 54), Padding = 6, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell hr = new PdfPCell(new Phrase("Role", headerFont)) { BackgroundColor = new BaseColor(24, 30, 54), Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER };
                    PdfPCell hs = new PdfPCell(new Phrase("Status", headerFont)) { BackgroundColor = new BaseColor(24, 30, 54), Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER };
                    partTable.AddCell(hn);
                    partTable.AddCell(hr);
                    partTable.AddCell(hs);

                    foreach (DataRow pr in partDt.Rows)
                    {
                        string fullName = pr["full_name"]?.ToString() ?? "";
                        string role = pr["role"]?.ToString() ?? "";
                        string status = pr["status"]?.ToString() ?? "";

                        PdfPCell c1 = new PdfPCell(new Phrase(fullName, summaryFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_LEFT };
                        PdfPCell c2 = new PdfPCell(new Phrase(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(role), summaryFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER };
                        PdfPCell c3 = new PdfPCell(new Phrase(status, summaryFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER };

                        partTable.AddCell(c1);
                        partTable.AddCell(c2);
                        partTable.AddCell(c3);
                    }

                    doc.Add(partTable);
                    hasAnyEventDetails = true;
                }
            }

            // If no event details were added, optionally add the old summary text on the page
            if (!hasAnyEventDetails)
            {
                string detailText = lblSummaryData?.Text ?? "";
                if (!string.IsNullOrWhiteSpace(detailText))
                {
                    // Start detailed breakdown on a new page
                    doc.NewPage();
                    Paragraph detailTitle2 = new Paragraph("DETAILED BREAKDOWN", summaryHeadingFont)
                    {
                        SpacingBefore = 8,
                        SpacingAfter = 6,
                        Alignment = Element.ALIGN_LEFT
                    };
                    doc.Add(detailTitle2);

                    string[] detailLines = detailText.Split(new[] { '\n' }, StringSplitOptions.None);
                    foreach (string line in detailLines)
                    {
                        doc.Add(new Paragraph(line.Trim(), summaryFont));
                    }
                }
            }

            // Footer
            doc.Add(new Paragraph("\n"));
            Paragraph footer = new Paragraph("This is a computer-generated report from Barangay Cogon Event Management System",
                FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8))
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(footer);

            doc.Close();
        }

        // Helper methods to extract numbers from summary text
        private int ExtractNumber(string line)
        {
            string[] parts = line.Split(':');
            if (parts.Length > 1)
            {
                string numberStr = parts[1].Trim().Replace("%", "");
                if (int.TryParse(numberStr, out int result))
                    return result;
            }
            return 0;
        }

        private double ExtractDouble(string line)
        {
            string[] parts = line.Split(':');
            if (parts.Length > 1)
            {
                string numberStr = parts[1].Trim().Replace("%", "");
                if (double.TryParse(numberStr, out double result))
                    return result;
            }
            return 0;
        }

        private void InitializeFilters(Panel parentContainer)
        {
            // Type filter label
            Label lblType = new Label
            {
                Text = "Filter by Type:",
                Location = new Point(0, 5),
                Size = new Size(100, 25),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Type filter dropdown
            cboTypeFilter = new ComboBox
            {
                Location = new Point(105, 0),
                Size = new Size(250, 30),
                Font = new System.Drawing.Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTypeFilter.Items.AddRange(new object[] { "All Types", "Community Service", "Health Drive", "Cleanup Drive", "Barangay Assembly", "Training / Workshop" });
            cboTypeFilter.SelectedIndex = 0;
            cboTypeFilter.SelectedIndexChanged += (s, ev) => LoadReports();

            parentContainer.Controls.Add(lblType);
            parentContainer.Controls.Add(cboTypeFilter);
            lblType.BringToFront();
            cboTypeFilter.BringToFront();
        }
    }
}

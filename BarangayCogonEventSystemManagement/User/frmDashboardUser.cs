using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using MySql.Data.MySqlClient;
using BarangayCogonEventSystemManagement.User;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmDashboardUser : Form
    {
        private int userId;
        private string userName;
        private string userRole;
        private IconButton currentActiveButton;
        private readonly Color activeOrHoverColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultColor = Color.Transparent;
        private Form currentChildForm;
        private Control[] dashboardControls;

        public frmDashboardUser(int userId, string userName, string userRole)
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;
            this.userRole = userRole;
            dashboardControls = new Control[]
            {
                pnlMyEventsCard, pnlPendingCard, pnlApprovedCard, pnlUpcomingEvents
            };
        }

        private void frmDashboardResident_Load(object sender, EventArgs e)
        {
            CustomizeUpcomingEventsTable();
            StyleViewAllButton();
            LoadDashboardData();
            AttachHoverHandlers();
            if (btnDashboard != null)
                HighlightNav(btnDashboard);
        }

        private void CustomizeUpcomingEventsTable()
        {
            dgvUpcomingEvents.Columns.Clear();
            dgvUpcomingEvents.AllowUserToAddRows = false;
            dgvUpcomingEvents.ReadOnly = true;

            // GENERAL GRID SETTINGS
            dgvUpcomingEvents.BackgroundColor = Color.FromArgb(37, 42, 64);
            dgvUpcomingEvents.BorderStyle = BorderStyle.None;
            dgvUpcomingEvents.GridColor = Color.FromArgb(60, 65, 90);
            dgvUpcomingEvents.EnableHeadersVisualStyles = false;
            dgvUpcomingEvents.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE
            dgvUpcomingEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvUpcomingEvents.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvUpcomingEvents.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUpcomingEvents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvUpcomingEvents.ColumnHeadersHeight = 45;

            // CELL STYLE
            dgvUpcomingEvents.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvUpcomingEvents.DefaultCellStyle.ForeColor = Color.White;
            dgvUpcomingEvents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvUpcomingEvents.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUpcomingEvents.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvUpcomingEvents.RowTemplate.Height = 55;
            dgvUpcomingEvents.RowHeadersVisible = false;
            dgvUpcomingEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - SAME color as default cells for consistency
            dgvUpcomingEvents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvUpcomingEvents.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvUpcomingEvents.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvUpcomingEvents.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvUpcomingEvents, new object[] { true });

            // Add columns
            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_name",
                HeaderText = "Event Name",
                ReadOnly = true,
                FillWeight = 30
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_date",
                HeaderText = "Event Date",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_time",
                HeaderText = "Event Schedule",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_venue",
                HeaderText = "Venue",
                ReadOnly = true,
                FillWeight = 20
            });

            dgvUpcomingEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "event_type",
                HeaderText = "Type",
                ReadOnly = true,
                FillWeight = 15
            });
        }

        private void StyleViewAllButton()
        {
            btnViewAllEvents.FlatStyle = FlatStyle.Flat;
            btnViewAllEvents.FlatAppearance.BorderSize = 0;
            btnViewAllEvents.Cursor = Cursors.Hand;
            btnViewAllEvents.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
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

        private void LoadDashboardData()
        {
            try
            {
                // Load statistics for this user
                string statsQuery = @"SELECT 
                                    (SELECT COUNT(*) FROM registrations WHERE user_id=@user_id) AS my_events,
                                    (SELECT COUNT(*) FROM registrations WHERE user_id=@user_id AND status='Pending') AS pending,
                                    (SELECT COUNT(*) FROM registrations WHERE user_id=@user_id AND status='Approved') AS approved";

                MySqlParameter[] statsParams = { new MySqlParameter("@user_id", userId) };
                DataTable dtStats = DatabaseHelper.ExecuteQuery(statsQuery, statsParams);

                if (dtStats.Rows.Count > 0)
                {
                    lblMyEventsCount.Text = dtStats.Rows[0]["my_events"].ToString();
                    lblPendingCount.Text = dtStats.Rows[0]["pending"].ToString();
                    lblApprovedCount.Text = dtStats.Rows[0]["approved"].ToString();
                }

                // Load upcoming events (next 10 events)
                string eventsQuery = @"SELECT 
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
                                    ORDER BY e.start_datetime ASC
                                    LIMIT 10";

                DataTable dtEvents = DatabaseHelper.ExecuteQuery(eventsQuery);

                // Clear existing rows
                dgvUpcomingEvents.Rows.Clear();

                // Check if there's data
                if (dtEvents.Rows.Count == 0)
                {
                    // Add placeholder row when no data
                    int placeholderIndex = dgvUpcomingEvents.Rows.Add(
                        0, // event_id
                        "No upcoming events available", // event_name (placeholder message)
                        "", // event_date
                        "", // event_time
                        "", // event_venue
                        ""  // event_type
                    );

                    // Style the placeholder row
                    DataGridViewRow placeholderRow = dgvUpcomingEvents.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    // Populate rows with data
                    foreach (DataRow dr in dtEvents.Rows)
                    {
                        dgvUpcomingEvents.Rows.Add(
                            dr["event_id"],
                            dr["event_name"],
                            dr["event_date"],
                            dr["event_time"],
                            dr["event_venue"],
                            dr["event_type"]
                        );
                    }
                }

                dgvUpcomingEvents.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenChild(Form child)
        {
            try
            {
                foreach (var frm in mainPanel.Controls.OfType<Form>().ToList())
                {
                    mainPanel.Controls.Remove(frm);
                    frm.Dispose();
                }
                currentChildForm = null;

                if (child == null)
                {
                    foreach (var ctrl in dashboardControls)
                        ctrl.Visible = true;
                    LoadDashboardData();
                    return;
                }

                foreach (var ctrl in dashboardControls)
                    ctrl.Visible = false;

                currentChildForm = child;
                child.TopLevel = false;
                child.FormBorderStyle = FormBorderStyle.None;
                child.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(child);
                child.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading view: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AttachHoverHandlers()
        {
            foreach (Control c in sidebar.Controls)
            {
                if (c is IconButton b)
                {
                    b.MouseEnter -= SidebarButton_MouseEnter;
                    b.MouseLeave -= SidebarButton_MouseLeave;
                    b.MouseEnter += SidebarButton_MouseEnter;
                    b.MouseLeave += SidebarButton_MouseLeave;
                }
            }
        }

        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is IconButton b)
                b.BackColor = activeOrHoverColor;
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is IconButton b && b != currentActiveButton)
                b.BackColor = defaultColor;
        }

        private void HighlightNav(IconButton btn)
        {
            if (pnlNav == null || sidebar == null || btn == null) return;
            pnlNav.Top = btn.Top;
            pnlNav.Height = btn.Height;
            currentActiveButton = btn;

            foreach (Control c in sidebar.Controls)
            {
                if (c is IconButton b && b != currentActiveButton)
                    b.BackColor = defaultColor;
            }
            btn.BackColor = activeOrHoverColor;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            HighlightNav(btnDashboard);
            lblTitle.Text = "Resident Dashboard";
            OpenChild(null);
        }

        private void btnBrowseEvents_Click(object sender, EventArgs e)
        {
            HighlightNav(btnBrowseEvents);
            lblTitle.Text = "Browse Events";
            OpenChild(new frmBrowseEvents(userId, userRole));
        }

        private void btnMyEvents_Click(object sender, EventArgs e)
        {
            HighlightNav(btnMyEvents);
            lblTitle.Text = "My Events";
            OpenChild(new frmMyEvents(userId));
        }

        private void btnMyQR_Click(object sender, EventArgs e)
        {
            HighlightNav(btnMyQR);
            lblTitle.Text = "My QR Codes";
            OpenChild(new frmMyQR(userId));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                frmUserLogin login = new frmUserLogin();
                login.ShowDialog();
                this.Close();
            }
        }

        private void btnViewAllEvents_Click(object sender, EventArgs e)
        {
            btnBrowseEvents_Click(sender, e);
        }
    }
}

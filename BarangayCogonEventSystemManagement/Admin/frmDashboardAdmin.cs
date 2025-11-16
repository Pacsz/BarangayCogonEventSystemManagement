using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmDashboardAdmin : Form
    {
        private IconButton currentActiveButton;
        private readonly Color activeOrHoverColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultColor = Color.Transparent;
        private Form currentChildForm;
        private Control[] dashboardControls;

        public frmDashboardAdmin()
        {
            InitializeComponent();
            dashboardControls = new[]
            {
                lblEventsCount, lblEvents, lblAttendeesCount, lblAttendees,
                lblVolunteersCount, lblVolunteers, lblPresentCount, lblPresent
            };
            LoadDashboardData();
            AttachHoverHandlers();
            if (btnDashboard != null)
                HighlightNav(btnDashboard);
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
            lblTitle.Text = "Admin Dashboard";
            OpenChild(null);
        }

        private void LoadDashboardData()
        {
            try
            {
                string query = @"SELECT 
                                    (SELECT COUNT(*) FROM events) AS total_events,
                                    (SELECT COUNT(*) FROM registrations WHERE role='attendee') AS total_attendees,
                                    (SELECT COUNT(*) FROM registrations WHERE role='volunteer') AS total_volunteers,
                                    (SELECT COUNT(*) FROM attendance) AS total_present";

                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    lblEventsCount.Text = dt.Rows[0]["total_events"].ToString();
                    lblAttendeesCount.Text = dt.Rows[0]["total_attendees"].ToString();
                    lblVolunteersCount.Text = dt.Rows[0]["total_volunteers"].ToString();
                    lblPresentCount.Text = dt.Rows[0]["total_present"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnManageEvents_Click(object sender, EventArgs e)
        {
            HighlightNav(btnManageEvents);
            lblTitle.Text = "Manage Events";
            OpenChild(new frmManageEvents());
        }

        private void btnRegistrations_Click(object sender, EventArgs e)
        {
            HighlightNav(btnRegistrations);
            lblTitle.Text = "Registrations";
            OpenChild(new frmApproveRegistrations());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            HighlightNav(btnReports);
            lblTitle.Text = "Reports";
            OpenChild(new frmReports());
        }

        private void btnScanner_Click(object sender, EventArgs e)
        {
            HighlightNav(btnQRScanner);
            lblTitle.Text = "QR Scanner";
            OpenChild(new frmAttendanceScanner());
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
                frmAdminLogin login = new frmAdminLogin();
                login.ShowDialog();
                this.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void frmDashboardAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}

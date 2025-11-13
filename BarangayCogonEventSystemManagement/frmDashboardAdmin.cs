using System;
using System.Data;
using System.Windows.Forms;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmDashboardAdmin : Form
    {
        public frmDashboardAdmin()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        // 🟦 Load statistics into the dashboard
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

        // 🟩 Manage Events Button
        private void btnManageEvents_Click(object sender, EventArgs e)
        {
            try
            {
                frmManageEvents manageEvents = new frmManageEvents();
                manageEvents.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Manage Events: " + ex.Message);
            }
        }

        // 🟩 Registrations Button (Approval)
        private void btnRegistrations_Click(object sender, EventArgs e)
        {
            try
            {
                frmApproveRegistrations regForm = new frmApproveRegistrations();
                regForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Registrations: " + ex.Message);
            }
        }

        // 🟩 Reports Button
        private void btnReports_Click(object sender, EventArgs e)
        {
            frmReports reports = new frmReports();
            reports.ShowDialog();
        }

        // 🟩 QR Scanner Button
        private void btnScanner_Click(object sender, EventArgs e)
        {
            try
            {
                frmAttendanceScanner scanForm = new frmAttendanceScanner();
                scanForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening QR Scanner: " + ex.Message);
            }
        }

        // 🟥 Logout Button
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Hide();
                frmLogin login = new frmLogin();
                login.ShowDialog();
                this.Close();
            }
        }
    }
}

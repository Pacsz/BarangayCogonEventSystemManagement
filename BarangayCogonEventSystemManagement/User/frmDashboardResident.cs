using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmDashboardResident : Form
    {
        private int userId;
        private string userName;
        private string userRole;

        public frmDashboardResident(int userId, string userName, string userRole)
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;
            this.userRole = userRole;
        }

        private void frmDashboardResident_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {userName}!";
            lblRole.Text = $"Role: {char.ToUpper(userRole[0]) + userRole.Substring(1)}";
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                string query = @"
                    SELECT 
                        e.id AS 'Event ID',
                        e.name AS 'Title',
                        e.date AS 'Date',
                        e.time AS 'Time',
                        e.venue AS 'Venue',
                        e.type AS 'Type',
                        e.organizer AS 'Organizer'
                    FROM events e
                    WHERE e.date >= CURDATE()
                    ORDER BY e.date ASC;";

                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                dgvEvents.DataSource = dt;
                dgvEvents.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMyEvents()
        {
            try
            {
                string query = @"
                    SELECT 
                        e.name AS 'Event',
                        e.date AS 'Date',
                        e.venue AS 'Venue',
                        r.role AS 'Role',
                        r.status AS 'Status'
                    FROM registrations r
                    INNER JOIN events e ON r.event_id = e.id
                    WHERE r.user_id = @user_id
                    ORDER BY e.date DESC;";

                MySqlParameter[] param = { new MySqlParameter("@user_id", userId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, param);
                dgvEvents.DataSource = dt;
                dgvEvents.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading your events: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                frmUserLogin login = new frmUserLogin();
                login.ShowDialog();
                this.Close();
            }
        }

        private void btnMyQR_Click(object sender, EventArgs e)
        {
            try
            {
                frmMyQR qrForm = new frmMyQR(userId);
                qrForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening My QR: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an event first.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int eventId = Convert.ToInt32(dgvEvents.SelectedRows[0].Cells["Event ID"].Value);

            try
            {
                string checkQuery = "SELECT * FROM registrations WHERE event_id=@event_id AND user_id=@user_id";
                MySqlParameter[] checkParams = {
                    new MySqlParameter("@event_id", eventId),
                    new MySqlParameter("@user_id", userId)
                };

                DataTable dt = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("You are already registered for this event.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string insertQuery = @"
                    INSERT INTO registrations (event_id, user_id, role, status, created_at)
                    VALUES (@event_id, @user_id, @role, 'Pending', NOW());";

                MySqlParameter[] insertParams = {
                    new MySqlParameter("@event_id", eventId),
                    new MySqlParameter("@user_id", userId),
                    new MySqlParameter("@role", userRole)
                };

                DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams);

                MessageBox.Show("You have successfully registered! Please wait for admin approval.",
                    "Registered", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering for event: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMyEvents_Click(object sender, EventArgs e)
        {
            frmMyEvents myEvents = new frmMyEvents(userId);
            myEvents.ShowDialog();
        }
    }
}

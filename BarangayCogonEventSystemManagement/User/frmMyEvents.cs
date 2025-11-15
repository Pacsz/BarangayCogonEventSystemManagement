using System;
using System.Data;
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
        }

        private void frmMyEvents_Load(object sender, EventArgs e)
        {
            LoadMyEvents();
        }

        private void LoadMyEvents()
        {
            try
            {
                string query = @"
                    SELECT 
                        e.name AS 'Event',
                        e.date AS 'Date',
                        e.time AS 'Time',
                        e.venue AS 'Venue',
                        r.role AS 'Role',
                        r.status AS 'Status'
                    FROM registrations r
                    INNER JOIN events e ON r.event_id = e.id
                    WHERE r.user_id = @user_id
                    ORDER BY e.date DESC;
                ";

                MySqlParameter[] param = { new MySqlParameter("@user_id", userId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, param);
                dgvMyEvents.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message);
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvMyEvents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an event to view details.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string eventName = dgvMyEvents.SelectedRows[0].Cells["Event"].Value.ToString();
            string status = dgvMyEvents.SelectedRows[0].Cells["Status"].Value.ToString();
            string venue = dgvMyEvents.SelectedRows[0].Cells["Venue"].Value.ToString();
            string date = dgvMyEvents.SelectedRows[0].Cells["Date"].Value.ToString();
            string time = dgvMyEvents.SelectedRows[0].Cells["Time"].Value.ToString();

            MessageBox.Show($"📅 {eventName}\n\nDate: {date}\nTime: {time}\nVenue: {venue}\nStatus: {status}",
                "Event Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgvMyEvents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a pending event to cancel.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string status = dgvMyEvents.SelectedRows[0].Cells["Status"].Value.ToString();
            string eventName = dgvMyEvents.SelectedRows[0].Cells["Event"].Value.ToString();

            if (status != "Pending")
            {
                MessageBox.Show("Only pending registrations can be canceled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to cancel your registration for '{eventName}'?",
                "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string query = @"
                        DELETE FROM registrations 
                        WHERE user_id = @user_id AND event_id = (
                            SELECT id FROM events WHERE title = @event_name LIMIT 1
                        );";

                    MySqlParameter[] param = {
                        new MySqlParameter("@user_id", userId),
                        new MySqlParameter("@event_name", eventName)
                    };

                    DatabaseHelper.ExecuteNonQuery(query, param);
                    MessageBox.Show("Registration canceled successfully!", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMyEvents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error canceling registration: " + ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

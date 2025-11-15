using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmManageEvents : Form
    {
        public frmManageEvents()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void LoadEvents()
        {
            string query = "SELECT id, name, description, date, time, venue, type, organizer FROM events";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            dgvEvents.DataSource = dt;
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"INSERT INTO events (name, description, date, time, venue, type, organizer)
                                 VALUES (@name, @description, @date, @time, @venue, @type, @organizer)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@name", txtTitle.Text),
                    new MySqlParameter("@description", txtDescription.Text),
                    new MySqlParameter("@date", dtpDate.Value.Date),
                    new MySqlParameter("@time", dtpTime.Value.TimeOfDay),
                    new MySqlParameter("@venue", txtVenue.Text),
                    new MySqlParameter("@type", cboType.Text),
                    new MySqlParameter("@organizer", txtOrganizer.Text)
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Event added successfully!");
                LoadEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding event: " + ex.Message);
            }
        }

        private void dgvEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEvents.CurrentRow == null) return;

            txtTitle.Text = dgvEvents.CurrentRow.Cells["name"].Value.ToString();
            txtDescription.Text = dgvEvents.CurrentRow.Cells["description"].Value.ToString();
            dtpDate.Value = Convert.ToDateTime(dgvEvents.CurrentRow.Cells["date"].Value);
            dtpTime.Value = DateTime.Today.Add((TimeSpan)dgvEvents.CurrentRow.Cells["time"].Value);
            txtVenue.Text = dgvEvents.CurrentRow.Cells["venue"].Value.ToString();
            cboType.Text = dgvEvents.CurrentRow.Cells["type"].Value.ToString();
            txtOrganizer.Text = dgvEvents.CurrentRow.Cells["organizer"].Value.ToString();
        }

        private void btnUpdateEvent_Click(object sender, EventArgs e)
        {
            if (dgvEvents.CurrentRow == null) return;

            try
            {
                int id = Convert.ToInt32(dgvEvents.CurrentRow.Cells["id"].Value);
                string query = @"UPDATE events SET 
                                name=@name, description=@description, date=@date, time=@time, 
                                venue=@venue, type=@type, organizer=@organizer WHERE id=@id";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@id", id),
                    new MySqlParameter("@name", txtTitle.Text),
                    new MySqlParameter("@description", txtDescription.Text),
                    new MySqlParameter("@date", dtpDate.Value.Date),
                    new MySqlParameter("@time", dtpTime.Value.TimeOfDay),
                    new MySqlParameter("@venue", txtVenue.Text),
                    new MySqlParameter("@type", cboType.Text),
                    new MySqlParameter("@organizer", txtOrganizer.Text)
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Event updated successfully!");
                LoadEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating event: " + ex.Message);
            }
        }

        private void btnDeleteEvent_Click(object sender, EventArgs e)
        {
            if (dgvEvents.CurrentRow == null) return;

            if (MessageBox.Show("Are you sure you want to delete this event?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvEvents.CurrentRow.Cells["id"].Value);
                    string query = "DELETE FROM events WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", id) };
                    DatabaseHelper.ExecuteNonQuery(query, parameters);
                    MessageBox.Show("Event deleted successfully!");
                    LoadEvents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting event: " + ex.Message);
                }
            }
        }

        private void dgvEvents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEvents.CurrentRow == null) return;

            txtTitle.Text = dgvEvents.CurrentRow.Cells["name"].Value.ToString();
            txtDescription.Text = dgvEvents.CurrentRow.Cells["description"].Value.ToString();
            dtpDate.Value = Convert.ToDateTime(dgvEvents.CurrentRow.Cells["date"].Value);
            dtpTime.Value = DateTime.Today.Add((TimeSpan)dgvEvents.CurrentRow.Cells["time"].Value);
            txtVenue.Text = dgvEvents.CurrentRow.Cells["venue"].Value.ToString();
            cboType.Text = dgvEvents.CurrentRow.Cells["type"].Value.ToString();
            txtOrganizer.Text = dgvEvents.CurrentRow.Cells["organizer"].Value.ToString();
        }
    }
}

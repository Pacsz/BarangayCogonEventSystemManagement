using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRCoder;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmApproveRegistrations : Form
    {
        public frmApproveRegistrations()
        {
            InitializeComponent();
            LoadPendingRegistrations();
        }

        private void LoadPendingRegistrations()
        {
            try
            {
                string query = @"SELECT r.id, e.name AS event_name, u.name AS user_name, r.role, r.status, r.qr_code 
                                 FROM registrations r
                                 INNER JOIN events e ON r.event_id = e.id
                                 INNER JOIN users u ON r.user_id = u.id
                                 ORDER BY r.status DESC";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                dgvRegistrations.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading registrations: " + ex.Message);
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (dgvRegistrations.CurrentRow == null)
            {
                MessageBox.Show("Please select a registration first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int regId = Convert.ToInt32(dgvRegistrations.CurrentRow.Cells["id"].Value);
            string eventName = dgvRegistrations.CurrentRow.Cells["event_name"].Value.ToString();
            string userName = dgvRegistrations.CurrentRow.Cells["user_name"].Value.ToString();

            try
            {
                string qrText = $"{eventName}_{userName}_{Guid.NewGuid()}";
                string folderPath = Path.Combine(Application.StartupPath, "Assets", "QR_Codes");
                Directory.CreateDirectory(folderPath);
                string fileName = $"{eventName}_{userName}.png".Replace(" ", "_");
                string fullPath = Path.Combine(folderPath, fileName);

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q))
                using (QRCode qrCode = new QRCode(qrCodeData))
                using (Bitmap qrImage = qrCode.GetGraphic(6))
                {
                    qrImage.Save(fullPath);
                }

                string query = @"UPDATE registrations 
                                 SET status='Approved', qr_code=@qr 
                                 WHERE id=@id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@qr", qrText),
                    new MySqlParameter("@id", regId)
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show($"Registration approved!\nQR code saved at:\n{fullPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPendingRegistrations();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error approving registration: " + ex.Message);
            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (dgvRegistrations.CurrentRow == null)
            {
                MessageBox.Show("Please select a registration first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int regId = Convert.ToInt32(dgvRegistrations.CurrentRow.Cells["id"].Value);

            try
            {
                string query = "UPDATE registrations SET status='Rejected' WHERE id=@id";
                MySqlParameter[] parameters = { new MySqlParameter("@id", regId) };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Registration rejected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPendingRegistrations();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rejecting registration: " + ex.Message);
            }
        }

        private void btnViewQR_Click(object sender, EventArgs e)
        {
            if (dgvRegistrations.CurrentRow == null)
            {
                MessageBox.Show("Please select a registration.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string eventName = dgvRegistrations.CurrentRow.Cells["event_name"].Value.ToString();
            string userName = dgvRegistrations.CurrentRow.Cells["user_name"].Value.ToString();
            string filePath = Path.Combine(Application.StartupPath, "Assets", "QR_Codes", $"{eventName}_{userName}.png".Replace(" ", "_"));

            if (File.Exists(filePath))
            {
                picQR.Image = Image.FromFile(filePath);
            }
            else
            {
                MessageBox.Show("QR image not found for this user.", "Missing QR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

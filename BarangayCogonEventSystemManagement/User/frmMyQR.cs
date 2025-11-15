using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRCoder;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmMyQR : Form
    {
        private int userId;

        public frmMyQR(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void frmMyQR_Load(object sender, EventArgs e)
        {
            LoadApprovedEvents();
        }

        private void LoadApprovedEvents()
        {
            try
            {
                string query = @"
                    SELECT 
                        e.name AS 'Event',
                        r.qr_code AS 'QR Code'
                    FROM registrations r
                    INNER JOIN events e ON r.event_id = e.id
                    WHERE r.user_id = @user_id AND r.status = 'Approved'
                    ORDER BY e.date ASC;";

                MySqlParameter[] param = { new MySqlParameter("@user_id", userId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, param);

                dgvQRList.DataSource = dt;
                dgvQRList.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading QR data: " + ex.Message);
            }
        }

        private void dgvQRList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvQRList.Rows.Count > 0)
            {
                string eventName = dgvQRList.Rows[e.RowIndex].Cells["Event"].Value.ToString();
                string qrCodeData = dgvQRList.Rows[e.RowIndex].Cells["QR Code"].Value.ToString();

                lblEvent.Text = eventName;

                if (!string.IsNullOrEmpty(qrCodeData))
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrData = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q);
                    QRCode qr = new QRCode(qrData);
                    picQR.Image = qr.GetGraphic(5, Color.Black, Color.White, true);
                }
                else
                {
                    picQR.Image = null;
                    lblEvent.Text = "No QR Code Available";
                }
            }
        }

        private void btnSaveQR_Click(object sender, EventArgs e)
        {
            if (picQR.Image == null)
            {
                MessageBox.Show("Please select an event with a QR code first.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = "MyQR_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                picQR.Image.Save(saveDialog.FileName);
                MessageBox.Show("QR Code saved successfully!", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}

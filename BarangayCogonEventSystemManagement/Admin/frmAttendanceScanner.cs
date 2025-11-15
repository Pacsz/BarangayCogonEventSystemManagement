using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmAttendanceScanner : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Timer scanTimer;

        public frmAttendanceScanner()
        {
            InitializeComponent();
            LoadCameras();
        }

        private void LoadCameras()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo device in videoDevices)
                {
                    cboCameras.Items.Add(device.Name);
                }

                if (cboCameras.Items.Count > 0)
                    cboCameras.SelectedIndex = 0;
                else
                    MessageBox.Show("No camera detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Camera loading error: " + ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (videoDevices == null || videoDevices.Count == 0) return;

            videoSource = new VideoCaptureDevice(videoDevices[cboCameras.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(Video_NewFrame);
            videoSource.Start();

            scanTimer = new Timer();
            scanTimer.Interval = 500;
            scanTimer.Tick += new EventHandler(ScanQRCode);
            scanTimer.Start();
        }

        private void Video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
                picCamera.Image = bitmap;
            }
            catch { }
        }

        private void ScanQRCode(object sender, EventArgs e)
        {
            if (picCamera.Image == null) return;

            try
            {
                Bitmap bitmap = new Bitmap(picCamera.Image);
                BarcodeReader reader = new BarcodeReader();
                var result = reader.Decode(bitmap);

                if (result != null)
                {
                    string qrText = result.Text;
                    lblStatus.Text = "QR Scanned: " + qrText;

                    RecordAttendance(qrText);

                    scanTimer.Stop();
                    System.Threading.Thread.Sleep(1000);
                    scanTimer.Start();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error scanning: " + ex.Message;
            }
        }

        private void RecordAttendance(string qrText)
        {
            try
            {
                string query = "SELECT id FROM registrations WHERE qr_code=@qr";
                MySqlParameter[] param = { new MySqlParameter("@qr", qrText) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, param);

                if (dt.Rows.Count == 0)
                {
                    lblStatus.Text = "QR not recognized.";
                    return;
                }

                int regId = Convert.ToInt32(dt.Rows[0]["id"]);

                string checkQuery = "SELECT * FROM attendance WHERE registration_id=@id";
                MySqlParameter[] checkParam = { new MySqlParameter("@id", regId) };
                DataTable checkDt = DatabaseHelper.ExecuteQuery(checkQuery, checkParam);

                if (checkDt.Rows.Count > 0)
                {
                    lblStatus.Text = "Already recorded.";
                    return;
                }

                string insert = "INSERT INTO attendance (registration_id, time_in) VALUES (@id, NOW())";
                MySqlParameter[] insertParam = { new MySqlParameter("@id", regId) };
                DatabaseHelper.ExecuteNonQuery(insert, insertParam);

                lblStatus.Text = "Attendance recorded successfully!";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error saving attendance: " + ex.Message;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        private void StopCamera()
        {
            try
            {
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
                if (scanTimer != null)
                    scanTimer.Stop();
            }
            catch { }
        }

        private void frmAttendanceScanner_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();
        }
    }
}

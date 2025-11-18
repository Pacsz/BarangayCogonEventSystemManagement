using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            StyleControls();
            LoadCameras();
        }

        private void StyleControls()
        {
            // Style the ComboBox
            cboCameras.BackColor = Color.FromArgb(37, 42, 69);
            cboCameras.ForeColor = Color.White;
            cboCameras.FlatStyle = FlatStyle.Flat;
            cboCameras.Font = new Font("Segoe UI", 11F);

            // Style Start button with rounded corners
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.BackColor = Color.FromArgb(0, 126, 249); // Accent blue
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStart.Cursor = Cursors.Hand;
            btnStart.Paint += (s, e) =>
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

            // Style Stop button with rounded corners
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.BackColor = Color.FromArgb(211, 47, 47); // Red for stop
            btnStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStop.Cursor = Cursors.Hand;
            btnStop.Paint += (s, e) =>
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
                MessageBox.Show("Camera loading error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (videoDevices == null || videoDevices.Count == 0)
            {
                MessageBox.Show("No camera available. Please check your camera connection.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                videoSource = new VideoCaptureDevice(videoDevices[cboCameras.SelectedIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(Video_NewFrame);
                videoSource.Start();

                scanTimer = new Timer();
                scanTimer.Interval = 500;
                scanTimer.Tick += new EventHandler(ScanQRCode);
                scanTimer.Start();

                lblStatus.Text = "Status: Scanner running... Waiting for QR code.";
                lblStatus.ForeColor = Color.FromArgb(76, 175, 80); // Green color
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting camera: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    lblStatus.ForeColor = Color.FromArgb(255, 193, 7); // Yellow color

                    RecordAttendance(qrText);

                    scanTimer.Stop();
                    System.Threading.Thread.Sleep(1000);
                    scanTimer.Start();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error scanning: " + ex.Message;
                lblStatus.ForeColor = Color.FromArgb(211, 47, 47); // Red color
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
                    lblStatus.Text = "Status: QR not recognized.";
                    lblStatus.ForeColor = Color.FromArgb(211, 47, 47); // Red color
                    return;
                }

                int regId = Convert.ToInt32(dt.Rows[0]["id"]);

                string checkQuery = "SELECT * FROM attendance WHERE registration_id=@id";
                MySqlParameter[] checkParam = { new MySqlParameter("@id", regId) };
                DataTable checkDt = DatabaseHelper.ExecuteQuery(checkQuery, checkParam);

                if (checkDt.Rows.Count > 0)
                {
                    lblStatus.Text = "Status: Attendance already recorded.";
                    lblStatus.ForeColor = Color.FromArgb(255, 152, 0); // Orange color
                    return;
                }

                string insert = "INSERT INTO attendance (registration_id, time_in) VALUES (@id, NOW())";
                MySqlParameter[] insertParam = { new MySqlParameter("@id", regId) };
                DatabaseHelper.ExecuteNonQuery(insert, insertParam);

                lblStatus.Text = "Status: ? Attendance recorded successfully!";
                lblStatus.ForeColor = Color.FromArgb(76, 175, 80); // Green color
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error saving attendance: " + ex.Message;
                lblStatus.ForeColor = Color.FromArgb(211, 47, 47); // Red color
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCamera();
            lblStatus.Text = "Status: Scanner stopped.";
            lblStatus.ForeColor = Color.White;
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

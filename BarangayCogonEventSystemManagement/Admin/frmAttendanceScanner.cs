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
        private string lastScannedQR = string.Empty;
        private DateTime lastScanTime = DateTime.MinValue;
        private Bitmap currentFrame; // Store current frame for scanning

        public frmAttendanceScanner()
        {
            InitializeComponent();
            StyleControls();
            LoadCameras();
            SetupScanningOverlay();
        }

        private void SetupScanningOverlay()
        {
            // Subscribe to the Paint event to draw the scanning frame
            picCamera.Paint += PicCamera_Paint;
        }

        private void PicCamera_Paint(object sender, PaintEventArgs e)
        {
            if (picCamera.Image == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Calculate the center scanning square (about 40% of the picture box size)
            int squareSize = Math.Min(picCamera.Width, picCamera.Height) * 40 / 100;
            int x = (picCamera.Width - squareSize) / 2;
            int y = (picCamera.Height - squareSize) / 2;
            Rectangle scanRect = new Rectangle(x, y, squareSize, squareSize);

            // Draw semi-transparent overlay outside the scanning area
            using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
            {
                // Top overlay
                g.FillRectangle(darkBrush, 0, 0, picCamera.Width, y);
                // Bottom overlay
                g.FillRectangle(darkBrush, 0, y + squareSize, picCamera.Width, picCamera.Height - (y + squareSize));
                // Left overlay
                g.FillRectangle(darkBrush, 0, y, x, squareSize);
                // Right overlay
                g.FillRectangle(darkBrush, x + squareSize, y, picCamera.Width - (x + squareSize), squareSize);
            }

            // Draw the scanning frame border
            using (Pen framePen = new Pen(Color.FromArgb(0, 255, 0), 3))
            {
                g.DrawRectangle(framePen, scanRect);
            }

            // Draw corner brackets for visual emphasis
            int cornerLength = 30;
            int cornerThickness = 4;
            using (Pen cornerPen = new Pen(Color.FromArgb(0, 255, 0), cornerThickness))
            {
                // Top-left corner
                g.DrawLine(cornerPen, x, y, x + cornerLength, y);
                g.DrawLine(cornerPen, x, y, x, y + cornerLength);

                // Top-right corner
                g.DrawLine(cornerPen, x + squareSize, y, x + squareSize - cornerLength, y);
                g.DrawLine(cornerPen, x + squareSize, y, x + squareSize, y + cornerLength);

                // Bottom-left corner
                g.DrawLine(cornerPen, x, y + squareSize, x + cornerLength, y + squareSize);
                g.DrawLine(cornerPen, x, y + squareSize, x, y + squareSize - cornerLength);

                // Bottom-right corner
                g.DrawLine(cornerPen, x + squareSize, y + squareSize, x + squareSize - cornerLength, y + squareSize);
                g.DrawLine(cornerPen, x + squareSize, y + squareSize, x + squareSize, y + squareSize - cornerLength);
            }

            // Draw instruction text
            string instructionText = "Position QR Code in the frame";
            using (Font font = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                SizeF textSize = g.MeasureString(instructionText, font);
                float textX = (picCamera.Width - textSize.Width) / 2;
                float textY = y - textSize.Height - 10;

                // Draw text shadow for better visibility
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                {
                    g.DrawString(instructionText, font, shadowBrush, textX + 2, textY + 2);
                }
                g.DrawString(instructionText, font, textBrush, textX, textY);
            }
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

            // Style Status Label with background container
            lblStatus.BackColor = Color.FromArgb(37, 42, 64); // Dark blue background
            lblStatus.AutoSize = false;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblStatus.Padding = new Padding(15, 10, 15, 10);
            lblStatus.BorderStyle = BorderStyle.FixedSingle;
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
                scanTimer.Interval = 1000; // Increased to 1 second for better performance
                scanTimer.Tick += new EventHandler(ScanQRCode);
                scanTimer.Start();

                lblStatus.Text = "Status: Scanner running... Waiting for QR code.";
                lblStatus.ForeColor = Color.FromArgb(76, 175, 80); // Green color
                
                System.Diagnostics.Debug.WriteLine("Scanner started successfully");
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
                // Dispose old frame
                if (currentFrame != null)
                {
                    currentFrame.Dispose();
                }
                
                // Store the current frame for scanning
                currentFrame = (Bitmap)eventArgs.Frame.Clone();
                
                // Update the display
                if (picCamera.InvokeRequired)
                {
                    picCamera.Invoke(new Action(() =>
                    {
                        if (picCamera.Image != null)
                        {
                            picCamera.Image.Dispose();
                        }
                        picCamera.Image = (Bitmap)currentFrame.Clone();
                        picCamera.Invalidate();
                    }));
                }
                else
                {
                    if (picCamera.Image != null)
                    {
                        picCamera.Image.Dispose();
                    }
                    picCamera.Image = (Bitmap)currentFrame.Clone();
                    picCamera.Invalidate();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Video_NewFrame: {ex.Message}");
            }
        }

        private void ScanQRCode(object sender, EventArgs e)
        {
            if (currentFrame == null) 
            {
                System.Diagnostics.Debug.WriteLine("No frame available for scanning");
                return;
            }

            try
            {
                // Create a copy of the current frame for scanning
                Bitmap bitmapToScan = null;
                lock (currentFrame)
                {
                    bitmapToScan = (Bitmap)currentFrame.Clone();
                }

                // Configure the barcode reader with proper hints for better QR detection
                BarcodeReader reader = new BarcodeReader
                {
                    AutoRotate = true,
                    TryInverted = true,
                    Options = new ZXing.Common.DecodingOptions
                    {
                        TryHarder = true,
                        PossibleFormats = new System.Collections.Generic.List<BarcodeFormat>
                        {
                            BarcodeFormat.QR_CODE
                        }
                    }
                };

                var result = reader.Decode(bitmapToScan);
                bitmapToScan.Dispose();

                if (result != null)
                {
                    string qrText = result.Text;
                    System.Diagnostics.Debug.WriteLine($"QR Code detected: {qrText}");
                    
                    // Prevent scanning the same QR code repeatedly within 3 seconds
                    if (qrText == lastScannedQR && (DateTime.Now - lastScanTime).TotalSeconds < 3)
                    {
                        System.Diagnostics.Debug.WriteLine("Same QR code scanned within 3 seconds, ignoring");
                        return;
                    }

                    lastScannedQR = qrText;
                    lastScanTime = DateTime.Now;

                    lblStatus.Text = "QR Scanned: Processing...";
                    lblStatus.ForeColor = Color.FromArgb(255, 193, 7); // Yellow color

                    RecordAttendance(qrText);
                }
                else
                {
                    // No QR code detected - this is normal, don't log
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scanning QR code: {ex.Message}");
                lblStatus.Text = "Error scanning: " + ex.Message;
                lblStatus.ForeColor = Color.FromArgb(211, 47, 47); // Red color
            }
        }

        private void RecordAttendance(string qrText)
        {
            try
            {
                // Query to get registration details with user and event information
                string query = @"
                    SELECT 
                        r.id AS registration_id,
                        r.status,
                        r.event_id,
                        CONCAT(u.first_name, ' ', u.last_name) AS user_name,
                        u.email,
                        e.name AS event_name,
                        e.start_datetime,
                        e.end_datetime
                    FROM registrations r
                    INNER JOIN users u ON r.user_id = u.id
                    INNER JOIN events e ON r.event_id = e.id
                    WHERE r.qr_code = @qr";
                
                MySqlParameter[] param = { new MySqlParameter("@qr", qrText) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, param);

                // Validation 1: Check if QR code exists in database
                if (dt.Rows.Count == 0)
                {
                    lblStatus.Text = "❌ Status: QR code not recognized. Invalid QR code.";
                    lblStatus.ForeColor = Color.FromArgb(211, 47, 47); // Red color
                    MessageBox.Show("This QR code is not registered in the system.", 
                        "Invalid QR Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRow row = dt.Rows[0];
                int regId = Convert.ToInt32(row["registration_id"]);
                string status = row["status"].ToString();
                string userName = row["user_name"].ToString();
                string userEmail = row["email"].ToString();
                string eventName = row["event_name"].ToString();
                DateTime eventStartDateTime = Convert.ToDateTime(row["start_datetime"]);
                DateTime eventEndDateTime = Convert.ToDateTime(row["end_datetime"]);
                DateTime currentTime = DateTime.Now;

                // Validation 2: Check if registration is approved
                if (status != "Approved" && status != "Checked-in")
                {
                    lblStatus.Text = $"❌ Status: Registration not approved. Status: {status}";
                    lblStatus.ForeColor = Color.FromArgb(255, 152, 0); // Orange color
                    MessageBox.Show($"This registration is not approved.\n\n" +
                        $"Name: {userName}\n" +
                        $"Email: {userEmail}\n" +
                        $"Event: {eventName}\n" +
                        $"Status: {status}\n\n" +
                        $"Please approve the registration first.",
                        "Registration Not Approved", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validation 3: Check if event has ended
                if (currentTime > eventEndDateTime)
                {
                    lblStatus.Text = "❌ Status: Event has already ended.";
                    lblStatus.ForeColor = Color.FromArgb(211, 47, 47); // Red color
                    MessageBox.Show($"This event has already ended.\n\n" +
                        $"Event: {eventName}\n" +
                        $"End Date: {eventEndDateTime.ToString("MMM dd, yyyy hh:mm tt")}\n\n" +
                        $"Cannot record attendance for past events.",
                        "Event Ended", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validation 4: Check if event has started (for check-in)
                if (currentTime < eventStartDateTime)
                {
                    lblStatus.Text = "⏰ Status: Event hasn't started yet.";
                    lblStatus.ForeColor = Color.FromArgb(255, 193, 7); // Yellow color
                    
                    TimeSpan timeUntilStart = eventStartDateTime - currentTime;
                    string timeMessage = "";
                    
                    if (timeUntilStart.TotalDays >= 1)
                    {
                        int days = (int)timeUntilStart.TotalDays;
                        timeMessage = $"{days} day{(days > 1 ? "s" : "")}";
                    }
                    else if (timeUntilStart.TotalHours >= 1)
                    {
                        int hours = (int)timeUntilStart.TotalHours;
                        timeMessage = $"{hours} hour{(hours > 1 ? "s" : "")}";
                    }
                    else
                    {
                        int minutes = (int)timeUntilStart.TotalMinutes;
                        timeMessage = $"{minutes} minute{(minutes > 1 ? "s" : "")}";
                    }
                    
                    MessageBox.Show($"This event hasn't started yet.\n\n" +
                        $"Event: {eventName}\n" +
                        $"Start Date: {eventStartDateTime.ToString("MMM dd, yyyy hh:mm tt")}\n" +
                        $"Current Time: {currentTime.ToString("MMM dd, yyyy hh:mm tt")}\n\n" +
                        $"Event starts in approximately {timeMessage}.\n" +
                        $"Please wait until the event begins to check in.",
                        "Event Not Started", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Check if attendance already recorded (check-in exists)
                string checkQuery = @"
                    SELECT 
                        a.id,
                        a.check_in_time,
                        a.check_out_time
                    FROM attendance a 
                    WHERE a.registration_id = @id";
                
                MySqlParameter[] checkParam = { new MySqlParameter("@id", regId) };
                DataTable checkDt = DatabaseHelper.ExecuteQuery(checkQuery, checkParam);

                if (checkDt.Rows.Count > 0)
                {
                    // Attendance record exists
                    DataRow attendanceRow = checkDt.Rows[0];
                    DateTime checkInTime = Convert.ToDateTime(attendanceRow["check_in_time"]);
                    object checkOutValue = attendanceRow["check_out_time"];
                    
                    if (checkOutValue != DBNull.Value)
                    {
                        // Both check-in and check-out recorded
                        DateTime checkOutTime = Convert.ToDateTime(checkOutValue);
                        lblStatus.Text = "ℹ️ Status: Attendance already completed (checked in & out).";
                        lblStatus.ForeColor = Color.FromArgb(255, 152, 0); // Orange color
                        MessageBox.Show($"Attendance already completed for this registration.\n\n" +
                            $"Name: {userName}\n" +
                            $"Event: {eventName}\n" +
                            $"Check-in: {checkInTime.ToString("MMM dd, yyyy hh:mm tt")}\n" +
                            $"Check-out: {checkOutTime.ToString("MMM dd, yyyy hh:mm tt")}",
                            "Already Recorded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        // Only check-in recorded, offer check-out
                        // Validation 5: Check if event is still ongoing (before allowing check-out)
                        if (currentTime < eventEndDateTime)
                        {
                            lblStatus.Text = "⏰ Status: Event is still ongoing.";
                            lblStatus.ForeColor = Color.FromArgb(255, 193, 7); // Yellow color
                            
                            TimeSpan timeUntilEnd = eventEndDateTime - currentTime;
                            string timeMessage = "";
                            
                            if (timeUntilEnd.TotalHours >= 1)
                            {
                                int hours = (int)timeUntilEnd.TotalHours;
                                timeMessage = $"{hours} hour{(hours > 1 ? "s" : "")}";
                            }
                            else
                            {
                                int minutes = (int)timeUntilEnd.TotalMinutes;
                                timeMessage = $"{minutes} minute{(minutes > 1 ? "s" : "")}";
                            }
                            
                            MessageBox.Show($"The event is still ongoing.\n\n" +
                                $"Event: {eventName}\n" +
                                $"End Date: {eventEndDateTime.ToString("MMM dd, yyyy hh:mm tt")}\n" +
                                $"Current Time: {currentTime.ToString("MMM dd, yyyy hh:mm tt")}\n\n" +
                                $"Event ends in approximately {timeMessage}.\n" +
                                $"Check-out will be available after the event ends.",
                                "Event Still Ongoing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        
                        // Event has ended, allow check-out
                        DialogResult result = MessageBox.Show(
                            $"User has already checked in.\n\n" +
                            $"Name: {userName}\n" +
                            $"Email: {userEmail}\n" +
                            $"Event: {eventName}\n" +
                            $"Check-in: {checkInTime.ToString("MMM dd, yyyy hh:mm tt")}\n\n" +
                            $"Would you like to record CHECK-OUT now?",
                            "Record Check-Out?", 
                            MessageBoxButtons.YesNo, 
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            RecordCheckOut(Convert.ToInt32(attendanceRow["id"]), regId, userName, eventName);
                        }
                        else
                        {
                            lblStatus.Text = "ℹ️ Status: Check-out cancelled.";
                            lblStatus.ForeColor = Color.White;
                        }
                        return;
                    }
                }

                // No attendance record exists, record check-in
                string insert = "INSERT INTO attendance (registration_id, check_in_time) VALUES (@id, NOW())";
                MySqlParameter[] insertParam = { new MySqlParameter("@id", regId) };
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insert, insertParam);

                if (rowsAffected > 0)
                {
                    // Update registration status to "Checked-in"
                    UpdateRegistrationStatus(regId, "Checked-in");
                    
                    lblStatus.Text = "✅ Status: Check-in recorded successfully!";
                    lblStatus.ForeColor = Color.FromArgb(76, 175, 80); // Green color
                    
                    MessageBox.Show($"✅ CHECK-IN SUCCESSFUL!\n\n" +
                        $"Name: {userName}\n" +
                        $"Email: {userEmail}\n" +
                        $"Event: {eventName}\n" +
                        $"Time: {DateTime.Now.ToString("MMM dd, yyyy hh:mm tt")}",
                        "Attendance Recorded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Error: " + ex.Message;
                lblStatus.ForeColor = Color.FromArgb(211, 47, 47); // Red color
                MessageBox.Show("Error recording attendance: " + ex.Message, 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecordCheckOut(int attendanceId, int registrationId, string userName, string eventName)
        {
            try
            {
                string updateQuery = "UPDATE attendance SET check_out_time = NOW() WHERE id = @id";
                MySqlParameter[] updateParam = { new MySqlParameter("@id", attendanceId) };
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(updateQuery, updateParam);

                if (rowsAffected > 0)
                {
                    // Update registration status to "Attended"
                    UpdateRegistrationStatus(registrationId, "Attended");
                    
                    lblStatus.Text = "✅ Status: Check-out recorded successfully!";
                    lblStatus.ForeColor = Color.FromArgb(76, 175, 80); // Green color
                    
                    MessageBox.Show($"✅ CHECK-OUT SUCCESSFUL!\n\n" +
                        $"Name: {userName}\n" +
                        $"Event: {eventName}\n" +
                        $"Time: {DateTime.Now.ToString("MMM dd, yyyy hh:mm tt")}",
                        "Check-out Recorded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Error recording check-out: " + ex.Message;
                lblStatus.ForeColor = Color.FromArgb(211, 47, 47); // Red color
                MessageBox.Show("Error recording check-out: " + ex.Message, 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRegistrationStatus(int registrationId, string newStatus)
        {
            try
            {
                string updateQuery = "UPDATE registrations SET status = @status WHERE id = @id";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@status", newStatus),
                    new MySqlParameter("@id", registrationId)
                };
                DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating registration status: {ex.Message}");
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
                if (scanTimer != null)
                {
                    scanTimer.Stop();
                    scanTimer.Dispose();
                    scanTimer = null;
                }
                
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    videoSource = null;
                }
                
                if (currentFrame != null)
                {
                    currentFrame.Dispose();
                    currentFrame = null;
                }
                
                System.Diagnostics.Debug.WriteLine("Scanner stopped");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping camera: {ex.Message}");
            }
        }

        private void frmAttendanceScanner_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();
        }
    }
}

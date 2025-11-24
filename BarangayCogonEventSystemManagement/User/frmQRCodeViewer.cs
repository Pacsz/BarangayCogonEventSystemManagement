using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QRCoder;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmQRCodeViewer : Form
    {
        private string eventName;
        private string qrCodeData;
        private DateTime eventEndDateTime;
        private string registrationStatus;
        private bool isEventEnded;
        private bool isGracePeriodExpired;
        private const int GRACE_PERIOD_HOURS = 2;

        public frmQRCodeViewer(string eventName, string qrCodeData, DateTime eventEndDateTime, string registrationStatus = "Approved")
        {
            InitializeComponent();
            this.eventName = eventName;
            this.qrCodeData = qrCodeData;
            this.eventEndDateTime = eventEndDateTime;
            this.registrationStatus = registrationStatus;
            
            DateTime currentTime = DateTime.Now;
            DateTime attendanceDeadline = eventEndDateTime.AddHours(GRACE_PERIOD_HOURS);
            
            this.isEventEnded = currentTime > eventEndDateTime;
            this.isGracePeriodExpired = currentTime > attendanceDeadline;
            
            InitializeQRViewer();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmQRCodeViewer
            // 
            this.ClientSize = new System.Drawing.Size(400, 550);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQRCodeViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QR Code";
            this.BackColor = Color.FromArgb(46, 51, 73);
            this.ResumeLayout(false);
        }

        private void InitializeQRViewer()
        {
            // Event name label
            Label lblEventName = new Label
            {
                Text = eventName,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(360, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblEventName);

            // QR Code picture box
            PictureBox picQR = new PictureBox
            {
                Location = new Point(90, 70),
                Size = new Size(220, 220),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Generate QR code
            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrData = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q))
                using (QRCode qr = new QRCode(qrData))
                {
                    picQR.Image = qr.GetGraphic(6);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating QR code: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            this.Controls.Add(picQR);

            // QR Code info label (with event status information)
            Label lblInfo = new Label
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(158, 161, 178),
                Location = new Point(20, 305),
                Size = new Size(360, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Check if user has already fully attended (checked in and out)
            if (registrationStatus == "Attended")
            {
                // User has completed attendance - show congratulatory message
                lblInfo.Text = "✅ Congratulations! You've completed your attendance\n" +
                              "You successfully checked in and checked out\n" +
                              "Thank you for participating in this event!";
                lblInfo.ForeColor = Color.FromArgb(76, 175, 80); // Green color
                lblInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
            else if (isGracePeriodExpired)
            {
                // Grace period has expired - QR code is no longer valid
                lblInfo.Text = "⏱️ Sorry, the check-in period has ended\n" +
                              "This QR code is no longer active\n" +
                              $"(Check-in closed {GRACE_PERIOD_HOURS} hours after event ended)";
                lblInfo.ForeColor = Color.FromArgb(211, 47, 47); // Red color
                lblInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
            else if (isEventEnded)
            {
                // Event has ended but still within grace period
                DateTime attendanceDeadline = eventEndDateTime.AddHours(GRACE_PERIOD_HOURS);
                TimeSpan timeRemaining = attendanceDeadline - DateTime.Now;
                string gracePeriodInfo = "";
                
                if (timeRemaining.TotalHours >= 1)
                {
                    int hours = (int)timeRemaining.TotalHours;
                    int minutes = timeRemaining.Minutes;
                    gracePeriodInfo = $"{hours}h {minutes}m";
                }
                else
                {
                    int minutes = (int)timeRemaining.TotalMinutes;
                    gracePeriodInfo = $"{minutes} minute{(minutes > 1 ? "s" : "")}";
                }
                
                lblInfo.Text = $"⏰ Hurry! Event has ended but you can still check in\n" +
                              $"Your QR code is valid for {gracePeriodInfo} more\n" +
                              $"Please scan at the registration desk now!";
                lblInfo.ForeColor = Color.FromArgb(255, 193, 7); // Yellow/amber color
                lblInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
            else
            {
                // Event is ongoing or hasn't started yet
                lblInfo.Text = "📱 Show this QR code when you arrive at the event\n" +
                              "Present it to the registration desk for check-in\n" +
                              $"(You can check in anytime during the event + {GRACE_PERIOD_HOURS} hours after)";
            }
            this.Controls.Add(lblInfo);

            // Save QR button with rounded corners
            Button btnSave = new Button
            {
                Text = "Save QR as Image",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 126, 249),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Location = new Point(100, 380),
                Cursor = Cursors.Hand,
                Tag = picQR // Store reference to picture box for save functionality
            };
            btnSave.FlatAppearance.BorderSize = 0;

            // Add Paint event for rounded Save button
            btnSave.Paint += (s, e) =>
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

            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            // Close button with rounded corners
            Button btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Location = new Point(100, 440),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;

            // Add Paint event for rounded Close button
            btnClose.Paint += (s, e) =>
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

            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            PictureBox picQR = btn.Tag as PictureBox;

            if (picQR?.Image == null)
            {
                MessageBox.Show("No QR code to save.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = $"QR_{eventName.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.png"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    picQR.Image.Save(saveDialog.FileName);
                    MessageBox.Show("QR Code saved successfully!", "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving QR code: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
    }
}

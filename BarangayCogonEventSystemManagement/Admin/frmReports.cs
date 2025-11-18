using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmReports : Form
    {
        public frmReports()
        {
            InitializeComponent();
            CustomizeDataGridView();
            StyleButtons();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void CustomizeDataGridView()
        {
            dgvReports.Columns.Clear();
            dgvReports.AllowUserToAddRows = false;
            dgvReports.ReadOnly = true;

            // GENERAL GRID SETTINGS - Match mainPanel background
            dgvReports.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvReports.BorderStyle = BorderStyle.None;
            dgvReports.GridColor = Color.FromArgb(60, 65, 90);
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // HEADER STYLE - Match sidebar color (same color when selected)
            dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReports.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54); // Same as normal background
            dgvReports.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvReports.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvReports.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvReports.ColumnHeadersHeight = 45;

            // CELL STYLE - Match mainPanel background (keep same color when selected)
            dgvReports.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvReports.DefaultCellStyle.ForeColor = Color.White;
            dgvReports.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73); // Same as normal background
            dgvReports.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReports.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            dgvReports.RowTemplate.Height = 60;
            dgvReports.RowHeadersVisible = false;
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Alternating rows - slightly darker for subtle contrast (same when selected)
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(37, 42, 64);
            dgvReports.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvReports.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 42, 64); // Same as normal background
            dgvReports.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Enable double buffering to reduce flicker
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvReports, new object[] { true });
        }

        private void StyleButtons()
        {
            // Style Export PDF button
            btnExportPDF.FlatStyle = FlatStyle.Flat;
            btnExportPDF.FlatAppearance.BorderSize = 0;
            btnExportPDF.BackColor = Color.FromArgb(0, 126, 249); // Accent blue
            btnExportPDF.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportPDF.Cursor = Cursors.Hand;
            btnExportPDF.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
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

            // Style Refresh button
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.BackColor = Color.FromArgb(60, 65, 90); // Subtle gray-blue
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
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

        private GraphicsPath GetRoundPath(System.Drawing.Rectangle rect, int radius)
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

        private void LoadReports()
        {
            try
            {
                string query = @"
                SELECT 
                    e.id,
                    e.name AS 'Event Name',
                    e.date AS 'Date',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.role = 'attendee') AS 'Attendees',
                    (SELECT COUNT(*) FROM registrations r WHERE r.event_id = e.id AND r.role = 'volunteer') AS 'Volunteers',
                    (SELECT COUNT(*) FROM attendance a 
                        INNER JOIN registrations r ON a.registration_id = r.id 
                        WHERE r.event_id = e.id) AS 'Present'
                FROM events e
                ORDER BY e.date DESC;
                ";

                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                
                // Clear existing rows
                dgvReports.Rows.Clear();

                // Manually configure columns for better control
                if (dgvReports.Columns.Count == 0)
                {
                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "id",
                        HeaderText = "ID",
                        ReadOnly = true,
                        Visible = false
                    });

                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "event_name",
                        HeaderText = "Event Name",
                        ReadOnly = true,
                        FillWeight = 30
                    });

                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "date",
                        HeaderText = "Date",
                        ReadOnly = true,
                        FillWeight = 15
                    });

                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "attendees",
                        HeaderText = "Attendees",
                        ReadOnly = true,
                        FillWeight = 15
                    });

                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "volunteers",
                        HeaderText = "Volunteers",
                        ReadOnly = true,
                        FillWeight = 15
                    });

                    dgvReports.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "present",
                        HeaderText = "Present",
                        ReadOnly = true,
                        FillWeight = 15
                    });
                }

                // Populate rows manually to maintain custom styling
                int totalEvents = dt.Rows.Count;
                int totalAttendees = 0;
                int totalVolunteers = 0;
                int totalPresent = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    int attendees = Convert.ToInt32(dr["Attendees"]);
                    int volunteers = Convert.ToInt32(dr["Volunteers"]);
                    int present = Convert.ToInt32(dr["Present"]);

                    totalAttendees += attendees;
                    totalVolunteers += volunteers;
                    totalPresent += present;

                    dgvReports.Rows.Add(
                        dr["id"],
                        dr["Event Name"],
                        Convert.ToDateTime(dr["Date"]).ToString("MMM dd, yyyy"),
                        attendees,
                        volunteers,
                        present
                    );
                }

                int totalRegistered = totalAttendees + totalVolunteers;
                double attendanceRate = totalRegistered > 0 ? ((double)totalPresent / totalRegistered) * 100 : 0;

                lblTotals.Text = $"Total Events: {totalEvents}\n" +
                                 $"Total Attendees: {totalAttendees}\n" +
                                 $"Total Volunteers: {totalVolunteers}\n" +
                                 $"Total Present: {totalPresent}\n" +
                                 $"Average Attendance Rate: {attendanceRate:F2}%";

                // Clear selection to show proper background color
                dgvReports.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading reports: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReports();
            MessageBox.Show("Reports refreshed successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvReports.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = "EventReports_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToPDF(saveFileDialog.FileName);
                    MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting PDF: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToPDF(string filePath)
        {
            Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

            Paragraph title = new Paragraph("Barangay Cogon Event Attendance Report", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);

            doc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("MMMM dd, yyyy"), normalFont));
            doc.Add(new Paragraph("\n"));

            // Create table with proper column count (excluding hidden ID column)
            PdfPTable table = new PdfPTable(dgvReports.Columns.Count - 1)
            {
                WidthPercentage = 100
            };

            // Add headers
            foreach (DataGridViewColumn column in dgvReports.Columns)
            {
                if (column.Visible)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(column.HeaderText, headerFont))
                    {
                        BackgroundColor = new BaseColor(24, 30, 54),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(headerCell);
                }
            }

            // Add data rows
            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (dgvReports.Columns[cell.ColumnIndex].Visible)
                    {
                        table.AddCell(new Phrase(cell.Value?.ToString() ?? "", normalFont));
                    }
                }
            }

            doc.Add(table);
            doc.Add(new Paragraph("\n"));

            Paragraph summaryTitle = new Paragraph("Summary", titleFont)
            {
                SpacingBefore = 10
            };
            doc.Add(summaryTitle);

            Paragraph summaryDetails = new Paragraph(lblTotals.Text, normalFont);
            doc.Add(summaryDetails);

            doc.Close();
        }
    }
}

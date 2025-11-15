using System;
using System.Data;
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
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            LoadReports();
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
                FROM events e;
                ";

                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                dgvReports.DataSource = dt;

                int totalEvents = dt.Rows.Count;
                int totalAttendees = 0;
                int totalVolunteers = 0;
                int totalPresent = 0;

                foreach (DataRow row in dt.Rows)
                {
                    totalAttendees += Convert.ToInt32(row["Attendees"]);
                    totalVolunteers += Convert.ToInt32(row["Volunteers"]);
                    totalPresent += Convert.ToInt32(row["Present"]);
                }

                int totalRegistered = totalAttendees + totalVolunteers;
                double attendanceRate = totalRegistered > 0 ? ((double)totalPresent / totalRegistered) * 100 : 0;

                lblTotals.Text = $"Total Events: {totalEvents}\n" +
                                 $"Total Attendees: {totalAttendees}\n" +
                                 $"Total Volunteers: {totalVolunteers}\n" +
                                 $"Total Present: {totalPresent}\n" +
                                 $"Average Attendance Rate: {attendanceRate:F2}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading reports: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReports();
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
                MessageBox.Show("Error exporting PDF: " + ex.Message);
            }
        }

        private void ExportToPDF(string filePath)
        {
            Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            Paragraph title = new Paragraph("Barangay Cogon Event Attendance Report", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);

            doc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("MMMM dd, yyyy"), normalFont));
            doc.Add(new Paragraph("\n"));

            PdfPTable table = new PdfPTable(dgvReports.Columns.Count)
            {
                WidthPercentage = 100
            };

            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Phrase(cell.Value?.ToString() ?? "", normalFont));
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

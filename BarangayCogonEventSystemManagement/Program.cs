using BarangayCogonEventManagementSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarangayCogonEventSystemManagement
{
    internal static class Program
    {
        // Import SetProcessDPIAware to disable DPI scaling
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Disable DPI scaling to ensure consistent form sizes
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmDashboardUser(1, "james", "attendee"));
            Application.Run(new frmDashboardAdmin());
            //Application.Run(new frmUserLogin());
        }
    }
}

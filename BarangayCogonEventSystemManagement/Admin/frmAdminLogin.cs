using System;
using System.Data;
using System.Windows.Forms;
using BarangayCogonEventSystemManagement;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmAdminLogin : Form
    {
        public frmAdminLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Email and Password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "SELECT * FROM users WHERE email = @email AND password = @password";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@email", email),
                    new MySqlParameter("@password", password)
                };

                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    string systemRole = dt.Rows[0]["system_role"].ToString().ToLower();
                    string firstName = dt.Rows[0]["first_name"].ToString();
                    string lastName = dt.Rows[0]["last_name"].ToString();
                    string fullName = $"{firstName} {lastName}";
                    int userId = Convert.ToInt32(dt.Rows[0]["id"]);

                    // Only allow admin
                    if (systemRole != "admin")
                    {
                        MessageBox.Show("Access denied. Only admins can log in here.", "Login Failed",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show($"Welcome, {fullName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    frmDashboardAdmin admin = new frmDashboardAdmin();
                    admin.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmRegister register = new frmRegister();
            register.ShowDialog();
            this.Close();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void frmAdminLogin_Load(object sender, EventArgs e)
        {

        }
    }
}

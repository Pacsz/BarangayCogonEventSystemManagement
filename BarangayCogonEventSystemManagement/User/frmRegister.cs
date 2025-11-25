using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string address = txtAddress.Text.Trim();
            string contact = txtContact.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || 
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all required fields (First Name, Last Name, Email, and Password).", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if passwords match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.", 
                    "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
                return;
            }

            try
            {
                string checkQuery = "SELECT * FROM users WHERE email = @email";
                MySqlParameter[] checkParam = { new MySqlParameter("@email", email) };
                DataTable dt = DatabaseHelper.ExecuteQuery(checkQuery, checkParam);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("This email is already registered.", "Duplicate Entry", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string insertQuery = @"INSERT INTO users (first_name, last_name, email, password, system_role, address, contact_number, created_at)
                                       VALUES (@first_name, @last_name, @email, @password, 'user', @address, @contact, NOW())";

                MySqlParameter[] insertParams = {
                    new MySqlParameter("@first_name", firstName),
                    new MySqlParameter("@last_name", lastName),
                    new MySqlParameter("@email", email),
                    new MySqlParameter("@password", password),
                    new MySqlParameter("@address", address),
                    new MySqlParameter("@contact", contact)
                };

                DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams);

                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                frmUserLogin login = new frmUserLogin();
                login.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmUserLogin login = new frmUserLogin();
            login.ShowDialog();
            this.Close();
        }
    }
}

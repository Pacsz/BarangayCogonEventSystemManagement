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

            // Validate empty fields
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(contact))
            {
                MessageBox.Show("Please fill in all required fields.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Philippine mobile number
            // Accepts: 09XXXXXXXXX or +639XXXXXXXXX
            bool isValidMobile = false;
            if (contact.StartsWith("09") && contact.Length == 11 && long.TryParse(contact, out _))
            {
                isValidMobile = true;
            }
            else if (contact.StartsWith("+639") && contact.Length == 13 && long.TryParse(contact.Substring(1), out _))
            {
                isValidMobile = true;
            }
            if (!isValidMobile)
            {
                MessageBox.Show("Please enter a valid Philippine mobile number (e.g., 09XXXXXXXXX or +639XXXXXXXXX).",
                    "Invalid Mobile Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContact.Focus();
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

        private void lblFirstName_Click(object sender, EventArgs e)
        {

        }
    }
}

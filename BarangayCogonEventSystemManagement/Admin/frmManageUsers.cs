using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using FontAwesome.Sharp;

namespace BarangayCogonEventManagementSystem
{
    public partial class frmManageUsers : Form
    {
        private ContextMenuStrip contextMenuActions;
        private TextBox txtSearch;

        public frmManageUsers()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(46, 51, 73);
            InitializeContextMenu();
            InitializeFilters();
            CustomizeDataGridView();
            LoadUsers();
            StyleAddButton();
        }

        private void InitializeContextMenu()
        {
            contextMenuActions = new ContextMenuStrip();
            contextMenuActions.BackColor = Color.FromArgb(37, 42, 64);
            contextMenuActions.ForeColor = Color.White;
            contextMenuActions.ShowImageMargin = false;
            contextMenuActions.Renderer = new ToolStripProfessionalRenderer(new CustomContextMenuColorTable());
        }

        private class CustomContextMenuColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(46, 51, 73); }
            }

            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(37, 42, 64); }
            }

            public override Color MenuBorder
            {
                get { return Color.FromArgb(60, 65, 90); }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(46, 51, 73); }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(46, 51, 73); }
            }

            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.FromArgb(46, 51, 73); }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.FromArgb(46, 51, 73); }
            }
        }

        private void InitializeFilters()
        {
            // Search box
            txtSearch = new TextBox
            {
                Location = new Point(20, 15),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(37, 42, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.Text = "🔍 Search users...";
            txtSearch.ForeColor = Color.Gray;
            
            txtSearch.Enter += (s, ev) => {
                if (txtSearch.Text == "🔍 Search users...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.White;
                }
            };
            
            txtSearch.Leave += (s, ev) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Search users...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            txtSearch.TextChanged += (s, ev) => LoadUsers();

            this.Controls.Add(txtSearch);
            txtSearch.BringToFront();

            if (dgvUsers != null)
            {
                dgvUsers.Location = new Point(20, 80);
                dgvUsers.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 140);
            }
        }

        private void CustomizeDataGridView()
        {
            dgvUsers.CellPainting -= dgvUsers_CellPainting;
            dgvUsers.CellClick -= dgvUsers_CellClick;

            dgvUsers.Columns.Clear();
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.ReadOnly = true;

            dgvUsers.BackgroundColor = Color.FromArgb(46, 51, 73);
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.GridColor = Color.FromArgb(60, 65, 90);
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 30, 54);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 30, 54);
            dgvUsers.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsers.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvUsers.ColumnHeadersHeight = 45;

            dgvUsers.DefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvUsers.DefaultCellStyle.ForeColor = Color.White;
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvUsers.RowTemplate.Height = 60;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(46, 51, 73);
            dgvUsers.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 51, 73);
            dgvUsers.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvUsers, new object[] { true });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                HeaderText = "ID",
                ReadOnly = true,
                Visible = false
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "first_name",
                HeaderText = "First Name",
                ReadOnly = true,
                FillWeight = 18
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "last_name",
                HeaderText = "Last Name",
                ReadOnly = true,
                FillWeight = 18
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "email",
                HeaderText = "Email",
                ReadOnly = true,
                FillWeight = 22
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "contact_number",
                HeaderText = "Contact Number",
                ReadOnly = true,
                FillWeight = 15
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "address",
                HeaderText = "Address",
                ReadOnly = true,
                FillWeight = 19
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "password",
                HeaderText = "Password",
                ReadOnly = true,
                Visible = false
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActionColumn",
                HeaderText = "Action",
                ReadOnly = true,
                FillWeight = 10
            });

            dgvUsers.CellPainting += dgvUsers_CellPainting;
            dgvUsers.CellClick += dgvUsers_CellClick;
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

        private void dgvUsers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var actionColumn = dgvUsers.Columns["ActionColumn"];
            if (actionColumn == null) return;

            if (e.ColumnIndex == actionColumn.Index)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                var idValue = dgvUsers.Rows[e.RowIndex].Cells["id"].Value;
                if (idValue == null || Convert.ToInt32(idValue) == 0)
                {
                    e.Handled = true;
                    return;
                }

                Rectangle cellBounds = e.CellBounds;
                int buttonWidth = 70;
                int buttonHeight = 30;

                int buttonX = cellBounds.X + (cellBounds.Width - buttonWidth) / 2;
                int buttonY = cellBounds.Y + (cellBounds.Height - buttonHeight) / 2;

                Rectangle viewRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                int radius = 10;

                using (GraphicsPath viewPath = GetRoundPath(viewRect, radius))
                using (SolidBrush viewBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                using (Font btnFont = new Font("Segoe UI", 12F, FontStyle.Bold))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(viewBrush, viewPath);
                    e.Graphics.DrawString("...", btnFont, textBrush, viewRect, sf);
                }
                
                e.Handled = true;
            }
        }

        private void LoadUsers()
        {
            try
            {
                string query = @"SELECT 
                                    id, 
                                    first_name, 
                                    last_name, 
                                    email, 
                                    password,
                                    contact_number,
                                    address
                                FROM users 
                                WHERE system_role = 'user'";

                var paramsList = new System.Collections.Generic.List<MySqlParameter>();

                // Add search filter
                if (txtSearch != null)
                {
                    string searchText = txtSearch.Text;
                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != "🔍 Search users...")
                    {
                        query += @" AND (first_name LIKE @search 
                                    OR last_name LIKE @search 
                                    OR email LIKE @search
                                    OR contact_number LIKE @search
                                    OR address LIKE @search)";
                        paramsList.Add(new MySqlParameter("@search", "%" + searchText + "%"));
                    }
                }

                query += " ORDER BY created_at DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, paramsList.ToArray());

                dgvUsers.Rows.Clear();

                if (dt.Rows.Count == 0)
                {
                    int placeholderIndex = dgvUsers.Rows.Add(
                        0, "", "", 
                        "No users found matching your criteria", 
                        "", "", "", ""
                    );

                    DataGridViewRow placeholderRow = dgvUsers.Rows[placeholderIndex];
                    placeholderRow.DefaultCellStyle.ForeColor = Color.FromArgb(158, 161, 178);
                    placeholderRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    placeholderRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int rowIndex = dgvUsers.Rows.Add(
                            dr["id"],
                            dr["first_name"],
                            dr["last_name"],
                            dr["email"],
                            dr["contact_number"],
                            dr["address"],
                            dr["password"],
                            ""
                        );
                    }
                }

                dgvUsers.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvUsers.Columns[e.ColumnIndex].Name == "ActionColumn")
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                
                var idValue = row.Cells["id"].Value;
                if (idValue == null || Convert.ToInt32(idValue) == 0)
                {
                    return;
                }
                
                int userId = Convert.ToInt32(row.Cells["id"].Value);

                contextMenuActions.Items.Clear();

                ToolStripMenuItem editItem = new ToolStripMenuItem("✏ Edit");
                editItem.Font = new Font("Segoe UI", 10F);
                editItem.Click += (s, ev) => ShowEditUserForm(userId);
                contextMenuActions.Items.Add(editItem);

                ToolStripMenuItem deleteItem = new ToolStripMenuItem("✗ Delete");
                deleteItem.Font = new Font("Segoe UI", 10F);
                deleteItem.Click += (s, ev) => DeleteUser(userId);
                contextMenuActions.Items.Add(deleteItem);

                Rectangle rect = dgvUsers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                int buttonWidth = 70;
                int buttonHeight = 30;
                int buttonX = rect.Left + (rect.Width - buttonWidth) / 2;
                int buttonY = rect.Top + (rect.Height - buttonHeight) / 2;

                Point pt = new Point(buttonX + buttonWidth + 5, buttonY);
                contextMenuActions.Show(dgvUsers, pt);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            ShowAddUserForm();
        }

        private void ShowAddUserForm()
        {
            Form addForm = CreateUserForm("Add User", null);
            addForm.ShowDialog();
        }

        private void ShowEditUserForm(int userId)
        {
            try
            {
                string query = "SELECT * FROM users WHERE id = @id";
                MySqlParameter[] parameters = { new MySqlParameter("@id", userId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    Form editForm = CreateUserForm("Edit User", dt.Rows[0]);
                    editForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Form CreateUserForm(string title, DataRow userData)
        {
            Form userForm = new Form
            {
                Text = title,
                Size = new Size(500, 650),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(46, 51, 73)
            };

            Label lblHeader = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblFirstName = new Label { Text = "First Name:", Location = new Point(30, 70), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtFirstName = new TextBox { Location = new Point(30, 100), Size = new Size(420, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblLastName = new Label { Text = "Last Name:", Location = new Point(30, 140), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtLastName = new TextBox { Location = new Point(30, 170), Size = new Size(420, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(30, 210), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtEmail = new TextBox { Location = new Point(30, 240), Size = new Size(420, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblPassword = new Label { Text = "Password:", Location = new Point(30, 280), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtPassword = new TextBox { Location = new Point(30, 310), Size = new Size(420, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, UseSystemPasswordChar = true };

            Label lblContact = new Label { Text = "Contact Number:", Location = new Point(30, 350), Size = new Size(140, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtContact = new TextBox { Location = new Point(30, 380), Size = new Size(420, 30), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblAddress = new Label { Text = "Address:", Location = new Point(30, 420), Size = new Size(120, 25), ForeColor = Color.White, Font = new Font("Segoe UI", 10F) };
            TextBox txtAddress = new TextBox { Location = new Point(30, 450), Size = new Size(420, 50), Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(37, 42, 69), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Multiline = true };

            RoundedButton btnSave = new RoundedButton
            {
                Text = userData == null ? "Add User" : "Update User",
                Location = new Point(150, 520),
                Size = new Size(125, 45),
                BackColor = Color.FromArgb(25, 118, 210),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderRadius = 10,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;

            RoundedButton btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(280, 520),
                Size = new Size(100, 45),
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderRadius = 10,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) => userForm.Close();

            // Populate data if editing
            if (userData != null)
            {
                txtFirstName.Text = userData["first_name"].ToString();
                txtLastName.Text = userData["last_name"].ToString();
                txtEmail.Text = userData["email"].ToString();
                txtPassword.Text = userData["password"].ToString();
                txtContact.Text = userData["contact_number"].ToString();
                txtAddress.Text = userData["address"].ToString();
            }

            btnSave.Click += (s, ev) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtFirstName.Text) || 
                        string.IsNullOrWhiteSpace(txtLastName.Text) ||
                        string.IsNullOrWhiteSpace(txtEmail.Text) ||
                        string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        MessageBox.Show("Please fill in all required fields (First Name, Last Name, Email, Password).", 
                            "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (userData == null)
                    {
                        // Check if email already exists
                        string checkQuery = "SELECT COUNT(*) FROM users WHERE email = @email";
                        MySqlParameter[] checkParams = { new MySqlParameter("@email", txtEmail.Text) };
                        DataTable checkDt = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);
                        
                        if (checkDt.Rows.Count > 0 && Convert.ToInt32(checkDt.Rows[0][0]) > 0)
                        {
                            MessageBox.Show("Email already exists. Please use a different email.", 
                                "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        DialogResult confirmResult = MessageBox.Show(
                            $"Do you want to add this user?\n\n" +
                            $"Name: {txtFirstName.Text} {txtLastName.Text}\n" +
                            $"Email: {txtEmail.Text}",
                            "Confirm Add User",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            string query = @"INSERT INTO users (first_name, last_name, email, password, contact_number, address, system_role)
                                             VALUES (@first_name, @last_name, @email, @password, @contact, @address, 'user')";
                            MySqlParameter[] parameters = {
                                new MySqlParameter("@first_name", txtFirstName.Text),
                                new MySqlParameter("@last_name", txtLastName.Text),
                                new MySqlParameter("@email", txtEmail.Text),
                                new MySqlParameter("@password", txtPassword.Text),
                                new MySqlParameter("@contact", txtContact.Text),
                                new MySqlParameter("@address", txtAddress.Text)
                            };
                            DatabaseHelper.ExecuteNonQuery(query, parameters);
                            MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsers();
                            userForm.Close();
                        }
                    }
                    else
                    {
                        // Check if email exists for other users
                        string checkQuery = "SELECT COUNT(*) FROM users WHERE email = @email AND id != @id";
                        MySqlParameter[] checkParams = { 
                            new MySqlParameter("@email", txtEmail.Text),
                            new MySqlParameter("@id", userData["id"])
                        };
                        DataTable checkDt = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);
                        
                        if (checkDt.Rows.Count > 0 && Convert.ToInt32(checkDt.Rows[0][0]) > 0)
                        {
                            MessageBox.Show("Email already exists. Please use a different email.", 
                                "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        DialogResult confirmResult = MessageBox.Show(
                            $"Do you want to update this user?\n\n" +
                            $"Name: {txtFirstName.Text} {txtLastName.Text}\n" +
                            $"Email: {txtEmail.Text}",
                            "Confirm Update User",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            string query = @"UPDATE users SET first_name=@first_name, last_name=@last_name, 
                                             email=@email, password=@password, contact_number=@contact, 
                                             address=@address WHERE id=@id";
                            MySqlParameter[] parameters = {
                                new MySqlParameter("@id", userData["id"]),
                                new MySqlParameter("@first_name", txtFirstName.Text),
                                new MySqlParameter("@last_name", txtLastName.Text),
                                new MySqlParameter("@email", txtEmail.Text),
                                new MySqlParameter("@password", txtPassword.Text),
                                new MySqlParameter("@contact", txtContact.Text),
                                new MySqlParameter("@address", txtAddress.Text)
                            };
                            DatabaseHelper.ExecuteNonQuery(query, parameters);
                            MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsers();
                            userForm.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            userForm.Controls.Add(lblHeader);
            userForm.Controls.Add(lblFirstName);
            userForm.Controls.Add(txtFirstName);
            userForm.Controls.Add(lblLastName);
            userForm.Controls.Add(txtLastName);
            userForm.Controls.Add(lblEmail);
            userForm.Controls.Add(txtEmail);
            userForm.Controls.Add(lblPassword);
            userForm.Controls.Add(txtPassword);
            userForm.Controls.Add(lblContact);
            userForm.Controls.Add(txtContact);
            userForm.Controls.Add(lblAddress);
            userForm.Controls.Add(txtAddress);
            userForm.Controls.Add(btnSave);
            userForm.Controls.Add(btnCancel);

            return userForm;
        }

        private void DeleteUser(int userId)
        {
            try
            {
                // Get user details and count of related records before deletion
                string countQuery = @"SELECT 
                                        CONCAT(u.first_name, ' ', u.last_name) AS user_name,
                                        u.email,
                                        (SELECT COUNT(*) FROM registrations WHERE user_id = @id) AS registration_count,
                                        (SELECT COUNT(*) FROM attendance a 
                                         INNER JOIN registrations r ON a.registration_id = r.id 
                                         WHERE r.user_id = @id) AS attendance_count
                                      FROM users u 
                                      WHERE u.id = @id";
                
                MySqlParameter[] countParams = { new MySqlParameter("@id", userId) };
                DataTable dt = DatabaseHelper.ExecuteQuery(countQuery, countParams);
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                string userName = dt.Rows[0]["user_name"].ToString();
                string userEmail = dt.Rows[0]["email"].ToString();
                int registrationCount = Convert.ToInt32(dt.Rows[0]["registration_count"]);
                int attendanceCount = Convert.ToInt32(dt.Rows[0]["attendance_count"]);
                
                // Build warning message
                string warningMessage = $"Are you sure you want to delete this user?\n\n" +
                                       $"User: {userName}\n" +
                                       $"Email: {userEmail}\n\n" +
                                       $"⚠️ WARNING: This will also permanently delete:\n" +
                                       $"  • {registrationCount} registration(s)\n" +
                                       $"  • {attendanceCount} attendance record(s)\n\n" +
                                       $"This action cannot be undone!";
                
                DialogResult result = MessageBox.Show(warningMessage, "Confirm Delete", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM users WHERE id=@id";
                    MySqlParameter[] parameters = { new MySqlParameter("@id", userId) };
                    DatabaseHelper.ExecuteNonQuery(query, parameters);
                    
                    MessageBox.Show(
                        $"User '{userName}' and all related records deleted successfully!\n\n" +
                        $"Deleted:\n" +
                        $"  • 1 user\n" +
                        $"  • {registrationCount} registration(s)\n" +
                        $"  • {attendanceCount} attendance record(s)", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleAddButton()
        {
            btnAddUser.FlatStyle = FlatStyle.Flat;
            btnAddUser.FlatAppearance.BorderSize = 0;
            btnAddUser.Paint += (s, e) =>
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
    }
}

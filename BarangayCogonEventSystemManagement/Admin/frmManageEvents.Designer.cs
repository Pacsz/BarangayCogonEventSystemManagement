namespace BarangayCogonEventManagementSystem
{
    partial class frmManageEvents
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Label lblVenue;
        private System.Windows.Forms.TextBox txtVenue;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label lblOrganizer;
        private System.Windows.Forms.TextBox txtOrganizer;
        private System.Windows.Forms.Button btnAddEvent;
        private System.Windows.Forms.Button btnUpdateEvent;
        private System.Windows.Forms.Button btnDeleteEvent;
        private System.Windows.Forms.DataGridView dgvEvents;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblTime = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.lblVenue = new System.Windows.Forms.Label();
            this.txtVenue = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lblOrganizer = new System.Windows.Forms.Label();
            this.txtOrganizer = new System.Windows.Forms.TextBox();
            this.btnAddEvent = new System.Windows.Forms.Button();
            this.btnUpdateEvent = new System.Windows.Forms.Button();
            this.btnDeleteEvent = new System.Windows.Forms.Button();
            this.dgvEvents = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(135, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Event Title:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(160, 20);
            this.txtTitle.Multiline = true;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(250, 35);
            this.txtTitle.TabIndex = 1;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblDescription.ForeColor = System.Drawing.Color.White;
            this.lblDescription.Location = new System.Drawing.Point(20, 70);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(142, 23);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(160, 70);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(250, 60);
            this.txtDescription.TabIndex = 3;
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(20, 145);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(100, 23);
            this.lblDate.TabIndex = 4;
            this.lblDate.Text = "Date:";
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(160, 145);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(250, 26);
            this.dtpDate.TabIndex = 5;
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(20, 185);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(100, 23);
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "Time:";
            // 
            // dtpTime
            // 
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTime.Location = new System.Drawing.Point(160, 185);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(250, 26);
            this.dtpTime.TabIndex = 7;
            // 
            // lblVenue
            // 
            this.lblVenue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblVenue.ForeColor = System.Drawing.Color.White;
            this.lblVenue.Location = new System.Drawing.Point(20, 225);
            this.lblVenue.Name = "lblVenue";
            this.lblVenue.Size = new System.Drawing.Size(100, 23);
            this.lblVenue.TabIndex = 8;
            this.lblVenue.Text = "Venue:";
            // 
            // txtVenue
            // 
            this.txtVenue.Location = new System.Drawing.Point(160, 225);
            this.txtVenue.Multiline = true;
            this.txtVenue.Name = "txtVenue";
            this.txtVenue.Size = new System.Drawing.Size(250, 35);
            this.txtVenue.TabIndex = 9;
            // 
            // lblType
            // 
            this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblType.ForeColor = System.Drawing.Color.White;
            this.lblType.Location = new System.Drawing.Point(20, 275);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(100, 23);
            this.lblType.TabIndex = 10;
            this.lblType.Text = "Type:";
            // 
            // cboType
            // 
            this.cboType.Items.AddRange(new object[] {
            "Community Service",
            "Health Drive",
            "Cleanup Drive",
            "Barangay Assembly",
            "Training / Workshop"});
            this.cboType.Location = new System.Drawing.Point(160, 275);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(250, 28);
            this.cboType.TabIndex = 11;
            // 
            // lblOrganizer
            // 
            this.lblOrganizer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblOrganizer.ForeColor = System.Drawing.Color.White;
            this.lblOrganizer.Location = new System.Drawing.Point(20, 320);
            this.lblOrganizer.Name = "lblOrganizer";
            this.lblOrganizer.Size = new System.Drawing.Size(116, 34);
            this.lblOrganizer.TabIndex = 12;
            this.lblOrganizer.Text = "Organizer:";
            // 
            // txtOrganizer
            // 
            this.txtOrganizer.Location = new System.Drawing.Point(160, 320);
            this.txtOrganizer.Multiline = true;
            this.txtOrganizer.Name = "txtOrganizer";
            this.txtOrganizer.Size = new System.Drawing.Size(250, 35);
            this.txtOrganizer.TabIndex = 13;
            // 
            // btnAddEvent
            // 
            this.btnAddEvent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnAddEvent.ForeColor = System.Drawing.Color.White;
            this.btnAddEvent.Location = new System.Drawing.Point(90, 380);
            this.btnAddEvent.Name = "btnAddEvent";
            this.btnAddEvent.Size = new System.Drawing.Size(95, 45);
            this.btnAddEvent.TabIndex = 14;
            this.btnAddEvent.Text = "Add";
            this.btnAddEvent.UseVisualStyleBackColor = false;
            this.btnAddEvent.Click += new System.EventHandler(this.btnAddEvent_Click);
            // 
            // btnUpdateEvent
            // 
            this.btnUpdateEvent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(142)))), ((int)(((byte)(60)))));
            this.btnUpdateEvent.ForeColor = System.Drawing.Color.White;
            this.btnUpdateEvent.Location = new System.Drawing.Point(210, 380);
            this.btnUpdateEvent.Name = "btnUpdateEvent";
            this.btnUpdateEvent.Size = new System.Drawing.Size(95, 45);
            this.btnUpdateEvent.TabIndex = 15;
            this.btnUpdateEvent.Text = "Update";
            this.btnUpdateEvent.UseVisualStyleBackColor = false;
            this.btnUpdateEvent.Click += new System.EventHandler(this.btnUpdateEvent_Click);
            // 
            // btnDeleteEvent
            // 
            this.btnDeleteEvent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnDeleteEvent.ForeColor = System.Drawing.Color.White;
            this.btnDeleteEvent.Location = new System.Drawing.Point(330, 380);
            this.btnDeleteEvent.Name = "btnDeleteEvent";
            this.btnDeleteEvent.Size = new System.Drawing.Size(95, 45);
            this.btnDeleteEvent.TabIndex = 16;
            this.btnDeleteEvent.Text = "Delete";
            this.btnDeleteEvent.UseVisualStyleBackColor = false;
            this.btnDeleteEvent.Click += new System.EventHandler(this.btnDeleteEvent_Click);
            // 
            // dgvEvents
            // 
            this.dgvEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEvents.ColumnHeadersHeight = 34;
            this.dgvEvents.Location = new System.Drawing.Point(450, 20);
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.ReadOnly = true;
            this.dgvEvents.RowHeadersWidth = 62;
            this.dgvEvents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEvents.Size = new System.Drawing.Size(476, 565);
            this.dgvEvents.TabIndex = 17;
            this.dgvEvents.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEvents_CellClick);
            this.dgvEvents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEvents_CellContentClick);
            // 
            // frmManageEvents
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(948, 608);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.lblVenue);
            this.Controls.Add(this.txtVenue);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.lblOrganizer);
            this.Controls.Add(this.txtOrganizer);
            this.Controls.Add(this.btnAddEvent);
            this.Controls.Add(this.btnUpdateEvent);
            this.Controls.Add(this.btnDeleteEvent);
            this.Controls.Add(this.dgvEvents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmManageEvents";
            this.Text = "Manage Events";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

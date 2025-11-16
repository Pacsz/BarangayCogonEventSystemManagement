using FontAwesome.Sharp;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BarangayCogonEventManagementSystem
{
    partial class frmApproveRegistrations
    {
        private IContainer components = null;
        private DataGridView dgvRegistrations;
        private ContextMenuStrip contextMenuActions;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvRegistrations = new System.Windows.Forms.DataGridView();
            this.contextMenuActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegistrations)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRegistrations
            // 
            this.dgvRegistrations.AllowUserToAddRows = false;
            this.dgvRegistrations.AllowUserToDeleteRows = false;
            this.dgvRegistrations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRegistrations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRegistrations.BackgroundColor = System.Drawing.Color.White;
            this.dgvRegistrations.ColumnHeadersHeight = 40;
            this.dgvRegistrations.Location = new System.Drawing.Point(43, 46);
            this.dgvRegistrations.Name = "dgvRegistrations";
            this.dgvRegistrations.ReadOnly = true;
            this.dgvRegistrations.RowHeadersVisible = false;
            this.dgvRegistrations.RowHeadersWidth = 62;
            this.dgvRegistrations.RowTemplate.Height = 35;
            this.dgvRegistrations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRegistrations.Size = new System.Drawing.Size(859, 541);
            this.dgvRegistrations.TabIndex = 1;
            // 
            // contextMenuActions
            // 
            this.contextMenuActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuActions.Name = "contextMenuActions";
            this.contextMenuActions.Size = new System.Drawing.Size(61, 4);
            // 
            // frmApproveRegistrations
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(948, 608);
            this.Controls.Add(this.dgvRegistrations);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmApproveRegistrations";
            this.Text = "Approve Registrations - BEMS";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegistrations)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

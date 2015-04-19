namespace Dimmer_Labels_Wizard
{
    partial class FORM_CabinetAddressResolution
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ResolvedDataGrid = new System.Windows.Forms.DataGridView();
            this.ResolvedCabinetsCount = new System.Windows.Forms.Label();
            this.ResolvedItemsLabel = new System.Windows.Forms.Label();
            this.UnresolvedDataGrid = new System.Windows.Forms.DataGridView();
            this.UnresolvedItemsLabel = new System.Windows.Forms.Label();
            this.UnresolvedCabinetsCount = new System.Windows.Forms.Label();
            this.Resolved_Channel_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dimmer_Number_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Resolved_Cabinet_Number_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Resolved_Rack_Number_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unresolved_Channel_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unresolved_Dimmer_Number_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unresolved_Cabinet_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unresolved_Rack_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ResolvedDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnresolvedDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Resolved Cabinet Addresses";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(475, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Unresolved Cabinet Addresses";
            // 
            // ResolvedDataGrid
            // 
            this.ResolvedDataGrid.AllowUserToAddRows = false;
            this.ResolvedDataGrid.AllowUserToDeleteRows = false;
            this.ResolvedDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResolvedDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Resolved_Channel_Col,
            this.Dimmer_Number_Col,
            this.Resolved_Cabinet_Number_Col,
            this.Resolved_Rack_Number_Col});
            this.ResolvedDataGrid.Location = new System.Drawing.Point(16, 30);
            this.ResolvedDataGrid.Name = "ResolvedDataGrid";
            this.ResolvedDataGrid.RowHeadersVisible = false;
            this.ResolvedDataGrid.Size = new System.Drawing.Size(304, 407);
            this.ResolvedDataGrid.TabIndex = 2;
            this.ResolvedDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResolvedDataGrid_CellContentClick);
            // 
            // ResolvedCabinetsCount
            // 
            this.ResolvedCabinetsCount.AutoSize = true;
            this.ResolvedCabinetsCount.Location = new System.Drawing.Point(258, 13);
            this.ResolvedCabinetsCount.Name = "ResolvedCabinetsCount";
            this.ResolvedCabinetsCount.Size = new System.Drawing.Size(25, 13);
            this.ResolvedCabinetsCount.TabIndex = 3;
            this.ResolvedCabinetsCount.Text = "000";
            // 
            // ResolvedItemsLabel
            // 
            this.ResolvedItemsLabel.AutoSize = true;
            this.ResolvedItemsLabel.Location = new System.Drawing.Point(289, 13);
            this.ResolvedItemsLabel.Name = "ResolvedItemsLabel";
            this.ResolvedItemsLabel.Size = new System.Drawing.Size(31, 13);
            this.ResolvedItemsLabel.TabIndex = 4;
            this.ResolvedItemsLabel.Text = "items";
            // 
            // UnresolvedDataGrid
            // 
            this.UnresolvedDataGrid.AllowUserToAddRows = false;
            this.UnresolvedDataGrid.AllowUserToDeleteRows = false;
            this.UnresolvedDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UnresolvedDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Unresolved_Channel_Col,
            this.Unresolved_Dimmer_Number_Col,
            this.Unresolved_Cabinet_Col,
            this.Unresolved_Rack_Col});
            this.UnresolvedDataGrid.Location = new System.Drawing.Point(478, 30);
            this.UnresolvedDataGrid.Name = "UnresolvedDataGrid";
            this.UnresolvedDataGrid.RowHeadersVisible = false;
            this.UnresolvedDataGrid.Size = new System.Drawing.Size(304, 407);
            this.UnresolvedDataGrid.TabIndex = 5;
            // 
            // UnresolvedItemsLabel
            // 
            this.UnresolvedItemsLabel.AutoSize = true;
            this.UnresolvedItemsLabel.Location = new System.Drawing.Point(752, 13);
            this.UnresolvedItemsLabel.Name = "UnresolvedItemsLabel";
            this.UnresolvedItemsLabel.Size = new System.Drawing.Size(31, 13);
            this.UnresolvedItemsLabel.TabIndex = 7;
            this.UnresolvedItemsLabel.Text = "items";
            // 
            // UnresolvedCabinetsCount
            // 
            this.UnresolvedCabinetsCount.AutoSize = true;
            this.UnresolvedCabinetsCount.Location = new System.Drawing.Point(721, 13);
            this.UnresolvedCabinetsCount.Name = "UnresolvedCabinetsCount";
            this.UnresolvedCabinetsCount.Size = new System.Drawing.Size(25, 13);
            this.UnresolvedCabinetsCount.TabIndex = 6;
            this.UnresolvedCabinetsCount.Text = "000";
            // 
            // Resolved_Channel_Col
            // 
            this.Resolved_Channel_Col.HeaderText = "Channel";
            this.Resolved_Channel_Col.Name = "Resolved_Channel_Col";
            this.Resolved_Channel_Col.ReadOnly = true;
            this.Resolved_Channel_Col.Width = 50;
            // 
            // Dimmer_Number_Col
            // 
            this.Dimmer_Number_Col.HeaderText = "Dimmer/Distro Number";
            this.Dimmer_Number_Col.Name = "Dimmer_Number_Col";
            // 
            // Resolved_Cabinet_Number_Col
            // 
            this.Resolved_Cabinet_Number_Col.HeaderText = "Cabinet Number";
            this.Resolved_Cabinet_Number_Col.Name = "Resolved_Cabinet_Number_Col";
            this.Resolved_Cabinet_Number_Col.Width = 75;
            // 
            // Resolved_Rack_Number_Col
            // 
            this.Resolved_Rack_Number_Col.HeaderText = "Rack Number";
            this.Resolved_Rack_Number_Col.Name = "Resolved_Rack_Number_Col";
            this.Resolved_Rack_Number_Col.Width = 75;
            // 
            // Unresolved_Channel_Col
            // 
            this.Unresolved_Channel_Col.HeaderText = "Channel";
            this.Unresolved_Channel_Col.Name = "Unresolved_Channel_Col";
            this.Unresolved_Channel_Col.ReadOnly = true;
            this.Unresolved_Channel_Col.Width = 50;
            // 
            // Unresolved_Dimmer_Number_Col
            // 
            this.Unresolved_Dimmer_Number_Col.HeaderText = "Dimmer/Distro Number";
            this.Unresolved_Dimmer_Number_Col.Name = "Unresolved_Dimmer_Number_Col";
            // 
            // Unresolved_Cabinet_Col
            // 
            this.Unresolved_Cabinet_Col.HeaderText = "Cabinet Number";
            this.Unresolved_Cabinet_Col.Name = "Unresolved_Cabinet_Col";
            this.Unresolved_Cabinet_Col.Width = 75;
            // 
            // Unresolved_Rack_Col
            // 
            this.Unresolved_Rack_Col.HeaderText = "Rack Number";
            this.Unresolved_Rack_Col.Name = "Unresolved_Rack_Col";
            this.Unresolved_Rack_Col.Width = 75;
            // 
            // FORM_CabinetAddressResolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 449);
            this.Controls.Add(this.UnresolvedItemsLabel);
            this.Controls.Add(this.UnresolvedCabinetsCount);
            this.Controls.Add(this.UnresolvedDataGrid);
            this.Controls.Add(this.ResolvedItemsLabel);
            this.Controls.Add(this.ResolvedCabinetsCount);
            this.Controls.Add(this.ResolvedDataGrid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FORM_CabinetAddressResolution";
            this.Text = "FORM_CabinetAddressResolution";
            this.Load += new System.EventHandler(this.FORM_CabinetAddressResolution_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ResolvedDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnresolvedDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView ResolvedDataGrid;
        private System.Windows.Forms.Label ResolvedCabinetsCount;
        private System.Windows.Forms.Label ResolvedItemsLabel;
        private System.Windows.Forms.DataGridView UnresolvedDataGrid;
        private System.Windows.Forms.Label UnresolvedItemsLabel;
        private System.Windows.Forms.Label UnresolvedCabinetsCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Resolved_Channel_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dimmer_Number_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn Resolved_Cabinet_Number_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn Resolved_Rack_Number_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unresolved_Channel_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unresolved_Dimmer_Number_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unresolved_Cabinet_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unresolved_Rack_Col;
    }
}
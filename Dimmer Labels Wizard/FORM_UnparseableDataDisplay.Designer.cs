namespace Dimmer_Labels_Wizard
{
    partial class FORM_UnparseableDataDisplay
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
            this.UnparseableDataGridView = new System.Windows.Forms.DataGridView();
            this.ChannelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DimmerNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstrumentTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MulticoreNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImportIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OmitColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.OmitAllCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BackButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DimmerFormatLabel = new System.Windows.Forms.Label();
            this.DistroFormatLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UnparseableDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // UnparseableDataGridView
            // 
            this.UnparseableDataGridView.AllowUserToAddRows = false;
            this.UnparseableDataGridView.AllowUserToDeleteRows = false;
            this.UnparseableDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UnparseableDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChannelColumn,
            this.DimmerNumberColumn,
            this.InstrumentTypeColumn,
            this.MulticoreNameColumn,
            this.ImportIndexColumn,
            this.OmitColumn});
            this.UnparseableDataGridView.Location = new System.Drawing.Point(403, 36);
            this.UnparseableDataGridView.Name = "UnparseableDataGridView";
            this.UnparseableDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UnparseableDataGridView.Size = new System.Drawing.Size(594, 365);
            this.UnparseableDataGridView.TabIndex = 0;
            // 
            // ChannelColumn
            // 
            this.ChannelColumn.HeaderText = "Channel";
            this.ChannelColumn.Name = "ChannelColumn";
            // 
            // DimmerNumberColumn
            // 
            this.DimmerNumberColumn.HeaderText = "Dimmer Number";
            this.DimmerNumberColumn.Name = "DimmerNumberColumn";
            // 
            // InstrumentTypeColumn
            // 
            this.InstrumentTypeColumn.HeaderText = "Instrument Type";
            this.InstrumentTypeColumn.Name = "InstrumentTypeColumn";
            // 
            // MulticoreNameColumn
            // 
            this.MulticoreNameColumn.HeaderText = "Multicore Name";
            this.MulticoreNameColumn.Name = "MulticoreNameColumn";
            // 
            // ImportIndexColumn
            // 
            this.ImportIndexColumn.HeaderText = "Import Index";
            this.ImportIndexColumn.Name = "ImportIndexColumn";
            // 
            // OmitColumn
            // 
            this.OmitColumn.HeaderText = "Omit";
            this.OmitColumn.Name = "OmitColumn";
            this.OmitColumn.Width = 50;
            // 
            // ContinueButton
            // 
            this.ContinueButton.Location = new System.Drawing.Point(922, 407);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(75, 23);
            this.ContinueButton.TabIndex = 1;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // OmitAllCheckBox
            // 
            this.OmitAllCheckBox.AutoSize = true;
            this.OmitAllCheckBox.Location = new System.Drawing.Point(936, 13);
            this.OmitAllCheckBox.Name = "OmitAllCheckBox";
            this.OmitAllCheckBox.Size = new System.Drawing.Size(61, 17);
            this.OmitAllCheckBox.TabIndex = 2;
            this.OmitAllCheckBox.Text = "Omit All";
            this.OmitAllCheckBox.UseVisualStyleBackColor = true;
            this.OmitAllCheckBox.CheckedChanged += new System.EventHandler(this.OmitAllCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Non Parsable Data";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(359, 39);
            this.label2.TabIndex = 4;
            this.label2.Text = "Some of the data supplied could not be correctly Parsed. This is caused by\r\ndata " +
    "in the Dimmer Column not matching the Distro or Dimmer Format\r\nstyle you selecte" +
    "d earlier.\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(355, 78);
            this.label3.TabIndex = 5;
            this.label3.Text = "Please edit the value in the \"Dimmer Number\" Column  to something\r\nthat matches t" +
    "he selected Format.\r\n\r\nAlternatively.\r\n\r\nSelect the Omit checkbox to remove that" +
    " channel from the Label creation.\r\n";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(253, 26);
            this.label4.TabIndex = 6;
            this.label4.Text = "You must edit or omit the data before the Application\r\ncan continue.";
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(841, 407);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 7;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 273);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Selected Dimmer Format";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(227, 273);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Selected Distro Format";
            // 
            // DimmerFormatLabel
            // 
            this.DimmerFormatLabel.AutoSize = true;
            this.DimmerFormatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DimmerFormatLabel.Location = new System.Drawing.Point(13, 298);
            this.DimmerFormatLabel.Name = "DimmerFormatLabel";
            this.DimmerFormatLabel.Size = new System.Drawing.Size(122, 13);
            this.DimmerFormatLabel.TabIndex = 10;
            this.DimmerFormatLabel.Text = "Selected Dimmer Format";
            // 
            // DistroFormatLabel
            // 
            this.DistroFormatLabel.AutoSize = true;
            this.DistroFormatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DistroFormatLabel.Location = new System.Drawing.Point(227, 298);
            this.DistroFormatLabel.Name = "DistroFormatLabel";
            this.DistroFormatLabel.Size = new System.Drawing.Size(122, 13);
            this.DistroFormatLabel.TabIndex = 11;
            this.DistroFormatLabel.Text = "Selected Dimmer Format";
            // 
            // FORM_UnparseableDataDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 443);
            this.Controls.Add(this.DistroFormatLabel);
            this.Controls.Add(this.DimmerFormatLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OmitAllCheckBox);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.UnparseableDataGridView);
            this.Name = "FORM_UnparseableDataDisplay";
            this.Text = "FORM_UnparseableDataDisplay";
            this.Load += new System.EventHandler(this.FORM_UnparseableDataDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UnparseableDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView UnparseableDataGridView;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.CheckBox OmitAllCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DimmerNumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstrumentTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MulticoreNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImportIndexColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn OmitColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label DimmerFormatLabel;
        private System.Windows.Forms.Label DistroFormatLabel;
    }
}
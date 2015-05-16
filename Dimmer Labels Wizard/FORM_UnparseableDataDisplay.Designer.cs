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
            this.UnparseableDataGridView.Location = new System.Drawing.Point(12, 12);
            this.UnparseableDataGridView.Name = "UnparseableDataGridView";
            this.UnparseableDataGridView.Size = new System.Drawing.Size(584, 418);
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
            this.OmitColumn.Width = 35;
            // 
            // FORM_UnparseableDataDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 443);
            this.Controls.Add(this.UnparseableDataGridView);
            this.Name = "FORM_UnparseableDataDisplay";
            this.Text = "FORM_UnparseableDataDisplay";
            this.Load += new System.EventHandler(this.FORM_UnparseableDataDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UnparseableDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView UnparseableDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DimmerNumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstrumentTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MulticoreNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImportIndexColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn OmitColumn;
    }
}
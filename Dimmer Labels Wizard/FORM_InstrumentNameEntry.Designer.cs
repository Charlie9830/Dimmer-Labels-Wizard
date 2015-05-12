namespace Dimmer_Labels_Wizard
{
    partial class FORM_InstrumentNameEntry
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
            this.InstrumentNamesTable = new System.Windows.Forms.DataGridView();
            this.ImportedNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShortenedInstrumentNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.SkipButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.InstrumentNamesTable)).BeginInit();
            this.SuspendLayout();
            // 
            // InstrumentNamesTable
            // 
            this.InstrumentNamesTable.AllowUserToAddRows = false;
            this.InstrumentNamesTable.AllowUserToDeleteRows = false;
            this.InstrumentNamesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InstrumentNamesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ImportedNameColumn,
            this.ShortenedInstrumentNameColumn});
            this.InstrumentNamesTable.Location = new System.Drawing.Point(13, 13);
            this.InstrumentNamesTable.Name = "InstrumentNamesTable";
            this.InstrumentNamesTable.Size = new System.Drawing.Size(519, 495);
            this.InstrumentNamesTable.TabIndex = 0;
            this.InstrumentNamesTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.InstrumentNames_CellContentClick);
            // 
            // ImportedNameColumn
            // 
            this.ImportedNameColumn.HeaderText = "Imported Instrument Name";
            this.ImportedNameColumn.Name = "ImportedNameColumn";
            this.ImportedNameColumn.ReadOnly = true;
            this.ImportedNameColumn.Width = 300;
            // 
            // ShortenedInstrumentNameColumn
            // 
            this.ShortenedInstrumentNameColumn.HeaderText = "Label Instrument Name";
            this.ShortenedInstrumentNameColumn.Name = "ShortenedInstrumentNameColumn";
            this.ShortenedInstrumentNameColumn.Width = 150;
            // 
            // ContinueButton
            // 
            this.ContinueButton.BackColor = System.Drawing.Color.Cornsilk;
            this.ContinueButton.Location = new System.Drawing.Point(918, 485);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(75, 23);
            this.ContinueButton.TabIndex = 1;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = false;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // SkipButton
            // 
            this.SkipButton.Location = new System.Drawing.Point(817, 485);
            this.SkipButton.Name = "SkipButton";
            this.SkipButton.Size = new System.Drawing.Size(95, 23);
            this.SkipButton.TabIndex = 2;
            this.SkipButton.Text = "Skip this Step";
            this.SkipButton.UseVisualStyleBackColor = true;
            // 
            // FORM_InstrumentNameEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 521);
            this.Controls.Add(this.SkipButton);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.InstrumentNamesTable);
            this.Name = "FORM_InstrumentNameEntry";
            this.Text = "FORM_InstrumentNameEntry";
            this.Load += new System.EventHandler(this.FORM_InstrumentNameEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.InstrumentNamesTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView InstrumentNamesTable;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.Button SkipButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImportedNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShortenedInstrumentNameColumn;
    }
}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FORM_InstrumentNameEntry));
            this.InstrumentNamesTable = new System.Windows.Forms.DataGridView();
            this.ImportedNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShortenedInstrumentNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CharacterCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.ShortenedInstrumentNameColumn,
            this.CharacterCountColumn});
            this.InstrumentNamesTable.Location = new System.Drawing.Point(433, 12);
            this.InstrumentNamesTable.Name = "InstrumentNamesTable";
            this.InstrumentNamesTable.Size = new System.Drawing.Size(579, 341);
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
            // CharacterCountColumn
            // 
            this.CharacterCountColumn.HeaderText = "Character Count";
            this.CharacterCountColumn.Name = "CharacterCountColumn";
            this.CharacterCountColumn.ReadOnly = true;
            this.CharacterCountColumn.Width = 75;
            // 
            // ContinueButton
            // 
            this.ContinueButton.BackColor = System.Drawing.Color.Cornsilk;
            this.ContinueButton.Location = new System.Drawing.Point(937, 359);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(75, 23);
            this.ContinueButton.TabIndex = 1;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = false;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Instrument Name Management";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(375, 91);
            this.label2.TabIndex = 4;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // FORM_InstrumentNameEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 392);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.InstrumentNamesTable);
            this.Name = "FORM_InstrumentNameEntry";
            this.Text = "FORM_InstrumentNameEntry";
            this.Load += new System.EventHandler(this.FORM_InstrumentNameEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.InstrumentNamesTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView InstrumentNamesTable;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImportedNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShortenedInstrumentNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CharacterCountColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
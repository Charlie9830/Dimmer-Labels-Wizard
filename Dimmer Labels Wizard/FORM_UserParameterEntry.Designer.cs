namespace Dimmer_Labels_Wizard
{
    partial class FORM_UserParameterEntry
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
            this.FirstDimmerNumberSelector = new System.Windows.Forms.NumericUpDown();
            this.LastDimmerNumberSelector = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FirstDistroNumberSelector = new System.Windows.Forms.NumericUpDown();
            this.LastDistroNumberSelector = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.DimmersUniverseSelector = new System.Windows.Forms.NumericUpDown();
            this.FiveKPanel = new System.Windows.Forms.Panel();
            this.FiveKDimmerAddressDataGrid = new System.Windows.Forms.DataGridView();
            this.DMXUniverseColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DMXAddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FiveKDimmersCheckBox = new System.Windows.Forms.CheckBox();
            this.ApplyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FirstDimmerNumberSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastDimmerNumberSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirstDistroNumberSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastDistroNumberSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DimmersUniverseSelector)).BeginInit();
            this.FiveKPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FiveKDimmerAddressDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Dimmer Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Last Dimmer Number";
            // 
            // FirstDimmerNumberSelector
            // 
            this.FirstDimmerNumberSelector.Location = new System.Drawing.Point(16, 30);
            this.FirstDimmerNumberSelector.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.FirstDimmerNumberSelector.Name = "FirstDimmerNumberSelector";
            this.FirstDimmerNumberSelector.Size = new System.Drawing.Size(93, 20);
            this.FirstDimmerNumberSelector.TabIndex = 2;
            // 
            // LastDimmerNumberSelector
            // 
            this.LastDimmerNumberSelector.Location = new System.Drawing.Point(160, 30);
            this.LastDimmerNumberSelector.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.LastDimmerNumberSelector.Name = "LastDimmerNumberSelector";
            this.LastDimmerNumberSelector.Size = new System.Drawing.Size(93, 20);
            this.LastDimmerNumberSelector.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "First Distro Number";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(157, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Last Distro Number";
            // 
            // FirstDistroNumberSelector
            // 
            this.FirstDistroNumberSelector.Location = new System.Drawing.Point(16, 89);
            this.FirstDistroNumberSelector.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.FirstDistroNumberSelector.Name = "FirstDistroNumberSelector";
            this.FirstDistroNumberSelector.Size = new System.Drawing.Size(93, 20);
            this.FirstDistroNumberSelector.TabIndex = 6;
            // 
            // LastDistroNumberSelector
            // 
            this.LastDistroNumberSelector.Location = new System.Drawing.Point(160, 88);
            this.LastDistroNumberSelector.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.LastDistroNumberSelector.Name = "LastDistroNumberSelector";
            this.LastDistroNumberSelector.Size = new System.Drawing.Size(93, 20);
            this.LastDistroNumberSelector.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(300, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Dimmers Universe";
            // 
            // DimmersUniverseSelector
            // 
            this.DimmersUniverseSelector.Location = new System.Drawing.Point(303, 30);
            this.DimmersUniverseSelector.Name = "DimmersUniverseSelector";
            this.DimmersUniverseSelector.Size = new System.Drawing.Size(89, 20);
            this.DimmersUniverseSelector.TabIndex = 9;
            // 
            // FiveKPanel
            // 
            this.FiveKPanel.Controls.Add(this.FiveKDimmerAddressDataGrid);
            this.FiveKPanel.Location = new System.Drawing.Point(16, 153);
            this.FiveKPanel.Name = "FiveKPanel";
            this.FiveKPanel.Size = new System.Drawing.Size(246, 283);
            this.FiveKPanel.TabIndex = 10;
            // 
            // FiveKDimmerAddressDataGrid
            // 
            this.FiveKDimmerAddressDataGrid.AllowUserToResizeColumns = false;
            this.FiveKDimmerAddressDataGrid.AllowUserToResizeRows = false;
            this.FiveKDimmerAddressDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.FiveKDimmerAddressDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DMXUniverseColumn,
            this.DMXAddressColumn});
            this.FiveKDimmerAddressDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FiveKDimmerAddressDataGrid.Location = new System.Drawing.Point(0, 0);
            this.FiveKDimmerAddressDataGrid.Name = "FiveKDimmerAddressDataGrid";
            this.FiveKDimmerAddressDataGrid.Size = new System.Drawing.Size(246, 283);
            this.FiveKDimmerAddressDataGrid.TabIndex = 0;
            // 
            // DMXUniverseColumn
            // 
            this.DMXUniverseColumn.HeaderText = "DMX Universe";
            this.DMXUniverseColumn.Name = "DMXUniverseColumn";
            // 
            // DMXAddressColumn
            // 
            this.DMXAddressColumn.HeaderText = "DMX Address";
            this.DMXAddressColumn.Name = "DMXAddressColumn";
            // 
            // FiveKDimmersCheckBox
            // 
            this.FiveKDimmersCheckBox.AutoSize = true;
            this.FiveKDimmersCheckBox.Location = new System.Drawing.Point(16, 130);
            this.FiveKDimmersCheckBox.Name = "FiveKDimmersCheckBox";
            this.FiveKDimmersCheckBox.Size = new System.Drawing.Size(117, 17);
            this.FiveKDimmersCheckBox.TabIndex = 11;
            this.FiveKDimmersCheckBox.Text = "5kw Dimmers Used";
            this.FiveKDimmersCheckBox.UseVisualStyleBackColor = true;
            this.FiveKDimmersCheckBox.CheckedChanged += new System.EventHandler(this.FiveKDimmersCheckBox_CheckedChanged);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(911, 409);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 12;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // FORM_UserParameterEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 448);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.FiveKDimmersCheckBox);
            this.Controls.Add(this.FiveKPanel);
            this.Controls.Add(this.DimmersUniverseSelector);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LastDistroNumberSelector);
            this.Controls.Add(this.FirstDistroNumberSelector);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LastDimmerNumberSelector);
            this.Controls.Add(this.FirstDimmerNumberSelector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FORM_UserParameterEntry";
            this.Text = "FORM_UserParameterEntry";
            this.Load += new System.EventHandler(this.FORM_UserParameterEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FirstDimmerNumberSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastDimmerNumberSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirstDistroNumberSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastDistroNumberSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DimmersUniverseSelector)).EndInit();
            this.FiveKPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FiveKDimmerAddressDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown FirstDimmerNumberSelector;
        private System.Windows.Forms.NumericUpDown LastDimmerNumberSelector;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown FirstDistroNumberSelector;
        private System.Windows.Forms.NumericUpDown LastDistroNumberSelector;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown DimmersUniverseSelector;
        private System.Windows.Forms.Panel FiveKPanel;
        private System.Windows.Forms.DataGridView FiveKDimmerAddressDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn DMXUniverseColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DMXAddressColumn;
        private System.Windows.Forms.CheckBox FiveKDimmersCheckBox;
        private System.Windows.Forms.Button ApplyButton;
    }
}
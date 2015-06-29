namespace Dimmer_Labels_Wizard
{
    partial class PrintRangeSelection
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ExampleText = new System.Windows.Forms.Label();
            this.SelectionTextBox = new System.Windows.Forms.TextBox();
            this.SelectionRadioButton = new System.Windows.Forms.RadioButton();
            this.RackRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.AllRadioButton = new System.Windows.Forms.RadioButton();
            this.LowerRangeSelector = new System.Windows.Forms.NumericUpDown();
            this.UpperRangeSelector = new System.Windows.Forms.NumericUpDown();
            this.NoneRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LowerRangeSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpperRangeSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Print Range";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.NoneRadioButton);
            this.panel1.Controls.Add(this.ExampleText);
            this.panel1.Controls.Add(this.SelectionTextBox);
            this.panel1.Controls.Add(this.SelectionRadioButton);
            this.panel1.Controls.Add(this.RackRadioButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.AllRadioButton);
            this.panel1.Controls.Add(this.LowerRangeSelector);
            this.panel1.Controls.Add(this.UpperRangeSelector);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 142);
            this.panel1.TabIndex = 3;
            // 
            // ExampleText
            // 
            this.ExampleText.AutoSize = true;
            this.ExampleText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ExampleText.Location = new System.Drawing.Point(0, 104);
            this.ExampleText.Name = "ExampleText";
            this.ExampleText.Size = new System.Drawing.Size(204, 26);
            this.ExampleText.TabIndex = 10;
            this.ExampleText.Text = "Seperate single Racks or Rack ranges by\r\n a comma, For example, 1-6, 9, 11.\r\n";
            // 
            // SelectionTextBox
            // 
            this.SelectionTextBox.Location = new System.Drawing.Point(76, 81);
            this.SelectionTextBox.Name = "SelectionTextBox";
            this.SelectionTextBox.Size = new System.Drawing.Size(124, 20);
            this.SelectionTextBox.TabIndex = 9;
            // 
            // SelectionRadioButton
            // 
            this.SelectionRadioButton.AutoSize = true;
            this.SelectionRadioButton.Location = new System.Drawing.Point(3, 82);
            this.SelectionRadioButton.Name = "SelectionRadioButton";
            this.SelectionRadioButton.Size = new System.Drawing.Size(69, 17);
            this.SelectionRadioButton.TabIndex = 5;
            this.SelectionRadioButton.TabStop = true;
            this.SelectionRadioButton.Text = "Selection";
            this.SelectionRadioButton.UseVisualStyleBackColor = true;
            // 
            // RackRadioButton
            // 
            this.RackRadioButton.AutoSize = true;
            this.RackRadioButton.Location = new System.Drawing.Point(3, 56);
            this.RackRadioButton.Name = "RackRadioButton";
            this.RackRadioButton.Size = new System.Drawing.Size(51, 17);
            this.RackRadioButton.TabIndex = 4;
            this.RackRadioButton.TabStop = true;
            this.RackRadioButton.Text = "Rack";
            this.RackRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(130, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "to";
            // 
            // AllRadioButton
            // 
            this.AllRadioButton.AutoSize = true;
            this.AllRadioButton.Location = new System.Drawing.Point(3, 30);
            this.AllRadioButton.Name = "AllRadioButton";
            this.AllRadioButton.Size = new System.Drawing.Size(36, 17);
            this.AllRadioButton.TabIndex = 3;
            this.AllRadioButton.TabStop = true;
            this.AllRadioButton.Text = "All";
            this.AllRadioButton.UseVisualStyleBackColor = true;
            // 
            // LowerRangeSelector
            // 
            this.LowerRangeSelector.Location = new System.Drawing.Point(76, 56);
            this.LowerRangeSelector.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.LowerRangeSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LowerRangeSelector.Name = "LowerRangeSelector";
            this.LowerRangeSelector.Size = new System.Drawing.Size(48, 20);
            this.LowerRangeSelector.TabIndex = 6;
            this.LowerRangeSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LowerRangeSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // UpperRangeSelector
            // 
            this.UpperRangeSelector.Location = new System.Drawing.Point(152, 56);
            this.UpperRangeSelector.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.UpperRangeSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpperRangeSelector.Name = "UpperRangeSelector";
            this.UpperRangeSelector.Size = new System.Drawing.Size(48, 20);
            this.UpperRangeSelector.TabIndex = 7;
            this.UpperRangeSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UpperRangeSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // NoneRadioButton
            // 
            this.NoneRadioButton.AutoSize = true;
            this.NoneRadioButton.Location = new System.Drawing.Point(3, 7);
            this.NoneRadioButton.Name = "NoneRadioButton";
            this.NoneRadioButton.Size = new System.Drawing.Size(51, 17);
            this.NoneRadioButton.TabIndex = 11;
            this.NoneRadioButton.TabStop = true;
            this.NoneRadioButton.Text = "None";
            this.NoneRadioButton.UseVisualStyleBackColor = true;
            // 
            // PrintRangeSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Name = "PrintRangeSelection";
            this.Size = new System.Drawing.Size(226, 167);
            this.Load += new System.EventHandler(this.PrintRangeSelection_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LowerRangeSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpperRangeSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox SelectionTextBox;
        private System.Windows.Forms.RadioButton SelectionRadioButton;
        private System.Windows.Forms.RadioButton RackRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton AllRadioButton;
        private System.Windows.Forms.NumericUpDown LowerRangeSelector;
        private System.Windows.Forms.NumericUpDown UpperRangeSelector;
        private System.Windows.Forms.Label ExampleText;
        private System.Windows.Forms.RadioButton NoneRadioButton;
    }
}

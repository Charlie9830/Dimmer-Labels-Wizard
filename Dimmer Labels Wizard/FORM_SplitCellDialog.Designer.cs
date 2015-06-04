namespace Dimmer_Labels_Wizard
{
    partial class FORM_SplitCellDialog
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
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LeftCellTextBox = new System.Windows.Forms.TextBox();
            this.RightCellTextBox = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.CascadeChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 20);
            this.label5.TabIndex = 33;
            this.label5.Text = "Split Cell";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(358, 26);
            this.label1.TabIndex = 34;
            this.label1.Text = "Header Label Cells are automatically merged if they contain the same Text.\r\nTo Sp" +
    "lit these cells, Give each Cell it\'s own Unique text.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Left Hand Cell Text";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Right Hand Cell Text";
            // 
            // LeftCellTextBox
            // 
            this.LeftCellTextBox.Location = new System.Drawing.Point(16, 115);
            this.LeftCellTextBox.Name = "LeftCellTextBox";
            this.LeftCellTextBox.Size = new System.Drawing.Size(140, 20);
            this.LeftCellTextBox.TabIndex = 37;
            // 
            // RightCellTextBox
            // 
            this.RightCellTextBox.Location = new System.Drawing.Point(217, 114);
            this.RightCellTextBox.Name = "RightCellTextBox";
            this.RightCellTextBox.Size = new System.Drawing.Size(140, 20);
            this.RightCellTextBox.TabIndex = 38;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(302, 162);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 39;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(217, 162);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 40;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // CascadeChangesCheckBox
            // 
            this.CascadeChangesCheckBox.AutoSize = true;
            this.CascadeChangesCheckBox.Location = new System.Drawing.Point(16, 154);
            this.CascadeChangesCheckBox.Name = "CascadeChangesCheckBox";
            this.CascadeChangesCheckBox.Size = new System.Drawing.Size(134, 30);
            this.CascadeChangesCheckBox.TabIndex = 41;
            this.CascadeChangesCheckBox.Text = "Apply this change upto\r\nthe next divider.";
            this.CascadeChangesCheckBox.UseVisualStyleBackColor = true;
            // 
            // FORM_SplitCellDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 196);
            this.Controls.Add(this.CascadeChangesCheckBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.RightCellTextBox);
            this.Controls.Add(this.LeftCellTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Name = "FORM_SplitCellDialog";
            this.Text = "FORM_SplitCellDialog";
            this.Load += new System.EventHandler(this.FORM_SplitCellDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox LeftCellTextBox;
        private System.Windows.Forms.TextBox RightCellTextBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.CheckBox CascadeChangesCheckBox;
    }
}
namespace Dimmer_Labels_Wizard
{
    partial class FORM_MainWindow
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.WelcomeScreenPanel = new System.Windows.Forms.Panel();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.NextButton = new System.Windows.Forms.Button();
            this.DebugModeCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.WelcomeScreenPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label2.Location = new System.Drawing.Point(162, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Welcome";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(146, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "To Begin, select a CSV File";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(177, 87);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 3;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // WelcomeScreenPanel
            // 
            this.WelcomeScreenPanel.Controls.Add(this.FileNameLabel);
            this.WelcomeScreenPanel.Controls.Add(this.label2);
            this.WelcomeScreenPanel.Controls.Add(this.BrowseButton);
            this.WelcomeScreenPanel.Controls.Add(this.label1);
            this.WelcomeScreenPanel.Location = new System.Drawing.Point(79, 49);
            this.WelcomeScreenPanel.Name = "WelcomeScreenPanel";
            this.WelcomeScreenPanel.Size = new System.Drawing.Size(432, 189);
            this.WelcomeScreenPanel.TabIndex = 4;
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(148, 132);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(51, 13);
            this.FileNameLabel.TabIndex = 4;
            this.FileNameLabel.Text = "FileName";
            this.FileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NextButton
            // 
            this.NextButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.NextButton.Location = new System.Drawing.Point(531, 250);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 5;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // DebugModeCheckBox
            // 
            this.DebugModeCheckBox.AutoSize = true;
            this.DebugModeCheckBox.Location = new System.Drawing.Point(518, 220);
            this.DebugModeCheckBox.Name = "DebugModeCheckBox";
            this.DebugModeCheckBox.Size = new System.Drawing.Size(88, 17);
            this.DebugModeCheckBox.TabIndex = 6;
            this.DebugModeCheckBox.Text = "Debug Mode";
            this.DebugModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(531, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Debug";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(531, 136);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 8;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // FORM_MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 285);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DebugModeCheckBox);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.WelcomeScreenPanel);
            this.Name = "FORM_MainWindow";
            this.Text = "Dimmer Labels Wizard";
            this.Load += new System.EventHandler(this.FORM_MainWindow_Load);
            this.WelcomeScreenPanel.ResumeLayout(false);
            this.WelcomeScreenPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel WelcomeScreenPanel;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Label FileNameLabel;
        private System.Windows.Forms.CheckBox DebugModeCheckBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button LoadButton;
    }
}
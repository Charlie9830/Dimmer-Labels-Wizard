namespace Dimmer_Labels_Wizard
{
    partial class FORM_LabelSetup
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
            this.ContinueButton = new System.Windows.Forms.Button();
            this.Part1 = new Dimmer_Labels_Wizard.LabelSetupPart1();
            this.Part2 = new Dimmer_Labels_Wizard.LabelSetupPart2();
            this.BackButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ContinueButton
            // 
            this.ContinueButton.Location = new System.Drawing.Point(782, 452);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(75, 23);
            this.ContinueButton.TabIndex = 2;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // Part1
            // 
            this.Part1.Location = new System.Drawing.Point(12, 12);
            this.Part1.Name = "Part1";
            this.Part1.Size = new System.Drawing.Size(849, 463);
            this.Part1.TabIndex = 0;
            // 
            // Part2
            // 
            this.Part2.Location = new System.Drawing.Point(12, 12);
            this.Part2.Name = "Part2";
            this.Part2.Size = new System.Drawing.Size(849, 463);
            this.Part2.TabIndex = 1;
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(701, 452);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 3;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // FORM_LabelSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 487);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.Part1);
            this.Controls.Add(this.Part2);
            this.Name = "FORM_LabelSetup";
            this.Text = "FORM_LabelSetup";
            this.Load += new System.EventHandler(this.FORM_LabelSetup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private LabelSetupPart1 Part1;
        private System.Windows.Forms.Button ContinueButton;
        private LabelSetupPart2 Part2;
        private System.Windows.Forms.Button BackButton;
    }
}
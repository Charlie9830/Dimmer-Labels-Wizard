namespace Dimmer_Labels_Wizard
{
    partial class DimmerRangeInputControl
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
            this.MinusButton = new System.Windows.Forms.Button();
            this.PlusButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MinusButton
            // 
            this.MinusButton.BackgroundImage = global::Dimmer_Labels_Wizard.Properties.Resources.Minus_Sign_12;
            this.MinusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MinusButton.Location = new System.Drawing.Point(27, 3);
            this.MinusButton.Name = "MinusButton";
            this.MinusButton.Size = new System.Drawing.Size(18, 18);
            this.MinusButton.TabIndex = 1;
            this.MinusButton.UseVisualStyleBackColor = true;
            this.MinusButton.Click += new System.EventHandler(this.MinusButton_Click);
            // 
            // PlusButton
            // 
            this.PlusButton.BackgroundImage = global::Dimmer_Labels_Wizard.Properties.Resources.Plus_Sign_12;
            this.PlusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PlusButton.Location = new System.Drawing.Point(3, 3);
            this.PlusButton.Name = "PlusButton";
            this.PlusButton.Size = new System.Drawing.Size(18, 18);
            this.PlusButton.TabIndex = 0;
            this.PlusButton.UseVisualStyleBackColor = true;
            this.PlusButton.Click += new System.EventHandler(this.PlusButton_Click);
            // 
            // DimmerRangeInputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.MinusButton);
            this.Controls.Add(this.PlusButton);
            this.Name = "DimmerRangeInputControl";
            this.Size = new System.Drawing.Size(289, 162);
            this.Load += new System.EventHandler(this.DimmerRangeInputControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button PlusButton;
        private System.Windows.Forms.Button MinusButton;

    }
}

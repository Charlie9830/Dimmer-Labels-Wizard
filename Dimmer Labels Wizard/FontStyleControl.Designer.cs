namespace Dimmer_Labels_Wizard
{
    partial class FontStyleControl
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
            this.BoldCheckBox = new System.Windows.Forms.CheckBox();
            this.ItalicsCheckBox = new System.Windows.Forms.CheckBox();
            this.UnderLineCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BoldCheckBox
            // 
            this.BoldCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.BoldCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BoldCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.BoldCheckBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.BoldCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BoldCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.text_bold_16_000000;
            this.BoldCheckBox.Location = new System.Drawing.Point(3, 1);
            this.BoldCheckBox.Name = "BoldCheckBox";
            this.BoldCheckBox.Size = new System.Drawing.Size(18, 18);
            this.BoldCheckBox.TabIndex = 0;
            this.BoldCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BoldCheckBox.UseVisualStyleBackColor = true;
            // 
            // ItalicsCheckBox
            // 
            this.ItalicsCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ItalicsCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ItalicsCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.ItalicsCheckBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.ItalicsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ItalicsCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.text_italic_16_000000;
            this.ItalicsCheckBox.Location = new System.Drawing.Point(27, 1);
            this.ItalicsCheckBox.Name = "ItalicsCheckBox";
            this.ItalicsCheckBox.Size = new System.Drawing.Size(18, 18);
            this.ItalicsCheckBox.TabIndex = 1;
            this.ItalicsCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ItalicsCheckBox.UseVisualStyleBackColor = true;
            // 
            // UnderLineCheckBox
            // 
            this.UnderLineCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.UnderLineCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UnderLineCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.UnderLineCheckBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.UnderLineCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnderLineCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.text_underline_16_000000;
            this.UnderLineCheckBox.Location = new System.Drawing.Point(51, 1);
            this.UnderLineCheckBox.Name = "UnderLineCheckBox";
            this.UnderLineCheckBox.Size = new System.Drawing.Size(18, 18);
            this.UnderLineCheckBox.TabIndex = 2;
            this.UnderLineCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.UnderLineCheckBox.UseVisualStyleBackColor = true;
            // 
            // FontStyleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UnderLineCheckBox);
            this.Controls.Add(this.ItalicsCheckBox);
            this.Controls.Add(this.BoldCheckBox);
            this.Name = "FontStyleControl";
            this.Size = new System.Drawing.Size(73, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox BoldCheckBox;
        private System.Windows.Forms.CheckBox ItalicsCheckBox;
        private System.Windows.Forms.CheckBox UnderLineCheckBox;



    }
}

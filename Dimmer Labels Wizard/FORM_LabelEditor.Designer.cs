namespace Dimmer_Labels_Wizard
{
    partial class FORM_LabelEditor
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
            this.CabinetSelector = new System.Windows.Forms.ComboBox();
            this.CabinetSelectorTitle = new System.Windows.Forms.Label();
            this.RackSelector = new System.Windows.Forms.ComboBox();
            this.RackSelectorTitle = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CabinetSelector
            // 
            this.CabinetSelector.FormattingEnabled = true;
            this.CabinetSelector.Location = new System.Drawing.Point(13, 197);
            this.CabinetSelector.Name = "CabinetSelector";
            this.CabinetSelector.Size = new System.Drawing.Size(121, 21);
            this.CabinetSelector.TabIndex = 1;
            this.CabinetSelector.SelectedIndexChanged += new System.EventHandler(this.LabelSelector_SelectedIndexChanged);
            // 
            // CabinetSelectorTitle
            // 
            this.CabinetSelectorTitle.AutoSize = true;
            this.CabinetSelectorTitle.Location = new System.Drawing.Point(32, 178);
            this.CabinetSelectorTitle.Name = "CabinetSelectorTitle";
            this.CabinetSelectorTitle.Size = new System.Drawing.Size(83, 13);
            this.CabinetSelectorTitle.TabIndex = 2;
            this.CabinetSelectorTitle.Text = "Cabinet Number";
            // 
            // RackSelector
            // 
            this.RackSelector.FormattingEnabled = true;
            this.RackSelector.Location = new System.Drawing.Point(155, 197);
            this.RackSelector.Name = "RackSelector";
            this.RackSelector.Size = new System.Drawing.Size(121, 21);
            this.RackSelector.TabIndex = 3;
            // 
            // RackSelectorTitle
            // 
            this.RackSelectorTitle.AutoSize = true;
            this.RackSelectorTitle.Location = new System.Drawing.Point(175, 178);
            this.RackSelectorTitle.Name = "RackSelectorTitle";
            this.RackSelectorTitle.Size = new System.Drawing.Size(73, 13);
            this.RackSelectorTitle.TabIndex = 4;
            this.RackSelectorTitle.Text = "Rack Number";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(830, 400);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FORM_LabelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 462);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.RackSelectorTitle);
            this.Controls.Add(this.RackSelector);
            this.Controls.Add(this.CabinetSelectorTitle);
            this.Controls.Add(this.CabinetSelector);
            this.Name = "FORM_LabelEditor";
            this.Text = "FORM_LabelEditor";
            this.Load += new System.EventHandler(this.FORM_LabelEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost CanvasHost;
        private System.Windows.Forms.ComboBox CabinetSelector;
        private System.Windows.Forms.Label CabinetSelectorTitle;
        private System.Windows.Forms.ComboBox RackSelector;
        private System.Windows.Forms.Label RackSelectorTitle;
        private System.Windows.Forms.Button button1;
    }
}
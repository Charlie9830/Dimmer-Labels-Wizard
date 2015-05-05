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
            this.button1 = new System.Windows.Forms.Button();
            this.LowerPanel = new System.Windows.Forms.Panel();
            this.RackLabelSelector = new System.Windows.Forms.TreeView();
            this.LowerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(935, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LowerPanel
            // 
            this.LowerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LowerPanel.Controls.Add(this.RackLabelSelector);
            this.LowerPanel.Controls.Add(this.button1);
            this.LowerPanel.Location = new System.Drawing.Point(12, 335);
            this.LowerPanel.Name = "LowerPanel";
            this.LowerPanel.Size = new System.Drawing.Size(1015, 158);
            this.LowerPanel.TabIndex = 6;
            // 
            // RackLabelSelector
            // 
            this.RackLabelSelector.Location = new System.Drawing.Point(4, 4);
            this.RackLabelSelector.Name = "RackLabelSelector";
            this.RackLabelSelector.Size = new System.Drawing.Size(140, 149);
            this.RackLabelSelector.TabIndex = 6;
            this.RackLabelSelector.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RackLabelSelector_AfterSelect);
            // 
            // FORM_LabelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 505);
            this.Controls.Add(this.LowerPanel);
            this.Name = "FORM_LabelEditor";
            this.Text = "FORM_LabelEditor";
            this.Load += new System.EventHandler(this.FORM_LabelEditor_Load);
            this.LowerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost CanvasHost;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel LowerPanel;
        private System.Windows.Forms.TreeView RackLabelSelector;
    }
}
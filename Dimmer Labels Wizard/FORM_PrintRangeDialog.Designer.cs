namespace Dimmer_Labels_Wizard
{
    partial class FORM_PrintRangeDialog
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
            this.label3 = new System.Windows.Forms.Label();
            this.DistroPrintRangeSelection = new Dimmer_Labels_Wizard.PrintRangeSelection();
            this.DimmerPrintRangeSelection = new Dimmer_Labels_Wizard.PrintRangeSelection();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Print Range";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Dimmer Labels";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(250, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Distro Labels";
            // 
            // DistroPrintRangeSelection
            // 
            this.DistroPrintRangeSelection.Location = new System.Drawing.Point(253, 62);
            this.DistroPrintRangeSelection.Name = "DistroPrintRangeSelection";
            this.DistroPrintRangeSelection.Size = new System.Drawing.Size(228, 165);
            this.DistroPrintRangeSelection.TabIndex = 3;
            // 
            // DimmerPrintRangeSelection
            // 
            this.DimmerPrintRangeSelection.Location = new System.Drawing.Point(12, 62);
            this.DimmerPrintRangeSelection.Name = "DimmerPrintRangeSelection";
            this.DimmerPrintRangeSelection.Size = new System.Drawing.Size(235, 170);
            this.DimmerPrintRangeSelection.TabIndex = 2;
            // 
            // ContinueButton
            // 
            this.ContinueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ContinueButton.Location = new System.Drawing.Point(392, 233);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(75, 23);
            this.ContinueButton.TabIndex = 6;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(311, 233);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // FORM_PrintRangeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 261);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DistroPrintRangeSelection);
            this.Controls.Add(this.DimmerPrintRangeSelection);
            this.Controls.Add(this.label1);
            this.Name = "FORM_PrintRangeDialog";
            this.Text = "FORM_PrintRangeDialog";
            this.Load += new System.EventHandler(this.FORM_PrintRangeDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private PrintRangeSelection DimmerPrintRangeSelection;
        private PrintRangeSelection DistroPrintRangeSelection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
namespace Dimmer_Labels_Wizard
{
    partial class DimmerRangeSelector
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
            this.UniverseSelector = new System.Windows.Forms.NumericUpDown();
            this.FirstChannelSelector = new System.Windows.Forms.NumericUpDown();
            this.LastChannelSelector = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UniverseSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirstChannelSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastChannelSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // UniverseSelector
            // 
            this.UniverseSelector.Location = new System.Drawing.Point(4, 18);
            this.UniverseSelector.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.UniverseSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UniverseSelector.Name = "UniverseSelector";
            this.UniverseSelector.Size = new System.Drawing.Size(46, 20);
            this.UniverseSelector.TabIndex = 0;
            this.UniverseSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UniverseSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FirstChannelSelector
            // 
            this.FirstChannelSelector.Location = new System.Drawing.Point(61, 18);
            this.FirstChannelSelector.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.FirstChannelSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FirstChannelSelector.Name = "FirstChannelSelector";
            this.FirstChannelSelector.Size = new System.Drawing.Size(46, 20);
            this.FirstChannelSelector.TabIndex = 1;
            this.FirstChannelSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FirstChannelSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LastChannelSelector
            // 
            this.LastChannelSelector.Location = new System.Drawing.Point(135, 18);
            this.LastChannelSelector.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.LastChannelSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LastChannelSelector.Name = "LastChannelSelector";
            this.LastChannelSelector.Size = new System.Drawing.Size(46, 20);
            this.LastChannelSelector.TabIndex = 2;
            this.LastChannelSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LastChannelSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Universe";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "First Channel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Last Channel";
            // 
            // DimmerRangeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LastChannelSelector);
            this.Controls.Add(this.FirstChannelSelector);
            this.Controls.Add(this.UniverseSelector);
            this.Name = "DimmerRangeSelector";
            this.Size = new System.Drawing.Size(204, 42);
            ((System.ComponentModel.ISupportInitialize)(this.UniverseSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirstChannelSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastChannelSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown UniverseSelector;
        private System.Windows.Forms.NumericUpDown FirstChannelSelector;
        private System.Windows.Forms.NumericUpDown LastChannelSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

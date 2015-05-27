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
            this.BreakButton = new System.Windows.Forms.Button();
            this.LowerPanel = new System.Windows.Forms.Panel();
            this.FooterBottomFontButton = new System.Windows.Forms.Button();
            this.FooterMiddleFontButton = new System.Windows.Forms.Button();
            this.FooterTopFontButton = new System.Windows.Forms.Button();
            this.InstrumentNameTextBox = new System.Windows.Forms.TextBox();
            this.FooterBottomDataTextBox = new System.Windows.Forms.Label();
            this.FooterMiddleDataTextBox = new System.Windows.Forms.TextBox();
            this.FooterMiddleLabel = new System.Windows.Forms.Label();
            this.FooterTopDataTextBox = new System.Windows.Forms.TextBox();
            this.FooterTopLabel = new System.Windows.Forms.Label();
            this.FooterBackgroundColorButton = new System.Windows.Forms.Button();
            this.FooterBackgroundColorLabel = new System.Windows.Forms.Label();
            this.PrintButton = new System.Windows.Forms.Button();
            this.RackLabelSelector = new System.Windows.Forms.TreeView();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.CanvasPanel = new System.Windows.Forms.Panel();
            this.backgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.FooterPanel = new System.Windows.Forms.Panel();
            this.LowerPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BreakButton
            // 
            this.BreakButton.Location = new System.Drawing.Point(935, 3);
            this.BreakButton.Name = "BreakButton";
            this.BreakButton.Size = new System.Drawing.Size(75, 23);
            this.BreakButton.TabIndex = 5;
            this.BreakButton.Text = "Break";
            this.BreakButton.UseVisualStyleBackColor = true;
            this.BreakButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // LowerPanel
            // 
            this.LowerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LowerPanel.Controls.Add(this.FooterPanel);
            this.LowerPanel.Controls.Add(this.PrintButton);
            this.LowerPanel.Controls.Add(this.RackLabelSelector);
            this.LowerPanel.Controls.Add(this.BreakButton);
            this.LowerPanel.Location = new System.Drawing.Point(12, 335);
            this.LowerPanel.Name = "LowerPanel";
            this.LowerPanel.Size = new System.Drawing.Size(1015, 272);
            this.LowerPanel.TabIndex = 6;
            this.LowerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.LowerPanel_Paint);
            // 
            // FooterBottomFontButton
            // 
            this.FooterBottomFontButton.Location = new System.Drawing.Point(148, 98);
            this.FooterBottomFontButton.Name = "FooterBottomFontButton";
            this.FooterBottomFontButton.Size = new System.Drawing.Size(75, 23);
            this.FooterBottomFontButton.TabIndex = 18;
            this.FooterBottomFontButton.Text = "Font";
            this.FooterBottomFontButton.UseVisualStyleBackColor = true;
            this.FooterBottomFontButton.Click += new System.EventHandler(this.InstrumentNameFontButton_Click);
            // 
            // FooterMiddleFontButton
            // 
            this.FooterMiddleFontButton.Location = new System.Drawing.Point(148, 58);
            this.FooterMiddleFontButton.Name = "FooterMiddleFontButton";
            this.FooterMiddleFontButton.Size = new System.Drawing.Size(75, 23);
            this.FooterMiddleFontButton.TabIndex = 17;
            this.FooterMiddleFontButton.Text = "Font";
            this.FooterMiddleFontButton.UseVisualStyleBackColor = true;
            this.FooterMiddleFontButton.Click += new System.EventHandler(this.ChannelFontButton_Click);
            // 
            // FooterTopFontButton
            // 
            this.FooterTopFontButton.Location = new System.Drawing.Point(148, 19);
            this.FooterTopFontButton.Name = "FooterTopFontButton";
            this.FooterTopFontButton.Size = new System.Drawing.Size(75, 23);
            this.FooterTopFontButton.TabIndex = 16;
            this.FooterTopFontButton.Text = "Font";
            this.FooterTopFontButton.UseVisualStyleBackColor = true;
            this.FooterTopFontButton.Click += new System.EventHandler(this.HeaderFontButton_Click);
            // 
            // InstrumentNameTextBox
            // 
            this.InstrumentNameTextBox.Location = new System.Drawing.Point(6, 100);
            this.InstrumentNameTextBox.Name = "InstrumentNameTextBox";
            this.InstrumentNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.InstrumentNameTextBox.TabIndex = 15;
            // 
            // FooterBottomDataTextBox
            // 
            this.FooterBottomDataTextBox.AutoSize = true;
            this.FooterBottomDataTextBox.Location = new System.Drawing.Point(3, 84);
            this.FooterBottomDataTextBox.Name = "FooterBottomDataTextBox";
            this.FooterBottomDataTextBox.Size = new System.Drawing.Size(40, 13);
            this.FooterBottomDataTextBox.TabIndex = 14;
            this.FooterBottomDataTextBox.Text = "Bottom";
            // 
            // FooterMiddleDataTextBox
            // 
            this.FooterMiddleDataTextBox.Location = new System.Drawing.Point(6, 61);
            this.FooterMiddleDataTextBox.Name = "FooterMiddleDataTextBox";
            this.FooterMiddleDataTextBox.Size = new System.Drawing.Size(100, 20);
            this.FooterMiddleDataTextBox.TabIndex = 13;
            // 
            // FooterMiddleLabel
            // 
            this.FooterMiddleLabel.AutoSize = true;
            this.FooterMiddleLabel.Location = new System.Drawing.Point(3, 44);
            this.FooterMiddleLabel.Name = "FooterMiddleLabel";
            this.FooterMiddleLabel.Size = new System.Drawing.Size(38, 13);
            this.FooterMiddleLabel.TabIndex = 12;
            this.FooterMiddleLabel.Text = "Middle";
            // 
            // FooterTopDataTextBox
            // 
            this.FooterTopDataTextBox.Location = new System.Drawing.Point(6, 21);
            this.FooterTopDataTextBox.Name = "FooterTopDataTextBox";
            this.FooterTopDataTextBox.Size = new System.Drawing.Size(100, 20);
            this.FooterTopDataTextBox.TabIndex = 11;
            // 
            // FooterTopLabel
            // 
            this.FooterTopLabel.AutoSize = true;
            this.FooterTopLabel.Location = new System.Drawing.Point(3, 4);
            this.FooterTopLabel.Name = "FooterTopLabel";
            this.FooterTopLabel.Size = new System.Drawing.Size(26, 13);
            this.FooterTopLabel.TabIndex = 10;
            this.FooterTopLabel.Text = "Top";
            // 
            // FooterBackgroundColorButton
            // 
            this.FooterBackgroundColorButton.Location = new System.Drawing.Point(148, 138);
            this.FooterBackgroundColorButton.Name = "FooterBackgroundColorButton";
            this.FooterBackgroundColorButton.Size = new System.Drawing.Size(75, 23);
            this.FooterBackgroundColorButton.TabIndex = 9;
            this.FooterBackgroundColorButton.Text = "Choose";
            this.FooterBackgroundColorButton.UseVisualStyleBackColor = true;
            this.FooterBackgroundColorButton.Click += new System.EventHandler(this.BackgroundColorButton_Click);
            // 
            // FooterBackgroundColorLabel
            // 
            this.FooterBackgroundColorLabel.AutoSize = true;
            this.FooterBackgroundColorLabel.Location = new System.Drawing.Point(3, 138);
            this.FooterBackgroundColorLabel.Name = "FooterBackgroundColorLabel";
            this.FooterBackgroundColorLabel.Size = new System.Drawing.Size(98, 13);
            this.FooterBackgroundColorLabel.TabIndex = 8;
            this.FooterBackgroundColorLabel.Text = "Background Colour";
            // 
            // PrintButton
            // 
            this.PrintButton.Location = new System.Drawing.Point(935, 32);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(75, 23);
            this.PrintButton.TabIndex = 7;
            this.PrintButton.Text = "Print";
            this.PrintButton.UseVisualStyleBackColor = true;
            this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // RackLabelSelector
            // 
            this.RackLabelSelector.Location = new System.Drawing.Point(4, 4);
            this.RackLabelSelector.Name = "RackLabelSelector";
            this.RackLabelSelector.Size = new System.Drawing.Size(140, 263);
            this.RackLabelSelector.TabIndex = 6;
            this.RackLabelSelector.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RackLabelSelector_AfterSelect);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1048, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printSettingsToolStripMenuItem,
            this.pageSettingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // printSettingsToolStripMenuItem
            // 
            this.printSettingsToolStripMenuItem.Name = "printSettingsToolStripMenuItem";
            this.printSettingsToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.printSettingsToolStripMenuItem.Text = "Print Settings";
            this.printSettingsToolStripMenuItem.Click += new System.EventHandler(this.printSettingsToolStripMenuItem_Click);
            // 
            // pageSettingsToolStripMenuItem
            // 
            this.pageSettingsToolStripMenuItem.Name = "pageSettingsToolStripMenuItem";
            this.pageSettingsToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pageSettingsToolStripMenuItem.Text = "Page Setup";
            this.pageSettingsToolStripMenuItem.Click += new System.EventHandler(this.pageSettingsToolStripMenuItem_Click);
            // 
            // printDialog
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // CanvasPanel
            // 
            this.CanvasPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CanvasPanel.Location = new System.Drawing.Point(12, 27);
            this.CanvasPanel.Name = "CanvasPanel";
            this.CanvasPanel.Size = new System.Drawing.Size(1015, 302);
            this.CanvasPanel.TabIndex = 8;
            // 
            // FooterPanel
            // 
            this.FooterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FooterPanel.Controls.Add(this.FooterTopLabel);
            this.FooterPanel.Controls.Add(this.FooterBackgroundColorButton);
            this.FooterPanel.Controls.Add(this.FooterBottomFontButton);
            this.FooterPanel.Controls.Add(this.FooterBackgroundColorLabel);
            this.FooterPanel.Controls.Add(this.FooterTopDataTextBox);
            this.FooterPanel.Controls.Add(this.FooterMiddleFontButton);
            this.FooterPanel.Controls.Add(this.FooterMiddleLabel);
            this.FooterPanel.Controls.Add(this.FooterTopFontButton);
            this.FooterPanel.Controls.Add(this.FooterMiddleDataTextBox);
            this.FooterPanel.Controls.Add(this.InstrumentNameTextBox);
            this.FooterPanel.Controls.Add(this.FooterBottomDataTextBox);
            this.FooterPanel.Location = new System.Drawing.Point(150, 4);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Size = new System.Drawing.Size(229, 220);
            this.FooterPanel.TabIndex = 19;
            // 
            // FORM_LabelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 619);
            this.Controls.Add(this.CanvasPanel);
            this.Controls.Add(this.LowerPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FORM_LabelEditor";
            this.Text = "FORM_LabelEditor";
            this.Load += new System.EventHandler(this.FORM_LabelEditor_Load);
            this.LowerPanel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.FooterPanel.ResumeLayout(false);
            this.FooterPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost CanvasHost;
        private System.Windows.Forms.Button BreakButton;
        private System.Windows.Forms.Panel LowerPanel;
        private System.Windows.Forms.TreeView RackLabelSelector;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.Button PrintButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageSettingsToolStripMenuItem;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.Panel CanvasPanel;
        private System.Windows.Forms.Button FooterBackgroundColorButton;
        private System.Windows.Forms.Label FooterBackgroundColorLabel;
        private System.Windows.Forms.ColorDialog backgroundColorDialog;
        private System.Windows.Forms.TextBox InstrumentNameTextBox;
        private System.Windows.Forms.Label FooterBottomDataTextBox;
        private System.Windows.Forms.TextBox FooterMiddleDataTextBox;
        private System.Windows.Forms.Label FooterMiddleLabel;
        private System.Windows.Forms.TextBox FooterTopDataTextBox;
        private System.Windows.Forms.Label FooterTopLabel;
        private System.Windows.Forms.Button FooterTopFontButton;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Button FooterMiddleFontButton;
        private System.Windows.Forms.Button FooterBottomFontButton;
        private System.Windows.Forms.Panel FooterPanel;
    }
}
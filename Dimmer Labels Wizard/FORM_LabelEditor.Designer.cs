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
            this.InstrumentNameTextBox = new System.Windows.Forms.TextBox();
            this.InstrumentNameLabel = new System.Windows.Forms.Label();
            this.ChannelTextBox = new System.Windows.Forms.TextBox();
            this.ChannelLabel = new System.Windows.Forms.Label();
            this.HeaderTextBox = new System.Windows.Forms.TextBox();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.BackgroundColorButton = new System.Windows.Forms.Button();
            this.BackgroundColorLabel = new System.Windows.Forms.Label();
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
            this.HeaderFontButton = new System.Windows.Forms.Button();
            this.ChannelFontButton = new System.Windows.Forms.Button();
            this.InstrumentNameFontButton = new System.Windows.Forms.Button();
            this.LowerPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            this.LowerPanel.Controls.Add(this.InstrumentNameFontButton);
            this.LowerPanel.Controls.Add(this.ChannelFontButton);
            this.LowerPanel.Controls.Add(this.HeaderFontButton);
            this.LowerPanel.Controls.Add(this.InstrumentNameTextBox);
            this.LowerPanel.Controls.Add(this.InstrumentNameLabel);
            this.LowerPanel.Controls.Add(this.ChannelTextBox);
            this.LowerPanel.Controls.Add(this.ChannelLabel);
            this.LowerPanel.Controls.Add(this.HeaderTextBox);
            this.LowerPanel.Controls.Add(this.HeaderLabel);
            this.LowerPanel.Controls.Add(this.BackgroundColorButton);
            this.LowerPanel.Controls.Add(this.BackgroundColorLabel);
            this.LowerPanel.Controls.Add(this.PrintButton);
            this.LowerPanel.Controls.Add(this.RackLabelSelector);
            this.LowerPanel.Controls.Add(this.BreakButton);
            this.LowerPanel.Location = new System.Drawing.Point(12, 335);
            this.LowerPanel.Name = "LowerPanel";
            this.LowerPanel.Size = new System.Drawing.Size(1015, 272);
            this.LowerPanel.TabIndex = 6;
            // 
            // InstrumentNameTextBox
            // 
            this.InstrumentNameTextBox.Location = new System.Drawing.Point(289, 126);
            this.InstrumentNameTextBox.Name = "InstrumentNameTextBox";
            this.InstrumentNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.InstrumentNameTextBox.TabIndex = 15;
            // 
            // InstrumentNameLabel
            // 
            this.InstrumentNameLabel.AutoSize = true;
            this.InstrumentNameLabel.Location = new System.Drawing.Point(286, 110);
            this.InstrumentNameLabel.Name = "InstrumentNameLabel";
            this.InstrumentNameLabel.Size = new System.Drawing.Size(87, 13);
            this.InstrumentNameLabel.TabIndex = 14;
            this.InstrumentNameLabel.Text = "Instrument Name";
            // 
            // ChannelTextBox
            // 
            this.ChannelTextBox.Location = new System.Drawing.Point(289, 87);
            this.ChannelTextBox.Name = "ChannelTextBox";
            this.ChannelTextBox.Size = new System.Drawing.Size(100, 20);
            this.ChannelTextBox.TabIndex = 13;
            // 
            // ChannelLabel
            // 
            this.ChannelLabel.AutoSize = true;
            this.ChannelLabel.Location = new System.Drawing.Point(286, 70);
            this.ChannelLabel.Name = "ChannelLabel";
            this.ChannelLabel.Size = new System.Drawing.Size(46, 13);
            this.ChannelLabel.TabIndex = 12;
            this.ChannelLabel.Text = "Channel";
            // 
            // HeaderTextBox
            // 
            this.HeaderTextBox.Location = new System.Drawing.Point(289, 21);
            this.HeaderTextBox.Name = "HeaderTextBox";
            this.HeaderTextBox.Size = new System.Drawing.Size(100, 20);
            this.HeaderTextBox.TabIndex = 11;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Location = new System.Drawing.Point(286, 4);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(42, 13);
            this.HeaderLabel.TabIndex = 10;
            this.HeaderLabel.Text = "Header";
            // 
            // BackgroundColorButton
            // 
            this.BackgroundColorButton.Location = new System.Drawing.Point(171, 21);
            this.BackgroundColorButton.Name = "BackgroundColorButton";
            this.BackgroundColorButton.Size = new System.Drawing.Size(75, 23);
            this.BackgroundColorButton.TabIndex = 9;
            this.BackgroundColorButton.Text = "Choose";
            this.BackgroundColorButton.UseVisualStyleBackColor = true;
            this.BackgroundColorButton.Click += new System.EventHandler(this.BackgroundColorButton_Click);
            // 
            // BackgroundColorLabel
            // 
            this.BackgroundColorLabel.AutoSize = true;
            this.BackgroundColorLabel.Location = new System.Drawing.Point(168, 4);
            this.BackgroundColorLabel.Name = "BackgroundColorLabel";
            this.BackgroundColorLabel.Size = new System.Drawing.Size(98, 13);
            this.BackgroundColorLabel.TabIndex = 8;
            this.BackgroundColorLabel.Text = "Background Colour";
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
            // HeaderFontButton
            // 
            this.HeaderFontButton.Location = new System.Drawing.Point(431, 18);
            this.HeaderFontButton.Name = "HeaderFontButton";
            this.HeaderFontButton.Size = new System.Drawing.Size(75, 23);
            this.HeaderFontButton.TabIndex = 16;
            this.HeaderFontButton.Text = "Font";
            this.HeaderFontButton.UseVisualStyleBackColor = true;
            this.HeaderFontButton.Click += new System.EventHandler(this.HeaderFontButton_Click);
            // 
            // ChannelFontButton
            // 
            this.ChannelFontButton.Location = new System.Drawing.Point(431, 84);
            this.ChannelFontButton.Name = "ChannelFontButton";
            this.ChannelFontButton.Size = new System.Drawing.Size(75, 23);
            this.ChannelFontButton.TabIndex = 17;
            this.ChannelFontButton.Text = "Font";
            this.ChannelFontButton.UseVisualStyleBackColor = true;
            this.ChannelFontButton.Click += new System.EventHandler(this.ChannelFontButton_Click);
            // 
            // InstrumentNameFontButton
            // 
            this.InstrumentNameFontButton.Location = new System.Drawing.Point(431, 124);
            this.InstrumentNameFontButton.Name = "InstrumentNameFontButton";
            this.InstrumentNameFontButton.Size = new System.Drawing.Size(75, 23);
            this.InstrumentNameFontButton.TabIndex = 18;
            this.InstrumentNameFontButton.Text = "Font";
            this.InstrumentNameFontButton.UseVisualStyleBackColor = true;
            this.InstrumentNameFontButton.Click += new System.EventHandler(this.InstrumentNameFontButton_Click);
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
            this.LowerPanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Button BackgroundColorButton;
        private System.Windows.Forms.Label BackgroundColorLabel;
        private System.Windows.Forms.ColorDialog backgroundColorDialog;
        private System.Windows.Forms.TextBox InstrumentNameTextBox;
        private System.Windows.Forms.Label InstrumentNameLabel;
        private System.Windows.Forms.TextBox ChannelTextBox;
        private System.Windows.Forms.Label ChannelLabel;
        private System.Windows.Forms.TextBox HeaderTextBox;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Button HeaderFontButton;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Button ChannelFontButton;
        private System.Windows.Forms.Button InstrumentNameFontButton;
    }
}
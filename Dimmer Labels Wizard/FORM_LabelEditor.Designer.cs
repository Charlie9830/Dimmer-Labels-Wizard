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
            this.components = new System.ComponentModel.Container();
            this.DebugButton = new System.Windows.Forms.Button();
            this.LowerPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GeneralControlsPanel = new System.Windows.Forms.Panel();
            this.LineWeightComboBox = new System.Windows.Forms.ComboBox();
            this.LineWeightGlobalApplyCheckBox = new System.Windows.Forms.CheckBox();
            this.BackgroundColorGlobalApplyCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.FooterBackgroundColorButton = new System.Windows.Forms.Button();
            this.FooterBackgroundColorLabel = new System.Windows.Forms.Label();
            this.HeaderControlsPanel = new System.Windows.Forms.Panel();
            this.HeaderGlobalApplyCheckBox = new System.Windows.Forms.CheckBox();
            this.HeaderFontStyleSelector = new Dimmer_Labels_Wizard.FontStyleControl();
            this.HeaderTextBox = new System.Windows.Forms.TextBox();
            this.HeaderFontSizeComboBox = new System.Windows.Forms.ComboBox();
            this.HeaderFontComboBox = new System.Windows.Forms.ComboBox();
            this.HeaderTextLabel = new System.Windows.Forms.Label();
            this.FooterControlsPanel = new System.Windows.Forms.Panel();
            this.FooterBottomGlobalApplyCheckBox = new System.Windows.Forms.CheckBox();
            this.FooterMiddleGlobalApplyCheckBox = new System.Windows.Forms.CheckBox();
            this.FooterTopGlobalApplyCheckBox = new System.Windows.Forms.CheckBox();
            this.FooterBottomFontStyleSelector = new Dimmer_Labels_Wizard.FontStyleControl();
            this.FooterMiddleFontStyleSelector = new Dimmer_Labels_Wizard.FontStyleControl();
            this.FooterTopFontStyleSelector = new Dimmer_Labels_Wizard.FontStyleControl();
            this.FooterBottomSizeComboBox = new System.Windows.Forms.ComboBox();
            this.FooterBottomFontComboBox = new System.Windows.Forms.ComboBox();
            this.FooterMiddleSizeComboBox = new System.Windows.Forms.ComboBox();
            this.FooterMiddleFontComboBox = new System.Windows.Forms.ComboBox();
            this.FooterTopSizeComboBox = new System.Windows.Forms.ComboBox();
            this.FooterTopFontComboBox = new System.Windows.Forms.ComboBox();
            this.FooterBottomTextLabel = new System.Windows.Forms.Label();
            this.FooterTopLabel = new System.Windows.Forms.Label();
            this.FooterTopDataTextBox = new System.Windows.Forms.TextBox();
            this.FooterMiddleLabel = new System.Windows.Forms.Label();
            this.FooterMiddleDataTextBox = new System.Windows.Forms.TextBox();
            this.FooterBottomDataTextBox = new System.Windows.Forms.TextBox();
            this.RackLabelSelector = new System.Windows.Forms.TreeView();
            this.PrintButton = new System.Windows.Forms.Button();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.backgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.ViewControlPanel = new System.Windows.Forms.Panel();
            this.CenterViewButton = new System.Windows.Forms.Button();
            this.MagnifyMinusButton = new System.Windows.Forms.Button();
            this.MagnifyPlusButton = new System.Windows.Forms.Button();
            this.ToolsPanel = new System.Windows.Forms.Panel();
            this.SplitCellButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BackButton = new System.Windows.Forms.Button();
            this.PerformanceTestButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.CanvasHost = new System.Windows.Forms.Integration.ElementHost();
            this.LowerPanel.SuspendLayout();
            this.GeneralControlsPanel.SuspendLayout();
            this.HeaderControlsPanel.SuspendLayout();
            this.FooterControlsPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.ViewControlPanel.SuspendLayout();
            this.ToolsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DebugButton
            // 
            this.DebugButton.Location = new System.Drawing.Point(952, 298);
            this.DebugButton.Name = "DebugButton";
            this.DebugButton.Size = new System.Drawing.Size(75, 23);
            this.DebugButton.TabIndex = 5;
            this.DebugButton.Text = "Debug";
            this.DebugButton.UseVisualStyleBackColor = true;
            this.DebugButton.Click += new System.EventHandler(this.DebugButton_Click);
            // 
            // LowerPanel
            // 
            this.LowerPanel.BackColor = System.Drawing.SystemColors.Control;
            this.LowerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LowerPanel.Controls.Add(this.label4);
            this.LowerPanel.Controls.Add(this.label2);
            this.LowerPanel.Controls.Add(this.label1);
            this.LowerPanel.Controls.Add(this.label3);
            this.LowerPanel.Controls.Add(this.GeneralControlsPanel);
            this.LowerPanel.Controls.Add(this.HeaderControlsPanel);
            this.LowerPanel.Controls.Add(this.FooterControlsPanel);
            this.LowerPanel.Controls.Add(this.RackLabelSelector);
            this.LowerPanel.Location = new System.Drawing.Point(12, 300);
            this.LowerPanel.Name = "LowerPanel";
            this.LowerPanel.Size = new System.Drawing.Size(662, 449);
            this.LowerPanel.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(168, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 20);
            this.label4.TabIndex = 33;
            this.label4.Text = "Appearance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(331, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 32;
            this.label2.Text = "Footer Label";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(331, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 31;
            this.label1.Text = "Header Label";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 20);
            this.label3.TabIndex = 32;
            this.label3.Text = "Rack";
            // 
            // GeneralControlsPanel
            // 
            this.GeneralControlsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GeneralControlsPanel.Controls.Add(this.LineWeightComboBox);
            this.GeneralControlsPanel.Controls.Add(this.LineWeightGlobalApplyCheckBox);
            this.GeneralControlsPanel.Controls.Add(this.BackgroundColorGlobalApplyCheckBox);
            this.GeneralControlsPanel.Controls.Add(this.label6);
            this.GeneralControlsPanel.Controls.Add(this.FooterBackgroundColorButton);
            this.GeneralControlsPanel.Controls.Add(this.FooterBackgroundColorLabel);
            this.GeneralControlsPanel.Location = new System.Drawing.Point(172, 29);
            this.GeneralControlsPanel.Name = "GeneralControlsPanel";
            this.GeneralControlsPanel.Size = new System.Drawing.Size(157, 138);
            this.GeneralControlsPanel.TabIndex = 21;
            // 
            // LineWeightComboBox
            // 
            this.LineWeightComboBox.FormattingEnabled = true;
            this.LineWeightComboBox.Location = new System.Drawing.Point(6, 77);
            this.LineWeightComboBox.Name = "LineWeightComboBox";
            this.LineWeightComboBox.Size = new System.Drawing.Size(95, 21);
            this.LineWeightComboBox.TabIndex = 37;
            this.LineWeightComboBox.SelectedIndexChanged += new System.EventHandler(this.LineWeightComboBox_SelectedIndexChanged);
            // 
            // LineWeightGlobalApplyCheckBox
            // 
            this.LineWeightGlobalApplyCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.LineWeightGlobalApplyCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.LineWeightGlobalApplyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LineWeightGlobalApplyCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.earth_16_000000;
            this.LineWeightGlobalApplyCheckBox.Location = new System.Drawing.Point(127, 60);
            this.LineWeightGlobalApplyCheckBox.Name = "LineWeightGlobalApplyCheckBox";
            this.LineWeightGlobalApplyCheckBox.Size = new System.Drawing.Size(25, 25);
            this.LineWeightGlobalApplyCheckBox.TabIndex = 36;
            this.LineWeightGlobalApplyCheckBox.UseVisualStyleBackColor = true;
            // 
            // BackgroundColorGlobalApplyCheckBox
            // 
            this.BackgroundColorGlobalApplyCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.BackgroundColorGlobalApplyCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.BackgroundColorGlobalApplyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackgroundColorGlobalApplyCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.earth_16_000000;
            this.BackgroundColorGlobalApplyCheckBox.Location = new System.Drawing.Point(127, 3);
            this.BackgroundColorGlobalApplyCheckBox.Name = "BackgroundColorGlobalApplyCheckBox";
            this.BackgroundColorGlobalApplyCheckBox.Size = new System.Drawing.Size(25, 25);
            this.BackgroundColorGlobalApplyCheckBox.TabIndex = 35;
            this.BackgroundColorGlobalApplyCheckBox.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Line Weight";
            // 
            // FooterBackgroundColorButton
            // 
            this.FooterBackgroundColorButton.Location = new System.Drawing.Point(6, 28);
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
            this.FooterBackgroundColorLabel.Location = new System.Drawing.Point(3, 6);
            this.FooterBackgroundColorLabel.Name = "FooterBackgroundColorLabel";
            this.FooterBackgroundColorLabel.Size = new System.Drawing.Size(98, 13);
            this.FooterBackgroundColorLabel.TabIndex = 8;
            this.FooterBackgroundColorLabel.Text = "Background Colour";
            // 
            // HeaderControlsPanel
            // 
            this.HeaderControlsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.HeaderControlsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HeaderControlsPanel.Controls.Add(this.HeaderGlobalApplyCheckBox);
            this.HeaderControlsPanel.Controls.Add(this.HeaderFontStyleSelector);
            this.HeaderControlsPanel.Controls.Add(this.HeaderTextBox);
            this.HeaderControlsPanel.Controls.Add(this.HeaderFontSizeComboBox);
            this.HeaderControlsPanel.Controls.Add(this.HeaderFontComboBox);
            this.HeaderControlsPanel.Controls.Add(this.HeaderTextLabel);
            this.HeaderControlsPanel.Location = new System.Drawing.Point(335, 29);
            this.HeaderControlsPanel.Name = "HeaderControlsPanel";
            this.HeaderControlsPanel.Size = new System.Drawing.Size(292, 88);
            this.HeaderControlsPanel.TabIndex = 20;
            // 
            // HeaderGlobalApplyCheckBox
            // 
            this.HeaderGlobalApplyCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.HeaderGlobalApplyCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.HeaderGlobalApplyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeaderGlobalApplyCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.earth_16_000000;
            this.HeaderGlobalApplyCheckBox.Location = new System.Drawing.Point(262, 17);
            this.HeaderGlobalApplyCheckBox.Name = "HeaderGlobalApplyCheckBox";
            this.HeaderGlobalApplyCheckBox.Size = new System.Drawing.Size(25, 25);
            this.HeaderGlobalApplyCheckBox.TabIndex = 34;
            this.HeaderGlobalApplyCheckBox.UseVisualStyleBackColor = true;
            // 
            // HeaderFontStyleSelector
            // 
            this.HeaderFontStyleSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.HeaderFontStyleSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HeaderFontStyleSelector.FontStyle = System.Drawing.FontStyle.Regular;
            this.HeaderFontStyleSelector.Location = new System.Drawing.Point(164, 20);
            this.HeaderFontStyleSelector.Name = "HeaderFontStyleSelector";
            this.HeaderFontStyleSelector.Size = new System.Drawing.Size(74, 21);
            this.HeaderFontStyleSelector.TabIndex = 28;
            // 
            // HeaderTextBox
            // 
            this.HeaderTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderTextBox.Location = new System.Drawing.Point(5, 47);
            this.HeaderTextBox.Name = "HeaderTextBox";
            this.HeaderTextBox.Size = new System.Drawing.Size(233, 26);
            this.HeaderTextBox.TabIndex = 20;
            // 
            // HeaderFontSizeComboBox
            // 
            this.HeaderFontSizeComboBox.FormattingEnabled = true;
            this.HeaderFontSizeComboBox.Location = new System.Drawing.Point(112, 20);
            this.HeaderFontSizeComboBox.Name = "HeaderFontSizeComboBox";
            this.HeaderFontSizeComboBox.Size = new System.Drawing.Size(46, 21);
            this.HeaderFontSizeComboBox.TabIndex = 30;
            this.HeaderFontSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.HeaderFontSizeComboBox_SelectedIndexChanged);
            // 
            // HeaderFontComboBox
            // 
            this.HeaderFontComboBox.FormattingEnabled = true;
            this.HeaderFontComboBox.Location = new System.Drawing.Point(6, 20);
            this.HeaderFontComboBox.Name = "HeaderFontComboBox";
            this.HeaderFontComboBox.Size = new System.Drawing.Size(100, 21);
            this.HeaderFontComboBox.TabIndex = 29;
            this.HeaderFontComboBox.SelectedIndexChanged += new System.EventHandler(this.HeaderFontComboBox_SelectedIndexChanged);
            // 
            // HeaderTextLabel
            // 
            this.HeaderTextLabel.AutoSize = true;
            this.HeaderTextLabel.Location = new System.Drawing.Point(2, 5);
            this.HeaderTextLabel.Name = "HeaderTextLabel";
            this.HeaderTextLabel.Size = new System.Drawing.Size(28, 13);
            this.HeaderTextLabel.TabIndex = 19;
            this.HeaderTextLabel.Text = "Text";
            // 
            // FooterControlsPanel
            // 
            this.FooterControlsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FooterControlsPanel.Controls.Add(this.FooterBottomGlobalApplyCheckBox);
            this.FooterControlsPanel.Controls.Add(this.FooterMiddleGlobalApplyCheckBox);
            this.FooterControlsPanel.Controls.Add(this.FooterTopGlobalApplyCheckBox);
            this.FooterControlsPanel.Controls.Add(this.FooterBottomFontStyleSelector);
            this.FooterControlsPanel.Controls.Add(this.FooterMiddleFontStyleSelector);
            this.FooterControlsPanel.Controls.Add(this.FooterTopFontStyleSelector);
            this.FooterControlsPanel.Controls.Add(this.FooterBottomSizeComboBox);
            this.FooterControlsPanel.Controls.Add(this.FooterBottomFontComboBox);
            this.FooterControlsPanel.Controls.Add(this.FooterMiddleSizeComboBox);
            this.FooterControlsPanel.Controls.Add(this.FooterMiddleFontComboBox);
            this.FooterControlsPanel.Controls.Add(this.FooterTopSizeComboBox);
            this.FooterControlsPanel.Controls.Add(this.FooterTopFontComboBox);
            this.FooterControlsPanel.Controls.Add(this.FooterBottomTextLabel);
            this.FooterControlsPanel.Controls.Add(this.FooterTopLabel);
            this.FooterControlsPanel.Controls.Add(this.FooterTopDataTextBox);
            this.FooterControlsPanel.Controls.Add(this.FooterMiddleLabel);
            this.FooterControlsPanel.Controls.Add(this.FooterMiddleDataTextBox);
            this.FooterControlsPanel.Controls.Add(this.FooterBottomDataTextBox);
            this.FooterControlsPanel.Location = new System.Drawing.Point(335, 149);
            this.FooterControlsPanel.Name = "FooterControlsPanel";
            this.FooterControlsPanel.Size = new System.Drawing.Size(292, 275);
            this.FooterControlsPanel.TabIndex = 19;
            // 
            // FooterBottomGlobalApplyCheckBox
            // 
            this.FooterBottomGlobalApplyCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.FooterBottomGlobalApplyCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.FooterBottomGlobalApplyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FooterBottomGlobalApplyCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.earth_16_000000;
            this.FooterBottomGlobalApplyCheckBox.Location = new System.Drawing.Point(262, 203);
            this.FooterBottomGlobalApplyCheckBox.Name = "FooterBottomGlobalApplyCheckBox";
            this.FooterBottomGlobalApplyCheckBox.Size = new System.Drawing.Size(25, 25);
            this.FooterBottomGlobalApplyCheckBox.TabIndex = 37;
            this.FooterBottomGlobalApplyCheckBox.UseVisualStyleBackColor = true;
            // 
            // FooterMiddleGlobalApplyCheckBox
            // 
            this.FooterMiddleGlobalApplyCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.FooterMiddleGlobalApplyCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.FooterMiddleGlobalApplyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FooterMiddleGlobalApplyCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.earth_16_000000;
            this.FooterMiddleGlobalApplyCheckBox.Location = new System.Drawing.Point(262, 106);
            this.FooterMiddleGlobalApplyCheckBox.Name = "FooterMiddleGlobalApplyCheckBox";
            this.FooterMiddleGlobalApplyCheckBox.Size = new System.Drawing.Size(25, 25);
            this.FooterMiddleGlobalApplyCheckBox.TabIndex = 36;
            this.FooterMiddleGlobalApplyCheckBox.UseVisualStyleBackColor = true;
            // 
            // FooterTopGlobalApplyCheckBox
            // 
            this.FooterTopGlobalApplyCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.FooterTopGlobalApplyCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.FooterTopGlobalApplyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FooterTopGlobalApplyCheckBox.Image = global::Dimmer_Labels_Wizard.Properties.Resources.earth_16_000000;
            this.FooterTopGlobalApplyCheckBox.Location = new System.Drawing.Point(262, 17);
            this.FooterTopGlobalApplyCheckBox.Name = "FooterTopGlobalApplyCheckBox";
            this.FooterTopGlobalApplyCheckBox.Size = new System.Drawing.Size(25, 25);
            this.FooterTopGlobalApplyCheckBox.TabIndex = 35;
            this.FooterTopGlobalApplyCheckBox.UseVisualStyleBackColor = true;
            // 
            // FooterBottomFontStyleSelector
            // 
            this.FooterBottomFontStyleSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FooterBottomFontStyleSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FooterBottomFontStyleSelector.FontStyle = System.Drawing.FontStyle.Regular;
            this.FooterBottomFontStyleSelector.Location = new System.Drawing.Point(164, 206);
            this.FooterBottomFontStyleSelector.Name = "FooterBottomFontStyleSelector";
            this.FooterBottomFontStyleSelector.Size = new System.Drawing.Size(74, 21);
            this.FooterBottomFontStyleSelector.TabIndex = 27;
            // 
            // FooterMiddleFontStyleSelector
            // 
            this.FooterMiddleFontStyleSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FooterMiddleFontStyleSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FooterMiddleFontStyleSelector.FontStyle = System.Drawing.FontStyle.Regular;
            this.FooterMiddleFontStyleSelector.Location = new System.Drawing.Point(164, 109);
            this.FooterMiddleFontStyleSelector.Name = "FooterMiddleFontStyleSelector";
            this.FooterMiddleFontStyleSelector.Size = new System.Drawing.Size(74, 21);
            this.FooterMiddleFontStyleSelector.TabIndex = 26;
            // 
            // FooterTopFontStyleSelector
            // 
            this.FooterTopFontStyleSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FooterTopFontStyleSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FooterTopFontStyleSelector.FontStyle = System.Drawing.FontStyle.Regular;
            this.FooterTopFontStyleSelector.Location = new System.Drawing.Point(164, 20);
            this.FooterTopFontStyleSelector.Name = "FooterTopFontStyleSelector";
            this.FooterTopFontStyleSelector.Size = new System.Drawing.Size(74, 21);
            this.FooterTopFontStyleSelector.TabIndex = 0;
            // 
            // FooterBottomSizeComboBox
            // 
            this.FooterBottomSizeComboBox.FormattingEnabled = true;
            this.FooterBottomSizeComboBox.Location = new System.Drawing.Point(112, 206);
            this.FooterBottomSizeComboBox.Name = "FooterBottomSizeComboBox";
            this.FooterBottomSizeComboBox.Size = new System.Drawing.Size(46, 21);
            this.FooterBottomSizeComboBox.TabIndex = 25;
            this.FooterBottomSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.FooterBottomSizeComboBox_SelectedIndexChanged);
            // 
            // FooterBottomFontComboBox
            // 
            this.FooterBottomFontComboBox.FormattingEnabled = true;
            this.FooterBottomFontComboBox.Location = new System.Drawing.Point(6, 206);
            this.FooterBottomFontComboBox.Name = "FooterBottomFontComboBox";
            this.FooterBottomFontComboBox.Size = new System.Drawing.Size(100, 21);
            this.FooterBottomFontComboBox.TabIndex = 24;
            this.FooterBottomFontComboBox.SelectedIndexChanged += new System.EventHandler(this.FooterBottomFontComboBox_SelectedIndexChanged);
            // 
            // FooterMiddleSizeComboBox
            // 
            this.FooterMiddleSizeComboBox.FormattingEnabled = true;
            this.FooterMiddleSizeComboBox.Location = new System.Drawing.Point(112, 109);
            this.FooterMiddleSizeComboBox.Name = "FooterMiddleSizeComboBox";
            this.FooterMiddleSizeComboBox.Size = new System.Drawing.Size(46, 21);
            this.FooterMiddleSizeComboBox.TabIndex = 23;
            this.FooterMiddleSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.FooterMiddleSizeComboBox_SelectedIndexChanged);
            // 
            // FooterMiddleFontComboBox
            // 
            this.FooterMiddleFontComboBox.FormattingEnabled = true;
            this.FooterMiddleFontComboBox.Location = new System.Drawing.Point(6, 109);
            this.FooterMiddleFontComboBox.Name = "FooterMiddleFontComboBox";
            this.FooterMiddleFontComboBox.Size = new System.Drawing.Size(100, 21);
            this.FooterMiddleFontComboBox.TabIndex = 22;
            this.FooterMiddleFontComboBox.SelectedIndexChanged += new System.EventHandler(this.FooterMiddleFontComboBox_SelectedIndexChanged);
            // 
            // FooterTopSizeComboBox
            // 
            this.FooterTopSizeComboBox.FormattingEnabled = true;
            this.FooterTopSizeComboBox.Location = new System.Drawing.Point(112, 20);
            this.FooterTopSizeComboBox.Name = "FooterTopSizeComboBox";
            this.FooterTopSizeComboBox.Size = new System.Drawing.Size(46, 21);
            this.FooterTopSizeComboBox.TabIndex = 21;
            this.FooterTopSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.FooterTopSizeComboBox_SelectedIndexChanged);
            // 
            // FooterTopFontComboBox
            // 
            this.FooterTopFontComboBox.FormattingEnabled = true;
            this.FooterTopFontComboBox.Location = new System.Drawing.Point(6, 20);
            this.FooterTopFontComboBox.Name = "FooterTopFontComboBox";
            this.FooterTopFontComboBox.Size = new System.Drawing.Size(100, 21);
            this.FooterTopFontComboBox.TabIndex = 20;
            this.FooterTopFontComboBox.SelectedIndexChanged += new System.EventHandler(this.FooterTopFontComboBox_SelectedIndexChanged);
            // 
            // FooterBottomTextLabel
            // 
            this.FooterBottomTextLabel.AutoSize = true;
            this.FooterBottomTextLabel.Location = new System.Drawing.Point(3, 191);
            this.FooterBottomTextLabel.Name = "FooterBottomTextLabel";
            this.FooterBottomTextLabel.Size = new System.Drawing.Size(63, 13);
            this.FooterBottomTextLabel.TabIndex = 19;
            this.FooterBottomTextLabel.Text = "Bottom Line";
            // 
            // FooterTopLabel
            // 
            this.FooterTopLabel.AutoSize = true;
            this.FooterTopLabel.Location = new System.Drawing.Point(3, 4);
            this.FooterTopLabel.Name = "FooterTopLabel";
            this.FooterTopLabel.Size = new System.Drawing.Size(49, 13);
            this.FooterTopLabel.TabIndex = 10;
            this.FooterTopLabel.Text = "Top Line";
            // 
            // FooterTopDataTextBox
            // 
            this.FooterTopDataTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FooterTopDataTextBox.Location = new System.Drawing.Point(6, 47);
            this.FooterTopDataTextBox.Name = "FooterTopDataTextBox";
            this.FooterTopDataTextBox.Size = new System.Drawing.Size(232, 26);
            this.FooterTopDataTextBox.TabIndex = 11;
            // 
            // FooterMiddleLabel
            // 
            this.FooterMiddleLabel.AutoSize = true;
            this.FooterMiddleLabel.Location = new System.Drawing.Point(3, 93);
            this.FooterMiddleLabel.Name = "FooterMiddleLabel";
            this.FooterMiddleLabel.Size = new System.Drawing.Size(61, 13);
            this.FooterMiddleLabel.TabIndex = 12;
            this.FooterMiddleLabel.Text = "Middle Line";
            // 
            // FooterMiddleDataTextBox
            // 
            this.FooterMiddleDataTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FooterMiddleDataTextBox.Location = new System.Drawing.Point(6, 139);
            this.FooterMiddleDataTextBox.Name = "FooterMiddleDataTextBox";
            this.FooterMiddleDataTextBox.Size = new System.Drawing.Size(232, 26);
            this.FooterMiddleDataTextBox.TabIndex = 13;
            // 
            // FooterBottomDataTextBox
            // 
            this.FooterBottomDataTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FooterBottomDataTextBox.Location = new System.Drawing.Point(6, 233);
            this.FooterBottomDataTextBox.Name = "FooterBottomDataTextBox";
            this.FooterBottomDataTextBox.Size = new System.Drawing.Size(232, 26);
            this.FooterBottomDataTextBox.TabIndex = 15;
            // 
            // RackLabelSelector
            // 
            this.RackLabelSelector.Location = new System.Drawing.Point(3, 29);
            this.RackLabelSelector.Name = "RackLabelSelector";
            this.RackLabelSelector.Size = new System.Drawing.Size(140, 313);
            this.RackLabelSelector.TabIndex = 6;
            this.RackLabelSelector.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RackLabelSelector_AfterSelect);
            // 
            // PrintButton
            // 
            this.PrintButton.Location = new System.Drawing.Point(965, 726);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(75, 23);
            this.PrintButton.TabIndex = 7;
            this.PrintButton.Text = "Print";
            this.PrintButton.UseVisualStyleBackColor = true;
            this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(8, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 32;
            this.label5.Text = "Label Viewer";
            // 
            // ViewControlPanel
            // 
            this.ViewControlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ViewControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewControlPanel.Controls.Add(this.CenterViewButton);
            this.ViewControlPanel.Controls.Add(this.MagnifyMinusButton);
            this.ViewControlPanel.Controls.Add(this.MagnifyPlusButton);
            this.ViewControlPanel.Location = new System.Drawing.Point(930, 15);
            this.ViewControlPanel.Name = "ViewControlPanel";
            this.ViewControlPanel.Size = new System.Drawing.Size(97, 31);
            this.ViewControlPanel.TabIndex = 0;
            // 
            // CenterViewButton
            // 
            this.CenterViewButton.Image = global::Dimmer_Labels_Wizard.Properties.Resources.crosshair_16_000000;
            this.CenterViewButton.Location = new System.Drawing.Point(65, 2);
            this.CenterViewButton.Name = "CenterViewButton";
            this.CenterViewButton.Size = new System.Drawing.Size(25, 25);
            this.CenterViewButton.TabIndex = 2;
            this.CenterViewButton.UseVisualStyleBackColor = true;
            this.CenterViewButton.Click += new System.EventHandler(this.CenterViewButton_Click);
            // 
            // MagnifyMinusButton
            // 
            this.MagnifyMinusButton.Image = global::Dimmer_Labels_Wizard.Properties.Resources.magnify_minus_16_000000;
            this.MagnifyMinusButton.Location = new System.Drawing.Point(34, 2);
            this.MagnifyMinusButton.Name = "MagnifyMinusButton";
            this.MagnifyMinusButton.Size = new System.Drawing.Size(25, 25);
            this.MagnifyMinusButton.TabIndex = 1;
            this.MagnifyMinusButton.UseVisualStyleBackColor = true;
            this.MagnifyMinusButton.Click += new System.EventHandler(this.MagnifyMinusButton_Click);
            // 
            // MagnifyPlusButton
            // 
            this.MagnifyPlusButton.Image = global::Dimmer_Labels_Wizard.Properties.Resources.magnify_add_16_000000;
            this.MagnifyPlusButton.Location = new System.Drawing.Point(3, 2);
            this.MagnifyPlusButton.Name = "MagnifyPlusButton";
            this.MagnifyPlusButton.Size = new System.Drawing.Size(25, 25);
            this.MagnifyPlusButton.TabIndex = 0;
            this.MagnifyPlusButton.UseVisualStyleBackColor = true;
            this.MagnifyPlusButton.Click += new System.EventHandler(this.MagnifyPlusButton_Click);
            // 
            // ToolsPanel
            // 
            this.ToolsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ToolsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ToolsPanel.Controls.Add(this.SplitCellButton);
            this.ToolsPanel.Location = new System.Drawing.Point(890, 15);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Size = new System.Drawing.Size(34, 31);
            this.ToolsPanel.TabIndex = 33;
            // 
            // SplitCellButton
            // 
            this.SplitCellButton.Image = global::Dimmer_Labels_Wizard.Properties.Resources.Split_16_000;
            this.SplitCellButton.Location = new System.Drawing.Point(3, 2);
            this.SplitCellButton.Name = "SplitCellButton";
            this.SplitCellButton.Size = new System.Drawing.Size(25, 24);
            this.SplitCellButton.TabIndex = 3;
            this.SplitCellButton.UseVisualStyleBackColor = true;
            this.SplitCellButton.Click += new System.EventHandler(this.SplitCellButton_Click);
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(884, 726);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 34;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // PerformanceTestButton
            // 
            this.PerformanceTestButton.Location = new System.Drawing.Point(952, 327);
            this.PerformanceTestButton.Name = "PerformanceTestButton";
            this.PerformanceTestButton.Size = new System.Drawing.Size(75, 48);
            this.PerformanceTestButton.TabIndex = 22;
            this.PerformanceTestButton.Text = "Performance Test";
            this.PerformanceTestButton.UseVisualStyleBackColor = true;
            this.PerformanceTestButton.Click += new System.EventHandler(this.PerformanceTestButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(952, 408);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 35;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(952, 437);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 36;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            // 
            // CanvasHost
            // 
            this.CanvasHost.Location = new System.Drawing.Point(12, 48);
            this.CanvasHost.Name = "CanvasHost";
            this.CanvasHost.Size = new System.Drawing.Size(1015, 242);
            this.CanvasHost.TabIndex = 37;
            this.CanvasHost.Text = "elementHost1";
            this.CanvasHost.Child = null;
            // 
            // FORM_LabelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1048, 759);
            this.Controls.Add(this.CanvasHost);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.ToolsPanel);
            this.Controls.Add(this.ViewControlPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PerformanceTestButton);
            this.Controls.Add(this.LowerPanel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.DebugButton);
            this.Controls.Add(this.PrintButton);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FORM_LabelEditor";
            this.Text = "Label Editor";
            this.Load += new System.EventHandler(this.FORM_LabelEditor_Load);
            this.LowerPanel.ResumeLayout(false);
            this.LowerPanel.PerformLayout();
            this.GeneralControlsPanel.ResumeLayout(false);
            this.GeneralControlsPanel.PerformLayout();
            this.HeaderControlsPanel.ResumeLayout(false);
            this.HeaderControlsPanel.PerformLayout();
            this.FooterControlsPanel.ResumeLayout(false);
            this.FooterControlsPanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ViewControlPanel.ResumeLayout(false);
            this.ToolsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DebugButton;
        private System.Windows.Forms.Panel LowerPanel;
        private System.Windows.Forms.TreeView RackLabelSelector;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.Button PrintButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageSettingsToolStripMenuItem;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.Button FooterBackgroundColorButton;
        private System.Windows.Forms.Label FooterBackgroundColorLabel;
        private System.Windows.Forms.ColorDialog backgroundColorDialog;
        private System.Windows.Forms.TextBox FooterBottomDataTextBox;
        private System.Windows.Forms.TextBox FooterMiddleDataTextBox;
        private System.Windows.Forms.Label FooterMiddleLabel;
        private System.Windows.Forms.TextBox FooterTopDataTextBox;
        private System.Windows.Forms.Label FooterTopLabel;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Panel FooterControlsPanel;
        private System.Windows.Forms.Panel GeneralControlsPanel;
        private System.Windows.Forms.Panel HeaderControlsPanel;
        private System.Windows.Forms.TextBox HeaderTextBox;
        private System.Windows.Forms.Label HeaderTextLabel;
        private System.Windows.Forms.Label FooterBottomTextLabel;
        private System.Windows.Forms.ComboBox FooterTopFontComboBox;
        private System.Windows.Forms.ComboBox FooterTopSizeComboBox;
        private System.Windows.Forms.ComboBox FooterBottomSizeComboBox;
        private System.Windows.Forms.ComboBox FooterBottomFontComboBox;
        private System.Windows.Forms.ComboBox FooterMiddleSizeComboBox;
        private System.Windows.Forms.ComboBox FooterMiddleFontComboBox;
        private FontStyleControl FooterTopFontStyleSelector;
        private FontStyleControl FooterMiddleFontStyleSelector;
        private FontStyleControl FooterBottomFontStyleSelector;
        private FontStyleControl HeaderFontStyleSelector;
        private System.Windows.Forms.ComboBox HeaderFontSizeComboBox;
        private System.Windows.Forms.ComboBox HeaderFontComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel ViewControlPanel;
        private System.Windows.Forms.Button MagnifyPlusButton;
        private System.Windows.Forms.Button MagnifyMinusButton;
        private System.Windows.Forms.Button CenterViewButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button SplitCellButton;
        private System.Windows.Forms.Panel ToolsPanel;
        private System.Windows.Forms.CheckBox HeaderGlobalApplyCheckBox;
        private System.Windows.Forms.CheckBox BackgroundColorGlobalApplyCheckBox;
        private System.Windows.Forms.CheckBox FooterBottomGlobalApplyCheckBox;
        private System.Windows.Forms.CheckBox FooterMiddleGlobalApplyCheckBox;
        private System.Windows.Forms.CheckBox FooterTopGlobalApplyCheckBox;
        private System.Windows.Forms.CheckBox LineWeightGlobalApplyCheckBox;
        private System.Windows.Forms.ComboBox LineWeightComboBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button PerformanceTestButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Integration.ElementHost CanvasHost;
    }
}
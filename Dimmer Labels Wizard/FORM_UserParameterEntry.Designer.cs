namespace Dimmer_Labels_Wizard
{
    partial class FORM_UserParameterEntry
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FirstDistroNumberSelector = new System.Windows.Forms.NumericUpDown();
            this.LastDistroNumberSelector = new System.Windows.Forms.NumericUpDown();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.DistroFormatComboBox = new System.Windows.Forms.ComboBox();
            this.DimmerFormatComboBox = new System.Windows.Forms.ComboBox();
            this.DistroNumberFormatLabel = new System.Windows.Forms.Label();
            this.DimmerNumberFormatLabel = new System.Windows.Forms.Label();
            this.UniverseColumnSelectPanel = new System.Windows.Forms.Panel();
            this.NoUniverseDataCheckBox = new System.Windows.Forms.CheckBox();
            this.UniverseDMXColumnsComboBox = new System.Windows.Forms.ComboBox();
            this.DMXAddressFormatComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.UniverseColumnLabel = new System.Windows.Forms.Label();
            this.CSVColumnMappingLabel = new System.Windows.Forms.Label();
            this.CSVColumnMappingPanel = new System.Windows.Forms.Panel();
            this.DimmerNumberMappingComboBox = new System.Windows.Forms.ComboBox();
            this.DimmerNumberMappingLabel = new System.Windows.Forms.Label();
            this.PositionMappingComboBox = new System.Windows.Forms.ComboBox();
            this.MulticoreNameMappingComboBox = new System.Windows.Forms.ComboBox();
            this.InstrumentNameMappingComboBox = new System.Windows.Forms.ComboBox();
            this.ChannelMappingComboBox = new System.Windows.Forms.ComboBox();
            this.PositionMappingLabel = new System.Windows.Forms.Label();
            this.MulticoreNameMappingLabel = new System.Windows.Forms.Label();
            this.InstrumentNameMappingLabel = new System.Windows.Forms.Label();
            this.ChannelMappingLabel = new System.Windows.Forms.Label();
            this.DistroNumberPrefixPanel = new System.Windows.Forms.Panel();
            this.DistroNumberPrefixLabel = new System.Windows.Forms.Label();
            this.DistroNumberPrefixTextBox = new System.Windows.Forms.TextBox();
            this.Panel8 = new System.Windows.Forms.Panel();
            this.CreateDistroLabelsCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateDimmerLabelsCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DistroNumberPanel = new System.Windows.Forms.Panel();
            this.DimmerRangeInput = new Dimmer_Labels_Wizard.DimmerRangeInputControl();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.FirstDistroNumberSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastDistroNumberSelector)).BeginInit();
            this.UniverseColumnSelectPanel.SuspendLayout();
            this.CSVColumnMappingPanel.SuspendLayout();
            this.DistroNumberPrefixPanel.SuspendLayout();
            this.Panel8.SuspendLayout();
            this.DistroNumberPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "First Distro Number";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(114, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Last Distro Number";
            // 
            // FirstDistroNumberSelector
            // 
            this.FirstDistroNumberSelector.Location = new System.Drawing.Point(6, 24);
            this.FirstDistroNumberSelector.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.FirstDistroNumberSelector.Name = "FirstDistroNumberSelector";
            this.FirstDistroNumberSelector.Size = new System.Drawing.Size(93, 20);
            this.FirstDistroNumberSelector.TabIndex = 6;
            this.FirstDistroNumberSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LastDistroNumberSelector
            // 
            this.LastDistroNumberSelector.Location = new System.Drawing.Point(117, 23);
            this.LastDistroNumberSelector.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.LastDistroNumberSelector.Name = "LastDistroNumberSelector";
            this.LastDistroNumberSelector.Size = new System.Drawing.Size(93, 20);
            this.LastDistroNumberSelector.TabIndex = 7;
            this.LastDistroNumberSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ContinueButton
            // 
            this.ContinueButton.Location = new System.Drawing.Point(703, 313);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(75, 23);
            this.ContinueButton.TabIndex = 12;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // DistroFormatComboBox
            // 
            this.DistroFormatComboBox.FormattingEnabled = true;
            this.DistroFormatComboBox.Items.AddRange(new object[] {
            "A### or AA###",
            "###",
            "#/###",
            "A/###"});
            this.DistroFormatComboBox.Location = new System.Drawing.Point(6, 21);
            this.DistroFormatComboBox.Name = "DistroFormatComboBox";
            this.DistroFormatComboBox.Size = new System.Drawing.Size(121, 21);
            this.DistroFormatComboBox.TabIndex = 13;
            this.DistroFormatComboBox.Text = "Select";
            this.DistroFormatComboBox.SelectedIndexChanged += new System.EventHandler(this.DistroFormatComboBox_SelectedIndexChanged);
            // 
            // DimmerFormatComboBox
            // 
            this.DimmerFormatComboBox.FormattingEnabled = true;
            this.DimmerFormatComboBox.Items.AddRange(new object[] {
            "#/###",
            "###",
            "A###",
            "A/###"});
            this.DimmerFormatComboBox.Location = new System.Drawing.Point(148, 21);
            this.DimmerFormatComboBox.Name = "DimmerFormatComboBox";
            this.DimmerFormatComboBox.Size = new System.Drawing.Size(121, 21);
            this.DimmerFormatComboBox.TabIndex = 14;
            this.DimmerFormatComboBox.Text = "Select";
            this.DimmerFormatComboBox.SelectedIndexChanged += new System.EventHandler(this.DimmerFormatComboBox_SelectedIndexChanged);
            // 
            // DistroNumberFormatLabel
            // 
            this.DistroNumberFormatLabel.AutoSize = true;
            this.DistroNumberFormatLabel.Location = new System.Drawing.Point(3, 3);
            this.DistroNumberFormatLabel.Name = "DistroNumberFormatLabel";
            this.DistroNumberFormatLabel.Size = new System.Drawing.Size(109, 13);
            this.DistroNumberFormatLabel.TabIndex = 15;
            this.DistroNumberFormatLabel.Text = "Distro Number Format";
            // 
            // DimmerNumberFormatLabel
            // 
            this.DimmerNumberFormatLabel.AutoSize = true;
            this.DimmerNumberFormatLabel.Location = new System.Drawing.Point(145, 4);
            this.DimmerNumberFormatLabel.Name = "DimmerNumberFormatLabel";
            this.DimmerNumberFormatLabel.Size = new System.Drawing.Size(117, 13);
            this.DimmerNumberFormatLabel.TabIndex = 16;
            this.DimmerNumberFormatLabel.Text = "Dimmer Number Format";
            // 
            // UniverseColumnSelectPanel
            // 
            this.UniverseColumnSelectPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UniverseColumnSelectPanel.Controls.Add(this.NoUniverseDataCheckBox);
            this.UniverseColumnSelectPanel.Controls.Add(this.UniverseDMXColumnsComboBox);
            this.UniverseColumnSelectPanel.Controls.Add(this.DMXAddressFormatComboBox);
            this.UniverseColumnSelectPanel.Controls.Add(this.label6);
            this.UniverseColumnSelectPanel.Controls.Add(this.UniverseColumnLabel);
            this.UniverseColumnSelectPanel.Location = new System.Drawing.Point(608, 39);
            this.UniverseColumnSelectPanel.Name = "UniverseColumnSelectPanel";
            this.UniverseColumnSelectPanel.Size = new System.Drawing.Size(167, 114);
            this.UniverseColumnSelectPanel.TabIndex = 17;
            // 
            // NoUniverseDataCheckBox
            // 
            this.NoUniverseDataCheckBox.AutoSize = true;
            this.NoUniverseDataCheckBox.Location = new System.Drawing.Point(6, 4);
            this.NoUniverseDataCheckBox.Name = "NoUniverseDataCheckBox";
            this.NoUniverseDataCheckBox.Size = new System.Drawing.Size(140, 17);
            this.NoUniverseDataCheckBox.TabIndex = 4;
            this.NoUniverseDataCheckBox.Text = "No Universe info in CSV";
            this.NoUniverseDataCheckBox.UseVisualStyleBackColor = true;
            this.NoUniverseDataCheckBox.CheckedChanged += new System.EventHandler(this.NoUniverseDataCheckBox_CheckedChanged);
            // 
            // UniverseDMXColumnsComboBox
            // 
            this.UniverseDMXColumnsComboBox.FormattingEnabled = true;
            this.UniverseDMXColumnsComboBox.Location = new System.Drawing.Point(6, 46);
            this.UniverseDMXColumnsComboBox.Name = "UniverseDMXColumnsComboBox";
            this.UniverseDMXColumnsComboBox.Size = new System.Drawing.Size(121, 21);
            this.UniverseDMXColumnsComboBox.TabIndex = 3;
            this.UniverseDMXColumnsComboBox.Text = "Select";
            this.UniverseDMXColumnsComboBox.SelectedIndexChanged += new System.EventHandler(this.UniverseDMXColumnsComboBox_SelectedIndexChanged);
            // 
            // DMXAddressFormatComboBox
            // 
            this.DMXAddressFormatComboBox.FormattingEnabled = true;
            this.DMXAddressFormatComboBox.Items.AddRange(new object[] {
            "#/###",
            "###",
            "A###",
            "A/###"});
            this.DMXAddressFormatComboBox.Location = new System.Drawing.Point(6, 88);
            this.DMXAddressFormatComboBox.Name = "DMXAddressFormatComboBox";
            this.DMXAddressFormatComboBox.Size = new System.Drawing.Size(121, 21);
            this.DMXAddressFormatComboBox.TabIndex = 2;
            this.DMXAddressFormatComboBox.Text = "Select";
            this.DMXAddressFormatComboBox.SelectedIndexChanged += new System.EventHandler(this.DMXAddressFormatComboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Number Format";
            // 
            // UniverseColumnLabel
            // 
            this.UniverseColumnLabel.AutoSize = true;
            this.UniverseColumnLabel.Location = new System.Drawing.Point(3, 29);
            this.UniverseColumnLabel.Name = "UniverseColumnLabel";
            this.UniverseColumnLabel.Size = new System.Drawing.Size(116, 13);
            this.UniverseColumnLabel.TabIndex = 0;
            this.UniverseColumnLabel.Text = "Universe/DMX Column";
            // 
            // CSVColumnMappingLabel
            // 
            this.CSVColumnMappingLabel.AutoSize = true;
            this.CSVColumnMappingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CSVColumnMappingLabel.Location = new System.Drawing.Point(328, 161);
            this.CSVColumnMappingLabel.Name = "CSVColumnMappingLabel";
            this.CSVColumnMappingLabel.Size = new System.Drawing.Size(165, 20);
            this.CSVColumnMappingLabel.TabIndex = 18;
            this.CSVColumnMappingLabel.Text = "CSV Column Mapping";
            // 
            // CSVColumnMappingPanel
            // 
            this.CSVColumnMappingPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CSVColumnMappingPanel.Controls.Add(this.DimmerNumberMappingComboBox);
            this.CSVColumnMappingPanel.Controls.Add(this.DimmerNumberMappingLabel);
            this.CSVColumnMappingPanel.Controls.Add(this.PositionMappingComboBox);
            this.CSVColumnMappingPanel.Controls.Add(this.MulticoreNameMappingComboBox);
            this.CSVColumnMappingPanel.Controls.Add(this.InstrumentNameMappingComboBox);
            this.CSVColumnMappingPanel.Controls.Add(this.ChannelMappingComboBox);
            this.CSVColumnMappingPanel.Controls.Add(this.PositionMappingLabel);
            this.CSVColumnMappingPanel.Controls.Add(this.MulticoreNameMappingLabel);
            this.CSVColumnMappingPanel.Controls.Add(this.InstrumentNameMappingLabel);
            this.CSVColumnMappingPanel.Controls.Add(this.ChannelMappingLabel);
            this.CSVColumnMappingPanel.Location = new System.Drawing.Point(326, 184);
            this.CSVColumnMappingPanel.Name = "CSVColumnMappingPanel";
            this.CSVColumnMappingPanel.Size = new System.Drawing.Size(276, 150);
            this.CSVColumnMappingPanel.TabIndex = 19;
            // 
            // DimmerNumberMappingComboBox
            // 
            this.DimmerNumberMappingComboBox.FormattingEnabled = true;
            this.DimmerNumberMappingComboBox.Location = new System.Drawing.Point(136, 36);
            this.DimmerNumberMappingComboBox.Name = "DimmerNumberMappingComboBox";
            this.DimmerNumberMappingComboBox.Size = new System.Drawing.Size(121, 21);
            this.DimmerNumberMappingComboBox.TabIndex = 28;
            this.DimmerNumberMappingComboBox.Text = "None";
            // 
            // DimmerNumberMappingLabel
            // 
            this.DimmerNumberMappingLabel.AutoSize = true;
            this.DimmerNumberMappingLabel.Location = new System.Drawing.Point(24, 39);
            this.DimmerNumberMappingLabel.Name = "DimmerNumberMappingLabel";
            this.DimmerNumberMappingLabel.Size = new System.Drawing.Size(82, 13);
            this.DimmerNumberMappingLabel.TabIndex = 27;
            this.DimmerNumberMappingLabel.Text = "Dimmer Number";
            // 
            // PositionMappingComboBox
            // 
            this.PositionMappingComboBox.FormattingEnabled = true;
            this.PositionMappingComboBox.Location = new System.Drawing.Point(136, 117);
            this.PositionMappingComboBox.Name = "PositionMappingComboBox";
            this.PositionMappingComboBox.Size = new System.Drawing.Size(121, 21);
            this.PositionMappingComboBox.TabIndex = 26;
            this.PositionMappingComboBox.Text = "None";
            // 
            // MulticoreNameMappingComboBox
            // 
            this.MulticoreNameMappingComboBox.FormattingEnabled = true;
            this.MulticoreNameMappingComboBox.Location = new System.Drawing.Point(136, 90);
            this.MulticoreNameMappingComboBox.Name = "MulticoreNameMappingComboBox";
            this.MulticoreNameMappingComboBox.Size = new System.Drawing.Size(121, 21);
            this.MulticoreNameMappingComboBox.TabIndex = 25;
            this.MulticoreNameMappingComboBox.Text = "None";
            // 
            // InstrumentNameMappingComboBox
            // 
            this.InstrumentNameMappingComboBox.FormattingEnabled = true;
            this.InstrumentNameMappingComboBox.Location = new System.Drawing.Point(136, 63);
            this.InstrumentNameMappingComboBox.Name = "InstrumentNameMappingComboBox";
            this.InstrumentNameMappingComboBox.Size = new System.Drawing.Size(121, 21);
            this.InstrumentNameMappingComboBox.TabIndex = 24;
            this.InstrumentNameMappingComboBox.Text = "None";
            // 
            // ChannelMappingComboBox
            // 
            this.ChannelMappingComboBox.FormattingEnabled = true;
            this.ChannelMappingComboBox.Location = new System.Drawing.Point(136, 9);
            this.ChannelMappingComboBox.Name = "ChannelMappingComboBox";
            this.ChannelMappingComboBox.Size = new System.Drawing.Size(121, 21);
            this.ChannelMappingComboBox.TabIndex = 23;
            this.ChannelMappingComboBox.Text = "None";
            // 
            // PositionMappingLabel
            // 
            this.PositionMappingLabel.AutoSize = true;
            this.PositionMappingLabel.Location = new System.Drawing.Point(59, 120);
            this.PositionMappingLabel.Name = "PositionMappingLabel";
            this.PositionMappingLabel.Size = new System.Drawing.Size(44, 13);
            this.PositionMappingLabel.TabIndex = 22;
            this.PositionMappingLabel.Text = "Position";
            // 
            // MulticoreNameMappingLabel
            // 
            this.MulticoreNameMappingLabel.AutoSize = true;
            this.MulticoreNameMappingLabel.Location = new System.Drawing.Point(24, 93);
            this.MulticoreNameMappingLabel.Name = "MulticoreNameMappingLabel";
            this.MulticoreNameMappingLabel.Size = new System.Drawing.Size(81, 13);
            this.MulticoreNameMappingLabel.TabIndex = 21;
            this.MulticoreNameMappingLabel.Text = "Multicore Name";
            // 
            // InstrumentNameMappingLabel
            // 
            this.InstrumentNameMappingLabel.AutoSize = true;
            this.InstrumentNameMappingLabel.Location = new System.Drawing.Point(18, 66);
            this.InstrumentNameMappingLabel.Name = "InstrumentNameMappingLabel";
            this.InstrumentNameMappingLabel.Size = new System.Drawing.Size(87, 13);
            this.InstrumentNameMappingLabel.TabIndex = 20;
            this.InstrumentNameMappingLabel.Text = "Instrument Name";
            // 
            // ChannelMappingLabel
            // 
            this.ChannelMappingLabel.AutoSize = true;
            this.ChannelMappingLabel.Location = new System.Drawing.Point(59, 12);
            this.ChannelMappingLabel.Name = "ChannelMappingLabel";
            this.ChannelMappingLabel.Size = new System.Drawing.Size(46, 13);
            this.ChannelMappingLabel.TabIndex = 19;
            this.ChannelMappingLabel.Text = "Channel";
            // 
            // DistroNumberPrefixPanel
            // 
            this.DistroNumberPrefixPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DistroNumberPrefixPanel.Controls.Add(this.DistroNumberPrefixLabel);
            this.DistroNumberPrefixPanel.Controls.Add(this.DistroNumberPrefixTextBox);
            this.DistroNumberPrefixPanel.Location = new System.Drawing.Point(6, 48);
            this.DistroNumberPrefixPanel.Name = "DistroNumberPrefixPanel";
            this.DistroNumberPrefixPanel.Size = new System.Drawing.Size(121, 51);
            this.DistroNumberPrefixPanel.TabIndex = 20;
            // 
            // DistroNumberPrefixLabel
            // 
            this.DistroNumberPrefixLabel.AutoSize = true;
            this.DistroNumberPrefixLabel.Location = new System.Drawing.Point(4, 5);
            this.DistroNumberPrefixLabel.Name = "DistroNumberPrefixLabel";
            this.DistroNumberPrefixLabel.Size = new System.Drawing.Size(103, 13);
            this.DistroNumberPrefixLabel.TabIndex = 1;
            this.DistroNumberPrefixLabel.Text = "Distro Number Prefix";
            // 
            // DistroNumberPrefixTextBox
            // 
            this.DistroNumberPrefixTextBox.Location = new System.Drawing.Point(7, 24);
            this.DistroNumberPrefixTextBox.Name = "DistroNumberPrefixTextBox";
            this.DistroNumberPrefixTextBox.Size = new System.Drawing.Size(100, 20);
            this.DistroNumberPrefixTextBox.TabIndex = 0;
            this.DistroNumberPrefixTextBox.Text = "Enter Letter Prefix";
            // 
            // Panel8
            // 
            this.Panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel8.Controls.Add(this.CreateDistroLabelsCheckBox);
            this.Panel8.Controls.Add(this.CreateDimmerLabelsCheckBox);
            this.Panel8.Controls.Add(this.label2);
            this.Panel8.Controls.Add(this.label1);
            this.Panel8.Controls.Add(this.DistroNumberPanel);
            this.Panel8.Controls.Add(this.DimmerRangeInput);
            this.Panel8.Location = new System.Drawing.Point(16, 39);
            this.Panel8.Name = "Panel8";
            this.Panel8.Size = new System.Drawing.Size(303, 295);
            this.Panel8.TabIndex = 21;
            // 
            // CreateDistroLabelsCheckBox
            // 
            this.CreateDistroLabelsCheckBox.AutoSize = true;
            this.CreateDistroLabelsCheckBox.Location = new System.Drawing.Point(201, 199);
            this.CreateDistroLabelsCheckBox.Name = "CreateDistroLabelsCheckBox";
            this.CreateDistroLabelsCheckBox.Size = new System.Drawing.Size(91, 17);
            this.CreateDistroLabelsCheckBox.TabIndex = 29;
            this.CreateDistroLabelsCheckBox.Text = "Create Labels";
            this.CreateDistroLabelsCheckBox.UseVisualStyleBackColor = true;
            this.CreateDistroLabelsCheckBox.CheckedChanged += new System.EventHandler(this.CreateDistroLabelsCheckBox_CheckedChanged);
            // 
            // CreateDimmerLabelsCheckBox
            // 
            this.CreateDimmerLabelsCheckBox.AutoSize = true;
            this.CreateDimmerLabelsCheckBox.BackColor = System.Drawing.SystemColors.Control;
            this.CreateDimmerLabelsCheckBox.Location = new System.Drawing.Point(204, 5);
            this.CreateDimmerLabelsCheckBox.Name = "CreateDimmerLabelsCheckBox";
            this.CreateDimmerLabelsCheckBox.Size = new System.Drawing.Size(91, 17);
            this.CreateDimmerLabelsCheckBox.TabIndex = 28;
            this.CreateDimmerLabelsCheckBox.Text = "Create Labels";
            this.CreateDimmerLabelsCheckBox.UseVisualStyleBackColor = false;
            this.CreateDimmerLabelsCheckBox.CheckedChanged += new System.EventHandler(this.CreateDimmerLabelsCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 17);
            this.label2.TabIndex = 27;
            this.label2.Text = "Dimmer Channels";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(3, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 17);
            this.label1.TabIndex = 26;
            this.label1.Text = "Distro Channels";
            // 
            // DistroNumberPanel
            // 
            this.DistroNumberPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DistroNumberPanel.Controls.Add(this.label3);
            this.DistroNumberPanel.Controls.Add(this.LastDistroNumberSelector);
            this.DistroNumberPanel.Controls.Add(this.FirstDistroNumberSelector);
            this.DistroNumberPanel.Controls.Add(this.label4);
            this.DistroNumberPanel.Location = new System.Drawing.Point(3, 219);
            this.DistroNumberPanel.Name = "DistroNumberPanel";
            this.DistroNumberPanel.Size = new System.Drawing.Size(289, 54);
            this.DistroNumberPanel.TabIndex = 25;
            // 
            // DimmerRangeInput
            // 
            this.DimmerRangeInput.AutoScroll = true;
            this.DimmerRangeInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DimmerRangeInput.Location = new System.Drawing.Point(6, 24);
            this.DimmerRangeInput.Name = "DimmerRangeInput";
            this.DimmerRangeInput.Size = new System.Drawing.Size(289, 162);
            this.DimmerRangeInput.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(12, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 20);
            this.label7.TabIndex = 10;
            this.label7.Text = "Label Ranges";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.DistroNumberFormatLabel);
            this.panel2.Controls.Add(this.DistroFormatComboBox);
            this.panel2.Controls.Add(this.DimmerFormatComboBox);
            this.panel2.Controls.Add(this.DistroNumberPrefixPanel);
            this.panel2.Controls.Add(this.DimmerNumberFormatLabel);
            this.panel2.Location = new System.Drawing.Point(325, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(277, 114);
            this.panel2.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label8.Location = new System.Drawing.Point(321, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 20);
            this.label8.TabIndex = 23;
            this.label8.Text = "CSV Format";
            // 
            // FORM_UserParameterEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 348);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Panel8);
            this.Controls.Add(this.CSVColumnMappingPanel);
            this.Controls.Add(this.CSVColumnMappingLabel);
            this.Controls.Add(this.UniverseColumnSelectPanel);
            this.Controls.Add(this.ContinueButton);
            this.Name = "FORM_UserParameterEntry";
            this.Text = "FORM_UserParameterEntry";
            this.Load += new System.EventHandler(this.FORM_UserParameterEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FirstDistroNumberSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastDistroNumberSelector)).EndInit();
            this.UniverseColumnSelectPanel.ResumeLayout(false);
            this.UniverseColumnSelectPanel.PerformLayout();
            this.CSVColumnMappingPanel.ResumeLayout(false);
            this.CSVColumnMappingPanel.PerformLayout();
            this.DistroNumberPrefixPanel.ResumeLayout(false);
            this.DistroNumberPrefixPanel.PerformLayout();
            this.Panel8.ResumeLayout(false);
            this.Panel8.PerformLayout();
            this.DistroNumberPanel.ResumeLayout(false);
            this.DistroNumberPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown FirstDistroNumberSelector;
        private System.Windows.Forms.NumericUpDown LastDistroNumberSelector;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.ComboBox DistroFormatComboBox;
        private System.Windows.Forms.ComboBox DimmerFormatComboBox;
        private System.Windows.Forms.Label DistroNumberFormatLabel;
        private System.Windows.Forms.Label DimmerNumberFormatLabel;
        private System.Windows.Forms.Panel UniverseColumnSelectPanel;
        private System.Windows.Forms.ComboBox UniverseDMXColumnsComboBox;
        private System.Windows.Forms.ComboBox DMXAddressFormatComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label UniverseColumnLabel;
        private System.Windows.Forms.Label CSVColumnMappingLabel;
        private System.Windows.Forms.Panel CSVColumnMappingPanel;
        private System.Windows.Forms.Label ChannelMappingLabel;
        private System.Windows.Forms.Label InstrumentNameMappingLabel;
        private System.Windows.Forms.Label PositionMappingLabel;
        private System.Windows.Forms.Label MulticoreNameMappingLabel;
        private System.Windows.Forms.ComboBox PositionMappingComboBox;
        private System.Windows.Forms.ComboBox MulticoreNameMappingComboBox;
        private System.Windows.Forms.ComboBox InstrumentNameMappingComboBox;
        private System.Windows.Forms.ComboBox ChannelMappingComboBox;
        private System.Windows.Forms.ComboBox DimmerNumberMappingComboBox;
        private System.Windows.Forms.Label DimmerNumberMappingLabel;
        private System.Windows.Forms.CheckBox NoUniverseDataCheckBox;
        private System.Windows.Forms.Panel DistroNumberPrefixPanel;
        private System.Windows.Forms.Label DistroNumberPrefixLabel;
        private System.Windows.Forms.TextBox DistroNumberPrefixTextBox;
        private System.Windows.Forms.Panel Panel8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private DimmerRangeInputControl DimmerRangeInput;
        private System.Windows.Forms.Panel DistroNumberPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CreateDistroLabelsCheckBox;
        private System.Windows.Forms.CheckBox CreateDimmerLabelsCheckBox;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
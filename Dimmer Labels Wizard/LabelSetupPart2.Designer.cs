namespace Dimmer_Labels_Wizard
{
    partial class LabelSetupPart2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelSetupPart2));
            this.label1 = new System.Windows.Forms.Label();
            this.ColorTable = new System.Windows.Forms.DataGridView();
            this.ItemColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColorDisplayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.ShowFieldsComboBox = new System.Windows.Forms.ComboBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.ColorSelectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ColorTable)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Label Colour";
            // 
            // ColorTable
            // 
            this.ColorTable.AllowUserToAddRows = false;
            this.ColorTable.AllowUserToDeleteRows = false;
            this.ColorTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ColorTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemColumn,
            this.ColorDisplayColumn});
            this.ColorTable.Location = new System.Drawing.Point(462, 57);
            this.ColorTable.Name = "ColorTable";
            this.ColorTable.ReadOnly = true;
            this.ColorTable.Size = new System.Drawing.Size(367, 370);
            this.ColorTable.TabIndex = 1;
            // 
            // ItemColumn
            // 
            this.ItemColumn.HeaderText = "Item";
            this.ItemColumn.Name = "ItemColumn";
            this.ItemColumn.ReadOnly = true;
            this.ItemColumn.Width = 250;
            // 
            // ColorDisplayColumn
            // 
            this.ColorDisplayColumn.HeaderText = "Colour";
            this.ColorDisplayColumn.Name = "ColorDisplayColumn";
            this.ColorDisplayColumn.ReadOnly = true;
            this.ColorDisplayColumn.Width = 72;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(459, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Show";
            // 
            // ShowFieldsComboBox
            // 
            this.ShowFieldsComboBox.FormattingEnabled = true;
            this.ShowFieldsComboBox.Location = new System.Drawing.Point(499, 30);
            this.ShowFieldsComboBox.Name = "ShowFieldsComboBox";
            this.ShowFieldsComboBox.Size = new System.Drawing.Size(108, 21);
            this.ShowFieldsComboBox.TabIndex = 3;
            this.ShowFieldsComboBox.SelectedIndexChanged += new System.EventHandler(this.ShowFieldsComboBox_SelectedIndexChanged);
            // 
            // ColorSelectButton
            // 
            this.ColorSelectButton.Location = new System.Drawing.Point(754, 30);
            this.ColorSelectButton.Name = "ColorSelectButton";
            this.ColorSelectButton.Size = new System.Drawing.Size(75, 23);
            this.ColorSelectButton.TabIndex = 4;
            this.ColorSelectButton.Text = "Select";
            this.ColorSelectButton.UseVisualStyleBackColor = true;
            this.ColorSelectButton.Click += new System.EventHandler(this.ColorSelectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(650, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Background Colour";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label4.Location = new System.Drawing.Point(3, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(323, 52);
            this.label4.TabIndex = 6;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(5, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(325, 26);
            this.label5.TabIndex = 7;
            this.label5.Text = "If you change your mind, these colours can be changed or adjusted\r\nwithin the Lab" +
    "el Editor.";
            // 
            // LabelSetupPart2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ColorSelectButton);
            this.Controls.Add(this.ShowFieldsComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ColorTable);
            this.Controls.Add(this.label1);
            this.Name = "LabelSetupPart2";
            this.Size = new System.Drawing.Size(849, 430);
            this.Load += new System.EventHandler(this.LabelSetupPart2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ColorTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView ColorTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ShowFieldsComboBox;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button ColorSelectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColorDisplayColumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

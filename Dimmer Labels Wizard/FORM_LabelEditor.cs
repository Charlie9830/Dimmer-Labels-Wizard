using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Diagnostics;

namespace Dimmer_Labels_Wizard
{
    public partial class FORM_LabelEditor : Form
    {
        // Currently Selected LabelStrip. Initialized by RackLabelSelect_AfterSelect().
        private LabelStripSelection ActiveLabelStrip;

        public static List<Rectangle> HeaderSelectionBounds = new List<Rectangle>();
        public static List<Rectangle> FooterSelectionBounds = new List<Rectangle>();

        private Graphics CanvasGraphics;
        private Point RenderOrigin;
        private Point CanvasPanelCenter;

        // A list of Lists. 1st Dimension RackTypes. 2nd Dimension Racks.
        private List<List<LabelStrip>> SelectorRackLabels = new List<List<LabelStrip>>();

        // Dictionary to Map User UserLabelSelection to RackLabel Objects.
        private Dictionary<TreeNode,LabelStrip> UserSelectionDict = new Dictionary<TreeNode,LabelStrip>();

        // Dictionary to Map User Combobox FontFamily Selections. Key: Implied ComboBox Index, Value: FontFamily.
        private Dictionary<int, FontFamily> fontFamilyTracking = new Dictionary<int, FontFamily>();

        // List to Hold FontFamily.Name Values.
        private List<string> fontFamilyNames = new List<string>();

        private List<CellSeperator> cellSeperators = new List<CellSeperator>();

        // Array to Hold Quick Accsess Font Sizes.
        private String[] fontSizes = {"","1","2","3","4","5","6","7","8","9","10","11","12","14","16","18","20",
                                     "22","24","26","28","30","32","34","36","38","40","42","44","46","48","50",
                                     "52","54","56","58","60","62","64","66","68","70","72"};

        // User Page Settings
        private PageSettings UserPageSettings = new PageSettings();

        // User Printer Settings
        private PrinterSettings UserPrinterSettings = new PrinterSettings();

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (ActiveLabelStrip != null)
            {
                Render(ActiveLabelStrip.LabelStrip);
            }
        }

        public FORM_LabelEditor()
        {
            InitializeComponent();

            this.printDocument.PrintPage +=
                new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            CanvasPanel.MouseClick += new MouseEventHandler(this.CanvasPanel_MouseClick);

            CanvasContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(this.CanvasContextMenu_ItemClicked);
           
            HeaderTextBox.KeyDown += new KeyEventHandler(this.TextBoxes_KeyDown);
            FooterTopDataTextBox.KeyDown += new KeyEventHandler(this.TextBoxes_KeyDown);
            FooterMiddleDataTextBox.KeyDown += new KeyEventHandler(this.TextBoxes_KeyDown);
            FooterBottomDataTextBox.KeyDown += new KeyEventHandler(this.TextBoxes_KeyDown);

            HeaderFontStyleSelector.FontStyleChanged += new EventHandler(this.HeaderFontStyleSelector_StyleChanged);
            FooterTopFontStyleSelector.FontStyleChanged += new EventHandler(this.FooterTopFontStyleSelector_StyleChanged);
            FooterMiddleFontStyleSelector.FontStyleChanged += new EventHandler(this.FooterMiddleFontStyleSelector_StyleChanged);
            FooterBottomFontStyleSelector.FontStyleChanged += new EventHandler(this.FooterBottomFontStyleSelector_StyleChanged);
        }

        private void FORM_LabelEditor_Load(object sender, EventArgs e)
        {
            // Generate Graphics Object
            CanvasGraphics = CanvasPanel.CreateGraphics();
            CanvasGraphics.PageUnit = GraphicsUnit.Pixel;
            //CanvasGraphics.PageScale = 3.54f;

            RenderOrigin.X = 20;
            RenderOrigin.Y = 10;

            CanvasPanelCenter.X = CanvasPanel.Width / 2;
            CanvasPanelCenter.Y = CanvasPanel.Height / 2;

            CollectRackLabels();
            PopulateRackLabelSelector();
            PopulateQuickAccessFontControls();
        }

        // Collects RackLabels and Adds them to the SelectorRackLabels List.
        private void CollectRackLabels()
        {
            int outputIndex = 0;

            // Copy the Existing list to a Buffer List.
            List<LabelStrip> bufferList = new List<LabelStrip>(Globals.LabelStrips);

            // Sort the Buffer List.
            bufferList.Sort();

            // Add elements to SelectorRackLabels List.
            for (int i = 0; i < bufferList.Count; i++)
            {
                // Dont Run Out of Index.
                if (i + 1 < bufferList.Count)
                {
                    // Do the RackTypes Match?
                    if (bufferList[i].RackUnitType == bufferList[i + 1].RackUnitType)
                    {
                        // Create a new 2nd Dimension List.
                        SelectorRackLabels.Insert(outputIndex, new List<LabelStrip>());

                        // Add all the Racks that reside within that Unit Type
                        for (int j = i; j < bufferList.Count; j++)
                        {
                            // Dont run out of Index.
                            if (j + 1 < bufferList.Count)
                            {
                                // Do the RackUnitTypes match?
                                if (bufferList[j].RackUnitType == bufferList[j + 1].RackUnitType)
                                {
                                    // Add them to the list.
                                    SelectorRackLabels[outputIndex].Add(bufferList[j]);
                                }

                                else
                                {
                                    // Add the Current object if it is in the same Cabinet as the last Object Added.
                                    if (bufferList[j].RackUnitType == SelectorRackLabels[outputIndex].Last<LabelStrip>().RackUnitType)
                                    {
                                        SelectorRackLabels[outputIndex].Add(bufferList[j]);
                                    }

                                    // Reset, Iterate and Break.
                                    outputIndex++;
                                    i = j - 1;
                                    break;
                                }
                            }
                            else
                            {
                                // Add The Current Object Anyway
                                SelectorRackLabels[outputIndex].Add(bufferList[j]);

                                outputIndex++;
                                i = j - 1;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void CanvasContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem selectedItem = e.ClickedItem;

            RenderCellSeperators();
        }

        private void RenderCellSeperators()
        {
            List<Point> availableSplitPoints = ActiveLabelStrip.DetermineCellSplitPoints();

            int splitIndexCounter = 0;
            // Create and Position CellSeperators.
            foreach (var splitPoint in availableSplitPoints)
            {
                cellSeperators.Add(new CellSeperator(splitIndexCounter));

                if (splitPoint.IsEmpty == true)
                {
                    cellSeperators[splitIndexCounter].Enabled = false;
                    cellSeperators[splitIndexCounter].Visible = false;
                }

                else
                {
                    cellSeperators[splitIndexCounter].Parent = CanvasPanel;
                }
                cellSeperators[splitIndexCounter].Location = availableSplitPoints[splitIndexCounter];

                splitIndexCounter++;
            }

            // Wire Up Events.
            foreach (var element in cellSeperators)
            {
                element.CellSeperatorSelectEvent += new CellSeperator.CellSeperatorEventHandler(CellSeperator_Select);
            }
        }

        private void DeRenderCellSeperators()
        {
            // Hide Seperators and Disconnect Events.
            foreach (var element in cellSeperators)
            {
                element.Visible = false;
                element.CellSeperatorSelectEvent -= CellSeperator_Select;
            }

            // Clear the Cell Seperator List.
            cellSeperators.Clear();
        }

        private void CellSeperator_Select(object sender, CellSeperatorSelectEventArgs e)
        {
            ActiveLabelStrip.SelectedSplitIndex = e.SplitIndex;
            FORM_SplitCellDialog splitCellDialog = new FORM_SplitCellDialog(ActiveLabelStrip, e.SplitIndex);
            
            if (splitCellDialog.ShowDialog() == DialogResult.OK)
            {
                DeRenderCellSeperators();
                Render(ActiveLabelStrip.LabelStrip);
            }
        }

       
        // Populate RackLabelSelector Treeview. Add Tracking KeyPairs to UserSelectionDict.
        private void PopulateRackLabelSelector()
        {
            RackLabelSelector.BeginUpdate();

            for (int i = 0; i < SelectorRackLabels.Count; i++)
            {
                List<TreeNode> children = new List<TreeNode>();

                foreach (var element in SelectorRackLabels[i])
                {
                    children.Add(new TreeNode("Rack " + element.RackNumber.ToString()));
                    
                    // Add a tracking Keypair to the Dictionary.
                    UserSelectionDict.Add(children.Last<TreeNode>(), element);
                }

                RackLabelSelector.Nodes.Insert(i, new TreeNode(SelectorRackLabels[i][0].RackUnitType.ToString(),children.ToArray()));
            }

            RackLabelSelector.EndUpdate();
        }

        private void RackLabelSelector_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LabelStrip label = new LabelStrip();
            TreeNode userSelection = RackLabelSelector.SelectedNode;

            if (userSelection.Parent != null)
            {
                UserSelectionDict.TryGetValue(RackLabelSelector.SelectedNode, out label);

                ActiveLabelStrip = new LabelStripSelection();
                ActiveLabelStrip.LabelStrip = label;
                ActiveLabelStrip.ClearSelections();
                Render(label);
                
            }   
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Globals.LabelStrips[0].RenderToPrinter(e.Graphics, new Point(20, 20));
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            printDocument.DefaultPageSettings = UserPageSettings;
            printDocument.PrinterSettings = UserPrinterSettings;
            printDocument.Print();
        }

        private void printSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printSettingsDialog = new PrintDialog();
            printSettingsDialog.PrinterSettings = UserPrinterSettings;
            printSettingsDialog.ShowDialog();
        }

        private void pageSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetup = new PageSetupDialog();
            pageSetup.PageSettings = UserPageSettings;
            pageSetup.PrinterSettings = UserPrinterSettings;

            pageSetup.ShowDialog();
        }

        private void BackgroundColorButton_Click(object sender, EventArgs e)
        {
            backgroundColorDialog.Color = ActiveLabelStrip.LabelStrip.Footers[0].BackgroundColor.Color;

            if (backgroundColorDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var element in ActiveLabelStrip.SelectedFooters)
                {
                        element.Cell.BackgroundColor = new SolidBrush(backgroundColorDialog.Color);   
                }

                foreach (var element in ActiveLabelStrip.SelectedHeaders)
                {
                    foreach (var cell in element.Cells)
                    {
                        cell.BackgroundColor = new SolidBrush(backgroundColorDialog.Color);
                    }
                }

                // Force a Render.
                Render(ActiveLabelStrip.LabelStrip);
            }
            
        }

        // Control Method for RenderToDisplay. Renders to Canvas Panel and collects Header and Cell Outlines.
        private void Render(LabelStrip label)
        {
            if (ActiveLabelStrip != null)
            {
                // Clear the Current Selection Rectangles.
                ActiveLabelStrip.RenderedHeaders.Clear();
                ActiveLabelStrip.RenderedFooters.Clear();

                // Render to Display and Collect Outline Rectangles.
                UserLabelSelection userSelections = label.RenderToDisplay(CanvasGraphics,
                    RenderOrigin, new Size(CanvasPanel.Width, CanvasPanel.Height));

                // Return Objects of RenderToDisplay().
                ActiveLabelStrip.RenderedHeaders.AddRange(userSelections.HeaderSelections);
                ActiveLabelStrip.RenderedFooters.AddRange(userSelections.FooterSelections);

                // Add the Selection Rectangles (Headers and Footer Concatenated).
                ActiveLabelStrip.HeaderStripOutline = new RectangleF(userSelections.HeaderSelections.First().Outline.X,
                    userSelections.HeaderSelections.First().Outline.Y, userSelections.HeaderSelections.Last().Outline.Right -
                    userSelections.HeaderSelections.First().Outline.Left, userSelections.HeaderSelections.First().Outline.Height);

                ActiveLabelStrip.FooterStripOutline = new RectangleF(userSelections.FooterSelections.First().Outline.X,
                    userSelections.FooterSelections.First().Outline.Y, userSelections.FooterSelections.Last().Outline.Right -
                    userSelections.FooterSelections.First().Outline.Left, userSelections.FooterSelections.First().Outline.Height);

                // Render Selection Outlines.
                RenderSelectionOutlines();

                Console.WriteLine("RENDERED");
            }
        }

        private void RenderSelectionOutlines()
        {
            Graphics graphics = CanvasPanel.CreateGraphics();

            // Fill Color. Full Blue. 50% Opacity.
            SolidBrush fillBrush = new SolidBrush(Color.FromArgb(128,0,0,255));

            if (ActiveLabelStrip.SelectedHeaders.Count != 0)
            {
                foreach (var header in ActiveLabelStrip.SelectedHeaders)
                {
                    graphics.FillRectangle(fillBrush, header.Outline);
                }
            }

            if (ActiveLabelStrip.SelectedFooters.Count != 0)
            {
                foreach (var footer in ActiveLabelStrip.SelectedFooters)
                {
                    graphics.FillRectangle(fillBrush, footer.Outline);
                }
            }
        }

        private void CanvasPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // Left Mouse Button Click
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (ActiveLabelStrip != null)
                {
                    // User has selected a Header.
                    if (ActiveLabelStrip.HeaderStripOutline.Contains(e.Location) == true)
                    {
                        ActiveLabelStrip.SelectHeaderCells(e.Location);
                        RenderHeaderCellControls();
                        Render(ActiveLabelStrip.LabelStrip);
                        DeRenderCellSeperators();
                    }

                    // User has Selected a Footer.
                    else if (ActiveLabelStrip.FooterStripOutline.Contains(e.Location) == true)
                    {
                        ActiveLabelStrip.SelectFooterCells(e.Location);
                        Render(ActiveLabelStrip.LabelStrip);
                        RenderFooterCellControls();
                    }

                    // User has Clicked the area outside the Header and Footer Labels.
                    else
                    {
                        ActiveLabelStrip.ClearSelections();

                        if (cellSeperators.Count != 0)
                        {
                            DeRenderCellSeperators();
                        }

                        Render(ActiveLabelStrip.LabelStrip);
                        RenderFooterCellControls();
                        RenderHeaderCellControls();
                    }
                }
            }

            // Right Mouse Button Click
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                CanvasContextMenu.Show(CanvasPanel,e.Location);
            }
        }

        // Renders HeaderCell Data to Textboxes.
        private void RenderHeaderCellControls()
        {
            if (ActiveLabelStrip.SelectedHeaders.Count == 1)
            {
                HeaderTextBox.Text = ActiveLabelStrip.SelectedHeaders.First().Cells.First().Data;
                HeaderFontComboBox.ResetText();
                HeaderFontComboBox.SelectedText = ActiveLabelStrip.SelectedHeaders.First().Cells.First().Font.Name;
                HeaderFontSizeComboBox.ResetText();
                HeaderFontSizeComboBox.SelectedText = ActiveLabelStrip.SelectedHeaders.First().Cells.First().Font.Size.ToString();
                HeaderFontStyleSelector.FontStyle = ActiveLabelStrip.SelectedHeaders.First().Cells.First().Font.Style;
            }

            else if (ActiveLabelStrip.SelectedHeaders.Count > 1)
            {
                // See Footer Version of this if you are confused.
                HeaderCell referenceCell = ActiveLabelStrip.SelectedHeaders.First().Cells.First();

                bool allSameData = ActiveLabelStrip.SelectedHeaders.All(item => item.Cells.First().Data == referenceCell.Data);
                HeaderTextBox.Text = allSameData ? ActiveLabelStrip.SelectedHeaders.First().Cells.First().Data : "*";

                bool allSameFont = ActiveLabelStrip.SelectedHeaders.All(item => item.Cells.First().Font == referenceCell.Font);
                if (allSameFont == true)
                {
                    HeaderFontComboBox.ResetText();
                    HeaderFontComboBox.SelectedText = referenceCell.Font.Name;
                }
                else
                {
                    HeaderFontComboBox.ResetText();
                }

                bool allSameFontStyle = ActiveLabelStrip.SelectedHeaders.All(item => item.Cells.First().Font.Style == referenceCell.Font.Style);
                HeaderFontStyleSelector.FontStyle = allSameFontStyle ? referenceCell.Font.Style : FontStyle.Regular;

                bool allSameFontSize = ActiveLabelStrip.SelectedHeaders.All(item => item.Cells.First().Font.Size == referenceCell.Font.Size);
                if (allSameFontSize == true)
                {
                    HeaderFontSizeComboBox.ResetText();
                    HeaderFontSizeComboBox.SelectedText = referenceCell.Font.Size.ToString();
                }
            }

            // Nothing Selected.
            else
            {
                HeaderFontComboBox.ResetText();
                HeaderFontSizeComboBox.ResetText();
                HeaderFontStyleSelector.FontStyle = FontStyle.Regular;
                HeaderTextBox.Text = "";
            }
        }

        // Renders Footercell Controls.
        private void RenderFooterCellControls()
        {   
            if (ActiveLabelStrip.SelectedFooters.Count == 1)
            {
                FooterTopDataTextBox.Text = ActiveLabelStrip.SelectedFooters.First().Cell.TopData;
                FooterTopFontComboBox.ResetText();
                FooterTopFontComboBox.SelectedText = ActiveLabelStrip.SelectedFooters.First().Cell.TopFont.Name;
                FooterTopSizeComboBox.ResetText();
                FooterTopSizeComboBox.SelectedText = ActiveLabelStrip.SelectedFooters.First().Cell.TopFont.Size.ToString();
                FooterTopFontStyleSelector.FontStyle = ActiveLabelStrip.SelectedFooters.First().Cell.TopFont.Style;

                FooterMiddleDataTextBox.Text = ActiveLabelStrip.SelectedFooters.First().Cell.MiddleData;
                FooterMiddleFontComboBox.ResetText();
                FooterMiddleFontComboBox.SelectedText = ActiveLabelStrip.SelectedFooters.First().Cell.MiddleFont.Name;
                FooterMiddleSizeComboBox.ResetText();
                FooterMiddleSizeComboBox.SelectedText = ActiveLabelStrip.SelectedFooters.First().Cell.MiddleFont.Size.ToString();
                FooterMiddleFontStyleSelector.FontStyle = ActiveLabelStrip.SelectedFooters.First().Cell.MiddleFont.Style;

                FooterBottomDataTextBox.Text = ActiveLabelStrip.SelectedFooters.First().Cell.BottomData;
                FooterBottomFontComboBox.ResetText();
                FooterBottomFontComboBox.SelectedText = ActiveLabelStrip.SelectedFooters.First().Cell.BottomFont.Name;
                FooterBottomSizeComboBox.ResetText();
                FooterBottomSizeComboBox.SelectedText = ActiveLabelStrip.SelectedFooters.First().Cell.BottomFont.Size.ToString();
                FooterBottomFontStyleSelector.FontStyle = ActiveLabelStrip.SelectedFooters.First().Cell.BottomFont.Style;
            }

            else if (ActiveLabelStrip.SelectedFooters.Count > 1)
            {
                // Using Lambda Expressions. Checks if All Cells in Selected Footers Contain the Same Data, Font and Font size
                // If they do. TextBox text is set to First element Data. If not, it is set to "*".

                // Reference Cell to Compare Subsequent Cells to.
                FooterCell referenceCell = ActiveLabelStrip.SelectedFooters.First().Cell;

                // Top
                bool allSameTopData = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.TopData == referenceCell.TopData);
                FooterTopDataTextBox.Text = allSameTopData ? ActiveLabelStrip.SelectedFooters.First().Cell.TopData : "*";

                bool allSameTopFont = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.TopFont == referenceCell.TopFont);
                if (allSameTopFont == true)
                {
                    FooterTopFontComboBox.ResetText();
                    FooterTopFontComboBox.SelectedText = referenceCell.TopFont.Name;
                }
                else
                {
                    FooterTopFontComboBox.ResetText();
                }

                bool allSameTopFontStyle = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.TopFont.Style == referenceCell.TopFont.Style);
                FooterTopFontStyleSelector.FontStyle = allSameTopFontStyle ? referenceCell.TopFont.Style : FontStyle.Regular;

                bool allSameTopFontSize = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.TopFont.Size == referenceCell.TopFont.Size);
                if (allSameTopFontSize == true)
                {
                    FooterTopSizeComboBox.ResetText();
                    FooterTopSizeComboBox.SelectedText = referenceCell.TopFont.Size.ToString();
                }
                else
                {
                    FooterTopSizeComboBox.ResetText();
                }

                // Middle
                bool allSameMiddleData = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.MiddleData == referenceCell.MiddleData);
                FooterMiddleDataTextBox.Text = allSameMiddleData ? ActiveLabelStrip.SelectedFooters.First().Cell.MiddleData : "*";

                bool allSameMiddleFont = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.MiddleFont == referenceCell.MiddleFont);
                if (allSameMiddleFont == true)
                {
                    FooterMiddleFontComboBox.ResetText();
                    FooterMiddleFontComboBox.SelectedText = referenceCell.MiddleFont.Name;
                }
                else
                {
                    FooterMiddleFontComboBox.ResetText();
                }

                bool allSameMiddleFontSize = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.MiddleFont.Size == referenceCell.MiddleFont.Size);
                if (allSameMiddleFontSize == true)
                {
                    FooterMiddleSizeComboBox.ResetText();
                    FooterMiddleSizeComboBox.SelectedText = referenceCell.MiddleFont.Size.ToString();
                }
                else
                {
                    FooterMiddleSizeComboBox.ResetText();
                }

                bool allSameMiddleFontStyle = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.MiddleFont.Style == referenceCell.MiddleFont.Style);
                FooterMiddleFontStyleSelector.FontStyle = allSameMiddleFontStyle ? referenceCell.MiddleFont.Style : FontStyle.Regular;

                // Bottom
                bool allSameBottomData = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.BottomData == referenceCell.BottomData);
                FooterBottomDataTextBox.Text = allSameBottomData ? ActiveLabelStrip.SelectedFooters.First().Cell.BottomData : "*";

                bool allSameBottomFont = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.BottomFont == referenceCell.BottomFont);
                if (allSameBottomFont == true)
                {
                    FooterBottomFontComboBox.ResetText();
                    FooterBottomFontComboBox.SelectedText = referenceCell.BottomFont.Name;
                }
                else
                {
                    FooterBottomFontComboBox.ResetText();
                }

                bool allSameBottomFontSize = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.BottomFont.Size == referenceCell.BottomFont.Size);
                if (allSameBottomFontSize == true)
                {
                    FooterBottomSizeComboBox.ResetText();
                    FooterBottomSizeComboBox.SelectedText = referenceCell.BottomFont.Size.ToString();
                }
                else
                {
                    FooterBottomSizeComboBox.ResetText();
                }

                bool allSameBottomFontStyle = ActiveLabelStrip.SelectedFooters.All(item => item.Cell.BottomFont.Style == referenceCell.BottomFont.Style);
                FooterBottomFontStyleSelector.FontStyle = allSameBottomFontStyle ? referenceCell.BottomFont.Style : FontStyle.Regular;
            }

            // No Current FooterCell UserLabelSelection.
            else
            {
                FooterTopDataTextBox.Text = "";
                FooterTopFontComboBox.ResetText();
                FooterTopSizeComboBox.ResetText();
                FooterTopFontStyleSelector.FontStyle = FontStyle.Regular;

                FooterMiddleDataTextBox.Text = "";
                FooterMiddleFontComboBox.SelectedText = "";
                FooterMiddleFontComboBox.ResetText();
                FooterMiddleSizeComboBox.ResetText();
                FooterMiddleFontStyleSelector.FontStyle = FontStyle.Regular;

                FooterBottomDataTextBox.Text = "";
                FooterBottomFontComboBox.ResetText();
                FooterBottomSizeComboBox.ResetText();
                FooterBottomFontStyleSelector.FontStyle = FontStyle.Regular;
            }
        }

        private void UpdateHeaderCellData()
        {
            if (HeaderTextBox.Text != "" && HeaderTextBox.Text != "*")
            {
                foreach (var element in ActiveLabelStrip.SelectedHeaders)
                {
                    foreach (var cell in element.Cells)
                    {
                        cell.Data = HeaderTextBox.Text;
                    }
                }
            }
        }

        private void UpdateFooterCellData()
        {
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                if (FooterTopDataTextBox.Text != "" && FooterTopDataTextBox.Text != "*")
                {
                    element.Cell.TopData = FooterTopDataTextBox.Text;
                }

                if (FooterMiddleDataTextBox.Text != "" && FooterMiddleDataTextBox.Text != "*")
                {
                    element.Cell.MiddleData = FooterMiddleDataTextBox.Text;
                }

                if (FooterBottomDataTextBox.Text != "" && FooterBottomDataTextBox.Text != "*")
                {
                    element.Cell.BottomData = FooterBottomDataTextBox.Text;
                }
            }
        }


        private void PopulateQuickAccessFontControls()
        {
            int fontIndex = 0;
            foreach (var fontFamily in FontFamily.Families)
            {
                fontFamilyTracking.Add(fontIndex, fontFamily);
                fontFamilyNames.Add(fontFamily.Name);
                fontIndex++;
            }

            // Populate Quick Access Font Combo Boxes.
            HeaderFontComboBox.Items.AddRange(fontFamilyNames.ToArray());

            FooterTopFontComboBox.Items.AddRange(fontFamilyNames.ToArray());
            FooterMiddleFontComboBox.Items.AddRange(fontFamilyNames.ToArray());
            FooterBottomFontComboBox.Items.AddRange(fontFamilyNames.ToArray());

            // Size Boxes
            HeaderFontSizeComboBox.Items.AddRange(fontSizes);

            FooterTopSizeComboBox.Items.AddRange(fontSizes);
            FooterMiddleSizeComboBox.Items.AddRange(fontSizes);
            FooterBottomSizeComboBox.Items.AddRange(fontSizes);
        }
        #region Text, Font, Size Selection/TextChanged Event Handlers

        // Event Handler Subscribed to All Three Footer Text Boxes and the Header Text Box.
        private void TextBoxes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateHeaderCellData();
                UpdateFooterCellData();

                // Force a Render
                Render(ActiveLabelStrip.LabelStrip);
            }

            
        }

        private void HeaderFontComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedHeaders)
            {
                foreach (var cell in element.Cells)
                {
                    cell.Font = new Font(fontFamilyTracking[HeaderFontComboBox.SelectedIndex], cell.Font.Size,
                           cell.Font.Style);
                }
            }

            // Force a Render.
            Render(ActiveLabelStrip.LabelStrip);
        }

        private void HeaderFontSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (String)HeaderFontSizeComboBox.Items[HeaderFontSizeComboBox.SelectedIndex];

            if (selectedValue != "" && selectedValue != "*")
            {
                float fontSize;
                float.TryParse(selectedValue, out fontSize);

                foreach (var element in ActiveLabelStrip.SelectedHeaders)
                {
                    foreach (var cell in element.Cells)
                    {
                        cell.Font = new Font(cell.Font.FontFamily, fontSize, cell.Font.Style);
                    }
                }

                // Force Render.
                Render(ActiveLabelStrip.LabelStrip);
            }
        }

        private void HeaderFontStyleSelector_StyleChanged(object sender, EventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedHeaders)
            {
                foreach (var cell in element.Cells)
                {
                    cell.Font = new Font(cell.Font, HeaderFontStyleSelector.FontStyle);
                }
            }

            // Force Render.
            Render(ActiveLabelStrip.LabelStrip);
        }

        private void FooterTopFontComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.Cell.TopFont = new Font(fontFamilyTracking[FooterTopFontComboBox.SelectedIndex], element.Cell.TopFont.Size,
                       element.Cell.TopFont.Style);

            }

            // Force Render
            Render(ActiveLabelStrip.LabelStrip);
        }

        private void FooterTopSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (String)FooterTopSizeComboBox.Items[FooterTopSizeComboBox.SelectedIndex];

            if (selectedValue != "" && selectedValue != "*")
            {
                float fontSize;
                float.TryParse(selectedValue, out fontSize);

                foreach (var element in ActiveLabelStrip.SelectedFooters)
                {
                    element.Cell.TopFont = new Font(element.Cell.TopFont.FontFamily, fontSize, element.Cell.TopFont.Style);
                }

                // Force Render.
                Render(ActiveLabelStrip.LabelStrip);
            }
        }

        private void FooterTopFontStyleSelector_StyleChanged(object sender, EventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.Cell.TopFont = new Font(element.Cell.TopFont, FooterTopFontStyleSelector.FontStyle);
            }

            // Force Render.
            Render(ActiveLabelStrip.LabelStrip);
        }

        private void FooterMiddleFontComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.Cell.MiddleFont = new Font(fontFamilyTracking[FooterMiddleFontComboBox.SelectedIndex], element.Cell.MiddleFont.Size,
                       element.Cell.MiddleFont.Style);

            }

            // Force Render
            Render(ActiveLabelStrip.LabelStrip);
        }

        private void FooterMiddleSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (String)FooterMiddleSizeComboBox.Items[FooterMiddleSizeComboBox.SelectedIndex];

            if (selectedValue != "" && selectedValue != "*")
            {
                float fontSize;
                float.TryParse(selectedValue, out fontSize);

                foreach (var element in ActiveLabelStrip.SelectedFooters)
                {
                    element.Cell.MiddleFont = new Font(element.Cell.MiddleFont.FontFamily, fontSize, element.Cell.MiddleFont.Style);
                }

                // Force Render.
                Render(ActiveLabelStrip.LabelStrip);
            }
        }

        private void FooterMiddleFontStyleSelector_StyleChanged(object sender, EventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.Cell.MiddleFont = new Font(element.Cell.MiddleFont, FooterMiddleFontStyleSelector.FontStyle);
            }

            // Force Render.
            Render(ActiveLabelStrip.LabelStrip);
        }

        private void FooterBottomFontComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.Cell.BottomFont = new Font(fontFamilyTracking[FooterBottomFontComboBox.SelectedIndex], element.Cell.BottomFont.Size,
                       element.Cell.BottomFont.Style);

            }

            // Force Render
            Render(ActiveLabelStrip.LabelStrip);
        }

        private void FooterBottomSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (String)FooterBottomSizeComboBox.Items[FooterBottomSizeComboBox.SelectedIndex];

            if (selectedValue != "" && selectedValue != "*")
            {
                float fontSize;
                float.TryParse(selectedValue, out fontSize);

                foreach (var element in ActiveLabelStrip.SelectedFooters)
                {
                    element.Cell.BottomFont = new Font(element.Cell.BottomFont.FontFamily, fontSize, element.Cell.BottomFont.Style);
                }

                // Force Render.
                Render(ActiveLabelStrip.LabelStrip);
            }
        }

        private void FooterBottomFontStyleSelector_StyleChanged(object sender, EventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.Cell.BottomFont = new Font(element.Cell.BottomFont, FooterBottomFontStyleSelector.FontStyle);
            }

            // Force Render.
            Render(ActiveLabelStrip.LabelStrip);
        }
        #endregion

        #region Performance Analysis

        private void PerformanceTestRender()
        {
            Stopwatch stopWatch = new Stopwatch();

            List<long> renderTimes = new List<long>();
            int externalCounter = 0;

            for (int counter = 0; counter <= 500; counter++)
            {
                stopWatch.Start();
                Render(ActiveLabelStrip.LabelStrip);
                stopWatch.Stop();
                renderTimes.Add(stopWatch.ElapsedMilliseconds);
                externalCounter++;
            }

            Console.WriteLine("==========");
            Console.WriteLine("Average Render Method Execution Time. {0} Times.: {1}", renderTimes.Average(),externalCounter);
            Console.WriteLine("==========");
        }

        private void PerformanceTestButton_Click(object sender, EventArgs e)
        {
            PerformanceTestRender();
        }

        #endregion

        private void DebugButton_Click(object sender, EventArgs e)
        {
            CenterLabelStrips();

            Render(ActiveLabelStrip.LabelStrip);
        }

        private void MagnifyPlusButton_Click(object sender, EventArgs e)
        {
            CanvasGraphics.ScaleTransform(1.1f, 1.1f);
            CanvasGraphics.TranslateTransform(2f, 2f);

            Render(ActiveLabelStrip.LabelStrip);
        }

        private void MagnifyMinusButton_Click(object sender, EventArgs e)
        {
            CanvasGraphics.ScaleTransform(0.9f, 0.9f);

            Render(ActiveLabelStrip.LabelStrip);
        }

        private void CenterLabelStrips()
        {

            int labelStripWidth = (int)Math.Round(ActiveLabelStrip.HeaderStripOutline.Width);
            RenderOrigin.X = (CanvasPanel.Width - labelStripWidth) / 2;
        }
    }
}

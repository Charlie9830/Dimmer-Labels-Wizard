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

namespace Dimmer_Labels_Wizard
{
    public partial class FORM_LabelEditor : Form
    {
        // Currently Selected LabelStrip.
        private LabelStripSelection ActiveLabelStrip = new LabelStripSelection();

        public static List<Rectangle> HeaderSelectionBounds = new List<Rectangle>();
        public static List<Rectangle> FooterSelectionBounds = new List<Rectangle>();

        // A list of Lists. 1st Dimension RackTypes. 2nd Dimension Racks.
        private List<List<LabelStrip>> SelectorRackLabels = new List<List<LabelStrip>>();

        // Dictionary to Map User UserLabelSelection to RackLabel Objects.
        private Dictionary<TreeNode,LabelStrip> UserSelectionDict = new Dictionary<TreeNode,LabelStrip>();

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

        public FORM_LabelEditor()
        {
            InitializeComponent();

            this.printDocument.PrintPage +=
                new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);

            CanvasPanel.MouseClick += new MouseEventHandler(this.CanvasPanel_MouseClick);
        }

        private void FORM_LabelEditor_Load(object sender, EventArgs e)
        {
            CollectRackLabels();
            PopulateRackLabelSelector();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Break");
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
                foreach (var element in ActiveLabelStrip.SelectedFooterCells)
                {
                    foreach (var footer in element.Footers)
                    {
                        footer.BackgroundColor = new SolidBrush(backgroundColorDialog.Color);
                    }
                }

                // Force a Render.
                Render(ActiveLabelStrip.LabelStrip);
            }
            
        }

        // Control Method for RenderToDisplay. Renders to Canvas Panel and collects Header and Footer Outlines.
        private void Render(LabelStrip label)
        {
            if (ActiveLabelStrip != null)
            {
                // Clear the Current Selection Rectangles.
                ActiveLabelStrip.RenderedHeaders.Clear();
                ActiveLabelStrip.RenderedFooters.Clear();

                // Render to Display and Collect Outline Rectangles.
                UserLabelSelection userSelections = label.RenderToDisplay(CanvasPanel.CreateGraphics(),
                    new Point(20, 10), new Size(CanvasPanel.Width, CanvasPanel.Height));

                // Return type of RenderToDisplay is Jagged Array. Headers in Index 0. Footers index 1.
                ActiveLabelStrip.RenderedHeaders.AddRange(userSelections.HeaderSelections);
                ActiveLabelStrip.RenderedFooters.AddRange(userSelections.FooterSelections);

                Console.WriteLine("RENDERED");
            }
        }

        private void CanvasPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (ActiveLabelStrip != null)
            {
                // User has selected a Header.
                if (ActiveLabelStrip.UpdateSelectedHeaderCells(e.Location) == true)
                {
                    // Populate headerCell Cell Text Boxes
                    Render(ActiveLabelStrip.LabelStrip);
                }

                // User has Selected a Footer.
                else if (ActiveLabelStrip.UpdateSelectedFooterCells(e.Location) == true)
                {
                    //PopulateFooterCellTextBoxes();
                    Render(ActiveLabelStrip.LabelStrip);
                }

                else
                {
                    // User has clicked on Canvas outside Selection Area. 
                    // UpdateSelectedHeaderCells() and UpdateSelectedFooterCells will Clear their own
                    // respective Selections. Only action required is a Forced Render.
                    Render(ActiveLabelStrip.LabelStrip);
                }
            }

            
        }

        // Renders Footercell data to Textboxes.
        private void PopulateFooterCellTextBoxes()
        {   
            if (ActiveLabelStrip.SelectedFooterCells.Count == 1)
            {
                //HeaderTextBox.Text =
                FooterMiddleDataTextBox.Text = ActiveLabelStrip.SelectedFooterCells.First().Footers.First().MiddleData;
                InstrumentNameTextBox.Text = ActiveLabelStrip.SelectedFooterCells.First().Footers.First().BottomData;
            }

            else if (ActiveLabelStrip.SelectedFooterCells.Count > 1)
            {
                FooterTopDataTextBox.Text = "*";
                FooterMiddleDataTextBox.Text = "*";
                FooterMiddleDataTextBox.Text = "*";
                InstrumentNameTextBox.Text = "*";
            }

            // No Current FooterCell UserLabelSelection.
            else
            {
                FooterTopDataTextBox.Text = "";
                FooterMiddleDataTextBox.Text = "";
                InstrumentNameTextBox.Text = "";
            }
        }

        private void HeaderFontButton_Click(object sender, EventArgs e)
        {
            // Font Choosing Dialog.
        }

        private void ChannelFontButton_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var element in ActiveLabelStrip.SelectedFooterCells)
                {
                    element.Footers.First().MiddleFont = fontDialog.Font;
                }
            }

            // Force Render
            Render(ActiveLabelStrip.LabelStrip);
            
        }

        private void InstrumentNameFontButton_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var element in ActiveLabelStrip.SelectedFooterCells)
                {
                    foreach (var footer in element.Footers)
                    {
                        footer.BottomFont = fontDialog.Font;
                    }
                }
            }

            // Force Render
            Render(ActiveLabelStrip.LabelStrip);
            
        }

        private void LowerPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

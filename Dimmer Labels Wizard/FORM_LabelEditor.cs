using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dimmer_Labels_Wizard
{
    public partial class FORM_LabelEditor : Form
    {
        // A list of Lists. 1st Dimension Cabinets. 2nd Dimension Racks.
        private List<List<LabelStrip>> SelectorRackLabels = new List<List<LabelStrip>>();

        // Dictionary to Map User Selections to RackLabel Objects.
        private Dictionary<TreeNode,LabelStrip> UserSelectionDict = new Dictionary<TreeNode,LabelStrip>();

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
                    // Do the cabinet numbers match?
                    if (bufferList[i].CabinetNumber == bufferList[i + 1].CabinetNumber)
                    {
                        // Create a new 2nd Dimension List.
                        SelectorRackLabels.Insert(outputIndex, new List<LabelStrip>());

                        // Add all the Racks that reside within that cabinet.
                        for (int j = i; j < bufferList.Count; j++)
                        {
                            // Dont run out of Index.
                            if (j + 1 < bufferList.Count)
                            {
                                // Do the Rack numbers match?
                                if (bufferList[j].CabinetNumber == bufferList[j + 1].CabinetNumber)
                                {
                                    // Add them to the list.
                                    SelectorRackLabels[outputIndex].Add(bufferList[j]);
                                }

                                else
                                {
                                    // Add the Current object if it is in the same Cabinet as the last Object Added.
                                    if (bufferList[j].CabinetNumber == SelectorRackLabels[outputIndex].Last<LabelStrip>().CabinetNumber)
                                    {
                                        SelectorRackLabels[outputIndex].Add(bufferList[j]);
                                    }

                                    // Reset, Iterate and Break.
                                    outputIndex++;
                                    i = j - 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public FORM_LabelEditor()
        {
            InitializeComponent();
        }

        private void FORM_LabelEditor_Load(object sender, EventArgs e)
        {
            CollectRackLabels();
            PopulateRackLabelSelector();
        }


        private void RenderLabelContainer()
        {
            System.Drawing.Graphics graphics = this.CreateGraphics();
            SolidBrush foregroundColour = new SolidBrush(Color.Black);
            SolidBrush backgroundColour = new SolidBrush(Color.LightGray);
            Pen foregroundPen = new Pen(foregroundColour);
            Pen backgroundPen = new Pen(backgroundColour);
            Rectangle container = new Rectangle(10, 10, 1000, 300);

            graphics.DrawRectangle(foregroundPen, container);
            graphics.FillRectangle(backgroundColour, container);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            RenderLabelContainer();
            
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

                RackLabelSelector.Nodes.Insert(i, new TreeNode("Cab" + SelectorRackLabels[i][0].CabinetNumber,children.ToArray()));
            }

            RackLabelSelector.EndUpdate();
        }

        private void RackLabelSelector_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LabelStrip Label = new LabelStrip();
            TreeNode UserSelection = RackLabelSelector.SelectedNode;

            if (UserSelection.Parent != null)
            {
                UserSelectionDict.TryGetValue(RackLabelSelector.SelectedNode, out Label);
                Label.Render(this.CreateGraphics(), new Point(20, 20));
            }

        }
    }
}

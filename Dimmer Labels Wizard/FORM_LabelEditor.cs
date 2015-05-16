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
        // A list of Lists. 1st Dimension RackTypes. 2nd Dimension Racks.
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
                                // Do the RackUnitTYpes match?
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
        }

        private void FORM_LabelEditor_Load(object sender, EventArgs e)
        {
            CollectRackLabels();
            PopulateRackLabelSelector();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Do Nothing!
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

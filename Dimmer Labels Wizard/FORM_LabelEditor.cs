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
        private List<List<RackLabel>> SelectorRackLabels = new List<List<RackLabel>>();

        // Dictionary to Map User Selections to RackLabel Objects.
        private Dictionary<TreeNode,RackLabel> UserSelectionDict = new Dictionary<TreeNode,RackLabel>();

        // Collects RackLabels and Adds them to the SelectorRackLabels List.
        private void CollectRackLabels()
        {
            int output_index = 0;

            // Copy the Existing list to a Buffer List.
            List<RackLabel> BufferList = new List<RackLabel>(Globals.RackLabels);

            // Sort the Buffer List.
            BufferList.Sort();

            // Add elements to SelectorRackLabels List.
            for (int i = 0; i < BufferList.Count; i++)
            {
                // Dont Run Out of Index.
                if (i + 1 < BufferList.Count)
                {
                    // Do the cabinet numbers match?
                    if (BufferList[i].cabinet_number == BufferList[i + 1].cabinet_number)
                    {
                        // Create a new 2nd Dimension List.
                        SelectorRackLabels.Insert(output_index, new List<RackLabel>());

                        // Add all the Racks that reside within that cabinet.
                        for (int j = i; j < BufferList.Count; j++)
                        {
                            // Dont run out of Index.
                            if (j + 1 < BufferList.Count)
                            {
                                // Do the Rack numbers match?
                                if (BufferList[j].cabinet_number == BufferList[j + 1].cabinet_number)
                                {
                                    // Add them to the list.
                                    SelectorRackLabels[output_index].Add(BufferList[j]);
                                }

                                else
                                {
                                    // Add the Current object if it is in the same Cabinet as the last Object Added.
                                    if (BufferList[j].cabinet_number == SelectorRackLabels[output_index].Last<RackLabel>().cabinet_number)
                                    {
                                        SelectorRackLabels[output_index].Add(BufferList[j]);
                                    }

                                    // Reset, Iterate and Break.
                                    output_index++;
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
            SolidBrush ForegroundColour = new SolidBrush(Color.Black);
            SolidBrush BackgroundColour = new SolidBrush(Color.LightGray);
            Pen ForegroundPen = new Pen(ForegroundColour);
            Pen BackgroundPen = new Pen(BackgroundColour);
            Rectangle ContainerRec = new Rectangle(10, 10, 1000, 300);

            graphics.DrawRectangle(ForegroundPen, ContainerRec);
            graphics.FillRectangle(BackgroundColour, ContainerRec);
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
                List<TreeNode> Children = new List<TreeNode>();

                foreach (var element in SelectorRackLabels[i])
                {
                    Children.Add(new TreeNode("Rack " + element.rack_number.ToString()));
                    
                    // Add a tracking Keypair to the Dictionary.
                    UserSelectionDict.Add(Children.Last<TreeNode>(), element);
                }

                RackLabelSelector.Nodes.Insert(i, new TreeNode("Cab" + SelectorRackLabels[i][0].cabinet_number,Children.ToArray()));
            }

            RackLabelSelector.EndUpdate();
        }

        private void RackLabelSelector_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RackLabel Label = new RackLabel();
            TreeNode UserSelection = RackLabelSelector.SelectedNode;

            if (UserSelection.Parent != null)
            {
                UserSelectionDict.TryGetValue(RackLabelSelector.SelectedNode, out Label);
                Label.Render(this.CreateGraphics(), new Point(20, 20));
            }

        }
    }
}

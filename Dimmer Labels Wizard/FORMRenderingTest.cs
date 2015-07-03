using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Dimmer_Labels_Wizard
{
    public partial class FORMRenderingTest : Form
    {
        private LabelCanvas labelCanvas = new LabelCanvas();

        public FORMRenderingTest()
        {
            InitializeComponent();
            elementHost.Child = labelCanvas;
        }

        private void FORMRenderingTest_Load(object sender, EventArgs e)
        {
            HookupEvents();
        }

        private void HookupEvents()
        {
            foreach (var element in labelCanvas.Outlines)
            {
                element.MouseDown += element_MouseDown;
            }
        }

        void element_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border outline = (Border) sender;
            int index = labelCanvas.Outlines.IndexOf(outline);
            Console.WriteLine("Index {0}",index);
        }

        
    }
}

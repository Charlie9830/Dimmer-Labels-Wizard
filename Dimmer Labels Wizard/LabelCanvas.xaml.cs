using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace Dimmer_Labels_Wizard
{
    /// <summary>
    /// Interaction logic for LabelCanvas.xaml
    /// </summary>
    public partial class LabelCanvas : UserControl
    {
        public List<Border> Outlines = new List<Border>();
        public List<Canvas> textCanvases = new List<Canvas>();

        public LabelCanvas()
        {
            InitializeComponent();

            Canvas.Background = Brushes.White;
        }
    }
}

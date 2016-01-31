using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for DrawingTest.xaml
    /// </summary>
    public partial class DrawingTest : Window
    {
        public DrawingTest()
        {
            InitializeComponent();
            var viewModel = new DrawingTestViewModel(labelCell);
            DataContext = viewModel;

            DimmerDistroUnit unit = new DimmerDistroUnit();
            unit.ChannelNumber = "131";
            unit.Position = "Position";
            unit.InstrumentName = "Polaris";
            unit.MulticoreName = "LX2A";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as DrawingTestViewModel;
            labelCell.Rows.Add(new CellRow(labelCell));
            viewModel.SelectedRow = labelCell.Rows.Last();
        }

        private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (labelCell.Rows.Count != 0)
            {
                labelCell.Rows.Remove(labelCell.Rows.Last());

                if (labelCell.Rows.Count == 0)
                {
                    var viewModel = DataContext as DrawingTestViewModel;
                    viewModel.SelectedRow = null;
                }

                else
                {
                    var viewModel = DataContext as DrawingTestViewModel;
                    viewModel.SelectedRow = labelCell.Rows.Last();
                }
            }
        }

        private void SingleFieldCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            if (checkBox.IsChecked == true)
            {
                labelCell.CellDataMode = CellDataMode.SingleField;
            }

            else
            {
                labelCell.CellDataMode = CellDataMode.MultiField;
            }
        }

        private void StripModeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (labelStrip.StripMode == LabelStripMode.Dual)
            {
                labelStrip.StripMode = LabelStripMode.Single;
            }

            else
            {
                labelStrip.StripMode = LabelStripMode.Dual;
            }
        }

        private void LineweightPlusButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LineweightMinusButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void InitTestButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

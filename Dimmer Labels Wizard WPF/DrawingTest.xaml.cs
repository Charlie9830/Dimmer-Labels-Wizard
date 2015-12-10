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

            labelCell.PreviousReference = unit;

            // Cascading Rows Test Initialization.
            labelCell.Rows.Add(new CellRow(labelCell));
            //labelCell.Rows.Add(new CellRow(labelCell));
            //labelCell.Rows.Add(new CellRow(labelCell));

            viewModel.SelectedRow = labelCell.Rows.First();

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

        private void PrintDataModelButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as DrawingTestViewModel;

            if (viewModel.SelectedRow != null)
            {
                Console.WriteLine(viewModel.SelectedRow.DataField.ToString() + " : " +
                    labelCell.PreviousReference.GetData(viewModel.SelectedRow.DataField));
            }
            else
            {
                Console.WriteLine("No Row Selected");
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
                labelCell.CellDataMode = CellDataMode.MixedField;
            }
        }
    }
}

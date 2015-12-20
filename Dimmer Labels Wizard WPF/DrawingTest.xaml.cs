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

        private void AddCellButton_Click(object sender, RoutedEventArgs e)
        {
            labelStrip.UpperCells.Add(new LabelCell());
        }

        private void RemoveCellButton_Click(object sender, RoutedEventArgs e)
        {
            if (labelStrip.UpperCells.Count > 0)
            {
                labelStrip.UpperCells.RemoveAt(labelStrip.UpperCells.Count - 1);
            }
        }

        private void RemoveLowerCellButton_Click(object sender, RoutedEventArgs e)
        {
            if (labelStrip.LowerCells.Count > 0)
            {
                labelStrip.LowerCells.RemoveAt(labelStrip.LowerCells.Count - 1);
            }
        }

        private void AddLowerCellButton_Click(object sender, RoutedEventArgs e)
        {
            labelStrip.LowerCells.Add(new LabelCell());
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
            foreach (var element in labelStrip.UpperCells)
            {
                element.LeftWeight += 1;
                element.TopWeight += 1;
                element.RightWeight += 1;
                element.BottomWeight += 1;
            }
        }

        private void LineweightMinusButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in labelStrip.UpperCells)
            {
                element.LeftWeight -= 1;
                element.TopWeight -= 1;
                element.RightWeight -= 1;
                element.BottomWeight -= 1;
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if (labelStrip.UpperCells.Count >= 3)
            {
                List<LabelCell> mergeList = new List<LabelCell>();
                mergeList.Add(labelStrip.UpperCells[1]);
                mergeList.Add(labelStrip.UpperCells[2]);

                labelStrip.Merge(labelStrip.UpperCells[0], mergeList);
            }
        }

        private void InitTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (labelStrip.UpperCells.Count >= 2)
            {
                labelStrip.UpperCells[0].Rows.Add(new CellRow(labelStrip.UpperCells[0]));
                labelStrip.UpperCells[1].Rows.Add(new CellRow(labelStrip.UpperCells[1]));

                labelStrip.UpperCells[0].Rows[0].Data = "Test Data";
                labelStrip.UpperCells[1].Rows[0].Data = "Test Data";

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for LabelRangeControl.xaml
    /// </summary>
    public partial class DimmerRangeSelectorHost : UserControl
    {
        public DimmerRangeSelectorHost()
        {
            InitializeComponent();
        }

        #region Dependancy Properties
        public List<DimmerRange> DimmerRanges
        {
            get { return (List<DimmerRange>)GetValue(DimmerRangesProperty); }
            set { SetValue(DimmerRangesProperty, value); }
        }

        public static readonly DependencyProperty DimmerRangesProperty =
            DependencyProperty.Register("DimmerRanges", typeof(List<DimmerRange>), typeof(DimmerRangeSelectorHost), 
                new FrameworkPropertyMetadata(new List<DimmerRange>(),FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public void UpdateDimmerRanges()
        {
            List<DimmerRange> buffer = new List<DimmerRange>();
            foreach (var element in SelectorsPanel.Children)
            {
                var selector = element as DimmerRangeSelector;
                var selectorViewModel = selector.DataContext as DimmerRangeSelectorViewModel;
                buffer.Add(selectorViewModel.DimmerRange);
            }

            SetValue(DimmerRangesProperty, buffer);
        }

        #endregion

        #region General Methods
        #endregion

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            SelectorsPanel.Children.Add(new DimmerRangeSelector());

            ShowHideStartupTip();
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectorsPanel.Children.Count > 0)
            {
                SelectorsPanel.Children.Remove(SelectorsPanel.Children[SelectorsPanel.Children.Count - 1]);
            }

            ShowHideStartupTip();
        }

        protected void ShowHideStartupTip()
        {
            if (SelectorsPanel.Children.Count == 0)
            {
                StartupTip.Visibility = Visibility.Visible;
            }

            else
            {
                StartupTip.Visibility = Visibility.Hidden;
            }
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for DistroRangeSelectorHost.xaml
    /// </summary>
    public partial class DistroRangeSelectorHost : UserControl
    {
        public DistroRangeSelectorHost()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public List<DistroRange> DistroRanges
        {
            get { return (List<DistroRange>)GetValue(DistroRangesProperty); }
            set { SetValue(DistroRangesProperty, value); }
        }

        public static readonly DependencyProperty DistroRangesProperty =
            DependencyProperty.Register("DistroRanges", typeof(List<DistroRange>), typeof(DistroRangeSelectorHost),
                new FrameworkPropertyMetadata(new List<DistroRange>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public void UpdateDistroRanges()
        {
            List<DistroRange> buffer = new List<DistroRange>();
            foreach (var element in SelectorsPanel.Children)
            {
                var selector = element as DistroRangeSelector;
                var selectorViewModel = selector.DataContext as DistroRangeSelectorViewModel;
                buffer.Add(selectorViewModel.DistroRange);
            }

            SetValue(DistroRangesProperty, buffer);
        }
        #endregion

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            SelectorsPanel.Children.Add(new DistroRangeSelector());
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

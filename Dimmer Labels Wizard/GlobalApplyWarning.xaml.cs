﻿using System;
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

namespace Dimmer_Labels_Wizard
{
    /// <summary>
    /// Interaction logic for GlobalApplyWarning.xaml
    /// </summary>
    public partial class GlobalApplyWarning : Window
    {
        public bool? DontShowDialogAgain = false;

        public GlobalApplyWarning()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            DontShowDialogAgain = ShowAgainCheckBox.IsChecked;
            this.Close();
        }
    }
}

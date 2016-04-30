using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class Globals
    {
        // Standard FontSizes.
        public static double[] StandardFontSizes = new double[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        // List to hold DimDistroUnit Objects
        // public static List<DimmerDistroUnit> DimmerDistroUnits = new List<DimmerDistroUnit>();

        // List to Hold StripData Objects.
        //public static ObservableCollection<Strip> Strips = new ObservableCollection<Strip>();

        // By Default, this is contained within the Templates Collection.
        //public static LabelStripTemplate DefaultTemplate;

        // Template Storage.
        //public static ObservableCollection<LabelStripTemplate> Templates = 
        //    new ObservableCollection<LabelStripTemplate>();

        // Color Storage. (Dimmer, Color).
        public static Dictionary<int, Color> DimmerLabelColors = new Dictionary<int, Color>();
        public static Dictionary<int, Color> DistroLabelColors = new Dictionary<int, Color>();

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Provides Grouping functionality to LabelColorManager.
    /// </summary>
    public class UnitGroup : ViewModelBase
    {
        #region Binding Source Properties

        protected string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;

                    // Notify.
                    OnPropertyChanged(nameof(Name));
                }
            }
        }


        protected IEnumerable<DimmerDistroUnit> _Units;

        public IEnumerable<DimmerDistroUnit> Units
        {
            get { return _Units; }
            set
            {
                if (_Units != value)
                {
                    _Units = value;

                    // Notify.
                    OnPropertyChanged(nameof(Units));
                }
            }
        }


        protected Color _Color;

        public Color Color
        {
            get { return _Color; }
            set
            {
                if (_Color != value)
                {
                    _Color = value;

                    // Notify.
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        public Brush DisplayedBrush
        {
            get
            {
                return GetDisplayedBrush();
            }
        }
        #endregion

        #region Methods
        protected IEnumerable<Color> GetColorRange()
        {
            var colorRange = new List<Color>();

            foreach (var element in Units)
            {
                if (colorRange.Contains(element.LabelColor) == false)
                {
                    colorRange.Add(element.LabelColor);
                }
            }

            return colorRange;
        }

        protected Brush GetDisplayedBrush()
        {
            var colorRange = GetColorRange();

            if (colorRange.Count() > 0)
            {
                return new SolidColorBrush(colorRange.First());
            }

            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        #endregion
    }
}

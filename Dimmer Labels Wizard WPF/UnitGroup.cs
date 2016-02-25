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

                    // Set Color.
                    _UnitGroupColor = GetColor();


                    // Notify.
                    OnPropertyChanged(nameof(Units));
                    OnPropertyChanged(nameof(UnitGroupColor));
                }
            }
        }

        protected Color _UnitGroupColor = Colors.White;

        public Color UnitGroupColor
        {
            get
            {
                return _UnitGroupColor;
            }

            set
            {
                if (value != _UnitGroupColor)
                {
                    _UnitGroupColor = value;

                    // Update Model.
                    SetColor(value);

                    // Notify.
                    OnPropertyChanged(nameof(UnitGroupColor));
                }
                
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

        protected Color GetColor()
        {
            var colorRange = GetColorRange();

            if (colorRange.Count() > 0)
            {
                return colorRange.First();
            }

            else
            {
                return Colors.White;
            }
        }

        protected void SetColor(Color desiredColor)
        {
            foreach (var element in Units)
            {
                element.LabelColor = desiredColor;
            }
        }

        #endregion
    }
}

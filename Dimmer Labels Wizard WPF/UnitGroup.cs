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
                    OnPropertyChanged(nameof(DisplayedBrush));
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
                    OnPropertyChanged(nameof(DisplayedBrush));
                    OnPropertyChanged(nameof(UnitGroupColor));
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



        protected bool _IsSelected = false;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsSelected));
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

        protected Brush GetDisplayedBrush()
        {
            var colorRange = GetColorRange().ToList();

            if (colorRange.Count() > 0)
            {
                // Test for Color Range Equality.
                if (colorRange.TrueForAll(item => item == colorRange.First()))
                {
                    // Colors are all Equal, return Single Color.
                    return new SolidColorBrush(colorRange.First());
                }

                else
                {
                    // Colors arent Equal, return Gradient.
                    // Get Gradient Stop Range.
                    var gradientOffsets = ComputeGradientOffsets(colorRange.Count);
                    var gradientOffsetsEnumerator = gradientOffsets.GetEnumerator();

                    var gradientStops = new List<GradientStop>();

                    // Assign 1 Color per 2 GradientStops and Apply to GradientStop Object.
                    foreach (var element in colorRange)
                    {
                        if (gradientOffsetsEnumerator.MoveNext())
                        {
                            gradientStops.Add(new GradientStop(element, gradientOffsetsEnumerator.Current));

                            if (gradientOffsetsEnumerator.MoveNext())
                            {
                                gradientStops.Add(new GradientStop(element, gradientOffsetsEnumerator.Current));
                            }
                        }
                    }

                    return new LinearGradientBrush(new GradientStopCollection(gradientStops), 0);
                }
            }

            else
            {
                // ColorRange is Empty.
                return null;
            }
        }

        protected IEnumerable<double> ComputeGradientOffsets(int colorCount)
        {
            double gradientDivision = 1d / colorCount;
            double currentDivision;

            var offsets = new List<double>() { 0d };

            for (int count = 1; count <= colorCount; count++)
            {
                currentDivision = offsets.Last() + gradientDivision;

                for (int j = 1; j <= 2; j++)
                {
                    offsets.Add(currentDivision);
                }
            }

            offsets.Add(1d);

            return offsets;
        }
        #endregion
    }
}

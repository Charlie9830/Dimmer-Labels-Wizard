using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Provides Grouping functionality to LabelColorManager and Label Field Short Names.
    /// </summary>
    public class UnitGroup : ViewModelBase
    {
        /// <summary>
        /// Used for creating Color Unit Groups.
        /// </summary>
        /// <param name="dimmerColorDictionary"></param>
        /// <param name="distroColorDictionary"></param>
        public UnitGroup(ColorDictionary dimmerColorDictionary, ColorDictionary distroColorDictionary)
        {
            _DimmerColorDictionary = dimmerColorDictionary;
            _DistroColorDictionary = distroColorDictionary;
        }

        ColorDictionary _DimmerColorDictionary;
        ColorDictionary _DistroColorDictionary;

        /// <summary>
        /// Used for creating Name Color Groups.
        /// </summary>
        /// <param name="originalImportName"></param>
        /// <param name="shortName"></param>
        public UnitGroup(string originalImportName, string shortName, LabelField labelField)
        {
            _OriginalImportName = originalImportName;
            _ShortName = shortName;
            _LabelField = labelField;
        }

        protected LabelField _LabelField;

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


        protected List<DimmerDistroUnit> _Units;

        public List<DimmerDistroUnit> Units
        {
            get { return _Units; }
            set
            {
                if (_Units != value)
                {
                    _Units = value;

                    // Notify.
                    OnPropertyChanged(nameof(Units));
                    OnPropertyChanged(nameof(UnitGroupColor));
                    OnPropertyChanged(nameof(DisplayedBrush));
                }
            }
        }

        public Color UnitGroupColor
        {
            get
            {
                return GetColor();
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


        protected string _OriginalImportName;

        public string OriginalImportName
        {
            get { return _OriginalImportName; }
            set
            {
                if (_OriginalImportName != value)
                {
                    _OriginalImportName = value;

                    // Notify.
                    OnPropertyChanged(nameof(OriginalImportName));
                }
            }
        }


        protected string _ShortName;

        public string ShortName
        {
            get { return _ShortName; }
            set
            {
                if (_ShortName != value)
                {
                    _ShortName = value;

                    // Update Other Units.
                    foreach (var unit in Units)
                    {
                        unit.SetData(value, _LabelField);
                    }

                    // Notify.
                    OnPropertyChanged(nameof(ShortName));
                    OnPropertyChanged(nameof(ShortNameCharacterCount));
                }
            }
        }

        public int ShortNameCharacterCount
        {
            get
            {
                return ShortName.Length;
            }
        }

        #endregion

        #region Methods
        public void InvalidateDisplayedBrush()
        {
            OnPropertyChanged(nameof(DisplayedBrush));
        }

        protected IEnumerable<Color> GetColorRange()
        {
            var colorRange = new List<Color>();

            foreach (var element in Units)
            {
                // Select Search Space and Find Color.
                var dictionary = element.RackUnitType == RackType.Dimmer ? _DimmerColorDictionary : _DistroColorDictionary;

                Color unitColor;
                if (dictionary.TryGetColor(element.UniverseNumber, element.DimmerNumber, out unitColor) == false)
                {
                    unitColor = Colors.White;
                }

                // Add to Color Range if not already existing.
                if (colorRange.Contains(unitColor) == false)
                {
                    colorRange.Add(unitColor);
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

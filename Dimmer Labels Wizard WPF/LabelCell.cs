using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Globalization;
using System.Windows.Data;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelCell : ContentControl
    {
        // Constants
        protected const double HeaderSingleLabelHeightDownFactor = 0.25d;
        protected const double FooterSingleLabelHeightDownFactor = 0.75d;
        protected const double UnitConversionRatio = 96d / 25.4d;

        // Properties.
        public DimmerDistroUnit PreviousReference { get; set; }

        protected bool _IsCellSelected = false;
        protected NeighbourCells _Neighbours;

        protected double _RealWidth;
        protected double _RealHeight;

        protected SolidColorBrush _TextBrush;
        protected SolidColorBrush _Background;
        protected double _LeftWeight = 2;
        protected double _TopWeight = 2;
        protected double _RightWeight = 2;
        protected double _BottomWeight = 2;

        protected LabelStripMode _LabelMode = LabelStripMode.Double;

        // Text Grid Rendering Elements.
        protected Grid _Grid = new Grid();

        #region Constructor
        public LabelCell()
        {
            // Assign Grid to Content Property.
            Content = _Grid;
        }
        #endregion

        #region Getters/Setters.
        /// <summary>
        /// The Grid in which CellRow's and their Child elements are Displayed.
        /// </summary>
        public Grid Grid
        {
            get { return _Grid; }
            set { _Grid = value; }
        }

        /// <summary>
        /// The Width of the Cell in real world millimetres.
        /// </summary>
        public double RealWidth
        {
            get
            {
                return _RealWidth;
            }
            set
            {
                _RealWidth = value;

                Width = _RealWidth * UnitConversionRatio;
            }
        }

        /// <summary>
        /// The Height of the Cell in real world millimeters.
        /// </summary>
        public double RealHeight
        {
            get
            {
                return _RealHeight;
            }
            set
            {
                _RealHeight = value;

                if (LabelMode == LabelStripMode.Single)
                {
                    if (GetType() == typeof(FooterCell))
                    {
                        if (LabelMode == LabelStripMode.Single)
                        {
                            Height = (_RealHeight * UnitConversionRatio) * FooterSingleLabelHeightDownFactor;
                        }
                        else
                        {
                            Height = (_RealHeight * UnitConversionRatio);
                        }
                    }

                    if (GetType() == typeof(HeaderCell))
                    {
                        if (LabelMode == LabelStripMode.Single)
                        {
                            Height = (_RealHeight * UnitConversionRatio) * HeaderSingleLabelHeightDownFactor;
                        }
                        else
                        {
                            Height = (_RealHeight * UnitConversionRatio);
                        }
                    }
                }

                else
                {
                    Height = (_RealHeight * UnitConversionRatio);
                }
            }
        }

        /// <summary>
        /// Describes the Type of LabelStrip. Single or Double Strip Modes.
        /// </summary>
        public virtual LabelStripMode LabelMode
        {
            get
            {
                return _LabelMode;
            }
            set
            {
                if (_LabelMode != value)
                {
                    _LabelMode = value;

                    //Increase or Decrease Height depending on LabelMode and Derived Type.
                    if (value == LabelStripMode.Single)
                    {
                        if (GetType() == typeof(HeaderCell))
                        {
                            Height = (_RealHeight * UnitConversionRatio) * HeaderSingleLabelHeightDownFactor;
                        }

                        if (GetType() == typeof(FooterCell))
                        {
                            Height = (_RealHeight * UnitConversionRatio) * FooterSingleLabelHeightDownFactor;
                        }
                    }

                    if (value == LabelStripMode.Double)
                    {
                        if (GetType() == typeof(HeaderCell))
                        {
                            Height = _RealHeight * UnitConversionRatio;
                        }

                        if (GetType() == typeof(FooterCell))
                        {
                            Height = _RealHeight * UnitConversionRatio;
                        }
                    }
                }
            }
        }

        public SolidColorBrush TextBrush
        {
            get
            {
                return _TextBrush;
            }

            protected set     // Set By BackgroundColor Setter
            {
                _TextBrush = value;
            }
        }

        public new SolidColorBrush Background
        {
            get
            {
                return _Background;
            }

            set
            {
                if (_Background == null || _Background.Color != value.Color)
                {
                    // Calculate Luminance of Color and set _textColor to White or Black based on this luminance result.
                    if ((0.299 * value.Color.R) + (0.587 * value.Color.G) + (0.114 * value.Color.B) > 128)
                    {
                        _TextBrush = new SolidColorBrush(Colors.Black);
                    }

                    else
                    {
                        _TextBrush = new SolidColorBrush(Colors.White);
                    }

                    _Background = value;
                    InvalidateVisual();
                }
            }
        }

        public NeighbourCells Neighbours
        {
            get
            {
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                else
                {
                    return _Neighbours;
                }
            }
            set
            {
                _Neighbours = value;
            }
        }

        public double LeftWeight
        {
            get
            {
                return _LeftWeight;
            }
            set
            {   if (_LeftWeight != value)
                {
                    _LeftWeight = value;
                    InvalidateVisual();
                }
                
            }
        }

        public double TopWeight
        {
            get
            {
                return _TopWeight;
            }
            set
            {
                if (_TopWeight != value)
                {
                    _TopWeight = value;
                    InvalidateVisual();
                }
            }
        }

        public double RightWeight
        {
            get
            {
                return _RightWeight;
            }
            set
            {
                if (_RightWeight != value)
                {
                    _RightWeight = value;
                    InvalidateVisual();
                }
            }
        }

        public double BottomWeight
        {
            get
            {
                return _BottomWeight;
            }
            set
            {
                if (_BottomWeight != value)
                {
                    _BottomWeight = value;
                    InvalidateVisual();
                }
            }
        }
        #endregion

        #region Overrides
        protected override void OnRender(DrawingContext drawingContext)
        {
            #region TextGrid Rendering.

            // Setup Grid.
            _Grid.Width = Width;
            _Grid.Height = Height;

            #endregion

            #region Outline Rendering
            // Declare Resources.
            var lineBrush = new SolidColorBrush(Colors.Black);

            // Pens.
            var leftPen = new Pen(lineBrush, _LeftWeight);
            var topPen = new Pen(lineBrush, _TopWeight);
            var rightPen = new Pen(lineBrush, _RightWeight);
            var bottomPen = new Pen(lineBrush, _BottomWeight);

            // Corner Points.
            var topLeft = new Point(0, 0);
            var topRight = new Point(Width, 0);
            var bottomRight = new Point(Width, Height);
            var bottomLeft = new Point(0, Height);

            // Drawing 
            drawingContext.DrawLine(leftPen, bottomLeft, topLeft);
            drawingContext.DrawLine(topPen, topLeft, topRight);
            drawingContext.DrawLine(rightPen, topRight, bottomRight);
            drawingContext.DrawLine(bottomPen, bottomRight, bottomLeft);

            #endregion

        }
        #endregion

        #region Dependency Properties
        #endregion

        #region Serialization Methods
        public void RebuildLabelCell(LabelCellStorage storage)
        {
            ByteColor textColor = storage.TextColor;
            ByteColor backgroundColor = storage.BackgroundColor;

            _TextBrush = new SolidColorBrush(storage.TextColor.ToColor());
            _Background = new SolidColorBrush(storage.BackgroundColor.ToColor());
        }

        public LabelCellStorage GenerateLabelCellStorage()
        {
            LabelCellStorage storage = new LabelCellStorage();

            storage.TextColor = new ByteColor(_TextBrush.Color);
            storage.BackgroundColor = new ByteColor(_Background.Color);

            storage.PreviousReference = PreviousReference;

            return storage;
        }
        #endregion

        #region Methods for Inhertitated Classes
        public static Typeface RebuildFont(string fontFamilyName, int openTypeFontWeight, string fontStyle)
        {
            FontFamily fontFamily = new FontFamily(fontFamilyName);
            FontWeight fontWeight = FontWeight.FromOpenTypeWeight(openTypeFontWeight);

            FontStyle style = new FontStyle();
            switch (fontStyle)
            {
                case "Normal":
                    style = FontStyles.Normal;
                    break;
                case "Italic":
                    style = FontStyles.Italic;
                    break;
                case "Oblique":
                    style = FontStyles.Oblique;
                    break;
                default:
                    style = FontStyles.Normal;
                    break;
            }

            return new Typeface(fontFamily, style, fontWeight, new FontStretch());
        }

        #endregion
    }

    public class NeighbourCells
    {
        public LabelCell Left { get; set; }
        public LabelCell Top { get; set; }
        public LabelCell Right { get; set; }
        public LabelCell Bottom { get; set; }

    }
    
    public class LabelCellStorage
    {
        public DimmerDistroUnit PreviousReference;

        public ByteColor TextColor;
        public ByteColor BackgroundColor;
    }

    
    public struct ByteColor
    {
        public ByteColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public ByteColor(Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;      
        }

        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }
}

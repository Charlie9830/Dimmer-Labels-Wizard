using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Globalization;

namespace Dimmer_Labels_Wizard_WPF
{
    public class FooterCell : LabelCell
    {
        #region Constructors
        public FooterCell()
        {
            // Setup TextBlocks and Assign as Grid Children.
            

        }

        public FooterCell(FooterCellStorage storage)
        {
            TopData = storage.TopData;
            TopFontSize = storage.TopFontSize;
            TopFont = RebuildFont(storage.TopFontFamilyName, storage.TopOpenTypeFontWeight,
                storage.TopFontStyle);

            MiddleData = storage.MiddleData;
            MiddleFontSize = storage.MiddleFontSize;
            MiddleFont = RebuildFont(storage.MiddleFontFamilyName, storage.MiddleOpenTypeFontWeight,
                storage.MiddleFontStyle);

            BottomData = storage.BottomData;
            BottomFontSize = storage.BottomFontSize;
            BottomFont = RebuildFont(storage.BottomFontFamilyName, storage.BottomOpenTypeFontWeight,
                storage.BottomFontStyle);

            // Set LabelCell Values.
            TextBrush = new SolidColorBrush(storage.BaseStorage.TextColor.ToColor());
            Background = new SolidColorBrush(storage.BaseStorage.BackgroundColor.ToColor());
            PreviousReference = storage.BaseStorage.PreviousReference;
        }
        #endregion

        // Top Line Properties
        protected string _TopData;
        protected double _TopFontSize;
        protected Typeface _TopFont;

        public string TopData
        {
            get
            {
                
                return _TopData;
            }
            set
            {
                if (value != _TopData)
                {
                    _TopData = value;
                }
            }
        }
        public Typeface TopFont
        {
            get
            {
                return _TopFont;
            }

            set
            {
                if (_TopFont != value)
                {
                    _TopFont = value;
                }
            }
        }
        public double TopFontSize
        {
            get
            {
                return _TopFontSize;
            }

            set
            {
                if (value != _TopFontSize)
                {
                    // Downsize FontSize if required.
                    double fontSize = HelperScaleDownFont(TopData, TopFont, value,
                        Width, Height, ScaleDirection.Horizontal);

                    // Round to Nearest Quarter.
                    _TopFontSize = Math.Round(fontSize * 4, MidpointRounding.AwayFromZero) / 4;

                    InvalidateVisual();
                }
            }
        }

        // Middle Line Properties
        protected string _MiddleData;
        protected double _MiddleFontSize;
        protected Typeface _MiddleFont;

        public string MiddleData
        {
            get
            {
                return _MiddleData;
            }
            set
            {
                if (value != _MiddleData)
                {
                    _MiddleData = value;
                }
            }
        }
        public Typeface MiddleFont
        {
            get
            {
                return _MiddleFont;
            }

            set
            {
                if (_MiddleFont != value)
                {
                    _MiddleFont = value;
                }
            }
        }
        public double MiddleFontSize
        {
            get
            {
                return _MiddleFontSize;
            }

            set
            {
                if (value != _MiddleFontSize)
                {
                    // Downsize FontSize if required.
                    double fontSize = HelperScaleDownFont(MiddleData, MiddleFont, value,
                        Width, Height, ScaleDirection.Horizontal);

                    // Round to Nearest Quarter.
                    _MiddleFontSize = Math.Round(fontSize * 4, MidpointRounding.AwayFromZero) / 4;

                    InvalidateVisual();
                }
            }
        }

        //Bottom line Properties
        protected string _BottomData;
        protected double _BottomFontSize;
        protected Typeface _BottomFont;

        public string BottomData
        {
            get
            {
                return _BottomData;
            }
            set
            {
                if (value != _BottomData)
                {
                    _BottomData = value;
                }
            }
        }
        public Typeface BottomFont
        {
            get
            {
                return _BottomFont;
            }

            set
            {
                if (_BottomFont != value)
                {
                    _BottomFont = value;
                }
            }
        }
        public double BottomFontSize
        {
            get
            {
                return _BottomFontSize;
            }

            set
            {
                if (value != _BottomFontSize)
                {
                    // Downsize FontSize if required.
                    double fontSize = HelperScaleDownFont(BottomData, BottomFont, value,
                        Width, Height, ScaleDirection.Horizontal);

                    // Round to Nearest Quarter.
                    _BottomFontSize = Math.Round(fontSize * 4, MidpointRounding.AwayFromZero) / 4;
                    InvalidateVisual();
                }
            }
        }

        #region Overrides
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
        #endregion

        #region Helper Methods
        // Measures Size of Text. Returns 0 Width, 0 Height if no Data exists.
        private Size HelperMeasureText(string data, Typeface font, double fontSize)
        {
            if (data == null || data == string.Empty)
            {
                return new Size(0, 0);
            }

            FormattedText formatter = new FormattedText(data, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                font, fontSize, Brushes.Black);

            return new Size(formatter.Width, formatter.Height);
        }

        // Scales Font Size and returns new FontSize. Returns same if no scaling was required.
        private double HelperScaleDownFont(string data, Typeface font, double fontSize,
            double containerWidth, double containerHeight, ScaleDirection scaleDirection)
        {
            Size textSize = HelperMeasureText(data, font, fontSize);

            // Scale Down by Horizontal Axis only.
            if (scaleDirection == ScaleDirection.Horizontal)
            {
                if (textSize.Width > containerWidth)
                {
                    double ratio = containerWidth / textSize.Width;
                    return fontSize * ratio;
                }

                else
                {
                    return fontSize;
                }
            }

            // Scale Down by Vertical axis only.
            if (scaleDirection == ScaleDirection.Vertical)
            {
                if (textSize.Height > containerHeight)
                {
                    double ratio = containerHeight / textSize.Height;
                    return fontSize * ratio;
                }
            }

            // Scale down by both Horizontal and Vertical axis.
            if (scaleDirection == ScaleDirection.Both)
            {
                double widthRatio = 1;
                double heightRatio = 1;

                // Calculate Ratios.
                if (textSize.Width > containerWidth)
                {
                    widthRatio = containerWidth / textSize.Width;
                }

                if (textSize.Height > containerHeight)
                {
                    heightRatio = containerHeight / textSize.Height;
                }

                // Scale FontSize by largest Ratio. (Furthest from 1).
                if (widthRatio < heightRatio)
                {
                    return fontSize * widthRatio;
                }

                if (heightRatio > widthRatio)
                {
                    return fontSize * heightRatio;
                }

                // Ratio's are either Identical or both have been left at 1.
                else
                {
                    return fontSize * widthRatio;
                }
            }

            else
            {
                return fontSize;
            }

        }
        #endregion

        #region Serialization Methods
        public FooterCellStorage GenerateStorage()
        {
            FooterCellStorage storage = new FooterCellStorage();
            storage.TopData = TopData;
            storage.TopFontSize = TopFontSize;
            storage.TopFontFamilyName = TopFont.FontFamily.Source;
            storage.TopOpenTypeFontWeight = TopFont.Weight.ToOpenTypeWeight();
            storage.TopFontStyle = TopFont.Style.ToString();

            storage.MiddleData = MiddleData;
            storage.MiddleFontSize = MiddleFontSize;
            storage.MiddleFontFamilyName = MiddleFont.FontFamily.Source;
            storage.MiddleOpenTypeFontWeight = MiddleFont.Weight.ToOpenTypeWeight();
            storage.MiddleFontStyle = MiddleFont.Style.ToString();

            storage.BottomData = BottomData;
            storage.BottomFontSize = BottomFontSize;
            storage.BottomFontFamilyName = BottomFont.FontFamily.Source;
            storage.BottomOpenTypeFontWeight = BottomFont.Weight.ToOpenTypeWeight();
            storage.BottomFontStyle = BottomFont.Style.ToString();

            // Collect Base Class's Data. "Pack your things Dad, we are going to the Nursing home".
            storage.BaseStorage = GenerateLabelCellStorage();

            return storage;
        }
        #endregion
    }

    // Wraps a Footercell together with a TextBlock Position Enumeration.
    public class FooterCellText
    {
        public FooterCell Cell { get; set; }
        public FooterTextPosition Position { get; set; }

        public FooterCellText(FooterCell cell, FooterTextPosition position)
        {
            Cell = cell;
            Position = position;
        }

        #region Getters/Setters
        // Collects Data based on Position member Enum.
        public string Data
        {
            get
            {
                switch (Position)
                {
                    case FooterTextPosition.Top:
                        return Cell.TopData;
                    case FooterTextPosition.Middle:
                        return Cell.MiddleData;
                    case FooterTextPosition.Bottom:
                        return Cell.BottomData;
                    case FooterTextPosition.NotAssigned:
                        throw new InvalidOperationException("FooterCellText.Position not initialized to Position Value.");
                    default:
                        return string.Empty;
                }
            }
            set
            {
                switch (Position)
                {
                    case FooterTextPosition.Top:
                        Cell.TopData = value;
                        return;
                    case FooterTextPosition.Middle:
                        Cell.MiddleData = value;
                        return;
                    case FooterTextPosition.Bottom:
                        Cell.BottomData = value;
                        return;
                    case FooterTextPosition.NotAssigned:
                        throw new InvalidOperationException("FooterCellText.Position enumeration set to Not Assigned.");
                    default:
                        return;
                }
            }
        }

        public Typeface Font
        {
            get
            {
                switch (Position)
                {
                    case FooterTextPosition.NotAssigned:
                        throw new InvalidOperationException("FooterCellText.Position not initialized to Position Value.");
                    case FooterTextPosition.Top:
                        return Cell.TopFont;
                    case FooterTextPosition.Middle:
                        return Cell.MiddleFont;
                    case FooterTextPosition.Bottom:
                        return Cell.BottomFont;
                    default:
                        throw new InvalidOperationException("FooterCellTextPosition Switch reached Default Case.");
                }
            }
            set
            {
                switch (Position)
                {
                    case FooterTextPosition.NotAssigned:
                        throw new InvalidOperationException("FooterCellText.Position not initialized to Position Value.");
                    case FooterTextPosition.Top:
                        Cell.TopFont = value;
                        return;
                    case FooterTextPosition.Middle:
                        Cell.MiddleFont = value;
                        return;
                    case FooterTextPosition.Bottom:
                        Cell.BottomFont = value;
                        return;
                    default:
                        throw new InvalidOperationException("FooterCellTextPosition Switch reached Default Case.");
                }
            }
        }

        public double FontSize
        {
            get
            {
                switch (Position)
                {
                    case FooterTextPosition.NotAssigned:
                        throw new InvalidOperationException("FooterCellText.Position not initialized to Position Value.");
                    case FooterTextPosition.Top:
                        return Cell.TopFontSize;
                    case FooterTextPosition.Middle:
                        return Cell.MiddleFontSize;
                    case FooterTextPosition.Bottom:
                        return Cell.BottomFontSize;
                    default:
                        throw new InvalidOperationException("FooterCellTextPosition Switch reached Default Case.");
                }
            }
            set
            {
                switch (Position)
                {
                    case FooterTextPosition.NotAssigned:
                        throw new InvalidOperationException("FooterCellText.Position not initialized to Position Value.");
                    case FooterTextPosition.Top:
                        Cell.TopFontSize = value;
                        return;
                    case FooterTextPosition.Middle:
                        Cell.MiddleFontSize = value;
                        return;
                    case FooterTextPosition.Bottom:
                        Cell.BottomFontSize = value;
                        return;
                    default:
                        throw new InvalidOperationException("FooterCellTextPosition Switch reached Default Case.");
                }
            }
        }
        #endregion
    }

    
    public class FooterCellStorage
    {
        public string TopData;
        public double TopFontSize;
        public string TopFontFamilyName;
        public int TopOpenTypeFontWeight;
        public string TopFontStyle;

        public string MiddleData;
        public double MiddleFontSize;
        public string MiddleFontFamilyName;
        public int MiddleOpenTypeFontWeight;
        public string MiddleFontStyle;

        public string BottomData;
        public double BottomFontSize;
        public string BottomFontFamilyName;
        public int BottomOpenTypeFontWeight;
        public string BottomFontStyle;

        public LabelCellStorage BaseStorage;
    }
}

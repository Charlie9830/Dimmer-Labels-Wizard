using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace Dimmer_Labels_Wizard_WPF
{
    public class FooterCell : LabelCell
    {
        public FooterCell()
        {
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
            BackgroundBrush = new SolidColorBrush(storage.BaseStorage.BackgroundColor.ToColor());
            PreviousReference = storage.BaseStorage.PreviousReference;
        }

        // Top Line Properties
        protected double _TopFontSize;
        protected Typeface _TopFont;

        public string TopData { get; set; }
        public Typeface TopFont
        {
            get
            {
                return _TopFont;
            }

            set
            {
                _TopFont = value;
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
                // Round to Nearest Quarter.
                _TopFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }

        // Middle Line Properties
        protected double _MiddleFontSize;
        protected Typeface _MiddleFont;

        public string MiddleData { get; set; }
        public Typeface MiddleFont
        {
            get
            {
                return _MiddleFont;
            }
            set
            {
                _MiddleFont = value;
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
                // Round to Nearest Quarter.
                _MiddleFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }

        //Bottom line Properties
        protected double _BottomFontSize;
        protected Typeface _BottomFont;

        public string BottomData { get; set; }
        public Typeface BottomFont
        {
            get
            {
                return _BottomFont;
            }
            set
            {
                _BottomFont = value;
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
                // Round to Nearest Quarter.
                _BottomFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }


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

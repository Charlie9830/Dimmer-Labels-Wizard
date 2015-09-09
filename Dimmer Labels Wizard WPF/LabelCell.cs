using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelCell
    {
        // Properties
        public DimmerDistroUnit PreviousReference { get; set; }

        protected SolidColorBrush _TextBrush;
        protected SolidColorBrush _BackgroundBrush;

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
        
        public SolidColorBrush BackgroundBrush
        {
            get
            {
                return _BackgroundBrush;
            }

            set
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

                _BackgroundBrush = value;
            }
        }

        // Serialization.

        #region Serialization Methods
        public void RebuildLabelCell(LabelCellStorage storage)
        {
            ByteColor textColor = storage.TextColor;
            ByteColor backgroundColor = storage.BackgroundColor;

            _TextBrush = new SolidColorBrush(storage.TextColor.ToColor());
            _BackgroundBrush = new SolidColorBrush(storage.BackgroundColor.ToColor());
        }

        public LabelCellStorage GenerateLabelCellStorage()
        {
            LabelCellStorage storage = new LabelCellStorage();

            storage.TextColor = new ByteColor(_TextBrush.Color);
            storage.BackgroundColor = new ByteColor(_BackgroundBrush.Color);

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

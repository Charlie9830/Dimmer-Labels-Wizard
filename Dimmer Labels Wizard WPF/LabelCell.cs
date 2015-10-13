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

        protected NeighbourCells _Neighbours;
        protected double _LeftWeight = 0;
        protected double _TopWeight = 0;
        protected double _RightWeight = 0;
        protected double _BottomWeight = 0;

        #region Getters/Setters.
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

        // Public Weights
        public double LeftWeight
        {
            get
            {
                // No Neighbour Object has been set.
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                // Left hand Neighbour exists.
                else if (_Neighbours.Left != null)
                {
                    if (_LeftWeight == 0)
                    {
                        return _LeftWeight;
                    }
                    else
                    {
                        return _LeftWeight / 2;
                    }
                }

                // Left Hand Neighbour does not exist.
                else
                {
                    return _LeftWeight;
                }
            }
            set
            {
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                else
                {
                    _LeftWeight = value;
                    if (Neighbours.Left != null)
                    {
                        Neighbours.Left.SneakRightWeight = value;
                    }
                }
            }
        }

        public double TopWeight
        {
            get
            {
                // No Neighbour Object has been set.
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                // Top Neighbour exists.
                else if (_Neighbours.Top != null)
                {
                    if (_TopWeight == 0)
                    {
                        return _TopWeight;
                    }
                    else
                    {
                        return _TopWeight / 2;
                    }
                }

                // Top Neighbour does not exist.
                else
                {
                    return _TopWeight;
                }
            }
            set
            {
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                else
                {
                    _TopWeight = value;
                    if (Neighbours.Bottom != null)
                    {
                        Neighbours.Bottom.SneakBottomWeight = value;
                    }
                }
            }
        }

        public double RightWeight
        {
            get
            {
                // No Neighbour Object has been set.
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                // Right Neighbour exists.
                else if (_Neighbours.Right != null)
                {
                    if (_RightWeight == 0)
                    {
                        return _RightWeight;
                    }
                    else
                    {
                        return _RightWeight / 2;
                    }
                }

                // Right Neighbour does not exist.
                else
                {
                    return _RightWeight;
                }
            }
            set
            {
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                else
                {
                    _RightWeight = value;
                    if (Neighbours.Right != null)
                    {
                        Neighbours.Right.SneakLeftWeight = value;
                    }
                }
            }
        }

        public double BottomWeight
        {
            get
            {
                // No Neighbour Object has been set.
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                // Bottom Neighbour exists.
                else if (_Neighbours.Bottom != null)
                {
                    if (_BottomWeight == 0)
                    {
                        return _BottomWeight;
                    }
                    else
                    {
                        return _BottomWeight / 2;
                    }
                }

                // Bottom Neighbour does not exist.
                else
                {
                    return _BottomWeight;
                }
            }
            set
            {
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                else
                {
                    _BottomWeight = value;
                    if (Neighbours.Bottom != null)
                    {
                        Neighbours.Bottom.SneakTopWeight = value;
                    }
                }
            }
        }

        // Public SneakWeights
        public double SneakLeftWeight
        {
            set
            {
                _LeftWeight = value;
            }
        }

        public double SneakTopWeight
        {
            set
            {
                _TopWeight = value;
            }
        }

        public double SneakRightWeight
        {
            set
            {
                _RightWeight = value;
            }
        }

        public double SneakBottomWeight
        {
            set
            {
                _BottomWeight = value;
            }
        }
        #endregion
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

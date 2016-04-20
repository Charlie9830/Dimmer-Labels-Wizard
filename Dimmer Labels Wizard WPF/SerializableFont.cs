using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Markup;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Provides the Type for the backing value of the Style Template Classes. Provides Database friendly
    /// storage of Typefaces.
    /// </summary>
    public class SerializableFont
    {
        public SerializableFont()
        {

        }

        public SerializableFont(string fontFamilyName)
        {
            FontFamilyString = fontFamilyName;
        }

        #region Database Properties.
        public int ID { get; set; }

        public bool IsBold { get; set; }
        public bool IsItalics { get; set; }
        public bool IsUnderline { get; set; }

        public string FontFamilyString { get; set; } = "Arial";
        #endregion

        #region POCO Properties.
        [NotMapped]
        public Typeface Typeface
        {
            get
            {
                return GetTypeface();
            }

            set
            {
                SetTypeface(value);
            }
        }
        #endregion

        #region Methods.
        protected Typeface GetTypeface()
        {
            return new Typeface(new FontFamily(FontFamilyString), GetFontStyle(), GetFontWeight(), GetFontStretch());
        }

        protected void SetTypeface(Typeface value)
        {
            FontFamilyString = value.FontFamily.Source;
            IsBold = value.Weight == FontWeights.Bold ? true : false;
            IsItalics = value.Style == FontStyles.Italic ? true : false;
            IsUnderline = value.UnderlineThickness > 0 ? true : false;
        }

        protected FontStyle GetFontStyle()
        {
            return IsItalics ? FontStyles.Italic : FontStyles.Normal;
        }

        protected FontWeight GetFontWeight()
        {
            return IsBold ? FontWeights.Bold : FontWeights.Normal;
        }

        protected FontStretch GetFontStretch()
        {
            return FontStretches.Normal;
        }
        #endregion

    }
}

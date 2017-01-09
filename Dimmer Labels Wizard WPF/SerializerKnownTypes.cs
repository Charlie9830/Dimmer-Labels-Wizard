using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class SerializerKnownTypes
    {
        #region Methods
        public static List<Type> GetTemplateKnownTypes()
        {
            return new List<Type>()
            {
                typeof(LabelStripTemplate),
                typeof(Strip),
                typeof(StripSpacer),
                typeof(Merge),
                typeof(DimmerDistroUnit),

                // LabelCellTemplate and it's Child Types.
                typeof(LabelCellTemplate),
                typeof(SerializableFont),
                typeof(StripAddress),

                // CellRowTemplate and It's Child Types.
                typeof(CellRowTemplate),
                // Serializable Font has Already been Added.
            };
        }

        public static List<Type> GetGlobalKnownTypes()
        {
            var types = new List<Type>(GetTemplateKnownTypes());

            types.Add(typeof(ProgramState));
            types.Add(typeof(ColorEntry));
            types.Add(typeof(ColorDictionary));

            return types;
        }
        #endregion
    }
}

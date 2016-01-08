using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class Merge
    {
        public CellVerticalPosition VerticalPosition;

        public DimmerDistroUnit PrimaryUnit;
        public List<DimmerDistroUnit> ConsumedUnits;

        public Merge(CellVerticalPosition verticalPosition,
            DimmerDistroUnit primaryUnit,
            List<DimmerDistroUnit> consumedUnits)
        {
            VerticalPosition = verticalPosition;
            PrimaryUnit = primaryUnit;
            ConsumedUnits = consumedUnits;
        }
    }
}

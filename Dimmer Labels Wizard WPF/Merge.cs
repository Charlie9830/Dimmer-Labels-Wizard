using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class Merge
    {
        public Merge()
        {

        }

        public Merge(CellVerticalPosition verticalPosition,
            DimmerDistroUnit primaryUnit,
            List<DimmerDistroUnit> consumedUnits)
        {
            VerticalPosition = verticalPosition;
            PrimaryUnit = primaryUnit;
            ConsumedUnits = consumedUnits;
        }

        // Database.
        public virtual int ID { get; set; }
        public virtual Strip Strip { get; set; }

        public virtual CellVerticalPosition VerticalPosition { get; set; }

        public virtual DimmerDistroUnit PrimaryUnit { get; set; }
        public virtual List<DimmerDistroUnit> ConsumedUnits { get; set; } = new List<DimmerDistroUnit>();


    }
}

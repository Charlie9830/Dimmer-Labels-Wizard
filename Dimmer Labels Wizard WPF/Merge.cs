using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Dimmer_Labels_Wizard_WPF
{
    [DataContract]
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

        // Database and Navigation Properties.
        [Key]
        public virtual int ID { get; set; }
        public virtual Strip Strip { get; set; }

        [DataMember]
        public virtual CellVerticalPosition VerticalPosition { get; set; }

        [DataMember]
        public virtual DimmerDistroUnit PrimaryUnit { get; set; }

        [DataMember]
        public virtual List<DimmerDistroUnit> ConsumedUnits { get; set; } = new List<DimmerDistroUnit>();


    }
}

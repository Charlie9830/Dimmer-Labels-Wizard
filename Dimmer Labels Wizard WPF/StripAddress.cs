using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.Serialization;

namespace Dimmer_Labels_Wizard_WPF
{
    [DataContract]
    public class StripAddress
    {
        // Database and Navigation Properties.
        public int ID { get; set; }
        public virtual LabelCellTemplate LabelCellTemplate { get; set; }

        // Position Values.
        [DataMember]
        public virtual Strip Strip { get; set; }
        [DataMember]
        public virtual CellVerticalPosition VerticalPosition { get; set; }

        // DimmerDistroUnit KeyValues.
        [DataMember]
        public virtual RackType RackUnitType { get; set; }
        [DataMember]
        public virtual int UniverseNumber { get; set; }
        [DataMember]
        public virtual int DimmerNumber { get; set; }
    }
}

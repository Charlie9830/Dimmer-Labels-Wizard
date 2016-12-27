using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    [DataContract]
    public class ProgramState
    {
        [DataMember]
        public List<DimmerDistroUnit> Units { get; set; } = new List<DimmerDistroUnit>();
        [DataMember]
        public List<Strip> Strips { get; set; } = new List<Strip>();
        [DataMember]
        public List<LabelStripTemplate> Templates { get; set; } = new List<LabelStripTemplate>();
        [DataMember]
        public List<ColorDictionary> ColorDictionaries { get; set; } = new List<ColorDictionary>();

    }
}

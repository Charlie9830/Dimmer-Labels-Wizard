using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class ImportConfiguration
    {
        // Import Settings.
        public ImportFormat DimmerImportFormat { get; set; }
        public ImportFormat UniverseImportFormat { get; set; }
        public ImportFormat DistroImportFormat { get; set; }
        public string DistroNumberPrefix { get; set; } = string.Empty;

        public List<DimmerRange> DimmerRanges = new List<DimmerRange>();
        public List<DistroRange> DistroRanges = new List<DistroRange>();

        // Column Indexes
        public int ChannelNumberColumnIndex { get; set; }
        public int DimmerNumberColumnIndex { get; set; }
        public int InstrumentTypeColumnIndex { get; set; }
        public int MulticoreNameColumnIndex { get; set; }
        public int PositionColumnIndex { get; set; }
        public int UniverseDataColumnIndex { get; set; }
        public int UserField1ColumnIndex { get; set; }
        public int UserField2ColumnIndex { get; set; }
        public int UserField3ColumnIndex { get; set; }
        public int UserField4ColumnIndex { get; set; }
    }
}

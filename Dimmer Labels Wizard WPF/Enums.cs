using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    // DimDistroUnit Object Data type tracking.
    public enum RackType { Dimmer, Distro, OutsideLabelRange, Unparseable, ClashingRange};
    public enum SortOrder { Default, DimmerAndDistroNumber}; // Update DimDistroUnit.CompareTo if you Add anything.

    // Distro Formating.     Format1 ND###: Format2 ###: Format3 #/###: Format4 A/###:.
    // Dimmer Formatting.    Format1 #/###: Format2 ###: Format3 A###: Format4 A/###:.
    // Dimmer Format2 Requires more infomation be Imported as that Format does not provide Universe Infomation.
    // NoUniverseData is used only as a Format2 Overide. SHould be referenced only by UserParameters.DMXAddresColumnFormatting.
    public enum ImportFormatting { Format1, Format2, Format3, Format4, NoUniverseData, NoAssignment }
    public enum LabelField { NoAssignment, ChannelNumber, InstrumentName, MulticoreName, Position, UserField1 ,UserField2, UserField3, UserField4}

    public enum RadioButtonSelection { None, All, Rack, Selection };

    public enum CellSelectionMode { Cell, Text};

    public enum FooterTextPosition {NotAssigned, Top, Middle, Bottom};

    public enum LabelStripMode { Single, Double};

    public enum ScaleDirection { Horizontal, Vertical, Both};

    public enum CellDataMode { SingleField, MixedField };

    public enum CellRowHeightMode { Static, Automatic, Manual};
}






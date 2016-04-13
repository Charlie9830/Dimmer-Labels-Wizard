using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    // DimDistroUnit Object Data type tracking.
    public enum RackType { Dimmer, Distro, OutsideLabelRange, Unparseable, ConflictingRange};
    public enum SortOrder { Default, DimmerAndDistroNumber}; // Update DimDistroUnit.CompareTo if you Add anything.

    // Distro Formating.     Format1 ND###: Format2 ###: Format3 #/###: Format4 A/###:.
    // Dimmer Formatting.    Format1 #/###: Format2 ###: Format3 A###: Format4 A/###:.
    // Dimmer Format2 Requires more infomation be Imported as that Format does not provide Universe Infomation.
    // NoUniverseData is used only as a Format2 Overide. Should be referenced only by UserParameters.DMXAddresColumnFormatting.
    public enum ImportFormat { Format1, Format2, Format3, Format4, NoUniverseData, NoAssignment }

    // Enum.GetName() is called on this enum by the LabelCell Data Reference Property changed Handler. Changes to the Names here,
    // may break that property handler. Names must remain the same as their DimmerDistroUnit counterpart properties.
    public enum LabelField { NoAssignment, ChannelNumber, InstrumentName, MulticoreName, Position, UserField1 ,UserField2, UserField3, UserField4, Custom}

    public enum RadioButtonSelection { None, All, Rack, Selection };

    public enum CellSelectionMode { Cell, Text};

    public enum FooterTextPosition {NotAssigned, Top, Middle, Bottom};

    public enum LabelStripMode { Single, Dual};

    public enum CellVerticalPosition { Upper, Lower};

    public enum ScaleDirection { Horizontal, Vertical, Both};

    public enum CellDataMode { SingleField, MultiField };

    public enum CellRowHeightMode { Static, Automatic, Manual};

    public enum MoveDirection { Up, Down};

    public enum TemplateEditorActiveTab { Settings, Assignments};

    public enum ImportType { Merge, Overwrite};
}






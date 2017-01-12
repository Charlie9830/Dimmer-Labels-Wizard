using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class FriendlyEnumCollections
    {
        public static List<FriendlyLabelField> AllFriendlyLabelFields
        {
            get
            {
                return new List<FriendlyLabelField>()
                {
                    new FriendlyLabelField(LabelField.NoAssignment, "No Assignment"),
                    new FriendlyLabelField(LabelField.ChannelNumber, "Channel Number"),
                    new FriendlyLabelField(LabelField.InstrumentName, "Instrument Name"),
                    new FriendlyLabelField(LabelField.MulticoreName, "Multicore Name"),
                    new FriendlyLabelField(LabelField.Position, "Position"),
                    new FriendlyLabelField(LabelField.UserField1, "User Field 1"),
                    new FriendlyLabelField(LabelField.UserField2, "User Field 2"),
                    new FriendlyLabelField(LabelField.UserField3, "User Field 3"),
                    new FriendlyLabelField(LabelField.UserField4, "User Field 4"),
                    new FriendlyLabelField(LabelField.Custom, "Custom")
                };
            }
        }

        public static List<FriendlyLabelField> FriendlyLabelFieldsDisplayedOnly
        {
            get
            {
                return new List<FriendlyLabelField>()
                {
                    new FriendlyLabelField(LabelField.ChannelNumber, "Channel Number"),
                    new FriendlyLabelField(LabelField.InstrumentName, "Instrument Name"),
                    new FriendlyLabelField(LabelField.MulticoreName, "Multicore Name"),
                    new FriendlyLabelField(LabelField.Position, "Position"),
                    new FriendlyLabelField(LabelField.UserField1, "User Field 1"),
                    new FriendlyLabelField(LabelField.UserField2, "User Field 2"),
                    new FriendlyLabelField(LabelField.UserField3, "User Field 3"),
                    new FriendlyLabelField(LabelField.UserField4, "User Field 4"),
                };
            }
        }

        public static List<FriendlyUnitGroupBy> FriendlyUnitGroupBys
        {
            get
            {
                return new List<FriendlyUnitGroupBy>()
                {
                    new FriendlyUnitGroupBy(UnitGroupBy.OriginalImportName, "Original Imported Name"),
                    new FriendlyUnitGroupBy(UnitGroupBy.ShortName, "Short Name")
                };
            }
        }

        public static List<FriendlySearchField> FriendlySearchFields
        {
            get
            {
                return new List<FriendlySearchField>()
                {
                    new FriendlySearchField(SearchField.ChannelNumber, "Channel Number"),
                    new FriendlySearchField(SearchField.All, "All"),
                    new FriendlySearchField(SearchField.InstrumentName, "Instrument Name"),
                    new FriendlySearchField(SearchField.MulticoreName, "Multicore Name"),
                    new FriendlySearchField(SearchField.Position, "Position"),
                    new FriendlySearchField(SearchField.UserField1, "User Field 1"),
                    new FriendlySearchField(SearchField.UserField2, "User Field 2"),
                    new FriendlySearchField(SearchField.UserField3, "User Field 3"),
                    new FriendlySearchField(SearchField.UserField4, "User Field 4")
                };
            }
        }

        public static List<FriendlySortField> FriendlySortFields
        {
            get
            {
                return new List<FriendlySortField>()
                {
                    new FriendlySortField(SortField.DBKeyValues, "Rack Type + Universe Number + Dimmer Number"),
                    new FriendlySortField(SortField.ChannelNumber, "Channel Number"),
                    new FriendlySortField(SortField.InstrumentName, "Instrument Name"),
                    new FriendlySortField(SortField.MulticoreName, "Multicore Name"),
                    new FriendlySortField(SortField.Position, "Position"),
                    new FriendlySortField(SortField.UserField1, "User Field 1"),
                    new FriendlySortField(SortField.UserField2, "User Field 2"),
                    new FriendlySortField(SortField.UserField3, "User Field 3"),
                    new FriendlySortField(SortField.UserField4, "User Field 4"),
                };
            }
        }

        public static List<FriendlyImportFormat> FriendlyDimmerImportFormats
        {
            get
            {
                return new List<FriendlyImportFormat>()
                {
                    new FriendlyImportFormat(ImportFormat.Format1, "Universe / Address (1/123)"),
                    new FriendlyImportFormat(ImportFormat.Format2, "Address (123)"),
                    new FriendlyImportFormat(ImportFormat.Format3, "Universe Letter Address (A123)"),
                    new FriendlyImportFormat(ImportFormat.Format4, "Universe Letter / Address (A/123)")
                };
            }
        }

        public static List<FriendlyImportFormat> FriendlyDistroImportFormats
        {
            get
            {
                return new List<FriendlyImportFormat>()
                {
                    new FriendlyImportFormat(ImportFormat.Format1, "Distro Prefix Dimmer Number (ND123)"),
                    new FriendlyImportFormat(ImportFormat.Format2, "Dimmer Number (123)"),
                    new FriendlyImportFormat(ImportFormat.Format3, "Distro Prefix / Dimmer Number (ND/123)"),
                    new FriendlyImportFormat(ImportFormat.Format4, "ID Letter / Dimmer Number (A/123)")
                };
            }
        }
    }
}

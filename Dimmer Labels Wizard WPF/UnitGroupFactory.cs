using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Helper Class that generates UnitGroup Collections.
    /// </summary>
    public class UnitGroupFactory
    {
        #region Methods.
        protected IEnumerable<IEnumerable<DimmerDistroUnit>> GetLabelFieldGroups(LabelField labelfield, UnitGroupBy groupBy,
            ICollection<DimmerDistroUnit> units)
        {
            // Group by Displayed (Short) Name.
            if (groupBy == UnitGroupBy.ShortName)
            {
                switch (labelfield)
                {
                    case LabelField.ChannelNumber:
                        return from item in units
                               group item by item.ChannelNumber into itemgroup
                               select itemgroup;

                    case LabelField.InstrumentName:
                        return from item in units
                               group item by item.InstrumentName into itemgroup
                               select itemgroup;

                    case LabelField.MulticoreName:
                        return from item in units
                               group item by item.MulticoreName into itemgroup
                               select itemgroup;

                    case LabelField.Position:
                        return from item in units
                               group item by item.Position into itemgroup
                               select itemgroup;

                    case LabelField.UserField1:
                        return from item in units
                               group item by item.UserField1 into itemgroup
                               select itemgroup;

                    case LabelField.UserField2:
                        return from item in units
                               group item by item.UserField2 into itemgroup
                               select itemgroup;
                    case LabelField.UserField3:
                        return from item in units
                               group item by item.UserField3 into itemgroup
                               select itemgroup;

                    case LabelField.UserField4:
                        return from item in units
                               group item by item.UserField4 into itemgroup
                               select itemgroup;
                    default:
                        return new List<List<DimmerDistroUnit>>() as IEnumerable<IEnumerable<DimmerDistroUnit>>;
                }
            }

            // Group by Original Imported Name
            else
            {
                switch (labelfield)
                {
                    case LabelField.ChannelNumber:
                        return from item in units
                               group item by item.LastImportedChannelNumber into itemgroup
                               select itemgroup;

                    case LabelField.InstrumentName:
                        return from item in units
                               group item by item.LastImportedInstrumentName into itemgroup
                               select itemgroup;

                    case LabelField.MulticoreName:
                        return from item in units
                               group item by item.LastImportedMulticoreName into itemgroup
                               select itemgroup;

                    case LabelField.Position:
                        return from item in units
                               group item by item.LastImportedPosition into itemgroup
                               select itemgroup;

                    case LabelField.UserField1:
                        return from item in units
                               group item by item.LastImportedUserField1 into itemgroup
                               select itemgroup;

                    case LabelField.UserField2:
                        return from item in units
                               group item by item.LastImportedUserField2 into itemgroup
                               select itemgroup;
                    case LabelField.UserField3:
                        return from item in units
                               group item by item.LastImportedUserField3 into itemgroup
                               select itemgroup;

                    case LabelField.UserField4:
                        return from item in units
                               group item by item.LastImportedUserField4 into itemgroup
                               select itemgroup;
                    default:
                        return new List<List<DimmerDistroUnit>>() as IEnumerable<IEnumerable<DimmerDistroUnit>>;
                }
            }

            
        }

        public List<UnitGroup> CreateColorUnitGroups(ICollection<DimmerDistroUnit> units,
            LabelField labelField, ColorDictionary dimmerColorDictionary,
            ColorDictionary distroColorDictionary)
        {
            var unitGroups = new List<UnitGroup>();

            var query = GetLabelFieldGroups(labelField, UnitGroupBy.ShortName, units);

            foreach (var element in query)
            {
                if (element.Count() > 0)
                {
                    var unitGroup = new UnitGroup(dimmerColorDictionary, distroColorDictionary)
                    {
                        Name = element.First().GetData(labelField),
                        Units = element.ToList(),
                    };

                    unitGroups.Add(unitGroup);
                }
            }

            return unitGroups;
        }

        public List<UnitGroup> CreateNameUnitGroups(ICollection<DimmerDistroUnit> units,
            LabelField labelField, UnitGroupBy unitGroupBy)
        {
            var unitGroups = new List<UnitGroup>();

            var query = GetLabelFieldGroups(labelField, unitGroupBy, units);

            foreach (var element in query)
            {
                if (element.Count() > 0)
                {
                    var firstUnit = element.First();

                    var unitGroup = new UnitGroup(firstUnit.GetOriginalData(labelField),
                        firstUnit.GetData(labelField), labelField)
                    {
                        Units = element.ToList()
                    };

                    unitGroups.Add(unitGroup);
                }
            }

            return unitGroups;
        }
        #endregion
    }
}

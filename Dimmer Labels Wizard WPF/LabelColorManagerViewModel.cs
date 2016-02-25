using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelColorManagerViewModel : ViewModelBase
    {
        public LabelColorManagerViewModel()
        {
            RefreshUnitGroups(LabelField.Position);
        }

        #region Binding Source Properties

        protected ObservableCollection<UnitGroup> _UnitGroups = new ObservableCollection<UnitGroup>();

        public ObservableCollection<UnitGroup> UnitGroups
        {
            get { return _UnitGroups; }
            set
            {
                if (_UnitGroups != value)
                {
                    _UnitGroups = value;

                    // Notify.
                    OnPropertyChanged(nameof(UnitGroups));
                }
            }
        }


        protected UnitGroup _SelectedUnitGroup;

        public UnitGroup SelectedUnitGroup
        {
            get { return _SelectedUnitGroup; }
            set
            {
                if (_SelectedUnitGroup != value)
                {
                    _SelectedUnitGroup = value;

                    // Set Color.
                    if (value != null)
                    {
                        SelectedUnitGroupColor = value.UnitGroupColor;
                    }
                    
                    else
                    {
                        SelectedUnitGroupColor = Colors.White;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUnitGroup));
                }
            }
        }


        protected Color _SelectedUnitGroupColor = Colors.White;

        public Color SelectedUnitGroupColor
        {
            get { return _SelectedUnitGroupColor; }
            set
            {
                if (_SelectedUnitGroupColor != value)
                {
                    _SelectedUnitGroupColor = value;

                    // Update Model.
                    if (SelectedUnitGroup != null)
                    {
                        SelectedUnitGroup.UnitGroupColor = value;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUnitGroupColor));
                }
            }
        }

        public LabelField[] LabelFields
        {
            get
            {
                return new LabelField[] {LabelField.ChannelNumber, LabelField.InstrumentName, LabelField.Position,
                LabelField.MulticoreName, LabelField.UserField1, LabelField.UserField2,
                    LabelField.UserField3, LabelField.UserField4};
            }
        }


        protected LabelField _SelectedLabelField = LabelField.Position;

        public LabelField SelectedLabelField
        {
            get { return _SelectedLabelField; }
            set
            {
                if (_SelectedLabelField != value)
                {
                    _SelectedLabelField = value;

                    RefreshUnitGroups(value);

                    // Notify.
                    OnPropertyChanged(nameof(SelectedLabelField));
                }
            }
        }
        #endregion

        #region Methods      
        protected void RefreshUnitGroups(LabelField labelField)
        {
            UnitGroups.Clear();

            var query = GetLabelFieldGroups(labelField);

            foreach (var element in query)
            {
                if (element.Count() > 0)
                {
                    var unitGroup = new UnitGroup()
                    {
                        Name = element.First().GetData(labelField),
                        Units = element,
                    };

                    UnitGroups.Add(unitGroup);
                }
            }
        }

        protected IEnumerable<IEnumerable<DimmerDistroUnit>> GetLabelFieldGroups(LabelField labelfield)
        {
            switch (labelfield)
            {
                case LabelField.ChannelNumber:
                    return from item in Globals.DimmerDistroUnits
                           group item by item.ChannelNumber into itemgroup
                           select itemgroup;
                    
                case LabelField.InstrumentName:
                    return from item in Globals.DimmerDistroUnits
                           group item by item.InstrumentName into itemgroup
                           select itemgroup;
                case LabelField.MulticoreName:
                    return from item in Globals.DimmerDistroUnits
                           group item by item.ChannelNumber into itemgroup
                           select itemgroup;
                case LabelField.Position:
                    return from item in Globals.DimmerDistroUnits
                           group item by item.Position into itemgroup
                           select itemgroup;
                case LabelField.UserField1:
                    return from item in Globals.DimmerDistroUnits
                           group item by item.UserField1 into itemgroup
                           select itemgroup;
                case LabelField.UserField2:
                    return from item in Globals.DimmerDistroUnits
                           group item by item.UserField2 into itemgroup
                           select itemgroup;
                case LabelField.UserField3:
                    return from item in Globals.DimmerDistroUnits
                           group item by item.UserField3 into itemgroup
                           select itemgroup;
                case LabelField.UserField4:
                    return from item in Globals.DimmerDistroUnits
                           group item by item.UserField4 into itemgroup
                           select itemgroup;
                default:
                    return new List<List<DimmerDistroUnit>>() as IEnumerable<IEnumerable<DimmerDistroUnit>>;
            }
        }
          
        #endregion
    }
}

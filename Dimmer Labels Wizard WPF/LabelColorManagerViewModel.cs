using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Dimmer_Labels_Wizard_WPF.Repositories;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelColorManagerViewModel : ViewModelBase
    {
        public LabelColorManagerViewModel()
        {
            // Repositories.
            var context = new PrimaryDB();
            _UnitRepository = new UnitRepository(context);
            _LocalUnits = _UnitRepository.GetUnits();

            _ColorDictionaryRepository = new ColorDictionaryRepository(context);
            _DimmerColorDictionary = _ColorDictionaryRepository.DimmerColorDictionary;
            _DistroColorDictionary = _ColorDictionaryRepository.DistroColorDictionary;

            RefreshUnitGroups(LabelField.Position);

            // Commands.
            _OkCommand = new RelayCommand(OkCommandExecute);
            _CancelCommand = new RelayCommand(CancelCommandExecute);

        }

        // Fields.
        protected UnitRepository _UnitRepository;
        protected ColorDictionaryRepository _ColorDictionaryRepository;
        protected IList<DimmerDistroUnit> _LocalUnits;

        protected ColorDictionary _DimmerColorDictionary;
        protected ColorDictionary _DistroColorDictionary;

        protected UnitGroupFactory _UnitGroupFactory = new UnitGroupFactory();

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

        protected Color _SelectedUnitGroupColor = Colors.White;

        public Color SelectedUnitGroupColor
        {
            get
            {
                return _SelectedUnitGroupColor;
            }
            set
            {
                if (_SelectedUnitGroupColor != value)
                {
                    _SelectedUnitGroupColor = value;

                    // Update Model.
                    foreach (var unitGroup in SelectedUnitGroups)
                    {
                        foreach (var unit in unitGroup.Units)
                        {
                            // Dimmer.
                            if (unit.RackUnitType == RackType.Dimmer)
                            {
                                if (_DimmerColorDictionary.ContainsKey(unit.UniverseNumber, unit.DimmerNumber))
                                {
                                    _DimmerColorDictionary[unit.UniverseNumber, unit.DimmerNumber] = value;
                                }

                                else
                                {
                                    _DimmerColorDictionary.Add(unit.UniverseNumber, unit.DimmerNumber, value);
                                }
                            }

                            // Distro.
                            if (unit.RackUnitType == RackType.Distro)
                            {
                                if (_DistroColorDictionary.ContainsKey(unit.UniverseNumber, unit.DimmerNumber))
                                {
                                    _DistroColorDictionary[unit.UniverseNumber, unit.DimmerNumber] = value;
                                }

                                else
                                {
                                    _DistroColorDictionary.Add(unit.UniverseNumber, unit.DimmerNumber, value);
                                }
                            }
                        }

                        unitGroup.InvalidateDisplayedBrush();
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

        #region Commands.

        protected RelayCommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _CancelCommand;
            }
        }

        protected void CancelCommandExecute(object parameter)
        {
            var window = parameter as LabelColorManager;

            window.DialogResult = false;

            window.Close();
        }

        protected RelayCommand _OkCommand;
        public ICommand OkCommand
        {
            get
            {
                return _OkCommand;
            }
        }

        protected void OkCommandExecute(object parameter)
        {
            var window = parameter as LabelColorManager;

            // Commit Changes to DB.
            _ColorDictionaryRepository.Update(_DimmerColorDictionary);
            _ColorDictionaryRepository.Update(_DistroColorDictionary);
            _ColorDictionaryRepository.Save();

            window.DialogResult = true;
            window.Close();
        }

        #endregion

        #region Non Binding Properties.
        protected List<UnitGroup> _SelectedUnitGroups = new List<UnitGroup>();

        public List<UnitGroup> SelectedUnitGroups
        {
            get { return _SelectedUnitGroups; }
            set
            {
                if (_SelectedUnitGroups != value)
                {
                    _SelectedUnitGroups = value;
                }
            }
        }

        #endregion

        #region Methods
        protected void RefreshUnitGroups(LabelField labelField)
        {
            foreach (var element in UnitGroups)
            {
                element.PropertyChanged -= UnitGroup_PropertyChanged;
            }

            UnitGroups.Clear();

            var newUnitGroups = _UnitGroupFactory.CreateColorUnitGroups(_LocalUnits, labelField,
                _DimmerColorDictionary, _DistroColorDictionary);


            foreach (var unitGroup in newUnitGroups)
            {
                // Attach Event Handler and Add to UnitGroups Collection.
                unitGroup.PropertyChanged += UnitGroup_PropertyChanged;
                UnitGroups.Add(unitGroup);
            }
        }

        #endregion

        #region Event Handlers.
        private void UnitGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var unitGroup = sender as UnitGroup;

            if (e.PropertyName == "IsSelected")
            {
                bool isSelected = unitGroup.IsSelected;

                if (isSelected)
                {
                    // Selected.
                    if (SelectedUnitGroups.Contains(unitGroup) == false)
                    {
                        SelectedUnitGroups.Add(unitGroup);
                    }
                }

                else
                {
                    // Deselected.
                    SelectedUnitGroups.Remove(unitGroup);
                }

                // Set Color.
                if (SelectedUnitGroups.Count > 0)
                {
                    _SelectedUnitGroupColor = SelectedUnitGroups.First().UnitGroupColor;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUnitGroupColor));
                }

                else
                {
                    _SelectedUnitGroupColor = Colors.White;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUnitGroupColor));
                }
            }

            Console.WriteLine(SelectedUnitGroups.Count);

        }
        #endregion
    }
}

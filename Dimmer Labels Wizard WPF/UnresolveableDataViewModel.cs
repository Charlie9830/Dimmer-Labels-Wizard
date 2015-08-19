using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Dimmer_Labels_Wizard_WPF
{
    public class UnresolveableDataViewModel : ViewModelBase
    {
        public UnresolveableDataViewModel()
        {
            CollectUnresolveableUnits();
        }

        protected ObservableCollection<DimmerDistroUnit> _UnresolveableUnits = new ObservableCollection<DimmerDistroUnit>();
        protected bool _OmitAll = false;

        #region Getters/Setters
        public ObservableCollection<DimmerDistroUnit> UnresolveableUnits
        {
            get
            {
                return _UnresolveableUnits;
            }
            set
            {
                _UnresolveableUnits = value;
                OnPropertyChanged("UnresolveableUnits");
            }
        } 

        public bool OmitAll
        {
            get
            {
                return _OmitAll;
            }
            set
            {
                _OmitAll = value;

                foreach (var element in _UnresolveableUnits)
                {
                    element.OmitUnit = value;
                }

                OnPropertyChanged("UnresolveableUnits");
                OnPropertyChanged("OmitAll");
            }
        }

        public string DimmerImportFormat
        {
            get
            {
                return GetDimmerImportFormat();
            }
        }

        public string DistroImportFormat
        {
            get
            {
                return GetDistroImportFormat();
            }
        }
        #endregion

        #region General Methods
        protected void CollectUnresolveableUnits()
        {
            foreach (var element in Globals.UnresolvableUnits)
            {
                _UnresolveableUnits.Add(element);
            }
        }

        protected string GetDimmerImportFormat()
        {
            switch (UserParameters.DimmerImportFormat)
            {
                case ImportFormatting.Format1:
                    return "#/### (eg: Universe Number / Dimmer Number)";
                case ImportFormatting.Format2:
                    return "### (eg: Dimmer Number)";
                case ImportFormatting.Format3:
                    return "A### (eg: Universe Letter  Dimmer Number)";
                case ImportFormatting.Format4:
                   return "A/### (eg: Universe Letter / Dimmer Number)";
                case ImportFormatting.NoUniverseData:
                   return string.Empty;
                case ImportFormatting.NoAssignment:
                    return "No Format Selected";
                default:
                    return "No Format Selected";
            }
        }

        protected string GetDistroImportFormat()
        {
            switch (UserParameters.DistroImportFormat)
            {
                case ImportFormatting.Format1:
                    return "A### or AA### (eg: N160 or ND160)";
                case ImportFormatting.Format2:
                    return "###";
                case ImportFormatting.Format3:
                    return "#/### (eg: Rack Number / Non Dim Number)";
                case ImportFormatting.Format4:
                    return "A/### (eg: Rack Letter / Non Dim Number)";
                case ImportFormatting.NoUniverseData:
                    return "No Format Selected";
                case ImportFormatting.NoAssignment:
                    return "No Format Selected";
                default:
                    return "No Format Selected";
            }
        }
        #endregion

        #region Update Methods
        // Initiate another Parse Attempt on _UnresolvableUnits. Returns True if All units were Correctly Parsed.
        public bool UpdateModel()
        {
            // Clear Unresolveable Units.
            Globals.UnresolvableUnits.Clear();

            // Call each Unresolveable Unit to Attempt to Parse it's data. It will bounce back into Globals.UnResolveableUnits
            // if Parse was unsucsefful.
            foreach (var element in _UnresolveableUnits)
            {
                // Parse if not Flagged to be Omitted.
                if (element.OmitUnit == false)
                {
                    element.ParseUnitData();
                }

                // Remove from Globals.DimmerDistroUnits if Flagged to be ommited.
                else
                {
                    if (Globals.DimmerDistroUnits.Contains(element))
                    {
                        Globals.DimmerDistroUnits.Remove(element);
                    }
                }
            }

            if (Globals.UnresolvableUnits.Count == 0)
            {
                return true;
            }

            else
            {
                _UnresolveableUnits.Clear();
                CollectUnresolveableUnits();
                return false;
            }
        }
        #endregion
    }
}

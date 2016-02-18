using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class Strip : ViewModelBase
    {
        public Strip()
        {
            
        }


        #region Fields.
        // Unique Cell Templates.
        public ObservableCollection<LabelCellTemplate> UpperUniqueCellTemplates = new ObservableCollection<LabelCellTemplate>();
        public ObservableCollection<LabelCellTemplate> LowerUniqueCellTemplates = new ObservableCollection<LabelCellTemplate>();

        // Mergers.
        public ObservableCollection<Merge> Mergers = new ObservableCollection<Merge>();
        #endregion

        #region Properties.
        private ObservableCollection<DimmerDistroUnit> _Units = 
            new ObservableCollection<DimmerDistroUnit>();

        public ObservableCollection<DimmerDistroUnit> Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        public int UnitCount
        {
            get
            {
                return Units.Count;
            }
        }

        private LabelStripTemplate _AssignedTemplate;

        public LabelStripTemplate AssignedTemplate
        {
            get { return _AssignedTemplate; }
            set
            {
                if (value != _AssignedTemplate)
                {
                    _AssignedTemplate = value;
                }
            }
        }

        #endregion

        #region Binding Sources.

        protected bool _IsPoolSelected;

        public bool IsPoolSelected
        {
            get { return _IsPoolSelected; }
            set
            {
                if (_IsPoolSelected != value)
                {
                    _IsPoolSelected = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsPoolSelected));
                }
            }
        }


        protected bool _IsAssignedSelected;

        public bool IsAssignedSelected
        {
            get { return _IsAssignedSelected; }
            set
            {
                if (_IsAssignedSelected != value)
                {
                    _IsAssignedSelected = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsAssignedSelected));
                }
            }
        }

        #endregion


        #region Overrides.
        public override string ToString()
        {
            string returnValue = "No Units Assigned";

            if (Units.Count > 0)
            {
                returnValue = Units.First().RackUnitType.ToString() + " : ";
                returnValue += "Dimmer Number " + Units.First().DimmerNumber + ">" + Units.Last().DimmerNumber;
            }

            return returnValue;
        }
        #endregion

    }
}

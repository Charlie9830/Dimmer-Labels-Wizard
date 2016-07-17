using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimmer_Labels_Wizard_WPF.Repositories;



namespace Dimmer_Labels_Wizard_WPF
{
    public class Strip : ViewModelBase
    {
        public Strip()
        {
        }

        #region Fields.
        // Unique Cell Templates.
        public virtual ObservableCollection<LabelCellTemplate> UpperUniqueCellTemplates { get; set; } =
            new ObservableCollection<LabelCellTemplate>();
        public virtual ObservableCollection<LabelCellTemplate> LowerUniqueCellTemplates { get; set; } =
            new ObservableCollection<LabelCellTemplate>();

        // Mergers.
        public virtual List<Merge> Mergers { get; set; } = new List<Merge>();

        // Database.
        public int ID { get; set; }

        #endregion

        #region Properties.
        private LabelStripTemplate _AssignedTemplate = null;

        public LabelStripTemplate AssignedTemplate
        {
            get { return _AssignedTemplate; }
            set
            {
                if (value != _AssignedTemplate)
                {
                    OnPropertyChanged(nameof(Name));
                    _AssignedTemplate = value;
                }
            }
        }


        protected int _Universe = 0;

        public int Universe
        {
            get { return _Universe; }
            set
            {
                if (_Universe != value)
                {
                    _Universe = value;

                    // Notify.
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        protected int _FirstDimmer = 0;

        public int FirstDimmer
        {
            get { return _FirstDimmer; }
            set
            {
                if (_FirstDimmer != value)
                {
                    _FirstDimmer = value;

                    // Notify.
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        protected int _LastDimmer = 0;

        public int LastDimmer
        {
            get { return _LastDimmer; }
            set
            {
                if (_LastDimmer != value)
                {
                    _LastDimmer = value;

                    // Notify.
                    OnPropertyChanged(nameof(Name));
                }
            }
        }


        protected RackType _RackType = RackType.Dimmer;

        public RackType RackType
        {
            get { return _RackType; }
            set
            {
                if (_RackType != value)
                {
                    _RackType = value;

                    // Notify.
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [NotMapped]
        public string Name
        {
            get
            {
                return GetName();
            }
        }

        protected bool _IsPoolSelected;
        [NotMapped]
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
        [NotMapped]
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


        protected bool _IsSelected;
        [NotMapped]
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        #endregion

        #region Methods.
        public IEnumerable<DimmerDistroUnit> GetUnits(UnitRepository unitRepository)
        {
            if (FirstDimmer == 0 && LastDimmer == 0)
            {
                // Return an Empty collection.
                return new List<DimmerDistroUnit>() as IEnumerable<DimmerDistroUnit>;
            }
            
            if (RackType == RackType.Dimmer)
            {
                // Dimmer.
                var query = from unit in unitRepository.GetDimmersSorted()
                            where unit.UniverseNumber == Universe
                            select unit;

                return query.Skip(FirstDimmer - 1).Take((LastDimmer + 1) - FirstDimmer);
            }

            else
            {
                // Distro.
                return unitRepository.GetDistrosSorted().Skip(FirstDimmer - 1).Take((LastDimmer + 1) - FirstDimmer);
            }
        }

        private string GetName()
        {
            if (RackType == RackType.Distro || RackType == RackType.Dimmer && Universe == 0)
            {
                // Distro or Dimmer with No Universe Data.
                return RackType.ToString() + " : " + FirstDimmer + " to " + LastDimmer;
            }

            else
            {
                // Dimmer with Universe Info.
                return RackType.ToString() + " : " + Universe + '/' + FirstDimmer + " to " + Universe + '/' + LastDimmer;
            }
            
        }

        #endregion
    }
}

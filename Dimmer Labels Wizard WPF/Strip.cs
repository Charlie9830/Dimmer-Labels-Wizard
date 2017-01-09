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
using System.Runtime.Serialization;

namespace Dimmer_Labels_Wizard_WPF
{
    [DataContract]
    public class Strip : ViewModelBase
    {
        public Strip()
        {
        }

        // Database and Navigation Properties.
        public int ID { get; set; }
        public virtual ICollection<StripAddress> StripAddresses { get; set; }

        #region Properties.
        // Unique Cell Templates.
        [DataMember]
        public virtual List<LabelCellTemplate> UniqueCellTemplates { get; set; } = new List<LabelCellTemplate>();

        // Mergers.
        [DataMember]
        public virtual List<Merge> Mergers { get; set; } = new List<Merge>();

        private LabelStripTemplate _AssignedTemplate = null;
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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


        protected bool _IsSelectedForPrinting;
        [NotMapped]
        public bool IsSelectedForPrinting
        {
            get { return _IsSelectedForPrinting; }
            set
            {
                if (_IsSelectedForPrinting != value)
                {
                    _IsSelectedForPrinting = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsSelectedForPrinting));
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

        [NotMapped]
        public List<LabelCellTemplateWrapper> FilteredUniqueCellTemplates
        {
            get
            {
                return GetFilteredUniqueCellTemplates();
            }
        }

        #endregion

        #region Methods.
        protected List<LabelCellTemplateWrapper> GetFilteredUniqueCellTemplates()
        {
            var returnValue = new List<LabelCellTemplateWrapper>();

            foreach (var element in UniqueCellTemplates)
            {
                var filteredAddresses = element.StripAddresses.Where(item => item.Strip == this).ToList();

                returnValue.Add(new LabelCellTemplateWrapper()
                {
                    CellTemplate = element,
                    FilteredStripAddresses = filteredAddresses
                });
            }

            return returnValue;
        }


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

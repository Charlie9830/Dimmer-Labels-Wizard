using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class TemplateEditorViewModel : ViewModelBase
    {
        #region Constructor
        public TemplateEditorViewModel()
        {
            BasedOnTemplateSelection = Globals.DefaultTemplate;
        }
        #endregion

        #region Fields.
        // Constants
        protected const double UnitConversionRatio = 96d / 25.4d;
        #endregion

        #region Binding Properties.

        public List<DimmerDistroUnit> ExampleUnits
        {
            get { return GenerateExampleUnits(); }
        }
        
        protected LabelStripTemplate _DisplayedTemplate;

        public LabelStripTemplate DisplayedTemplate
        {
            get
            {
                return _DisplayedTemplate;
            }
            set
            {
                if (_DisplayedTemplate != value)
                {
                    _DisplayedTemplate = value;
                    OnPropertyChanged(nameof(DisplayedTemplate));
                }
            }
        }

        protected LabelCellTemplate _DisplayedUpperCellTemplate;

        public LabelCellTemplate DisplayedUpperCellTemplate
        {
            get
            {
                return _DisplayedUpperCellTemplate;
            }
            set
            {
                // Here.
            }
        }

        public string TemplateName
        {
            get
            {
                return DisplayedTemplate.Name;
            }

            set
            {
                if (DisplayedTemplate.Name != value)
                {
                    DisplayedTemplate.Name = value;
                    OnPropertyChanged(nameof(TemplateName));
                }
            }
        }

        public IEnumerable<LabelStripTemplate> BasedOnTemplates
        {
            get
            {
                return Globals.Templates;
            }
        }

        protected LabelStripTemplate _BasedOnTemplateSelection;

        public LabelStripTemplate BasedOnTemplateSelection
        {
            get
            {
                return _BasedOnTemplateSelection;
            }

            set
            {
                if (_BasedOnTemplateSelection != value)
                {
                    _BasedOnTemplateSelection = value;
                    DisplayedTemplate = new LabelStripTemplate(_BasedOnTemplateSelection);

                    // Notify other Controls.
                    OnBasedOnTemplateSelectionChanged();

                    // Notify.
                    OnPropertyChanged(nameof(BasedOnTemplateSelection));
                }
            }
        }

        public double StripWidth
        {
            get
            {
                return DisplayedTemplate.StripWidth / UnitConversionRatio;
            }

            set
            {
                if (DisplayedTemplate.StripWidth / UnitConversionRatio != value)
                {
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        Name = TemplateName,
                        StripWidth = value * UnitConversionRatio
                    };
                }
            }
        }

        public double StripHeight
        {
            get
            {
                return DisplayedTemplate.StripHeight / UnitConversionRatio;
            }
            set
            {
                if (DisplayedTemplate.StripHeight / UnitConversionRatio != value)
                {
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        Name = TemplateName,
                        StripHeight = value * UnitConversionRatio
                    };
                }
            }
        }

        public IEnumerable<LabelStripMode> StripModes
        {
            get
            {
                return new LabelStripMode[] { LabelStripMode.Dual, LabelStripMode.Single };
                
            }
        }

        public LabelStripMode SelectedStripMode
        {
            get
            {
                return DisplayedTemplate.StripMode;
            }
            set
            {
                if (DisplayedTemplate.StripMode != value)
                {
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        Name = TemplateName,
                        StripMode = value
                    };
                }
            }
        }

        public IEnumerable<CellDataMode> CellDataModes
        {
            get
            {
                return new CellDataMode[] { CellDataMode.MixedField, CellDataMode.SingleField };

            }
        }

        public CellDataMode SelectedCellDataMode
        {
            get
            {
                return CellDataMode.MixedField;

                // ****************************************************************************************
                // You were Coding the DisplayedUpperCellTemplate Getter/Setter.
            }
        }


        #endregion

        #region Methods
        private void OnBasedOnTemplateSelectionChanged()
        {
            TemplateName = string.Empty;

            // Raise Property Changed Notifications.
            OnPropertyChanged(nameof(StripWidth));
            OnPropertyChanged(nameof(StripHeight));
            OnPropertyChanged(nameof(SelectedStripMode));
        }

        private List<DimmerDistroUnit> GenerateExampleUnits()
        {
            var exampleUnit = new DimmerDistroUnit()
            {
                ChannelNumber = "Chan",
                InstrumentName = "Instr",
                Position = "Position",
                MulticoreName = "Multi",
                UserField1 = "User Field 1",
                UserField2 = "User Field 2",
                UserField3 = "User Field 3",
                UserField4 = "User Field 4",
                Custom = "Custom",
            };

            var examples = new List<DimmerDistroUnit>();

            for (int count = 1; count <= 12; count++)
            {
                examples.Add(exampleUnit);
            }

            return examples;
        }
        #endregion
    }
}

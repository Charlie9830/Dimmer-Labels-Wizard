using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Dimmer_Labels_Wizard_WPF.Repositories;

namespace Dimmer_Labels_Wizard_WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor.
        public MainWindowViewModel()
        {
            // Repositories.
            PrimaryDB context = new PrimaryDB();
            _UnitRepository = new UnitRepository(context);
            _TemplateRepository = new TemplateRepository(context);
            _StripRepository = new StripRepository(context);
            _ColorDictionaryRepository = new ColorDictionaryRepository(context);

            // Initialize.
            InitializeDatabase();

            // Commands.
            _NewProjectCommand = new RelayCommand(NewProjectCommandExecute);

        }
        #endregion

        protected UnitRepository _UnitRepository;
        protected TemplateRepository _TemplateRepository;
        protected StripRepository _StripRepository;
        protected ColorDictionaryRepository _ColorDictionaryRepository;

        #region Commands.

        protected RelayCommand _NewProjectCommand;
        public ICommand NewProjectCommand
        {
            get
            {
                return _NewProjectCommand;
            }
        }

        protected void NewProjectCommandExecute(object parameter)
        {
            // Clear Database.
            _UnitRepository.RemoveAllUnits();
            _TemplateRepository.RemoveAllUserTemplates();
            _StripRepository.RemoveAll();
            _ColorDictionaryRepository.RemoveAll();

            var ImportWindow = new ImportUnitsWindow();
            var viewModel = ImportWindow.DataContext as ImportUnitsViewModel;
            viewModel.InSetup = true;

            ImportWindow.Show();
        }
        #endregion

        #region Methods.
        protected void InitializeDatabase()
        {
            var query = from template in _TemplateRepository.GetTemplates()
                        where template.IsBuiltIn == true &&
                        template.Name == "Default"
                        select template;

            if (query.Count() > 0)
            {
                return;
            }

            else
            {
                _TemplateRepository.InsertTemplate(ConstructDefaultTemplate());
                _TemplateRepository.Save();
            }
        }

        protected LabelStripTemplate ConstructDefaultTemplate()
        {
            // Construct the Default LabelStrip Template.

            // Upper Strip is in Single Field Mode.
            // Lower Strip Cell Row Templates.
            var lowerTopRow = new CellRowTemplate()
            {
                DataField = LabelField.MulticoreName,
                Font = new Typeface("Arial"),
                DesiredFontSize = 12d,
            };

            var lowerMiddleRow = new CellRowTemplate()
            {
                DataField = LabelField.ChannelNumber,
                Font = new Typeface("Arial"),
                DesiredFontSize = 16d,
            };

            var lowerBottomRow = new CellRowTemplate()
            {
                DataField = LabelField.InstrumentName,
                Font = new Typeface("Arial"),
                DesiredFontSize = 12d,
            };

            // Upper Cell Template.
            var upperCellTemplate = new LabelCellTemplate()
            {
                CellDataMode = CellDataMode.SingleField,
                RowHeightMode = CellRowHeightMode.Static,
                SingleFieldDataField = LabelField.Position,
                SingleFieldDesiredFontSize = 16d,
                SingleFieldFont = new Typeface("Arial"),
            };

            // Generate List of CellRowTemplates for construction of lowerCellTemplate.
            var lowerCellRowTemplates = new List<CellRowTemplate>();
            lowerCellRowTemplates.Add(lowerTopRow);
            lowerCellRowTemplates.Add(lowerMiddleRow);
            lowerCellRowTemplates.Add(lowerBottomRow);

            var lowerCellTemplate = new LabelCellTemplate()
            {
                CellDataMode = CellDataMode.MultiField,
                RowHeightMode = CellRowHeightMode.Automatic,
                CellRowTemplates = lowerCellRowTemplates,
            };


            // Generate LabelStripTemplate
            var labelStripTemplate = new LabelStripTemplate()
            {
                Name = "Default",
                IsBuiltIn = true,
                StripMode = LabelStripMode.Dual,
                StripHeight = 70d,
                UpperCellTemplate = upperCellTemplate,
                LowerCellTemplate = lowerCellTemplate,
            };

            // Templates.Add(labelStripTemplate);
            return labelStripTemplate;
        }
        #endregion
    }
}

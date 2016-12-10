using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Printing;
using Dimmer_Labels_Wizard_WPF.Repositories;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class PrintWindowViewModel : ViewModelBase
    {
        // Repositories.
        StripRepository _StripRepo;
        UnitRepository _UnitRepo;

        // Printing.
        PageMediaSize _PageSize = new PageMediaSize(PageMediaSizeName.ISOA4Rotated, 297 * _UnitConversionRatio, 210 * _UnitConversionRatio);

        // Private Fields.
        List<Strip> _Strips = new List<Strip>();
        const double _UnitConversionRatio = 96d / 25.4d;
        const double _BetweenStripMargin = 45d;
        double _PrintMargin = 10d * _UnitConversionRatio;


        #region Constructor
        public PrintWindowViewModel()
        {
            var context = new PrimaryDB();
            _StripRepo = new StripRepository(context);
            _UnitRepo = new UnitRepository(context);

            _Strips = _StripRepo.GetStrips().ToList();

            // Populate StripsCollectionView Sorting Descriptions and Source.
            _StripsCollectionView.SortDescriptions.Add(new SortDescription(nameof(Strip.Universe), ListSortDirection.Ascending));
            _StripsCollectionView.SortDescriptions.Add(new SortDescription(nameof(Strip.FirstDimmer), ListSortDirection.Ascending));
            _StripsCollectionView.Source = _Strips;

            // Command Bindings.
            _PrintCommand = new RelayCommand(_PrintCommandExecute);
        }

        #endregion

        #region Bindings.

        protected Array _PageSizes;

        public Array PageSizes
        {
            get { return Enum.GetValues(typeof(PageMediaSizeName)); }
        }


        protected PageMediaSizeName _PageSizeName = PageMediaSizeName.ISOA4Rotated;

        public PageMediaSizeName PageSizeName
        {
            get { return _PageSizeName; }
            set
            {
                if (_PageSizeName != value)
                {
                    _PageSizeName = value;

                    // Set PageSize.
                    // DISABLED TO LOCK PROGRAM TO A4 ROTATED SIZE
                    // ***** AN INBUILT WPF LIBRARY OF PAPER SIZES HAS NOT BEEN FOUND YET ********
                    //_PageSize = new PageMediaSize(value);

                    // Notify.
                    OnPropertyChanged(nameof(PageSizeName));
                }
            }
        }


        protected bool _PrintAll = true;

        public bool PrintAll
        {
            get { return _PrintAll; }
            set
            {
                if (_PrintAll != value)
                {
                    _PrintAll = value;

                    // Notify.
                    OnPropertyChanged(nameof(PrintAll));
                }
            }
        }


        protected bool _PrintAllDimmers;

        public bool PrintAllDimmers
        {
            get { return _PrintAllDimmers; }
            set
            {
                if (_PrintAllDimmers != value)
                {
                    _PrintAllDimmers = value;

                    // Notify.
                    OnPropertyChanged(nameof(PrintAllDimmers));
                }
            }
        }


        protected bool _PrintAllDistros;

        public bool PrintAllDistros
        {
            get { return _PrintAllDistros; }
            set
            {
                if (_PrintAllDistros != value)
                {
                    _PrintAllDistros = value;

                    // Notify.
                    OnPropertyChanged(nameof(PrintAllDistros));
                }
            }
        }


        protected bool _PrintSelection;

        public bool PrintSelection
        {
            get { return _PrintSelection; }
            set
            {
                if (_PrintSelection != value)
                {
                    _PrintSelection = value;

                    // Notify.
                    OnPropertyChanged(nameof(PrintSelection));
                }
            }
        }


        protected CollectionViewSource _StripsCollectionView = new CollectionViewSource();

        public CollectionViewSource StripsCollectionView
        {
            get { return _StripsCollectionView; }
        }
        #endregion

        #region Commands.

        protected RelayCommand _PrintCommand;
        public ICommand PrintCommand
        {
            get
            {
                return _PrintCommand;
            }
        }

        protected void _PrintCommandExecute(object parameter)
        {
            SendToPrinter();
        }

        #endregion

        #region
        protected void SendToPrinter()
        {
            var fixedPages = GeneratePages();
            var document = new FixedDocument();

            // Add Pages to FixedDocument.
            foreach (var fixedPage in fixedPages)
            {
                var pageContent = new PageContent();
                pageContent.Child = fixedPage;

                document.Pages.Add(pageContent);
            }

            PrintDialog dialog = new PrintDialog();
            dialog.PrintDocument(document.DocumentPaginator, "Dimmer System Labels");
        }

        protected List<FixedPage> GeneratePages()
        {
            List<FixedPage> _pages = new List<FixedPage>();

            // Calculate Printable Area Height.
            double printableAreaHeight = (double)_PageSize.Height - (_PrintMargin * 2);

            // Collect Strips to Print.
            List<Strip> stripsToPrint = CollectStripsToPrint();

            // Generate LabelStrips.
            var labelStrips = new List<LabelStrip>();
            foreach (var element in stripsToPrint)
            {
                labelStrips.Add(CreateNewLabelStrip(element));
            }

            // Page Flow Loop. Flow Strips into Each Page.
            int currentLabelStripIndex = 0;
            double availablePageHeight = printableAreaHeight;
            var printableAreaStackPanel = new StackPanel(); // This StackPanel is dimensioned to the PRINTABLE AREA of the Page.

            while (currentLabelStripIndex < labelStrips.Count)
            {
                // If First Iteration or Current Label Strip won't fit onto the page.
                if (_pages.Count == 0 || labelStrips[currentLabelStripIndex].StripHeight + (_BetweenStripMargin * 2) > availablePageHeight)
                {
                    // Create a new Page.
                    _pages.Add(CreateNewPage());

                    // Init a new StackPanel.
                    printableAreaStackPanel = new StackPanel();
                    printableAreaStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    printableAreaStackPanel.VerticalAlignment = VerticalAlignment.Center;
                    printableAreaStackPanel.Margin = new Thickness(10d * _UnitConversionRatio);

                    // Reset AvailablePageHeight.
                    availablePageHeight = printableAreaHeight;

                    // Add it to the Page.
                    _pages.Last().Children.Add(printableAreaStackPanel);

                    // Set the LabelStrips Margin and Add it to the StackPanel.
                    labelStrips[currentLabelStripIndex].Margin = new Thickness(0, _BetweenStripMargin, 0, _BetweenStripMargin);
                    printableAreaStackPanel.Children.Add(labelStrips[currentLabelStripIndex]);

                    // Recalculate remaining space on the page.
                    availablePageHeight -= labelStrips[currentLabelStripIndex].StripHeight + (_BetweenStripMargin * 2);
                }


                else
                {
                    // Set the LabelStrips Margin and Add it to the StackPanel.
                    labelStrips[currentLabelStripIndex].Margin = new Thickness(0, _BetweenStripMargin, 0, _BetweenStripMargin);
                    printableAreaStackPanel.Children.Add(labelStrips[currentLabelStripIndex]);

                    // Recalculate remaining space on the page.
                    availablePageHeight -= labelStrips[currentLabelStripIndex].StripHeight + (_BetweenStripMargin * 2);
                }

                // Iterate.
                currentLabelStripIndex++;
            }

            return _pages;
        }
        protected LabelStrip CreateNewLabelStrip(Strip strip)
        {
            var labelStrip = new LabelStrip();

            // Load Units.
            labelStrip.DataSource = strip.GetUnits(_UnitRepo);

            // Load Merges.
            labelStrip.Mergers = strip.Mergers;

            // Load Template.
            labelStrip.Style = strip.AssignedTemplate.Style;

            // Load Unique UpperCellTemplates.
            labelStrip.UniqueUpperCellTemplates = strip.UpperUniqueCellTemplates;

            // Load Unique LowerCellTemplates.
            labelStrip.UniqueLowerCellTemplates = strip.LowerUniqueCellTemplates;
            return labelStrip;
        }


        protected FixedPage CreateNewPage()
        {
            var page = new FixedPage();
            
            // Set Dimensions.
            page.Width = (double)_PageSize.Width;
            page.Height = (double)_PageSize.Height;

            return page;
        }

        protected List<Strip> CollectStripsToPrint()
        {
            var stripsToPrint = new List<Strip>();

            if (PrintAll == true)
            {
                foreach (var element in _Strips)
                {
                    stripsToPrint.Add(element);
                }

                return stripsToPrint;
            }

            if (_PrintAllDimmers == true)
            {
                foreach (var element in _Strips)
                {
                    if (element.RackType == RackType.Dimmer)
                    {
                        stripsToPrint.Add(element);
                    }
                }

                return stripsToPrint;
            }

            if (_PrintAllDistros == true)
            {
                foreach (var element in _Strips)
                {
                    if (element.RackType == RackType.Distro)
                    {
                        stripsToPrint.Add(element);
                    }
                }

                return stripsToPrint;
            }

            if (_PrintSelection == true)
            {
                foreach (var element in _Strips)
                {
                    if (element.IsSelectedForPrinting == true)
                    {
                        stripsToPrint.Add(element);
                    }
                }

                return stripsToPrint;
            }

            return stripsToPrint;
        }
        #endregion
    }
}

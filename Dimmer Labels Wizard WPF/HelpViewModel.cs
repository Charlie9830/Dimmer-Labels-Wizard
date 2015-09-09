using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Dimmer_Labels_Wizard_WPF
{
    public class HelpViewModel : ViewModelBase
    {
        protected ObservableCollection<HelpTopic> _Topics = new ObservableCollection<HelpTopic>();
        protected HelpPage _SelectedHelpPage = new HelpPage();
        protected ObservableCollection<UserControl> _SelectedControl = new ObservableCollection<UserControl>();

        public HelpViewModel()
        {
            _Topics.CollectionChanged += _Topics_CollectionChanged;

            List<HelpPage> pages = new List<HelpPage>();
            pages.Add(new HelpPage("Page 1", new HelpScreens.LabelRanging()));

            _Topics.Add(new HelpTopic("Topic 1", pages));

        }

        #region Getters/Setters
        public ObservableCollection<HelpTopic> Topics
        {
            get
            {
                return _Topics;
            }
            set
            {
                _Topics = value;
                OnPropertyChanged("Topics");
            }
        }

        public HelpPage SelectedHelpPage
        {
            get
            {
                return _SelectedHelpPage;
            }
            set
            {
                _SelectedHelpPage = value;
                _SelectedControl.Clear();
                _SelectedControl.Add(value.Control);

                OnPropertyChanged("SelectedHelpPage");
                OnPropertyChanged("SelectedControl");
            }
        }

        public ObservableCollection<UserControl> SelectedControl
        {
            get
            {
                return _SelectedControl;
            }
            set
            {
                _SelectedControl = value;
                OnPropertyChanged("SelectedControl");
            }
        }
        #endregion

        #region Internal Event Handling
        private void _Topics_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                // Connect Incoming ViewModels.
                foreach (var element in e.NewItems)
                {
                    var viewModel = element as HelpTopic;
                    
                    foreach (var page in viewModel.HelpPages)
                    {
                        page.PropertyChanged += Page_PropertyChanged;
                    } 
                }
            }

            if (e.OldItems != null)
            {
                // Disconnect Outgoing ViewModels.
                foreach (var element in e.OldItems)
                {
                    var viewModel = element as HelpTopic;

                    foreach (var page in viewModel.HelpPages)
                    {
                        page.PropertyChanged -= Page_PropertyChanged;
                    }
                }
            }
        }

        private void Page_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                SelectedHelpPage = sender as HelpPage;
            }
        }
        #endregion
    }

    public class HelpTopic : ViewModelBase
    {
        protected string _Title = string.Empty;
        protected ObservableCollection<HelpPage> _HelpPages = new ObservableCollection<HelpPage>();

        public HelpTopic()
        {
        }

        public HelpTopic(string title, List<HelpPage> helpPages)
        {
            _Title = title;
            foreach (var element in helpPages)
            {
                _HelpPages.Add(element);
            }
        }

        #region Getters/Setters
        public string Title
        {
            get
            {
                return _Title;
            }
        }

        public ObservableCollection<HelpPage> HelpPages
        {
            get
            {
                return _HelpPages;
            }
            set
            {
                _HelpPages = value;
            }
        }

        #endregion
    }

    public class HelpPage : ViewModelBase
    {
        protected string _PageTitle = string.Empty;
        protected UserControl _Control = new UserControl();
        protected bool _IsSelected = false;

        public HelpPage()
        {
        }

        public HelpPage(string pageTitle, UserControl control)
        {
            _PageTitle = pageTitle;
            _Control = control;
        }

        #region Getters/Setters
        public string PageTitle
        {
            get
            {
                return _PageTitle;
            }
            set
            {
                _PageTitle = value;
            }
        }

        public UserControl Control
        {
            get
            {
                return _Control;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                // Property Name Directly Referenced in HelpViewModel.
                OnPropertyChanged("IsSelected");
            }
        }
        #endregion
    }
}

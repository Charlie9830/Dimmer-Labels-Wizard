using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelColorManagerViewModel : ViewModelBase
    {
        public LabelColorManagerViewModel()
        {
            _Units = CollectionViewSource.GetDefaultView(Globals.DimmerDistroUnits);

            
        }

        #region Binding Source Properties

        protected ICollectionView _Units;

        public ICollectionView Units
        {
            get { return _Units; }
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


        protected LabelField _SelectedLabelField;

        public LabelField SelectedLabelField
        {
            get { return _SelectedLabelField; }
            set
            {
                if (_SelectedLabelField != value)
                {
                    _SelectedLabelField = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedLabelField));
                }
            }
        }
        #endregion

        #region Methods

        
        #endregion
    }
}

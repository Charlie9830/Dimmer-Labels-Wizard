using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Dimmer_Labels_Wizard_WPF.Repositories
{
    public class StripRepository
    {
        public StripRepository(PrimaryDB context)
        {
            _Context = context;
        }

        private PrimaryDB _Context;

        #region Properties.
        public ObservableCollection<Strip> Local
        {
            get
            {
                return _Context.Strips.Local;
            }
        }

        public IList<Strip> GetStrips()
        {
            return (from strip in _Context.Strips
                    select strip).ToList();
        }


        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class CellCollection : ObservableCollection<LabelCell>
    {
        public LabelStrip Instance; 

        public CellCollection() : base()
        {

        }

        public CellCollection(LabelStrip ownerClassInstance) : base()
        {
            Instance = ownerClassInstance;
        }
    }
}

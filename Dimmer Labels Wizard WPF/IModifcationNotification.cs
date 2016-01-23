using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public delegate void NotifyModificationEventHandler(object sender, NotifyModificationEventArgs e);

    public interface INotifyModification
    {
        event NotifyModificationEventHandler NotifyModification;
    }
}

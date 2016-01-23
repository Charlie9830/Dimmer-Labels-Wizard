using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class NotifyModificationEventArgs : EventArgs
    {
        public NotifyModificationEventArgs(object target, object property, object oldValue)
        {
            _Target = target;
            _Property = property;
            _OldValue = oldValue;
        }

        protected object _Target;
        public object Target
        {
            get { return _Target; }
            set { _Target = value; }
        }

        protected object _Property;
        public object Property
        {
            get { return _Property; }
            set { _Property = value; }
        }
        
        protected object _OldValue;
        public object OldValue
        {
            get { return _OldValue; }
            set { _OldValue = value; }
        }
    }
}

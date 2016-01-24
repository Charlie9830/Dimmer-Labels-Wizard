using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public abstract class ModificationBase
    {
        // Target.
        public virtual object Target { get; set; }

        // Property.
        public virtual object Property { get; set; }

        // Value.
        public virtual object Value { get; set; }
    }

    public class DataModification : ModificationBase
    {
        public DataModification(DimmerDistroUnit target, string property, string value)
        {
            _Target = target;
            _Property = property;
            _Value = value;
        }

        // Target.
        protected DimmerDistroUnit _Target;
        public new DimmerDistroUnit Target
        {
            get
            {
                return _Target;
            }

            set
            {
                _Target = value;
            }
        }

        // Property.
        protected string _Property;
        public new string Property
        {
            get
            {
                return _Property;
            }

            set
            {
                _Property = value;
            }
        }

        // Value.
        protected string _Value;
        public new string Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;
            }
        }
    }

    
}

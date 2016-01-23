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
        public abstract object Target { get; set; }

        // Property.
        public abstract object Property { get; set; }

        // Value.
        public abstract object Value { get; set; }
    }

    public class DataModification : ModificationBase
    {
        // Target.
        protected DimmerDistroUnit _Target;
        public override object Target
        {
            get
            {
                return _Target;
            }

            set
            {
                _Target = (DimmerDistroUnit)value;
            }
        }

        // Property.
        protected string _Property;
        public override object Property
        {
            get
            {
                return _Property;
            }

            set
            {
                _Property = (string)value;
            }
        }

        // Value.
        protected string _Value;
        public override object Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = (string)value;
            }
        }
    }
}

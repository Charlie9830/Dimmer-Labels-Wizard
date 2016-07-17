using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class ValidationError
    {
        public ValidationError(DimmerDistroUnit unit, string errorMessage)
        {
            Unit = unit;
            ErrorMessage = errorMessage;
        }

        public DimmerDistroUnit Unit { get; set; }
        public string ErrorMessage { get; set; }
    }
}

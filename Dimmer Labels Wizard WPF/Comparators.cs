using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class SolidColorBrushComparer : IEqualityComparer<SolidColorBrush>
    {
        public bool Equals(SolidColorBrush x, SolidColorBrush y)
        {
            if (x.Color == y.Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(SolidColorBrush obj)
        {
            return obj.GetHashCode();
        }
    }

    /// <summary>
    /// Equality Comparer for CellRows. Performs a Value Comparison of CellRow.Data AND CellRow.DataField.
    /// </summary>
    public class CellRowDataValueComparer : IEqualityComparer<CellRow>
    {
        public bool Equals(CellRow x, CellRow y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            if (x.Data == y.Data && x.DataField == y.DataField)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public int GetHashCode(CellRow obj)
        {
            int hCode = obj.Data.GetHashCode() ^ (int)obj.DataField;
            return hCode.GetHashCode();
        }
    }

    public class StripAddressComparer : IEqualityComparer<StripAddress>
    {
        public bool Equals(StripAddress x, StripAddress y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            if (x.Strip == y.Strip &&
                x.VerticalPosition == y.VerticalPosition &&
                x.RackUnitType == y.RackUnitType &&
                x.UniverseNumber == y.UniverseNumber &&
                x.DimmerNumber == y.DimmerNumber)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public int GetHashCode(StripAddress obj)
        {
            int hCode = obj.Strip.GetHashCode() ^
                obj.VerticalPosition.GetHashCode() ^
                obj.RackUnitType.GetHashCode() ^
                obj.UniverseNumber.GetHashCode() ^
                obj.DimmerNumber.GetHashCode();
             
            return hCode.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF.Repositories
{
    public interface IUnitRepository
    {
        // Gets.
        IList<DimmerDistroUnit> GetUnits();
        IList<DimmerDistroUnit> GetUnitsSorted();
        DimmerDistroUnit GetUnit(RackType rackType, int universe, int dimmerNumber);

        // Inserts.
        void InsertUnit(DimmerDistroUnit unit);
        void InsertUnitRange(IEnumerable<DimmerDistroUnit> units);

        // Updates.
        void UpdateUnit(DimmerDistroUnit unit);

        // Removes.
        void RemoveUnit(DimmerDistroUnit unit);
        void RemoveUnitRange(IEnumerable<DimmerDistroUnit> units);
        void RemoveAllUnits();

        // Save.
        void Save();
    }
}

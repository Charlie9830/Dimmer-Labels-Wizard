using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Dimmer_Labels_Wizard_WPF.Repositories
{
    public class UnitRepository : IUnitRepository, IDisposable
    {
        public UnitRepository(PrimaryDB context)
        {
            _Context = context;
        }

        private PrimaryDB _Context;

        public DimmerDistroUnit GetUnit(RackType rackType, int universe, int dimmerNumber)
        {
            return _Context.Units.Find(rackType, universe, dimmerNumber);
        }

        public IList<DimmerDistroUnit> GetUnits()
        {
            return (from unit in _Context.Units
                    select unit).ToList();
        }

        public IList<DimmerDistroUnit> GetUnitsSorted()
        {
            return (from unit in _Context.Units
                    orderby unit.RackUnitType
                    orderby unit.UniverseNumber
                    orderby unit.DimmerNumber
                    select unit).ToList();
        }

        public IList<DimmerDistroUnit> GetDimmersSorted()
        {
            return (from unit in GetUnitsSorted()
                   where unit.RackUnitType == RackType.Dimmer
                   select unit).ToList();
        }

        public IList<DimmerDistroUnit> GetDistrosSorted()
        {
            return (from unit in GetUnitsSorted()
                    where unit.RackUnitType == RackType.Distro
                    select unit).ToList();
        }

        public void InsertUnit(DimmerDistroUnit unit)
        {
            _Context.Units.Add(unit);
        }

        public void InsertUnitRange(IEnumerable<DimmerDistroUnit> units)
        {
            _Context.Units.AddRange(units);
        }

        public void RemoveAllUnits()
        {
            _Context.Database.ExecuteSqlCommand("DELETE from DimmerDistroUnitMerges");
            _Context.Database.ExecuteSqlCommand("DELETE from Merges");
            _Context.Database.ExecuteSqlCommand("DELETE from DimmerDistroUnits");
        }

        public void RemoveUnit(DimmerDistroUnit unit)
        {
            foreach (var element in unit.MergePrimaryUnit.ToList())
            {
                _Context.Entry(element).State = EntityState.Deleted;
            }

            foreach (var element in unit.MergeConsumedUnits.ToList())
            {
                _Context.Entry(element).State = EntityState.Deleted;
            }

            _Context.Units.Remove(unit);
        }

        public void RemoveUnitRange(IEnumerable<DimmerDistroUnit> units)
        {
            foreach (var element in units)
            {
                RemoveUnit(element);
            }
        }

        public void ReplaceUnit(DimmerDistroUnit oldUnit, DimmerDistroUnit newUnit)
        {
            _Context.Entry(oldUnit).CurrentValues.SetValues(newUnit);
        }

        public void Save()
        {
            _Context.SaveChanges();
        }

        public void Load()
        {
            _Context.Units.Load();
        }

        public void UpdateUnit(DimmerDistroUnit unit)
        {
            _Context.Entry(unit).State = System.Data.Entity.EntityState.Modified;
        }

        private bool _Disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }

            _Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Debugging Code.
        public EntityState GetEntityState(object entity)
        {
            return _Context.Entry(entity).State;
        }
    }
}

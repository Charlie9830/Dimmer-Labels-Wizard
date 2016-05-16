using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF.Repositories
{
    public class ColorDictionaryRepository : IDisposable
    {
        public ColorDictionaryRepository(PrimaryDB context)
        {
            _Context = context;
        }

        protected PrimaryDB _Context;

        public ColorDictionary DistroColorDictionary
        {
            get
            {
                // If an exising Dictionary does not already exist. Make one.
                var query = (from item in _Context.ColorDictionaries
                            where item.EntriesRackType == RackType.Distro
                            select item);

                if (query.Count() == 0)
                {
                    _Context.ColorDictionaries.Add(new ColorDictionary() { EntriesRackType = RackType.Distro });
                    _Context.SaveChanges();
                }

                return query.First();
            }
        }


        public ColorDictionary DimmerColorDictionary
        {
            get
            {
                // If an exising Dictionary does not already exist. Make one.
                var query = (from item in _Context.ColorDictionaries
                             where item.EntriesRackType == RackType.Dimmer
                             select item);

                if (query.Count() == 0)
                {
                    _Context.ColorDictionaries.Add(new ColorDictionary() { EntriesRackType = RackType.Dimmer });
                    _Context.SaveChanges();
                }

                return query.First();
            }
        }

        public void Update(ColorDictionary colorDictionary)
        {
            _Context.Entry(colorDictionary).State = System.Data.Entity.EntityState.Modified;
        }

        public void RemoveAll()
        {
            _Context.Database.ExecuteSqlCommand("DELETE from ColorEntries");
            _Context.Database.ExecuteSqlCommand("DELETE from ColorDictionaries");
        }

        public void Save()
        {
            // Remove Orphans.
            foreach (var element in _Context.ColorDictionaries.SelectMany(item => item.Entries).ToList())
            {
                if (element.ColorDictionary == null)
                {
                    _Context.Entry(element).State = System.Data.Entity.EntityState.Deleted;
                }
            }

            _Context.SaveChanges();
        }


        #region Interfaces.
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

        #endregion 
    }
}

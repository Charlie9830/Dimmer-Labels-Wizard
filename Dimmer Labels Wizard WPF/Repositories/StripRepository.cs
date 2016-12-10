using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace Dimmer_Labels_Wizard_WPF.Repositories
{
    public class StripRepository
    {
        public StripRepository(PrimaryDB context)
        {
            _Context = context;
        }

        private PrimaryDB _Context;

        #region Properties.
        public ObservableCollection<Strip> Local
        {
            get
            {
                return _Context.Strips.Local;
            }
        }

        public IList<Strip> GetStrips()
        {
            return _Context.Strips.Select(item => item)
                                  .Include(item => item.AssignedTemplate.UpperCellTemplate)
                                  .Include(item => item.AssignedTemplate.LowerCellTemplate)
                                  .Include(item => item.Mergers)
                                  .ToList();
        }

        public void Insert(Strip strip)
        {
            _Context.Strips.Add(strip);
        }

        public void Remove(Strip strip)
        {
            _Context.Strips.Remove(strip);
        }

        public void RemoveAll()
        {
            _Context.Database.ExecuteSqlCommand("DELETE from Strips");
        }

        public void Load()
        {
            _Context.Strips.Load();
        }

        public void Save()
        {
            _Context.SaveChanges();
        }
        #endregion
    }
}

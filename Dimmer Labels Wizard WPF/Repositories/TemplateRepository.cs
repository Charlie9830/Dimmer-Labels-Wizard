using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF.Repositories
{
    public class TemplateRepository : IDisposable
    {
        public TemplateRepository(PrimaryDB context)
        {
            _Context = context;
        }

        protected PrimaryDB _Context;

        public ObservableCollection<LabelStripTemplate> Local
        {
            get
            {
                return _Context.Templates.Local;
            }
        }

        public IList<LabelStripTemplate> GetTemplates()
        {
            // Changes in Include Statements should be carried over to UniqueCellTemplateRepository.GetUniqueTemplates().
            return _Context.Templates
                           .Include(item => item.UpperCellTemplate.CellRowTemplates.Select(c => c.SerializableFont))
                           .Include(item => item.UpperCellTemplate.SingleFieldSerializableFont)
                           .Include(item => item.LowerCellTemplate.CellRowTemplates.Select(c => c.SerializableFont))
                           .Include(item => item.LowerCellTemplate.SingleFieldSerializableFont)
                           .Include(item => item.StripSpacers)
                           .ToList();
        }


        public LabelStripTemplate GetTemplate(string templateName)
        {
            var query = from template in GetTemplates()
                        where template.Name == templateName
                        select template;

            if (query.Count() > 0)
            {
                return query.First();
            }

            else
            {
                return null;
            }
        }

        public LabelStripTemplate GetDefaultTemplate()
        {
            var query = from template in GetTemplates()
                        where template.IsBuiltIn == true &&
                        template.Name == "Default"
                        select template;

            if (query.Count() > 0)
            {
                return query.First();
            }

            else
            {
                return null;
            }
        }

        public void InsertTemplate(LabelStripTemplate template)
        {
            _Context.Templates.Add(template);
        }

        public void RemoveTemplate(LabelStripTemplate template)
        {
            _Context.Templates.Remove(template);
        }

        public void RemoveAllTemplates()
        {
            _Context.Database.ExecuteSqlCommand("DELETE from LabelStripTemplates");
            _Context.Database.ExecuteSqlCommand("DELETE from CellRowTemplates");
            _Context.Database.ExecuteSqlCommand("DELETE from LabelCellTemplates");
            _Context.Database.ExecuteSqlCommand("DELETE from SerializableFonts");
            _Context.Database.ExecuteSqlCommand("DELETE from StripSpacers");
        }

        public void RemoveAllUserTemplates()
        {
            _Context.Templates.Load();

            var query = from template in _Context.Templates.Local
                        where template.IsBuiltIn == false
                        select template;

            foreach (var element in query.ToList())
            {
                _Context.Templates.Local.Remove(element);
            }
        }

        public void UpdateTemplate(LabelStripTemplate template)
        {
            _Context.Entry(template).State = EntityState.Modified;
        }

        public void Save()
        {
            _Context.SaveChanges();
        }

        public void Load()
        {
            _Context.Templates.Load();
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

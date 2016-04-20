using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return (from template in _Context.Templates
                    select template).ToList();
        }

        public LabelStripTemplate GetTemplate(string templateName)
        {
            var query = from template in _Context.Templates
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

        public void InsertTemplate(LabelStripTemplate template)
        {
            _Context.Templates.Add(template);
        }

        public void RemoveTemplate(LabelStripTemplate template)
        {
            _Context.Templates.Remove(template);
        }

        public void Save()
        {
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

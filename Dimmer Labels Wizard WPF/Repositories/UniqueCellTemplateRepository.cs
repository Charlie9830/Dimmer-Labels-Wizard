using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Dimmer_Labels_Wizard_WPF.Repositories
{
    public class UniqueCellTemplateRepository : IDisposable
    {
        public UniqueCellTemplateRepository(PrimaryDB context)
        {
            _Context = context;
        }

        private PrimaryDB _Context;

        public IList<LabelCellTemplate> GetUniqueCellTemplates()
        {
            return (_Context.UniqueCellTemplates
                .Include(item => item.SingleFieldSerializableFont)
                .Include(item => item.CellRowTemplates.Select(c => c.SerializableFont)))
                .ToList();
        }

        public LabelCellTemplate GetUniqueCellTemplate(Strip strip, int horizontalIndex,
            CellVerticalPosition verticalPosition)
        {
            var query = from template in GetUniqueCellTemplates()
                        where template.Strip == strip &&
                        template.UniqueCellIndex == horizontalIndex &&
                        template.UniqueCellVerticalPosition == verticalPosition
                        select template;

            if (query.Count() == 0)
            {
                return null;
            }

            else
            {
                return query.First();
            }

        }

        public void InsertUniqueTemplate(LabelCellTemplate template)
        {
            if (template.IsUniqueTemplate == false)
            {
                throw new NotSupportedException("The Unique Template you are trying to add has not been flagged as Unique.");
            }

            _Context.UniqueCellTemplates.Add(template);
        }

        public void UpdateUniqueTemplate(LabelCellTemplate template)
        {
            if (template.IsUniqueTemplate == false)
            {
                throw new NotSupportedException("The Template you are trying to update is not flagged as Unique.");
            }

            _Context.Entry(template).State = EntityState.Modified;
        }

        public void RemoveUniqueTemplate(LabelCellTemplate template)
        {
            if (template.IsUniqueTemplate == false)
            {
                throw new NotSupportedException("The Template you are trying to remove is not flagged as Unique.");
            }

            _Context.Entry(template).State = EntityState.Deleted;
        }


        #region Interfaces.
        // IDispoable.
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

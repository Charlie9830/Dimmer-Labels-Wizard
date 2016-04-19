using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF.Repositories
{
    public class TemplateRepository
    {
        public TemplateRepository(PrimaryDB context)
        {
            _Context = context;
        }

        protected PrimaryDB _Context;

        public IList<LabelStripTemplate> GetTemplates()
        {
            return (from template in _Context.Templates
                    select template).ToList();
        }
    }
}

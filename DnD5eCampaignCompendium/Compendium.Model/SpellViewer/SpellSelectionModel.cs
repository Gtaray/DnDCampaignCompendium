using Assisticant;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.SpellViewer
{
    public class SpellSelectionModel
    {
        private Observable<Spell> _Value = new Observable<Spell>();
        public Spell Value
        {
            get { return _Value; }
            set { _Value.Value = value; }
        }
    }
}

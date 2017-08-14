using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.CharacterClasses
{
    public class ClassSelectionModel
    {
        private Observable<CharacterClass> _Value = new Observable<CharacterClass>();
        public CharacterClass Value
        {
            get { return _Value; }
            set { _Value.Value = value; }
        }
    }
}

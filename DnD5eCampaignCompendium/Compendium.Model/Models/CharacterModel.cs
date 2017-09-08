using Assisticant.Collections;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Models
{
    public class CharacterModel
    {
        public CharacterModel()
        { }

        #region Properties and Accessors
        private Observable<string> _Name = new Observable<string>("New Character");
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private ObservableList<SpellModel> _Spells = new ObservableList<SpellModel>();
        public IEnumerable<SpellModel> Spells => _Spells;

        private ObservableList<SpellModel> _PreparedSpells = new ObservableList<SpellModel>();
        public IEnumerable<SpellModel> PreparedSpells => _PreparedSpells;
        #endregion
    }
}

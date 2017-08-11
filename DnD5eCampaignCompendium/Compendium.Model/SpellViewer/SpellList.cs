using Assisticant.Collections;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.SpellViewer
{
    public class SpellList
    {
        public SpellList()
        { }

        #region Properties and Accessors
        private Observable<string> _Name = new Observable<string>("Spellbook");
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private Observable<bool> _ReadOnly = new Observable<bool>(false);
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set { _ReadOnly.Value = value; }
        }

        private ObservableList<Spell> _Spells = new ObservableList<Spell>();
        public IEnumerable<Spell> Spells
        {
            get { return _Spells; }
        }

        private Observable<string> _Notes = new Observable<string>(default(string));
        public string Notes
        {
            get { return _Notes; }
            set { _Notes.Value = value; }
        }
        #endregion

        public Spell GetSpellByID(string id)
        {
            Spell spell = Spells.SingleOrDefault(s => s.ID == id);
            return spell;
        }

        public bool Contains(Spell s)
        {
            return Spells.Any(x => x.ID == s.ID);
        }

        public bool AddSpell(Spell s)
        {
            if (s == null)
                return false;
            if (Contains(s))
            {
                return false;
            }
            _Spells.Add(s);
            return true;
        }

        public void AddManySpells(IEnumerable<Spell> spells)
        {
            foreach(Spell s in spells)
                AddSpell(s);
        }

        public void RemoveSpell(Spell s)
        {
            throw new NotImplementedException("RemoveSpell from spell list is not implemented.");
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} spells)", Name, Spells.Count().ToString());
        }
    }
}

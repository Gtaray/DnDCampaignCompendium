using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.SpellViewer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.CharacterClasses
{
    public class CharacterClass : BaseModel
    {
        public CharacterClass()
        {
            _Spells = new ObservableList<Spell>();
            _Subclasses = new ObservableList<CharacterClass>();
        }

        #region Properties and Accessors
        private Observable<string> _Name = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private Observable<CharacterClass> _Parent = new Observable<CharacterClass>(default(CharacterClass));
        [JsonIgnore]
        public CharacterClass Parent
        {
            get { return _Parent; }
            set { _Parent.Value = value; }
        }

        private ObservableList<CharacterClass> _Subclasses = new ObservableList<CharacterClass>();
        [JsonProperty(PropertyName = "subclasses")]
        public IEnumerable<CharacterClass> Subclasses
        {
            get { return _Subclasses; }
        }

        private Observable<string> _SubclassTitle = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "subclassTitle")]
        public string SubclassTitle
        {
            get { return _SubclassTitle; }
            set { _SubclassTitle.Value = value; }
        }

        private ObservableList<Spell> _Spells = new ObservableList<Spell>();
        [JsonIgnore]
        public IEnumerable<Spell> Spells
        {
            get { return _Spells; }
        }

        private Observable<ContentSource> _Source = new Observable<ContentSource>(default(ContentSource));
        public ContentSource Source
        {
            get { return _Source; }
            set { _Source.Value = value; }
        }

        [JsonProperty(PropertyName = "spells")]
        public IEnumerable<string> SpellIDs
        {
            get { return _Spells.Select(s => s.ID); }
        }

        private Computed<bool> _HasSpells;
        [JsonIgnore]
        public bool HasSpells
        {
            get
            {
                if (_HasSpells == null)
                    _HasSpells = new Computed<bool>(() => _Spells.Count() > 0 || Subclasses.Any(s => s.HasSpells));
                return _HasSpells.Value;
            }
        }

        private Computed<bool> _IsRoot;
        [JsonIgnore]
        public bool IsRoot
        {
            get
            {
                if (_IsRoot == null)
                    _IsRoot = new Computed<bool>(() => Parent == null);
                return _IsRoot.Value;
            }
        }

        private Computed<string> _Header;
        [JsonIgnore]
        public string Header
        {
            get
            {
                if (_Header == null)
                    _Header = new Computed<string>(() => IsRoot ? Name : Parent.Header + " > " + Name);
                return _Header.Value;
            }
        }

        private Computed<string> _Path;
        [JsonIgnore]
        public string Path
        {
            get
            {
                if (_Path == null)
                    _Path = new Computed<string>(() => IsRoot ? Name : Parent.Path + "." + Path);
                return _Path.Value;
            }
        }
        #endregion

        public void AddSpell(Spell spell)
        {
            if (Spells.Contains(spell))
                return;

            _Spells.Add(spell);
            spell.AddClass(this);
        }

        public void RemoveSpell(Spell toRemove)
        {
            if (!Spells.Contains(toRemove))
                return;

            _Spells.Remove(toRemove);
            toRemove.RemoveClass(this);
        }

        public void AddSubclass(CharacterClass newClass)
        {
            newClass.Parent = this;
            _Subclasses.Add(newClass);
        }

        public void RemoveSubclass(CharacterClass toRemove)
        {
            toRemove.Parent = null;
            _Subclasses.Remove(toRemove);
        }

        public override string ToString()
        {
            return Header;
        }
    }
}

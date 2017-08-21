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
        #region Basic
        private Observable<string> _Name = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private Observable<string> _ShortName = new Observable<string>();
        public string ShortName
        {
            get { return _ShortName; }
            set { _ShortName.Value = value; }
        }

        private ObservableList<string> _Description = new ObservableList<string>();
        public IEnumerable<string> Description => _Description;

        private Observable<ContentSource> _Source = new Observable<ContentSource>(default(ContentSource));
        public ContentSource Source
        {
            get { return _Source; }
            set { _Source.Value = value; }
        }

        private Observable<string> _HD = new Observable<string>("d8");
        public string HD
        {
            get { return _HD; }
            set { _HD.Value = value; }
        }
        #endregion

        #region Proficiencies
        private Observable<string> _ArmorProfs = new Observable<string>();
        public string ArmorProfs
        {
            get { return _ArmorProfs; }
            set { _ArmorProfs.Value = value; }
        }

        private Observable<string> _WeaponProfs = new Observable<string>();
        public string WeaponProfs
        {
            get { return _WeaponProfs; }
            set { _WeaponProfs.Value = value; }
        }

        private Observable<string> _ToolProfs = new Observable<string>();
        public string ToolProfs
        {
            get { return _ToolProfs; }
            set { _ToolProfs.Value = value; }
        }

        private Observable<string> _SaveProfs = new Observable<string>();
        public string SaveProfs
        {
            get { return _SaveProfs; }
            set { _SaveProfs.Value = value; }
        }

        private Observable<string> _SkillProfs = new Observable<string>();
        public string SkillProfs
        {
            get { return _SkillProfs; }
            set { _SkillProfs.Value = value; }
        }
        #endregion

        #region Starting Gear
        private ObservableList<string> _StartingGear = new ObservableList<string>();
        public IEnumerable<string> StartingGear => _StartingGear;
        #endregion

        private ObservableList<ClassFeature> _ClassFeatures = new ObservableList<ClassFeature>();
        public IEnumerable<ClassFeature> ClassFeatures => _ClassFeatures;

        private ObservableList<ClassTableColumn> _ClassTableColumns = new ObservableList<ClassTableColumn>();
        public IEnumerable<ClassTableColumn> ClassTableColumns => _ClassTableColumns;

        #region Subclass
        private Observable<string> _SubclassTitle = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "subclassTitle")]
        public string SubclassTitle
        {
            get { return _SubclassTitle; }
            set { _SubclassTitle.Value = value; }
        }

        private ObservableList<CharacterClass> _Subclasses = new ObservableList<CharacterClass>();
        [JsonProperty(PropertyName = "subclasses")]
        public IEnumerable<CharacterClass> Subclasses => _Subclasses;
        #endregion

        private ObservableList<Spell> _Spells = new ObservableList<Spell>();
        [JsonIgnore]
        public IEnumerable<Spell> Spells => _Spells;

        private Observable<CharacterClass> _Parent = new Observable<CharacterClass>(default(CharacterClass));
        [JsonIgnore]
        public CharacterClass Parent
        {
            get { return _Parent; }
            set { _Parent.Value = value; }
        }
        #endregion

        #region Computed Properties
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

        [JsonProperty(PropertyName = "spells")]
        public IEnumerable<string> SpellIDs
        {
            get { return _Spells.Select(s => s.ID); }
        }
        #endregion

        #region Add Remove Functions
        public void AddDescriptionLine(string line)
        {
            _Description.Add(line);
        }

        public void AddFeature(ClassFeature feature)
        {
            if (!ClassFeatures.Contains(feature))
                _ClassFeatures.Add(feature);
        }

        public void RemoveFeature(ClassFeature feature)
        {
            if (ClassFeatures.Contains(feature))
                _ClassFeatures.Remove(feature);
        }

        public void AddTableColumn(ClassTableColumn column)
        {
            if (!ClassTableColumns.Contains(column))
                _ClassTableColumns.Add(column);
        }

        public void RemoveTableColumn(ClassTableColumn column)
        {
            if (ClassTableColumns.Contains(column))
                _ClassTableColumns.Remove(column);
        }

        public void AddStartingGear(string line)
        {
            _StartingGear.Add(line);
        }

        public void RemoveStartingGear(string line)
        {
            if (_StartingGear.Any(s => s.Equals(line, StringComparison.OrdinalIgnoreCase)))
                _StartingGear.Remove(line);
        }

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
        #endregion

        public override string ToString()
        {
            return Header;
        }
    }
}

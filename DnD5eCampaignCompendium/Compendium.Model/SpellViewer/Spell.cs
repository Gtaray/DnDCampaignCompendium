// TODO: Add character classes

using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.CharacterClasses;
using Compendium.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.SpellViewer
{
    public class Spell : BaseModel
    {
        public Spell()
        { }

        #region Properties and Accessors
        private Observable<string> _ID = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "id")]
        public string ID
        {
            get { return _ID; }
            set { _ID.Value = value; }
        }

        private Observable<string> _Name = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private Observable<bool> _IsConcentration = new Observable<bool>(false);
        [JsonProperty(PropertyName = "concentration")]
        public bool IsConcentration
        {
            get { return _IsConcentration; }
            set { _IsConcentration.Value = value; }
        }

        private Observable<bool> _IsRitual = new Observable<bool>(false);
        [JsonProperty(PropertyName = "isRitual")]
        public bool IsRitual
        {
            get { return _IsRitual; }
            set { _IsRitual.Value = value; }
        }

        private Observable<int> _Level = new Observable<int>(0);
        [JsonProperty(PropertyName = "level")]
        public int Level
        {
            get { return _Level; }
            set { _Level.Value = value; }
        }

        private Observable<SpellSchool> _School = new Observable<SpellSchool>(default(SpellSchool));
        [JsonIgnore]
        public SpellSchool School
        {
            get { return _School; }
            set { _School.Value = value; }
        }

        private Computed<string> _SpellSchoolName;
        [JsonProperty(PropertyName = "school")]
        public string SpellSchoolName
        {
            get
            {
                if (_SpellSchoolName == null)
                    _SpellSchoolName = new Computed<string>(() => School.Name);
                return _SpellSchoolName.Value;
            }
        }

        private ObservableList<SpellComponent> _Components = new ObservableList<SpellComponent>();
        [JsonIgnore]
        public IEnumerable<SpellComponent> Components
        {
            get { return _Components; }
        }

        [JsonProperty(PropertyName = "components")]
        public IEnumerable<string> ComponentInitials
        {
            get { return _Components.Select(c => c.Initial); }
        }

        private Observable<string> _MaterialComponent = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "material")]
        public string MaterialComponent
        {
            get { return _MaterialComponent; }
            set { _MaterialComponent.Value = value; }
        }

        private Observable<string> _Range = new Observable<string>(default(string));
        public string Range
        {
            get { return _Range; }
            set { _Range.Value = value; }
        }

        private Observable<string> _CastingTime = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "castingTime")]
        public string CastingTime
        {
            get { return _CastingTime; }
            set { _CastingTime.Value = value; }
        }

        private Observable<string> _Duration = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "duration")]
        public string Duration
        {
            get { return _Duration; }
            set { _Duration.Value = value; }
        }

        private Observable<string> _Description = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get { return _Description; }
            set { _Description.Value = value; }
        }

        private Observable<string> _HigherLevel = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "higherLevel")]
        public string HigherLevel
        {
            get { return _HigherLevel; }
            set { _HigherLevel.Value = value; }
        }

        private ObservableList<CharacterClass> _Classes = new ObservableList<CharacterClass>();
        public IEnumerable<CharacterClass> Classes
        {
            get { return _Classes; }
        }

        private Observable<ContentSource> _Source = new Observable<ContentSource>(default(ContentSource));
        [JsonIgnore]
        public ContentSource Source
        {
            get { return _Source; }
            set { _Source.Value = value; }
        }

        private Computed<string> _SourceName;
        [JsonProperty(PropertyName = "source")]
        public string SourceName
        {
            get
            {
                if (_SourceName == null)
                    _SourceName = new Computed<string>(() => Source.Name);
                return _SourceName.Value;
            }
        }

        private ObservableList<Errata> _ErrataList = new ObservableList<Errata>();
        [JsonProperty(PropertyName = "errata")]
        public IEnumerable<Errata> ErrataList
        {
            get { return _ErrataList; }
        }
        #endregion

        public void AddComponent(SpellComponent component)
        {
            if (!_Components.Contains(component))
                _Components.Add(component);
        }

        public void AddErrata(Errata errata)
        {
            if (!_ErrataList.Contains(errata))
                _ErrataList.Add(errata);
        }

        public void AddClass(CharacterClass newClass)
        {
            if (_Classes.Contains(newClass))
                return;

            _Classes.Add(newClass);
            newClass.AddSpell(this);
        }

        public void RemoveClass(CharacterClass toRemove)
        {
            if (!_Classes.Contains(toRemove))
                return;

            _Classes.Remove(toRemove);
            toRemove.RemoveSpell(this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

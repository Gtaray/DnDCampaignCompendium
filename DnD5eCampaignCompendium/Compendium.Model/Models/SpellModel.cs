using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Models
{
    public class SpellModel : IContent
    {
        public SpellModel()
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

        private Observable<int> _Level = new Observable<int>(0);
        [JsonProperty(PropertyName = "level")]
        public int Level
        {
            get { return _Level; }
            set { _Level.Value = value; }
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

        private Observable<SpellSchool> _School = new Observable<SpellSchool>(default(SpellSchool));
        [JsonIgnore]
        public SpellSchool School
        {
            get { return _School; }
            set { _School.Value = value; }
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
        [JsonProperty(PropertyName = "range")]
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
        [JsonIgnore]
        public string Description
        {
            get { return _Description; }
            set { _Description.Value = value; }
        }

        private Observable<string> _HigherLevel = new Observable<string>(default(string));
        [JsonIgnore]
        public string HigherLevel
        {
            get { return _HigherLevel; }
            set { _HigherLevel.Value = value; }
        }

        private ObservableList<ClassModel> _Classes = new ObservableList<ClassModel>();
        [JsonIgnore]
        public IEnumerable<ClassModel> Classes
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

        private ObservableList<Errata> _ErrataList = new ObservableList<Errata>();
        [JsonProperty(PropertyName = "errata")]
        public IEnumerable<Errata> ErrataList
        {
            get { return _ErrataList; }
        }
        #endregion

        #region Markdown and Display Properties
        [JsonIgnore]
        public string Markdown
        {
            get
            {
                return string.Format(
                    "## {0}\n\n" +
                    "_{1}_\n\n" +
                    "#### Casting Time:\n\n{2}\n\n" +
                    "#### Range:\n\n{3}\n\n" +
                    "#### Components:\n\n{4}\n\n" +
                    "#### Duration:\n\n{5}\n\n" +
                    "{6}\n\n" +
                    "{7}" +
                    "#### Source:\n\n{8}" +
                    "{9}",
                    Name,
                    LevelAndSchool,
                    CastingTime,
                    Range,
                    string.Format("{0}{1}",
                        string.Join(", ", ComponentInitials),
                        string.IsNullOrWhiteSpace(MaterialComponent) ? "" : string.Format(" ({0})", MaterialComponent)),
                    IsConcentration ? "Concentration, up to " + Duration : Duration,
                    Description,
                    string.IsNullOrEmpty(HigherLevel)
                        ? ""
                        : string.Format("**At Higher Levels:** {0}\n\n", HigherLevel),
                    Source != null ? Source.ToString() : "Unknown",
                    ErrataList.Count() > 0 
                        ? string.Format("\n\n#### Errata: \n\n{0}", 
                            string.Join("\n", ErrataList.Select(e => string.Format("+ {0}", e.ToString()))))
                        : ""
                );
            }
            set { }
        }

        [JsonIgnore]
        public string LevelAndSchool
        {
            get
            {
                string s = "";
                switch (Level)
                {
                    case (0):
                        s = School + " cantrip";
                        break;
                    case (1):
                        s = "1st-level " + School;
                        break;
                    case (2):
                        s = "2nd-level " + School;
                        break;
                    case (3):
                        s = "3rd-level " + School;
                        break;
                    case (4):
                    case (5):
                    case (6):
                    case (7):
                    case (8):
                    case (9):
                        s = Level + "th-level " + School;
                        break;
                }
                if (IsRitual)
                    s += " (ritual)";

                return s;
            }
        }
        #endregion

        #region Json Properties
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

        [JsonProperty(PropertyName = "description")]
        public IEnumerable<string> DescriptionAsParagraphs
        {
            get
            {
                var list = Description
                    .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToList();
                int max = list.Count();
                for(int i = 0; i < max; i+=2)
                {
                    if(list[i] != list.Last())
                        list.Insert(i + 1, "");    
                }
                return list;
            }
        }

        [JsonProperty(PropertyName = "higherLevel")]
        public IEnumerable<string> HigherLevelAsParagraphs
        {
            get
            {
                var list = HigherLevel
                    .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToList();
                int max = list.Count();
                for (int i = 0; i < max; i += 2)
                {
                    if (list[i] != list.Last())
                        list.Insert(i + 1, "");
                }
                return list;
            }
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

        public void AddClass(ClassModel newClass)
        {
            if (_Classes.Contains(newClass))
                return;

            _Classes.Add(newClass);
            newClass.AddSpell(this);
        }

        public void RemoveClass(ClassModel toRemove)
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

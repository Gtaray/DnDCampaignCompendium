using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Models
{
    public class SpellPageModel : IContentPage<SpellModel>
    {
        private readonly CompendiumModel _Compendium;
        private readonly SelectionModel<SpellModel> _Selection;

        public SpellPageModel(CompendiumModel compendium)
        {
            _Compendium = compendium;
            _Compendium.SpellViewer = this;

            _Selection = new SelectionModel<SpellModel>();

            _Content = new ObservableList<SpellModel>();
            _SpellSchools = new ObservableList<SpellSchool>();
            _Components = new ObservableList<SpellComponent>();
        }

        #region Properties and Accessors
        public SpellModel SelectedItem
        {
            get { return _Selection.Value; }
            set { _Selection.Value = value; }
        }

        private Observable<string> _Header = new Observable<string>(default(string));
        public string Header
        {
            get { return _Header; }
            set { _Header.Value = value; }
        }

        private ObservableList<SpellModel> _Content = new ObservableList<SpellModel>();
        [JsonProperty(propertyName:"spells")]
        public IEnumerable<SpellModel> Content
        {
            get { return _Content; }
        }

        private ObservableList<SpellSchool> _SpellSchools = new ObservableList<SpellSchool>();
        public IEnumerable<SpellSchool> SpellSchools
        {
            get { return _SpellSchools; }
        }

        private ObservableList<SpellComponent> _Components = new ObservableList<SpellComponent>();
        public IEnumerable<SpellComponent> Components
        {
            get { return _Components; }
        }
        #endregion

        #region Add/Remove functions

        #endregion

        #region Accessor Functions
        public SpellComponent GetComponentByInitial(string initial)
        {
            return Components.FirstOrDefault(c => string.Equals(c.Initial, initial));
        }

        public SpellSchool GetSchoolByName(string name)
        {
            return SpellSchools.FirstOrDefault(s => string.Equals(s.Name, name));
        }

        public SpellModel GetSpellByID(string id)
        {
            return Content.FirstOrDefault(s => string.Equals(s.ID, id));
        }
        #endregion

        #region Json Parsing
        public void DeserializeContent(string dataDir, string json) { }

        public void DeserializeSchools(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var school in obj.schools)
                _SpellSchools.Add(
                    new SpellSchool((string)school.name));

            if(SpellSchools.Count() <= 0)
            {
                throw new NullReferenceException("No spell schools were loaded from SpellSchools.json. Is the file empty?");
            }
        }

        public void DeserializeComponents(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var comp in obj.components)
                _Components.Add(
                    new SpellComponent((string)comp.name, (string)comp.initial));

            if(Components.Count() <= 0)
            {
                throw new NullReferenceException("No spell components were loaded from SpellComponents.json. Is the file empty?");
            }
        }

        public void DeserializeSpells(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

            List<SpellModel> spells = new List<SpellModel>();
            foreach(var spell in obj.spells)
            {
                // Initialize with basic values
                SpellModel newSpell = new SpellModel()
                {
                    ID = (string)spell.id,
                    Name = (string)spell.name,
                    Level = (int)spell.level,
                    IsConcentration = (bool)spell.concentration,
                    IsRitual = (bool)spell.isRitual,
                    MaterialComponent = (string)spell.material,
                    Range = (string)spell.range,
                    CastingTime = (string)spell.castingTime,
                    Duration = (string)spell.duration,
                    Description = string.Join("\r\n", spell.description),
                    HigherLevel = string.Join("\r\n", spell.higherLevel)
                };

                // Parse school
                var school = GetSchoolByName((string)spell.school);
                if (school != null)
                    newSpell.School = school;
                else
                    newSpell.School = SpellSchools.First();

                // Parse components
                foreach(string compString in spell.components)
                {
                    var comp = GetComponentByInitial(compString);
                    if(comp != null)
                        newSpell.AddComponent(comp);
                }

                // Parse errata
                foreach (var errata in spell.errata)
                    newSpell.AddErrata(new Errata((string)spell.month, (string)spell.year, (string)spell.text, newSpell));

                // Parse source
                var source = _Compendium.GetSourceByName((string)spell.source);
                if (source != null)
                    newSpell.Source = source;
                else
                    newSpell.Source = null;

                spells.Add(newSpell);
            }

            // Sort Spells by level and then name
            _Content = new ObservableList<SpellModel>(
                spells.OrderBy(s => s.Level).ThenBy(s => s.Name));
        }
        #endregion
    }
}

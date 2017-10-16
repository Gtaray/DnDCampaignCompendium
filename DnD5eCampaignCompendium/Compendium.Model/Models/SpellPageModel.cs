using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Interfaces;
using Compendium.Model.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Compendium.Model.Models
{
    public class SpellPageModel : IContentPage<SpellModel>
    {
        private readonly CompendiumModel _Compendium;
        private readonly SelectionModel<SpellModel> _Selection;

        public SpellPageModel(CompendiumModel compendium)
        {
            _Compendium = compendium;
            _Compendium.SpellPage = this;

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
        public void DeserializeContent(string dataDir, Base_Json json) { }

        public void DeserializeSchools(Schools_Json schools)
        {
            foreach (var school in schools.schools)
                _SpellSchools.Add(
                    new SpellSchool(school.name));

            if(SpellSchools.Count() <= 0)
            {
                throw new NullReferenceException("No spell schools were loaded from SpellSchools.json. Is the file empty?");
            }
        }

        public void DeserializeComponents(Components_Json components)
        {
            foreach (var comp in components.components)
                _Components.Add(
                    new SpellComponent(comp.name, comp.initial));

            if(Components.Count() <= 0)
            {
                throw new NullReferenceException("No spell components were loaded from SpellComponents.json. Is the file empty?");
            }
        }

        public void DeserializeSpells(Spells_Json spells)
        {
            List<SpellModel> allSpells = new List<SpellModel>();
            foreach(var spell in spells.spells)
            {
                // Initialize with basic values
                SpellModel newSpell = new SpellModel()
                {
                    ID = spell.id,
                    Name = spell.name,
                    Level = spell.level,
                    IsConcentration = spell.concentration,
                    IsRitual = spell.isRitual,
                    MaterialComponent = spell.material,
                    Range = spell.range,
                    CastingTime = spell.castingTime,
                    Duration = spell.duration,
                    Description = string.Join("\n", spell.description),
                    HigherLevel = string.Join("\n", spell.higherLevel)
                };

                // Parse school
                var school = GetSchoolByName(spell.school);
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
                    newSpell.AddErrata(new Errata(errata.month, errata.year, errata.text, newSpell));

                // Parse source
                var source = _Compendium.GetSourceByName((string)spell.source);
                if (source != null)
                    newSpell.Source = source;
                else
                    newSpell.Source = null;

                allSpells.Add(newSpell);
            }

            // Sort Spells by level and then name
            _Content = new ObservableList<SpellModel>(
                allSpells.OrderBy(s => s.Level).ThenBy(s => s.Name));
        }

        public string SerializeSpells()
        {
            dynamic json = new ExpandoObject();
            json.spells = Content;
            return JsonConvert.SerializeObject(json, Formatting.Indented);
        }
        #endregion
    }
}

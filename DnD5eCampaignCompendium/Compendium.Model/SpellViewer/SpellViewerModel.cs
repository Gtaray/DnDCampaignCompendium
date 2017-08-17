using Assisticant.Collections;
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
    public class SpellViewerModel
    {
        private readonly CompendiumModel Compendium;

        public SpellViewerModel(CompendiumModel compendium)
        {
            Compendium = compendium;
            Compendium.SpellViewer = this;

            _AllSpells = new ObservableList<Spell>();
            _SpellSchools = new ObservableList<SpellSchool>();
            _Components = new ObservableList<SpellComponent>();
            _SpellLists = new ObservableList<SpellList>();

            //var sl = new SpellList()
            //{
            //    Name = "All Spells",
            //    ReadOnly = true
            //};
            //sl.AddManySpells(AllSpells);
            //_SpellLists.Add(sl);
        }

        #region Properties and Accessors
        private ObservableList<SpellList> _SpellLists = new ObservableList<SpellList>();
        public IEnumerable<SpellList> SpellLists
        {
            get { return _SpellLists; }
        }

        private ObservableList<Spell> _AllSpells = new ObservableList<Spell>();
        public IEnumerable<Spell> AllSpells
        {
            get { return _AllSpells; }
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

        #region Filtering
        public void InitializeFilters(IEnumerable<CharacterClass> classes)
        {
            _LevelFilterFlags = new ObservableList<FilterFlag<int>>()
            {
                new FilterFlag<int>(0, () => "Cantrips", false),
                new FilterFlag<int>(1, () => "1st-level", false),
                new FilterFlag<int>(2, () => "2nd-level", false),
                new FilterFlag<int>(3, () => "3rd-level", false),
                new FilterFlag<int>(4, () => "4th-level", false),
                new FilterFlag<int>(5, () => "5th-level", false),
                new FilterFlag<int>(6, () => "6th-level", false),
                new FilterFlag<int>(7, () => "7th-level", false),
                new FilterFlag<int>(8, () => "8th-level", false),
                new FilterFlag<int>(9, () => "9th-level", false)
            };

            foreach(CharacterClass cc in classes)
                _ClassFilterFlags.Add(GetClassFilterObject(cc));
        }

        private FilterFlag<CharacterClass> GetClassFilterObject(CharacterClass cc)
        {
            FilterFlag<CharacterClass> newfilter = new FilterFlag<CharacterClass>(cc, () => cc.Name, false);
            foreach (var child in cc.Subclasses)
                newfilter.AddChildFilter(GetClassFilterObject(child));

            return newfilter;
        }

        private ObservableList<FilterFlag<int>> _LevelFilterFlags = 
            new ObservableList<FilterFlag<int>>();
        public IEnumerable<FilterFlag<int>> LevelFilterFlags
        {
            get { return _LevelFilterFlags; }
        }

        private ObservableList<FilterFlag<CharacterClass>> _ClassFilterFlags = 
            new ObservableList<FilterFlag<CharacterClass>>();
        public IEnumerable<FilterFlag<CharacterClass>> ClassFilterFlags
        {
            get { return _ClassFilterFlags; }
        }
        #endregion

        #region Add/Remove functions
        public SpellList AddNewSpellList()
        {
            SpellList sl = new SpellList()
            {
                Name = "New Spellbook",
                ReadOnly = false
            };

            _SpellLists.Add(sl);
            return sl;
        }

        public void RemoveSpellList(SpellList list)
        {
            if (_SpellLists.Contains(list))
            {
                _SpellLists.Remove(list);
            }
        }
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

        public Spell GetSpellByID(string id)
        {
            return AllSpells.FirstOrDefault(s => string.Equals(s.ID, id));
        }
        #endregion

        #region Json Parsing
        public void DeserializeSchools(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException("SpellSchools.json is null, empty, or is not being read properly");

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
            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException("SpellComponents.json is null, empty, or is not being read properly");

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
            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException("Spells.json is null, empty, or is not being read properly");

            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

            foreach(var spell in obj.spells)
            {
                // Initialize with basic values
                Spell newSpell = new Spell()
                {
                    ID = (string)spell.id,
                    Name = (string)spell.name,
                    IsConcentration = (bool)spell.concentration,
                    IsRitual = (bool)spell.isRitual,
                    Level = (int)spell.level,
                    Range = (string)spell.range,
                    CastingTime = (string)spell.castingTime,
                    Duration = (string)spell.duration,
                    Description = (string)spell.description,
                    HigherLevel = (string)spell.higherLevel
                };

                // Parse school
                var school = GetSchoolByName((string)spell.school);
                if (school != null)
                    newSpell.School = school;
                else
                    newSpell.School = SpellSchools.First();

                // Parse components
                foreach(string compString in spell.spellComponents.components)
                {
                    var comp = GetComponentByInitial(compString);
                    if(comp != null)
                        newSpell.AddComponent(comp);
                }
                newSpell.MaterialComponent = spell.spellComponents.material;

                // Parse errata
                foreach (var errata in spell.errata)
                    newSpell.AddErrata(new Errata((string)spell.month, (string)spell.year, (string)spell.text, newSpell));

                // Parse source
                var source = Compendium.GetSourceByName((string)spell.source);
                if (source != null)
                    newSpell.Source = source;
                else
                    newSpell.Source = null;

                _AllSpells.Add(newSpell);
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assisticant.Fields;
using System.IO;
using System.Reflection;
using Compendium.Model.Helpers;
using Compendium.Model.Common;
using Assisticant.Collections;
using Newtonsoft.Json;
using Compendium.Model.Interfaces;
using Compendium.Model.Filtering;
using Compendium.Model.JsonConverters;

namespace Compendium.Model.Models
{
    public class CompendiumModel
    {
        //private readonly SelectionModel<CharacterModel> _CharacterSelection;

        public SpellPageModel SpellPage;
        public ClassPageModel ClassPage;
        public List<ContentPageModel> OtherPages;

        public CompendiumModel()
        {
            OtherPages = new List<ContentPageModel>();
            //_CharacterSelection = new SelectionModel<CharacterModel>();
        }


        private ObservableList<ContentPageModel> _ContentPages = new ObservableList<ContentPageModel>();
        public IEnumerable<ContentPageModel> ContentPages
        {
            get { return _ContentPages; }
        }

        private ObservableList<ContentSource> _ContentSources = new ObservableList<ContentSource>();
        public IEnumerable<ContentSource> ContentSources
        {
            get { return _ContentSources; }
        }

        #region Accessor Functions
        public ContentSource GetSourceByID(string sourceID)
        {
            return ContentSources.FirstOrDefault(c => string.Equals(c.ID, sourceID));
        }
        // DELTE ONCE SPELLS HAVE BEEN RE-SAVED WITH NEW JSON FORMAT
        public ContentSource GetSourceByName(string source)
        {
            return ContentSources.FirstOrDefault(c => string.Equals(c.Name, source));
        }

        public void AddContentPage(ContentPageModel content)
        {
            _ContentPages.Add(content);
        }
        #endregion

        #region Character Stuff
        private ObservableList<CharacterModel> _Characters = new ObservableList<CharacterModel>();
        public IEnumerable<CharacterModel> Characters
        {
            get { return _Characters; }
        }

        //public SelectionModel<CharacterModel> _CharacterSelection;
        //public CharacterModel SelectedCharacter => _CharacterSelection.Value;

        public void AddNewCharacter()
        {
            _Characters.Add(new CharacterModel());
        }
        #endregion

        #region Deserialize Functions
        public void DeserializeContentSources(Sources_Json sources)
        {
            foreach (var source in sources.sources)
                _ContentSources.Add(new ContentSource(source.name, source.id));

            if (ContentSources.Count() <= 0)
            {
                throw new NullReferenceException("No sources were loaded from source json file. Is the file empty?");
            }
        }

        //public void DeserializeSpellPage(string spellString, string schoolString, string componentString)
        //{
        //    dynamic spells = JsonConvert.DeserializeObject<dynamic>(spellString);
        //    dynamic schools = JsonConvert.DeserializeObject<dynamic>(schoolString);
        //    dynamic components = JsonConvert.DeserializeObject<dynamic>(componentString);

        //    ContentPageModel page = new ContentPageModel(this);

        //    page.AddFilterGroup(new FilterGroup(
        //        "level",
        //        "Filter by Level",
        //        new List<string>() {
        //            "Cantrips",
        //            "1st-level",
        //            "2nd-level",
        //            "3rd-level",
        //            "4th-level",
        //            "5th-level",
        //            "6th-level",
        //            "7th-level",
        //            "8th-level",
        //            "9th-level"
        //        }));

        //    FilterGroup classFilters = new FilterGroup("class", "Filter by Class", new List<string>());
        //    //foreach(var c in ClassPage.Content)
        //    //{
        //    //    classFilters.AddItem()
        //    //}

        //    FilterGroup schoolFilters = new FilterGroup("school", "Filter by School", new List<string>());
        //    foreach(var s in schools.scholls)
        //        schoolFilters.AddItem((string)s);
        //    page.AddFilterGroup(schoolFilters);

        //    FilterGroup componentFilters = new FilterGroup("components", "Filter by Components", new List<string>());
        //    foreach(var c in components.components)
        //        componentFilters.AddItem((string)c.name);
        //    page.AddFilterGroup(componentFilters);

        //    List<ContentItemModel> allSpells = new List<ContentItemModel>();
        //    foreach(var spell in obj.spells)
        //    {

        //    }
        //}
        #endregion
    }
}

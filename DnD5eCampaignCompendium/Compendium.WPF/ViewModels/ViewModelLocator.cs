using Assisticant;
using Compendium.Model.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Compendium.Model.Models;
using Compendium.WPF.ViewModels.Common;
using Compendium.WPF.Views;
using Compendium.Model.JsonConverters;
using System.Linq;
using Compendium.Model.Filtering;

namespace Compendium.WPF.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private const string DATA_FOLDER = @"..\..\..\Compendium.Model\Json";
        private const string INDEX = "index.json";

        private readonly CompendiumModel _Compendium;
        private readonly SpellPageModel _SpellPage;
        private readonly ClassPageModel _ClassPage;

        public object CompendiumVM => ViewModel(() => new CompendiumViewModel(_Compendium));
        public object SpellPageVM => ViewModel(() => new SpellPageViewModel(_Compendium));
        public object ClassPageVM => ViewModel(() => new ClassPageViewModel(_ClassPage));
        private List<Page> _AllPagesVM;

        //public object CharacterVM => ViewModel(() => new CharacterViewModel(_Compendium, _CharacterSelection));


        public ViewModelLocator()
        {
            _Compendium = new CompendiumModel();
            _SpellPage = new SpellPageModel(_Compendium);
            _ClassPage = new ClassPageModel(_Compendium);
            _AllPagesVM = new List<Page>();

            if (DesignMode)
            {

            }
            else
            {
                string dataDir = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    DATA_FOLDER);
                string indexFile = Path.Combine(dataDir, INDEX);

                if (!File.Exists(indexFile))
                    throw new ArgumentNullException(indexFile + " not found.");

                // Parse the index file that contains all the other pages that will be loaded
                //dynamic index = JsonConvert.DeserializeObject(ResourceHelper.ReadTextFromFile(indexFile));
                Index_Json index = JsonConvert.DeserializeObject<Index_Json>(ResourceHelper.ReadTextFromFile(indexFile));

                // Load spells and supporting info
                if (index.spells != null)
                {
                    _Compendium.AddContentPage(LoadSpellInfo(dataDir, index));
                }

                // Load classes
                if (index.classes != null && index.classSpells != null)
                {

                }

                // Load all other pages
                if (index.otherPages != null)
                {
                    foreach(var page in index.otherPages)
                    {
                        // TODO: More error checking
                        if (page.title == null || page.content == null)
                            continue;

                        ContentPageModel cpm = new ContentPageModel(_Compendium)
                        {
                            Header = page.title
                        };
                        cpm.DeserializeContent(
                            dataDir,
                            JsonConvert.DeserializeObject<ContentPage_Json>(
                                ReadFileFromIndex(Path.Combine(dataDir, page.content))));

                        _Compendium.OtherPages.Add(cpm);
                    }
                }

                #region old
                // Load the content sources first, so that other content can use the sources.
                // Sources being PHB, EE, Volo's, SCAG, etc.
                //_Compendium.DeserializeContentSources(
                //    JsonConvert.DeserializeObject<Sources_Json>(
                //        ReadFileFromIndex(
                //            Path.Combine(dataDir, index.sources))));

                //// If all of the spell json files are present, load the spells
                //if (index.spells != null && index.spellSchools != null && index.spellComponents != null)
                //{
                //    // Do schools and components first since the spells rely on those
                //    _SpellPage.DeserializeSchools(
                //        JsonConvert.DeserializeObject<Schools_Json>(
                //            ReadFileFromIndex(
                //                Path.Combine(dataDir, index.spellSchools))));
                //    _SpellPage.DeserializeComponents(
                //        JsonConvert.DeserializeObject<Components_Json>(
                //            ReadFileFromIndex(
                //                Path.Combine(dataDir, index.spellComponents))));
                //    _SpellPage.DeserializeSpells(
                //        JsonConvert.DeserializeObject<Spells_Json>(
                //            ReadFileFromIndex(Path.Combine(dataDir, index.spells))));
                //}

                //// If both class json files are present, load the classe
                //if (index.classes != null && index.classSpells != null)
                //{
                //    // Load classes firest since they need to be loaded before binding classes with their spell lists
                //    _ClassPage.DeserializeContent(
                //        dataDir,
                //        JsonConvert.DeserializeObject<Classes_Json>(
                //            ReadFileFromIndex(Path.Combine(dataDir, index.classes))));
                //    _ClassPage.DeserializeClassSpells(
                //        ReadFileFromIndex(Path.Combine(dataDir, index.classSpells)));
                //}

                //// Load the rest of the content from the "otherPages" into their own content viewers
                //if (index.otherPages != null)
                //{
                //    foreach (var page in index.otherPages)
                //    {
                //        // TODO: Add dialog about incorrect json
                //        if (page.title == null || page.content == null)
                //            continue;

                //        ContentPageModel cpm = new ContentPageModel(_Compendium)
                //        {
                //            Header = page.title
                //        };
                //        cpm.DeserializeContent(
                //            dataDir,
                //            JsonConvert.DeserializeObject<ContentPage_Json>(
                //                ReadFileFromIndex(Path.Combine(dataDir, page.content))));

                //        _Compendium.OtherPages.Add(cpm);
                //    }
                //}
                #endregion
            }
        }

        #region Loading Spells
        private ContentPageModel LoadSpellInfo(string dataDir, Index_Json index)
        {
            var spells = JsonConvert.DeserializeObject<Spells_Json>(
                ReadFileFromIndex(Path.Combine(dataDir, index.spells)));
            var components = JsonConvert.DeserializeObject<Components_Json>(
                ReadFileFromIndex(Path.Combine(dataDir, index.spellComponents)));
            var classSpells = JsonConvert.DeserializeObject<ClassSpellListCollection_Json>(
                ReadFileFromIndex(Path.Combine(dataDir, index.classSpells)));

            ContentPageModel cpm = new ContentPageModel(_Compendium)
            {
                Header = "Spells"
            };

            cpm.AddFilterGroup("level", "Filter by Level");
            cpm.AddFilterGroup("class", "Filter by Class");
            cpm.AddFilterGroup("school", "Filter by School");
            cpm.AddFilterGroup("components", "Filter by Components");
            cpm.AddFilterGroup("other", "Other Filters");

            // Create all spells (and initialize with basic filter properties)
            foreach (var spell in spells.spells)
            {
                ContentItemModel spellModel = new ContentItemModel()
                {
                    Name = spell.name,
                    ID = spell.id,
                    Markdown = GetSpellMarkdownFromJson(spell)
                };
                
                spellModel.AddFilterProperty("level", spell.level.ToString());

                spellModel.AddFilterProperty("school", spell.school);
                // If the current spell's school is not in the filter list, add it
                if(!cpm.FilterGroups.Any(g => string.Equals(g.Header, spell.school)))
                    cpm.AddOptionToFilterGroup("school", spell.school, spell.school.ToLower().Replace(' ', '_'));

                foreach (var comp in spell.components)
                    spellModel.AddFilterProperty("components", comp);

                // ADD SINGLE TRUE/FALSE FILTER PROPERTIES
                //spellModel.AddFilterProperty("ritual", spell.isRitual ? "true" : "false");
                //spellModel.AddFilterProperty("concentration", spell.concentration ? "true" : "false");

                cpm.AddContentItem(spellModel);
            }

            // Add class filter properties for all spells
            foreach(var charClass in classSpells.classes)
                AddClassFiltersToSpell(cpm, charClass);

            // Add filter options to the filter groups
            cpm.AddOptionToFilterGroup("level", "Cantrips", "0");
            cpm.AddOptionToFilterGroup("level", "1st-level", "1");
            cpm.AddOptionToFilterGroup("level", "2nd-level", "2");
            cpm.AddOptionToFilterGroup("level", "3rd-level", "3");
            cpm.AddOptionToFilterGroup("level", "4th-level", "4");
            cpm.AddOptionToFilterGroup("level", "5th-level", "5");
            cpm.AddOptionToFilterGroup("level", "6th-level", "6");
            cpm.AddOptionToFilterGroup("level", "7th-level", "7");
            cpm.AddOptionToFilterGroup("level", "8th-level", "8");
            cpm.AddOptionToFilterGroup("level", "9th-level", "9");

            foreach(var comp in components.components)
                cpm.AddOptionToFilterGroup("components", comp.name, comp.initial);

            cpm.AddOptionToFilterGroup("other", "Show only ritual spells", "ritual");
            cpm.AddOptionToFilterGroup("other", "Show only concentration spells", "conc");

            return cpm;
        }

        private void AddClassFiltersToSpell(ContentPageModel spellPage, ClassSpellList_Json json)
        {
            if(json.spells != null)
            {
                foreach (var spellID in json.spells)
                    spellPage.GetItemByID(spellID).AddFilterProperty("class", json.id);
            }

            if(json.subclasses != null)
            {
                foreach (var subclass in json.subclasses)
                    AddClassFiltersToSpell(spellPage, subclass);
            }
        }

        private string GetSpellMarkdownFromJson(Spell_Json json)
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
                    "{8}",
                    json.name,
                    GetSpellLevelAndSchool(json.level, json.school, json.isRitual),
                    json.castingTime,
                    json.range,
                    string.Format("{0}{1}",
                        string.Join(", ", json.components),
                        string.IsNullOrWhiteSpace(json.material) ? "" : string.Format(" ({0})", json.material)),
                    json.concentration ? "Concentration, up to " + json.duration : json.duration,
                    string.Join("\n", json.description),
                    json.higherLevel.Count > 0
                        ? string.Format("**At Higher Levels:** {0}\n\n", json.higherLevel)
                        : "",
                    json.errata.Count > 0
                        ? string.Format("\n\n#### Errata: \n\n{0}",
                            string.Join("\n", json.errata.Select(e => string.Format("+ {0}", e.ToString()))))
                        : ""
                );
        }

        private string GetSpellLevelAndSchool(int level, string school, bool ritual)
        {
            string s = "";
            switch (level)
            {
                case (0):
                    s = school + " cantrip";
                    break;
                case (1):
                    s = "1st-level " + school;
                    break;
                case (2):
                    s = "2nd-level " + school;
                    break;
                case (3):
                    s = "3rd-level " + school;
                    break;
                default:
                    s = level + "th-level " + school;
                    break;
            }
            if (ritual)
                s += " (ritual)";

            return s;
        }
        #endregion

        #region Loading Classes
        #endregion

        #region Loading all other pages
        #endregion

        private string ReadFileFromIndex(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentNullException(path + " does not exist.");
            return ResourceHelper.ReadTextFromFile(path);
        }

        // WILL NEED TO UPDATE
        private void ExtractResourcesToDisk(string dataDir)
        {
            // Extract all content to the data folder
            Directory.CreateDirectory(dataDir);
            Directory.CreateDirectory(Path.Combine(dataDir, "Common"));
            Directory.CreateDirectory(Path.Combine(dataDir, "Spells"));

            Directory.CreateDirectory(Path.Combine(dataDir, "Classes"));
            Directory.CreateDirectory(Path.Combine(dataDir, "Classes\\Druid"));
        }
    }
}

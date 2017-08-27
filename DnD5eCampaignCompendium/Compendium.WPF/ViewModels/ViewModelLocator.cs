using Assisticant;
using Compendium.Model;
using Compendium.Model.ClassViewer;
using Compendium.Model.SpellViewer;
using Compendium.WPF.ViewModels.ClassViewer;
using Compendium.WPF.ViewModels.SpellViewer;
using Compendium.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Compendium.Model.Races;
using Compendium.WPF.ViewModels.RaceViewer;
using Compendium.Model.Common;

namespace Compendium.WPF.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private const string DATA_FOLDER = @"..\..\..\Compendium.Model\Json";
        private const string INDEX = "index.json";

        //private const string DATA_FOLDER = "Compendium.Model.Json";
        //private const string CLASSES_FOLDER = "Compendium.Model.Json.Classes";
        //private const string CLASSES_INDEX = "ClassIndex.json";
        //private const string SOURCES = "Compendium.Model.Json.Common.Sources.json";
        //private const string SPELLS = "Compendium.Model.Json.Spells.Spells.json";
        //private const string SPELL_COMPONENTS = "Compendium.Model.Json.Spells.SpellComponents.json";
        //private const string SPELL_SCHOOLS = "Compendium.Model.Json.Spells.SpellSchools.json";

        private readonly CompendiumModel _Compendium;
        private readonly SpellViewerModel _SpellViewer;
        private readonly SelectionModel<Spell> _SelectedSpell;
        private readonly ClassViewerModel _ClassViewer;
        private readonly SelectionModel<CharacterClass> _SelectedClass;
        private readonly RaceViewerModel _RaceViewer;
        private readonly SelectionModel<Race> _SelectedRace;

        public object Compendium => ViewModel(() => new CompendiumViewModel(_Compendium));

        public object SpellViewer => ViewModel(() => new SpellViewerViewModel(_Compendium, _SelectedSpell));

        public object ClassViewer => ViewModel(() => new ClassViewerViewModel(_ClassViewer, _SelectedClass));

        public object RaceViewer => ViewModel(() => new RaceViewerViewModel(_RaceViewer, _SelectedRace));

        public ViewModelLocator()
        {
            _Compendium = new CompendiumModel();
            _SpellViewer = new SpellViewerModel(_Compendium);
            _ClassViewer = new ClassViewerModel(_Compendium);
            _RaceViewer = new RaceViewerModel(_Compendium);
            _SelectedSpell = new SelectionModel<Spell>();
            _SelectedClass = new SelectionModel<CharacterClass>();
            _SelectedRace = new SelectionModel<Race>();

            if (DesignMode)
            {

            }
            else
            {
                // TODO
                // Actually extract embedded resources from the resources to the location
                string dataDir = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    DATA_FOLDER);
                string indexFile = Path.Combine(dataDir, INDEX);

                if (!File.Exists(indexFile))
                    throw new ArgumentNullException(indexFile + " not found.");

                dynamic index = JsonConvert.DeserializeObject(ResourceHelper.ReadTextFromFile(indexFile));

                // CONTENT SOURCES
                _Compendium.DeserializeContentSources(
                    ReadFileFromIndex(Path.Combine(dataDir, (string)index.sources)));

                // SPELLS
                if (index.spells != null && index.spellSchools != null && index.spellComponents != null)
                {
                    _SpellViewer.DeserializeSchools(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spellSchools)));
                    _SpellViewer.DeserializeComponents(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spellComponents)));
                    _SpellViewer.DeserializeSpells(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spells)));
                }

                // CLASSES
                if(index.classes != null && index.classSpells != null)
                {
                    _ClassViewer.DeserializeClasses(
                        dataDir,
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.classes)));
                    _ClassViewer.DeserializeClassSpells(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.classSpells)));
                }

                // Races
                if(index.races != null)
                {
                    _RaceViewer.DeserializeRaces(
                        dataDir, ReadFileFromIndex(Path.Combine(dataDir, (string)index.races)));
                }
            }
        }

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

using Assisticant;
using Compendium.Model;
using Compendium.Model.CharacterClasses;
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

namespace Compendium.WPF.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private const string DATA_FOLDER = "CompendiumData";
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
        private readonly SpellSelectionModel _SelectedSpell;
        private readonly ClassViewerModel _ClassViewer;
        private readonly ClassSelectionModel _SelectedClass;

        public object Compendium => ViewModel(() => new CompendiumViewModel(_Compendium));

        public object SpellViewer => ViewModel(() => new SpellViewerViewModel(_Compendium, _SelectedSpell));
        public object SpellDisplay => ViewModel(() => new SpellDisplayViewModel(_SelectedSpell));

        public object ClassViewer => ViewModel(() => new ClassViewerViewModel(_ClassViewer, _SelectedClass));

        public ViewModelLocator()
        {
            _Compendium = new CompendiumModel();
            _SpellViewer = new SpellViewerModel(_Compendium);
            _ClassViewer = new ClassViewerModel(_Compendium);
            _SelectedSpell = new SpellSelectionModel();
            _SelectedClass = new ClassSelectionModel();

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
                    ReadFileFromIndex(dataDir, index.sources, "index.json is missing entry for sources."));

                // SPELLS
                _SpellViewer.DeserializeSchools(
                    ReadFileFromIndex(dataDir, index.spellSchools, "index.json is missing entry for spell schools."));
                _SpellViewer.DeserializeComponents(
                    ReadFileFromIndex(dataDir, index.spellComponents, "index.json is missing entry for spell components."));
                _SpellViewer.DeserializeSpells(
                    ReadFileFromIndex(dataDir, index.spells, "index.json is missing entry for spells."));

                // CLASSES
                _ClassViewer.DeserializeClasses(
                    dataDir,
                    ReadFileFromIndex(dataDir, index.classes, "index.json is missing entry for classes."));

                // SPELLS
                //_SpellViewer.DeserializeSchools(
                //    ResourceHelper.ReadEmbeddedResourceContent(SPELL_SCHOOLS));
                //_SpellViewer.DeserializeComponents(
                //    ResourceHelper.ReadEmbeddedResourceContent(SPELL_COMPONENTS));
                //_SpellViewer.DeserializeSpells(
                //    ResourceHelper.ReadEmbeddedResourceContent(SPELLS));

                // CLASSES
                // Classes HAVE to be done after spells, because classes contain spells, and thus the spell list
                // needs to be populated before creating classes that query for those spells
                //_ClassViewer.DeserializeClasses(CLASSES_FOLDER, CLASSES_INDEX);
            }
        }

        private string ReadFileFromIndex(string dataDir, dynamic json, string error)
        {
            if (json == null)
                throw new ArgumentNullException(error);
            string path = Path.Combine(dataDir, (string)json);
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

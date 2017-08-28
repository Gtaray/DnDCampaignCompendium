using Assisticant;
using Compendium.Model;
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
using Compendium.Model.Common;
using Compendium.Model.Models;

namespace Compendium.WPF.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private const string DATA_FOLDER = @"..\..\..\Compendium.Model\Json";
        private const string INDEX = "index.json";

        private readonly CompendiumModel _Compendium;
        private readonly SpellPageModel _SpellViewer;
        private readonly ClassPageModel _ClassViewer;

        public object Compendium => ViewModel(() => new CompendiumViewModel(_Compendium));

        public object SpellViewer => ViewModel(() => new SpellPageViewModel(_Compendium));

        public object ClassViewer => ViewModel(() => new ClassPageViewModel(_ClassViewer));


        public ViewModelLocator()
        {
            _Compendium = new CompendiumModel();
            _SpellViewer = new SpellPageModel(_Compendium);
            _ClassViewer = new ClassPageModel(_Compendium);

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

                // Parse the index file that contains all the other pages that will be loaded
                dynamic index = JsonConvert.DeserializeObject(ResourceHelper.ReadTextFromFile(indexFile));

                // Load the content sources first, so that other content can use the sources.
                // Sources being PHB, EE, Volo's, SCAG, etc.
                _Compendium.DeserializeContentSources(
                    ReadFileFromIndex(Path.Combine(dataDir, (string)index.sources)));

                // If all of the spell json files are present, load the spells
                if (index.spells != null && index.spellSchools != null && index.spellComponents != null)
                {
                    // Do schools and components first since the spells rely on those
                    _SpellViewer.DeserializeSchools(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spellSchools)));
                    _SpellViewer.DeserializeComponents(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spellComponents)));
                    _SpellViewer.DeserializeSpells(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spells)));
                }

                // If both class json files are present, load the classe
                if(index.classes != null && index.classSpells != null)
                {
                    // Load classes firest since they need to be loaded before binding classes with their spell lists
                    _ClassViewer.DeserializeContent(
                        dataDir,
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.classes)));
                    _ClassViewer.DeserializeClassSpells(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.classSpells)));
                }

                // Load the rest of the content from the "otherPages" into their own content viewers
                if(index.otherPages != null)
                {
                    foreach(var page in index.otherPages)
                    {
                        // TODO: Add dialog about incorrect json
                        if (page.title == null || page.content == null)
                            continue;

                        ContentPageModel cpm = new ContentPageModel(_Compendium)
                        {
                            Header = (string)page.title
                        };
                        cpm.DeserializeContent(
                            dataDir,
                            ReadFileFromIndex(Path.Combine(dataDir, (string)page.content)));
                        _Compendium.OtherPages.Add(cpm);
                    }
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

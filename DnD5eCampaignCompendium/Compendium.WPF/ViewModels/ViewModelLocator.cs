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

namespace Compendium.WPF.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private const string DATA_FOLDER = @"..\..\..\Compendium.Model\Json";
        private const string INDEX = "index.json";

        private readonly CompendiumModel _Compendium;
        private readonly SpellPageModel _SpellPage;
        private readonly ClassPageModel _ClassPage;

        private readonly SelectionModel<CharacterModel> _CharacterSelection;

        public object CompendiumVM => ViewModel(() => new CompendiumViewModel(_Compendium, _AllPagesVM));
        public object SpellPageVM => ViewModel(() => new SpellPageViewModel(_Compendium));
        public object ClassPageVM => ViewModel(() => new ClassPageViewModel(_ClassPage));
        private List<Page> _AllPagesVM;

        public object CharacterVM => ViewModel(() => new CharacterViewModel(_Compendium, _CharacterSelection));


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
                    _SpellPage.DeserializeSchools(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spellSchools)));
                    _SpellPage.DeserializeComponents(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spellComponents)));
                    _SpellPage.DeserializeSpells(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.spells)));
                }

                // If both class json files are present, load the classe
                if (index.classes != null && index.classSpells != null)
                {
                    // Load classes firest since they need to be loaded before binding classes with their spell lists
                    _ClassPage.DeserializeContent(
                        dataDir,
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.classes)));
                    _ClassPage.DeserializeClassSpells(
                        ReadFileFromIndex(Path.Combine(dataDir, (string)index.classSpells)));
                }

                // Load the rest of the content from the "otherPages" into their own content viewers
                if (index.otherPages != null)
                {
                    foreach (var page in index.otherPages)
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

            var spells = new SpellViewerView();
            spells.DataContext = SpellPageVM;
            _AllPagesVM.Add(new Page("Spells", spells));

            var classes = new ClassViewerView();
            classes.DataContext = ClassPageVM;
            _AllPagesVM.Add(new Page("Classes", classes));

            foreach(var page in _Compendium.OtherPages)
            {
                var content = new ContentViewerView();
                content.DataContext = ViewModel(() => new ContentPageViewModel(_Compendium, page));
                _AllPagesVM.Add(new Page(page.Header, content));
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compendium.Model.SpellViewer;
using Compendium.Model.CharacterClasses;
using Assisticant.Fields;
using System.IO;
using System.Reflection;
using Compendium.Model.Helpers;
using Compendium.Model.Common;
using Assisticant.Collections;
using Newtonsoft.Json;

namespace Compendium.Model
{
    public class CompendiumModel
    {
        private const string SOURCES = "Compendium.Model.Json.Sources.json";
        private const string SPELLS = "Compendium.Model.Json.Spells.json";
        private const string SPELL_COMPONENTS = "Compendium.Model.Json.SpellComponents.json";
        private const string SPELL_SCHOOLS = "Compendium.Model.Json.SpellSchools.json";
        private const string CLASSES = "Compendium.Model.Json.Classes.json";

        public CompendiumModel()
        {
            // CONTENT SOURCES
            DeserializeContentSources(
                ResourceHelper.ReadEmbeddedResourceContent(SOURCES));

            // SPELLS
            SpellViewer = new SpellViewerModel(this);
            SpellViewer.DeserializeSchools(
                ResourceHelper.ReadEmbeddedResourceContent(SPELL_SCHOOLS));
            SpellViewer.DeserializeComponents(
                ResourceHelper.ReadEmbeddedResourceContent(SPELL_COMPONENTS));
            SpellViewer.DeserializeSpells(
                ResourceHelper.ReadEmbeddedResourceContent(SPELLS));

            // CLASSES
            // Classes HAVE to be done after spells, because classes contain spells, and thus the spell list
            // needs to be populated before creating classes that query for those spells
            ClassViewer = new ClassViewerModel(this);
            ClassViewer.DeserializeClasses(
                ResourceHelper.ReadEmbeddedResourceContent(CLASSES));
        }

        private ObservableList<ContentSource> _ContentSources = new ObservableList<ContentSource>();
        public IEnumerable<ContentSource> ContentSources
        {
            get { return _ContentSources; }
        }

        #region Content models
        private Observable<SpellViewerModel> _SpellViewer = new Observable<SpellViewerModel>();
        public SpellViewerModel SpellViewer
        {
            get { return _SpellViewer; }
            set { _SpellViewer.Value = value; }
        }

        private Observable<ClassViewerModel> _ClassViewer = new Observable<ClassViewerModel>();
        public ClassViewerModel ClassViewer
        {
            get { return _ClassViewer; }
            set { _ClassViewer.Value = value; }
        }
        #endregion

        #region Accessor Functions
        public ContentSource GetSourceByName(string source)
        {
            return ContentSources.FirstOrDefault(c => string.Equals(c.Name, source));
        }
        #endregion

        #region Deserialize Functions
        private void DeserializeContentSources(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException("Sources.json is null, empty, or is not being read properly");

            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var source in obj.sources)
                _ContentSources.Add(new ContentSource((string)source.name));

            if (ContentSources.Count() <= 0)
            {
                throw new NullReferenceException("No sources were loaded from Sources.json. Is the file empty?");
            }
        }
        #endregion
    }
}

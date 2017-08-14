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
        public SpellViewerModel SpellViewer;
        public ClassViewerModel ClassViewer;

        public CompendiumModel()
        {

        }

        private ObservableList<ContentSource> _ContentSources = new ObservableList<ContentSource>();
        public IEnumerable<ContentSource> ContentSources
        {
            get { return _ContentSources; }
        }

        #region Accessor Functions
        public ContentSource GetSourceByName(string source)
        {
            return ContentSources.FirstOrDefault(c => string.Equals(c.Name, source));
        }
        #endregion

        #region Deserialize Functions
        public void DeserializeContentSources(string json)
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

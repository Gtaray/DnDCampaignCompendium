﻿using System;
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
        public void DeserializeContentSources(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var source in obj.sources)
                _ContentSources.Add(new ContentSource((string)source.name, (string)source.id));

            if (ContentSources.Count() <= 0)
            {
                throw new NullReferenceException("No sources were loaded from source json file. Is the file empty?");
            }
        }
        #endregion
    }
}

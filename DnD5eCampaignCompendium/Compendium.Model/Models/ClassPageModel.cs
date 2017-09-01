using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Helpers;
using Compendium.Model.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Models
{
    public class ClassPageModel : IContentPage<ClassModel>
    {
        private readonly CompendiumModel _Compendium;
        private readonly SelectionModel<ClassModel> _Selection;

        public ClassPageModel(CompendiumModel compendium)
        {
            _Compendium = compendium;
            _Compendium.ClassViewer = this;
            _Content = new ObservableList<ClassModel>();
            _Selection = new SelectionModel<ClassModel>();
        }

        #region Properties and Accessors
        // This is made public because we need to be able to pass it to the view model
        // so that the vm can handle selection within the hierarchical/tree vieww of the classes.
        // if ever there's a way to set a tree's selectedItem binding, then this won't be necessary
        public SelectionModel<ClassModel> Selection => _Selection;

        public ClassModel SelectedItem
        {
            get { return _Selection.Value; }
            set { _Selection.Value = value; }
        }

        private Observable<string> _Header = new Observable<string>(default(string));
        public string Header
        {
            get { return _Header; }
            set { _Header.Value = value; }
        }

        private ObservableList<ClassModel> _Content;
        public IEnumerable<ClassModel> Content
        {
            get { return _Content; }
        }
        #endregion

        #region Json Parsing
        public void DeserializeContent(string dataDir, string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var charClass in obj.classes)
            {
                _Content.Add(DeserializeSingleClass(dataDir, charClass));
            }

            if (Content.Count() <= 0)
            {
                throw new ArgumentNullException("No classes were loaded from classes json file. Is the file empty?");
            }
        }

        private ClassModel DeserializeSingleClass(string dataDir, dynamic json)
        {
            string name = json.name != null ? (string)json.name : "Unknown";
            string id = json.id != null ? (string)json.id : "";
            string source = json.source != null ? (string)json.source : null;
            string filePath = json.file != null ? Path.Combine(dataDir, (string)json.file) : "";
            ClassModel newClass = new ClassModel()
            {
                Name = name,
                ShortName = json.shortName != null ? json.shortName : "",
                ID = id,
                Source = _Compendium.GetSourceByID(source),
                Markdown = ResourceHelper.ReadTextFromFile(filePath),
                ShowInClassList = json.showInClassList != null ? json.showInClassList : true,
                ShowInFilterList = json.showInFilterList != null ? json.showInFilterList : true
            };

            if(json.subclasses != null)
            {
                foreach (var subclass in json.subclasses)
                    newClass.AddSubclass(DeserializeSingleClass(dataDir, subclass));
            }

            return newClass;            
        }

        public void DeserializeClassSpells(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach(var charClass in obj.classes)
            {
                if (charClass.id == null) continue;

                var cc = Content.Flatten(c => c.Subclasses).FirstOrDefault(c => string.Equals(c.ID, (string)charClass.id));

                if (cc == null) continue;
                if (charClass.spells == null) continue;

                foreach (var s in charClass.spells)
                {
                    if (s == null) continue;
                    var spell = _Compendium.SpellViewer.GetSpellByID((string)s);
                    if (spell == null) continue;
                    cc.AddSpell(spell);
                }
            }
        }
        #endregion
    }
}

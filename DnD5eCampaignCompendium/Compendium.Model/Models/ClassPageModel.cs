using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Filtering;
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
            _Compendium.ClassPage = this;
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

        private ObservableList<FilterGroup> _FilterGroups = new ObservableList<FilterGroup>();
        public IEnumerable<FilterGroup> FilterGroups => _FilterGroups;
        #endregion

        #region Json Parsing
        public void DeserializeContent(string dataDir, string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var charClass in obj.classes)
            {
                _Content.Add(
                    DeserializeSingleClass(dataDir, charClass));
            }

            if (obj.filters != null)
            {
                foreach (var filter in obj.filters)
                {
                    FilterGroup fg = new FilterGroup()
                    {
                        ID = filter.id != null ? (string)filter.id : "",
                        Header = filter.title != null ? (string)filter.title : "Filter by Unknown",
                        ShowExlusiveToggle = filter.showExclusiveToggle != null ? (bool)filter.showExclusiveToggle : false,
                        ExclusiveToggleLabel = filter.exclusiveToggleLabel != null ? (string)filter.exclusiveToggleLabel : "Exclude unchecked options",
                        IsExclusive = filter.isExclusive != null ? (bool)filter.isExclusive : false
                    };
                    foreach (var option in filter.options)
                        fg.AddItem((string)option);

                    _FilterGroups.Add(fg);
                }
            }

            if (Content.Count() <= 0)
            {
                throw new ArgumentNullException("No classes were loaded from classes json file. Is the file empty?");
            }
        }

        private ClassModel DeserializeSingleClass(string dataDir, dynamic json)
        {
            ClassModel newClass = new ClassModel()
            {
                Name = json.name != null ? (string)json.name : "Unknown",
                ShortName = json.shortName != null ? json.shortName : "",
                ID = json.id != null ? (string)json.id : "",
                Source = _Compendium.GetSourceByID(json.source != null ? (string)json.source : null),
                Markdown = ResourceHelper.ReadTextFromFile(json.file != null ? Path.Combine(dataDir, (string)json.file) : ""),
                ShowInClassList = json.showInClassList != null ? json.showInClassList : true,
                ShowInFilterList = json.showInFilterList != null ? json.showInFilterList : true,
            };

            // Set all filters
            if (json.filters != null)
                foreach (var filter in json.filters)
                    newClass.AddFilterProperty(
                        filter.id != null ? (string)filter.id : "",
                        filter.value != null ? (string)filter.value : "");

            if (json.subclasses != null)
                foreach (var subclass in json.subclasses)
                    newClass.AddSubclass(DeserializeSingleClass(dataDir, subclass));

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
                    var spell = _Compendium.SpellPage.GetSpellByID((string)s);
                    if (spell == null) continue;
                    cc.AddSpell(spell);
                }
            }
        }
        #endregion
    }
}

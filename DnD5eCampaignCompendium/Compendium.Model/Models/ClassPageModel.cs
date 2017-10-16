using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Filtering;
using Compendium.Model.Helpers;
using Compendium.Model.Interfaces;
using Compendium.Model.JsonConverters;
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
        public void DeserializeContent(string dataDir, Base_Json json)
        {
            Classes_Json classes = json as Classes_Json;
            foreach (var charClass in classes.classes)
            {
                _Content.Add(
                    DeserializeSingleClass(dataDir, charClass));
            }

            if (classes.filters != null)
            {
                foreach (var filter in classes.filters)
                {
                    FilterGroup fg = new FilterGroup()
                    {
                        ID = filter.id != null ? filter.id : "",
                        Header = filter.title != null ? filter.title : "Filter by Unknown",
                        ShowExlusiveToggle = filter.showExclusiveToggle,
                        ExclusiveToggleLabel = filter.exclusiveToggleLabel != null ? filter.exclusiveToggleLabel : "Exclude unchecked options",
                        IsExclusive = filter.isExclusive
                    };
                    foreach (var option in filter.options)
                        fg.AddItem(option.id, option.display);

                    _FilterGroups.Add(fg);
                }
            }

            if (Content.Count() <= 0)
            {
                throw new ArgumentNullException("No classes were loaded from classes json file. Is the file empty?");
            }
        }

        private ClassModel DeserializeSingleClass(string dataDir, Class_Json charClass)
        {
            ClassModel newClass = new ClassModel()
            {
                Name = charClass.name != null ? charClass.name : "Unknown",
                ShortName = charClass.shortName != null ? charClass.shortName : "",
                ID = charClass.id != null ? charClass.id : "",
                Source = _Compendium.GetSourceByID(charClass.source != null ? charClass.source : null),
                Markdown = ResourceHelper.ReadTextFromFile(charClass.file != null ? Path.Combine(dataDir, charClass.file) : ""),
                ShowInClassList = charClass.showInClassList,
                ShowInFilterList = charClass.showInFilterList,
            };

            // Set all filters
            if (charClass.filters != null)
                foreach (var filter in charClass.filters)
                    newClass.AddFilterProperty(
                        filter.groupID != null ? filter.groupID : "",
                        filter.valueID != null ? filter.valueID : "");

            if (charClass.subclasses != null)
                foreach (var subclass in charClass.subclasses)
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

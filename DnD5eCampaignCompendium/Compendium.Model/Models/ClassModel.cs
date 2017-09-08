using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Helpers;
using Compendium.Model.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Models
{
    public class ClassModel : IContent
    {
        public ClassModel()
        {
            _Spells = new ObservableList<SpellModel>();
            _Subclasses = new ObservableList<ClassModel>();
        }

        public ClassModel(string name, ContentSource source, string markdown)
        {
            _Spells = new ObservableList<SpellModel>();
            _Subclasses = new ObservableList<ClassModel>();

            Name = name;
            Source = source;
            Markdown = markdown;
        }

        #region Properties and Accessors
        private Observable<string> _Name = new Observable<string>(default(string));
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private Observable<string> _ShortName = new Observable<string>(default(string));
        public string ShortName
        {
            get { return _ShortName; }
            set { _ShortName.Value = value; }
        }

        private Observable<string> _ID = new Observable<string>(default(string));
        public string ID
        {
            get { return _ID; }
            set { _ID.Value = value; }
        }

        private Observable<ContentSource> _Source = new Observable<ContentSource>(default(ContentSource));
        public ContentSource Source
        {
            get { return _Source; }
            set { _Source.Value = value; }
        }

        private Observable<bool> _ShowInClassList = new Observable<bool>(true);
        public bool ShowInClassList
        {
            get { return _ShowInClassList; }
            set { _ShowInClassList.Value = value; }
        }

        private Observable<bool> _ShowInFilterList = new Observable<bool>(true);
        public bool ShowInFilterList
        {
            get { return _ShowInFilterList; }
            set { _ShowInFilterList.Value = value; }
        }

        private Observable<string> _Markdown = new Observable<string>("");
        public string Markdown
        {
            get { return _Markdown; }
            set { _Markdown.Value = value; }
        }

        private ObservableList<ClassModel> _Subclasses = new ObservableList<ClassModel>();
        public IEnumerable<ClassModel> Subclasses => _Subclasses;

        private ObservableList<SpellModel> _Spells = new ObservableList<SpellModel>();
        public IEnumerable<SpellModel> Spells => _Spells;

        private Observable<ClassModel> _Parent = new Observable<ClassModel>(default(ClassModel));
        public ClassModel Parent
        {
            get { return _Parent; }
            set { _Parent.Value = value; }
        }
        #endregion

        #region Computed Properties
        private Computed<bool> _HasSpells;
        [JsonIgnore]
        public bool HasSpells
        {
            get
            {
                if (_HasSpells == null)
                    _HasSpells = new Computed<bool>(() => _Spells.Count() > 0 || Subclasses.Any(s => s.HasSpells));
                return _HasSpells.Value;
            }
        }

        private Computed<bool> _IsRoot;
        [JsonIgnore]
        public bool IsRoot
        {
            get
            {
                if (_IsRoot == null)
                    _IsRoot = new Computed<bool>(() => Parent == null);
                return _IsRoot.Value;
            }
        }

        private Computed<string> _Header;
        [JsonIgnore]
        public string Header
        {
            get
            {
                if (_Header == null)
                    _Header = new Computed<string>(() => IsRoot ? Name : Parent.Header + " > " + Name);
                return _Header.Value;
            }
        }

        private Computed<string> _Path;
        [JsonIgnore]
        public string Path
        {
            get
            {
                if (_Path == null)
                    _Path = new Computed<string>(() => IsRoot ? Name : Parent.Path + "." + Path);
                return _Path.Value;
            }
        }

        [JsonProperty(PropertyName = "spells")]
        public IEnumerable<string> SpellIDs
        {
            get { return _Spells.Select(s => s.ID); }
        }
        #endregion

        #region Add Remove Functions
        public void AddSpell(SpellModel spell)
        {
            if (Spells.Contains(spell))
                return;

            _Spells.Add(spell);
            spell.AddClass(this);
        }

        public void RemoveSpell(SpellModel toRemove)
        {
            if (!Spells.Contains(toRemove))
                return;

            _Spells.Remove(toRemove);
            toRemove.RemoveClass(this);
        }

        public void AddSubclass(ClassModel newClass)
        {
            newClass.Parent = this;
            _Subclasses.Add(newClass);
        }

        public void RemoveSubclass(ClassModel toRemove)
        {
            toRemove.Parent = null;
            _Subclasses.Remove(toRemove);
        }
        #endregion


        #region Filtering
        private ObservableDictionary<string, List<string>> _FilterProperties = new ObservableDictionary<string, List<string>>();
        public Dictionary<string, List<string>> FilterProperties =>
            _FilterProperties.ToDictionary(x => x.Key, x => x.Value);

        public void AddFilterProperty(string key, string value)
        {
            // If key doesn't exist, add it and a new list
            if (!_FilterProperties.ContainsKey(key))
                _FilterProperties.Add(key, new List<string>());

            if (!_FilterProperties[key].Contains(value))
                _FilterProperties[key].Add(value);
        }
        #endregion

        #region Misc Functions
        public bool ContainsText(string query)
        {
            // if the query is empty, return true
            if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
                return true;
            // if the query is not empty test
            return 
                (!string.IsNullOrEmpty(Markdown) && Markdown.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                || Subclasses.Any(s => s.ContainsText(query));
        }
        #endregion

        public override string ToString()
        {
            return Header;
        }
    }
}

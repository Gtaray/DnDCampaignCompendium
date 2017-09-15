using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compendium.Model.Models
{
    public class ContentItemModel : IContent
    {
        public ContentItemModel()
        { }

        public ContentItemModel(ContentItemModel parent)
        {
            Parent = parent;
        }

        #region Properties / Accessors
        private Observable<string> _Name = new Observable<string>(default(string));
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private Observable<string> _ID = new Observable<string>(default(string));
        public string ID
        {
            get { return _ID; }
            set { _ID.Value = value; }
        }

        private Observable<ContentItemModel> _Parent = new Observable<ContentItemModel>(null);
        public ContentItemModel Parent
        {
            get { return _Parent; }
            set { _Parent.Value = value; }
        }

        private Observable<ContentSource> _Source = new Observable<ContentSource>(default(ContentSource));
        public ContentSource Source
        {
            get { return _Source; }
            set { _Source.Value = value; }
        }

        private Observable<string> _Markdown = new Observable<string>("");
        public string Markdown
        {
            get { return _Markdown; }
            set { _Markdown.Value = value; }
        }

        private ObservableList<ContentItemModel> _SubContent = new ObservableList<ContentItemModel>();
        public IEnumerable<ContentItemModel> SubContent
        {
            get { return _SubContent; }
        }
        #endregion

        #region Computed Values
        private Computed<bool> _IsRoot;
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
        public string Path
        {
            get
            {
                if (_Path == null)
                    _Path = new Computed<string>(() => IsRoot ? Name : Parent.Path + "." + Path);
                return _Path.Value;
            }
        }
        #endregion

        #region Add SubContent
        public void AddSubContent(ContentItemModel toAdd)
        {
            if (_SubContent.Contains(toAdd))
                return;
            toAdd.Parent = this;
            _SubContent.Add(toAdd);
        }

        public void RemoveSubContent(ContentItemModel toRemove)
        {
            if (!_SubContent.Contains(toRemove))
                return;
            toRemove.Parent = null;
            _SubContent.Remove(toRemove);
        }
        #endregion

        #region Filtering
        // Dictionary that contains an id for a filter group, and then a list of strings
        // that act as the values for that filter.
        // When filtering, the key is used with filter groups, and if a particular
        // key's list of strings contains the value to be filtered for, the item is shown.
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

        public bool ContainsText(string query)
        {
            // if the query is empty, return true
            if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
                return true;
            // if the query is not empty test
            return
                (!string.IsNullOrEmpty(Markdown) && Markdown.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                || SubContent.Any(s => s.ContainsText(query));
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Source?.ToString());
        }
    }
}

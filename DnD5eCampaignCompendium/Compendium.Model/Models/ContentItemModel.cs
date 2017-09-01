using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Compendium.Model.Models
{
    public class ContentItemModel : IContent
    {
        public ContentItemModel()
        { }

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

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Source?.ToString());
        }
    }
}

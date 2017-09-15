using Assisticant.Collections;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Filtering
{
    public class FilterGroup
    {
        public FilterGroup()
        { }

        public FilterGroup(string id, string header, List<string> items)
        {
            ID = id;
            Header = header;
            _Items = new ObservableList<FilterItem>(items.Select(i => new FilterItem(i)));
        }

        private Observable<string> _ID = new Observable<string>(default(string));
        public string ID
        {
            get { return _ID; }
            set { _ID.Value = value; }
        }

        private Observable<string> _Header = new Observable<string>("Filter by Unknown");
        public string Header
        {
            get { return _Header; }
            set { _Header.Value = value; }
        }

        private Observable<bool> _ShowExlusiveToggle = new Observable<bool>(false);
        public bool ShowExlusiveToggle
        {
            get { return _ShowExlusiveToggle; }
            set { _ShowExlusiveToggle.Value = value; }
        }

        private Observable<string> _ExclusiveToggleLabel = new Observable<string>("Exclusive Filter");
        public string ExclusiveToggleLabel
        {
            get { return _ExclusiveToggleLabel; }
            set { _ExclusiveToggleLabel.Value = value; }
        }

        private Observable<bool> _IsExclusive = new Observable<bool>(false);
        public bool IsExclusive
        {
            get { return _IsExclusive; }
            set { _IsExclusive.Value = value; }
        }

        private ObservableList<FilterItem> _Items = new ObservableList<FilterItem>();
        public IEnumerable<FilterItem> Items
        {
            get { return _Items; }
        }

        public void AddItem(string item)
        {
            _Items.Add(new FilterItem(item));
        }

        public void AddItem(FilterItem item)
        {
            _Items.Add(item);
        }

        public void AddItems(IEnumerable<FilterItem> items)
        {
            foreach (var item in items)
                AddItem(item);
        }
    }
}

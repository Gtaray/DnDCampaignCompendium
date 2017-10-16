using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Filtering
{
    public class FilterItem
    {
        public FilterItem(string id, string label)
        {
            ID = id;
            Label = label;
        }
        public FilterItem(FilterItem parent, string id, string label)
        {
            Parent = parent;
            ID = id;
            Label = label;
        }


        private Observable<FilterItem> _Parent = new Observable<FilterItem>(null);
        public FilterItem Parent
        {
            get { return _Parent; }
            set { _Parent.Value = value; }
        }

        private Observable<string> _ID = new Observable<string>(default(string));
        public string ID
        {
            get { return _ID; }
            set { _ID.Value = value; }
        }

        private Observable<string> _Label = new Observable<string>("Unknown");
        public string Label
        {
            get { return _Label; }
            set { _Label.Value = value; }
        }

        private Observable<bool> _IsChecked = new Observable<bool>(false);
        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked.Value = value; }
        }

        private Observable<bool> _IsExpanded = new Observable<bool>(false);
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set { _IsExpanded.Value = value; }
        }
    }
}

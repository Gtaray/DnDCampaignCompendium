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
        public FilterItem(string value)
        {
            Value = value;
        }
        public FilterItem(FilterItem parent, string value)
        {
            Parent = parent;
            Value = value;
        }


        private Observable<FilterItem> _Parent = new Observable<FilterItem>(null);
        public FilterItem Parent
        {
            get { return _Parent; }
            set { _Parent.Value = value; }
        }

        private Observable<string> _Value = new Observable<string>("Unknown");
        public string Value
        {
            get { return _Value; }
            set { _Value.Value = value; }
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

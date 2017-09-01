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
        public FilterGroup(string id, string header, List<string> items)
        {
            ID = id;
            Header = header;
            _Items = new ObservableList<string>(items);
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

        private ObservableList<string> _Items = new ObservableList<string>();
        public IEnumerable<string> Items
        {
            get { return _Items; }
        }
    }
}

using Assisticant.Collections;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Common
{
    public class FilterFlag<T>
    {
        public FilterFlag(T filter, Func<string> label, bool check = false)
        {
            _Label = new Computed<string>(label);
            IsChecked = check;
            Filter = filter;
        }

        private Observable<T> _Filter = new Observable<T>(default(T));
        public T Filter
        {
            get { return _Filter; }
            set { _Filter.Value = value; }
        }

        private Computed<string> _Label;
        public string Label
        {
            get
            {
                if (_Label == null)
                    return _Filter.ToString();
                return _Label.Value;
            }
        }

        private Observable<bool> _IsChecked = new Observable<bool>(default(bool));
        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked.Value = value; }
        }

        private ObservableList<FilterFlag<T>> _Children = new ObservableList<FilterFlag<T>>();
        public IEnumerable<FilterFlag<T>> Children
        {
            get { return _Children; }
        }

        public void AddChildFilter(FilterFlag<T> child)
        {
            _Children.Add(child);
        }

        public bool Equals(T toCompare)
        {
            return Filter.Equals(toCompare);
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Label, IsChecked.ToString());
        }
    }
}

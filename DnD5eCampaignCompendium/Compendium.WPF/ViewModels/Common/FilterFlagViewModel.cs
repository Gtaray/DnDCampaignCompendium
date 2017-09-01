using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.Common
{
    public class FilterFlagViewModel<T>
    {
        public FilterFlagViewModel(T filter, Func<string> label, bool check = false)
        {
            _Label = new Computed<string>(label);
            IsChecked = check;
            Filter = filter;
        }

        public FilterFlagViewModel(T filter, bool check = false)
        {
            _Label = new Computed<string>(() => filter.ToString());
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

        public bool AnyChecked =>
            IsChecked || Children.Any(f => f.AnyChecked);

        private ObservableList<FilterFlagViewModel<T>> _Children = new ObservableList<FilterFlagViewModel<T>>();
        public IEnumerable<FilterFlagViewModel<T>> Children
        {
            get { return _Children; }
        }

        public void AddChildFilter(FilterFlagViewModel<T> child)
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

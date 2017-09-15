using Assisticant.Collections;
using Compendium.Model.Filtering;
using Compendium.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.Common
{
    public class FilterGroupViewModel
    {
        private readonly FilterGroup _Model;

        public FilterGroupViewModel(FilterGroup model)
        {
            _Model = model;
            foreach (var item in _Model.Items)
                _Options.Add(new FilterFlagViewModel<string>(item.Value));
        }

        public string Header => _Model.Header;

        public string ID => _Model.ID;

        private ObservableList<FilterFlagViewModel<string>> _Options = 
            new ObservableList<FilterFlagViewModel<string>>();
        public IEnumerable<FilterFlagViewModel<string>> Options => _Options;

        public bool AnyChecked => Options.Any(i => i.IsChecked);

        // pass in a list of strings representing an item's filter properties
        // return true if the checkbox matching those filter properties is checked
        // return false is none of the checkboxes for that item's properties are checked
        public bool FilterContent(ContentItemModel item)
        {
            if (!item.FilterProperties.ContainsKey(ID)) return false;

            return Options
                .Where(o => item.FilterProperties[ID].Contains(o.Filter, StringComparer.OrdinalIgnoreCase))
                .Any(o => o.IsChecked);
        }

        public bool FilterContent(ClassModel item)
        {
            if (AnyChecked == false) return true;
            if (!item.FilterProperties.ContainsKey(ID)) return false;

            return Options
                .Where(o => item.FilterProperties[ID].Contains(o.Filter, StringComparer.OrdinalIgnoreCase))
                .Any(o => o.IsChecked);
        }
    }
}

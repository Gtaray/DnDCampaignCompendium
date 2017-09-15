using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model;
using Compendium.Model.Filtering;
using Compendium.Model.Models;
using Compendium.WPF.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class ContentPageViewModel : BasePageViewModel
    {
        private readonly CompendiumModel _Compendium;
        private readonly ContentPageModel _Model;

        public ContentPageViewModel(CompendiumModel compendium, ContentPageModel model) : base(model.Header)
        {
            _Compendium = compendium;
            _Model = model;
            foreach (FilterGroup group in _Model.FilterGroups)
                _FilterGroups.Add(new FilterGroupViewModel(group));
        }

        public new string Header => _Model.Header;

        public IEnumerable<ContentItemViewModel> FilteredContent
        {
            get
            {
                var list = _Model.Content;
                if (!string.IsNullOrEmpty(SearchFilter))
                    list = list.Where(i => i?.Markdown != null && i.Markdown.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                if (FilterGroups.Any(g => g.AnyChecked))
                    list = list.Where(c => FilterGroups.Any(g => g.FilterContent(c)));
                return list.Select(c => new ContentItemViewModel(c, _Model.Selection));
            }
        }

        public ContentItemViewModel Selected =>
            _Model.SelectedItem != null ? new ContentItemViewModel(_Model.SelectedItem, _Model.Selection) : null;

        private ObservableList<FilterGroupViewModel> _FilterGroups = new ObservableList<FilterGroupViewModel>();
        public IEnumerable<FilterGroupViewModel> FilterGroups
        {
            get { return _FilterGroups; }
        }

        Observable<string> _SearchFilter = new Observable<string>();
        public string SearchFilter
        {
            get { return _SearchFilter.Value; }
            set { _SearchFilter.Value = value; }
        }
    }
}

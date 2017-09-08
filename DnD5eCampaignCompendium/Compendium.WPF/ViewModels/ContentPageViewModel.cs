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
        private readonly SelectionModel<ContentItemModel> _Selected;

        public ContentPageViewModel(CompendiumModel compendium, ContentPageModel model)
        {
            _Compendium = compendium;
            _Model = model;
            _Selected = new SelectionModel<ContentItemModel>();
            foreach (FilterGroup group in _Model.FilterGroups)
                _FilterGroups.Add(new FilterGroupViewModel(group));
        }

        public string Header => _Model.Header;

        public IEnumerable<ContentItemViewModel> Content => 
            _Model.Content.Select(c => new ContentItemViewModel(c));

        public IEnumerable<ContentItemViewModel> FilteredContent
        {
            get
            {
                var list = _Model.Content;
                if (!string.IsNullOrEmpty(SearchFilter))
                    list = list.Where(i => i?.Markdown != null && i.Markdown.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                if (FilterGroups.Any(g => g.AnyChecked))
                    list = list.Where(c => FilterGroups.Any(g => g.FilterContent(c)));
                return list.Select(c => new ContentItemViewModel(c));
            }
        }

        public ContentItemViewModel Selected
        {
            get { return _Selected.Value != null ? new ContentItemViewModel(_Selected.Value) : null; }
            set { _Selected.Value = value.Model; }
        }

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

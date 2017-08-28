using Compendium.Model;
using Compendium.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.ContentViewer
{
    public class ContentPageViewModel
    {
        private readonly CompendiumModel _Compendium;
        private readonly ContentPageModel _Model;
        private readonly SelectionModel<ContentItemModel> _Selected;

        public ContentPageViewModel(CompendiumModel compendium, ContentPageModel model)
        {
            _Compendium = compendium;
            _Model = model;
            _Selected = new SelectionModel<ContentItemModel>();
        }

        public string Header => _Model.Header;

        public IEnumerable<ContentItemViewModel> Content => 
            _Model.Content.Select(c => new ContentItemViewModel(c));

        public ContentItemViewModel Selected
        {
            get { return new ContentItemViewModel(_Selected.Value); }
            set { _Selected.Value = value.Model; }
        }
    }
}

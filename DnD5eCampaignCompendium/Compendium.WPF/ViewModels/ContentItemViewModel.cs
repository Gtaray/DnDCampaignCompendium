using Assisticant.Fields;
using Compendium.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class ContentItemViewModel
    {
        private readonly ContentItemModel _Model;
        private readonly SelectionModel<ContentItemModel> _Selection;

        public ContentItemViewModel(ContentItemModel model, SelectionModel<ContentItemModel> selection)
        {
            _Model = model;
            _Selection = selection;
        }

        public ContentItemModel Model => _Model;

        public string Name => _Model.Name;

        public string Markdown => string.IsNullOrEmpty(Model?.Markdown) ? "" : Model.Markdown;

        private Observable<bool> _IsSelected = new Observable<bool>(default(bool));
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected.Value = value;
                if (value == true)
                    _Selection.Value = _Model;
            }
        }

        public IEnumerable<ContentItemViewModel> SubContent =>
            _Model.SubContent.Select(c => new ContentItemViewModel(c, _Selection));

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            ContentItemViewModel that = obj as ContentItemViewModel;
            if (that == null)
                return false;
            return Object.Equals(this._Model, that._Model);
        }

        public override int GetHashCode()
        {
            return _Model.GetHashCode();
        }

        public override string ToString()
        {
            return _Model.ToString();
        }
    }
}

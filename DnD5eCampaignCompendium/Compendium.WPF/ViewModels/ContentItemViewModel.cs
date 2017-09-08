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

        public ContentItemViewModel(ContentItemModel model)
        {
            _Model = model;
        }

        public ContentItemModel Model => _Model;

        public string Name => _Model.Name;

        public string Markdown => _Model.Markdown;

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

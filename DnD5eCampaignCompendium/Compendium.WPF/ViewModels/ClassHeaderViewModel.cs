using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class ClassHeaderViewModel
    {
        private readonly ClassModel _Model;
        private readonly SelectionModel<ClassModel> _Selection;

        public ClassHeaderViewModel(ClassModel model, SelectionModel<ClassModel> selection)
        {
            _Model = model;
            _Selection = selection;
        }

        internal ClassModel Model => _Model;

        public string Name
        {
            get { return _Model.Name; }
        }

        private Observable<bool> _IsSelected = new Observable<bool>(false);
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected.Value = value;
                if(value == true)
                    _Selection.Value = _Model;
            }
        }

        public IEnumerable<ClassHeaderViewModel> Subclasses =>
            _Model.Subclasses
                .Where(s => s.ShowInClassList)
                .Select(s => new ClassHeaderViewModel(s, _Selection));

        public string Markdown => string.IsNullOrEmpty(Model?.Markdown) ? "" : Model.Markdown;

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            ClassHeaderViewModel that = obj as ClassHeaderViewModel;
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

using Compendium.Model.Races;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.RaceViewer
{
    public class RaceHeaderViewModel
    {
        private readonly Race _Model;

        public RaceHeaderViewModel(Race model)
        {
            _Model = model;
        }

        public string Name => _Model.Name;

        internal Race Model
        { get { return _Model; } }

        public string Markdown => string.IsNullOrEmpty(Model?.Markdown) ? "" : Model.Markdown;

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            RaceHeaderViewModel that = obj as RaceHeaderViewModel;
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

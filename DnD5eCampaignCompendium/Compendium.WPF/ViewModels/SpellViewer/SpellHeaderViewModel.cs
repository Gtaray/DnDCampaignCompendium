using Assisticant;
using Compendium.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.SpellViewer
{
    public class SpellHeaderViewModel
    {
        private readonly SpellModel _Model;

        public SpellHeaderViewModel(SpellModel model)
        {
            _Model = model;
        }

        internal SpellModel Model
        { get { return _Model; } }

        public string Markdown => _Model.Markdown;

        public string Name
        {
            get { return _Model.Name; }
        }

        public int Level
        {
            get { return _Model.Level; }
        }

        public string School
        {
            get { return _Model.School.Name; }
        }

        public bool Concentration
        {
            get { return _Model.IsConcentration; }
        }

        public bool Ritual
        {
            get { return _Model.IsRitual; }
        }

        public string LevelAndSchool => _Model.LevelAndSchool;

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            SpellHeaderViewModel that = obj as SpellHeaderViewModel;
            if (that == null)
                return false;
            return Object.Equals(this._Model, that._Model);
        }

        public override int GetHashCode()
        {
            return _Model.GetHashCode();
        }
    }
}

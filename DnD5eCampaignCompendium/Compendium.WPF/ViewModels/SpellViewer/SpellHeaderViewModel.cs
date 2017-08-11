using Compendium.Model.SpellViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.SpellViewer
{
    public class SpellHeaderViewModel
    {
        private readonly Spell _Model;

        public SpellHeaderViewModel(Spell model)
        {
            _Model = model;
        }

        internal Spell Model
        { get { return _Model; } }

        public string Name
        {
            get { return _Model.Name; }
        }

        internal int Level
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

        public SpellDisplayViewModel Display
        {
            get { return new SpellDisplayViewModel(_Model); }
        }

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

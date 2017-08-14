using Assisticant;
using Compendium.Model.SpellViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.SpellViewer
{
    public class SpellViewModel
    {
        private readonly Spell _Model;

        public SpellViewModel(Spell model)
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

        public string CastingTime
        { get { return _Model.CastingTime; } }

        public string Range
        { get { return _Model.Range; } }

        public string Components
        {
            get
            {
                return string.Format("{0} {1}",
                    string.Join(", ", _Model.ComponentInitials),
                    string.IsNullOrEmpty(_Model.MaterialComponent) ? "" : "(" + _Model.MaterialComponent + ")")
                    .Trim();
            }
        }

        public string Duration
        {
            get
            {
                return _Model != null ?
                    _Model.IsConcentration ?
                        "Concentration, up to " + _Model.Duration :
                        _Model?.Duration :
                    "";
            }
        }

        public string Description
        { get { return _Model.Description; } }

        public string HigherLevel
        { get { return _Model.HigherLevel; } }

        public string Source
        { get { return _Model.Source != null ? _Model.Source.ToString() : "Unknown"; } }

        public string LevelAndSchool
        {
            get
            {
                if (_Model == null)
                    return "";
                string s = "";
                switch (_Model.Level)
                {
                    case (0):
                        s = _Model.School + " cantrip";
                        break;
                    case (1):
                        s = "1st-level " + _Model.School;
                        break;
                    case (2):
                        s = "2nd-level " + _Model.School;
                        break;
                    case (3):
                        s = "3rd-level " + _Model.School;
                        break;
                    case (4):
                    case (5):
                    case (6):
                    case (7):
                    case (8):
                    case (9):
                        s = _Model.Level + "th-level " + _Model.School;
                        break;
                }
                if (_Model.IsRitual)
                    s += " (ritual)";

                return s;
            }
        }

        public string ErrataList
        {
            get
            {
                return _Model != null ?
                    string.Join("\n\r", _Model.ErrataList.Select(e => e.ToString())) :
                    "";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            SpellViewModel that = obj as SpellViewModel;
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

using Compendium.Model.SpellViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.SpellViewer
{
    public class SpellDisplayViewModel
    {
        private readonly SpellSelectionModel _Selection;

        public SpellDisplayViewModel(SpellSelectionModel selection)
        {
            _Selection = selection;
        }

        internal Spell Selected => _Selection.Value;

        public string Name => Selected?.Name;

        internal int Level => Selected == null 
            ? 0 
            : Selected.Level;

        public string School => Selected == null || Selected.School == null 
            ? "Unknown"
            : Selected.School.Name;

        public bool Concentration => Selected == null
            ? false
            : Selected.IsConcentration;

        public bool Ritual => Selected == null
            ? false
            : Selected.IsRitual;

        public string CastingTime => Selected?.CastingTime;

        public string Range => Selected?.Range;

        public string Components => Selected != null
            ? string.Format("{0}{1}",
                    string.Join(", ", Selected.ComponentInitials),
                    string.IsNullOrEmpty(Selected.MaterialComponent)
                        ? ""
                        : string.Format(" ({0})", Selected.MaterialComponent))
            : "";

        public string Duration => Selected != null
            ? Selected.IsConcentration
                ? "Concentration, up to " + Selected.Duration
                : Selected?.Duration
            : "";

        public string Description => Selected?.Description;

        public string HigherLevel => string.IsNullOrEmpty(Selected?.HigherLevel) ? null : Selected.HigherLevel;

        public string Source => Selected?.Source != null
            ? Selected.Source.ToString() 
            : "Unknown";

        public string LevelAndSchool
        {
            get
            {
                if (Selected == null)
                    return "";
                string s = "";
                switch (Selected.Level)
                {
                    case (0):
                        s = Selected.School + " cantrip";
                        break;
                    case (1):
                        s = "1st-level " + Selected.School;
                        break;
                    case (2):
                        s = "2nd-level " + Selected.School;
                        break;
                    case (3):
                        s = "3rd-level " + Selected.School;
                        break;
                    case (4):
                    case (5):
                    case (6):
                    case (7):
                    case (8):
                    case (9):
                        s = Selected.Level + "th-level " + Selected.School;
                        break;
                }
                if (Selected.IsRitual)
                    s += " (ritual)";

                return s;
            }
        }

        public string ErrataList => Selected != null
            ? string.Join("\n\r", Selected.ErrataList.Select(e => e.ToString()))
            : "";

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            SpellDisplayViewModel that = obj as SpellDisplayViewModel;
            if (that == null)
                return false;
            return Object.Equals(this.Selected, that.Selected);
        }

        public override int GetHashCode()
        {
            return Selected != null
                ? Selected.GetHashCode()
                : base.GetHashCode();
        }
    }
}

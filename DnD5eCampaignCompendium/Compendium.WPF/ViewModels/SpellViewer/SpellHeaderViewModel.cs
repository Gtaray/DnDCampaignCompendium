using Assisticant;
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

        public string Markdown =>
            string.Format(
                "# {0}\n\n" +
                "### {1}\n\n" +
                "---\n\n" +
                "**Casting Time:** {2}\n\n" +
                "**Range:** {3}\n\n" +
                "**Components:** {4}\n\n" +
                "**Duration:** {5}\n\n" +
                "{6}\n\n" +
                "{7}" +
                "**Source:** {8}",
                Name,
                LevelAndSchool,
                Model?.CastingTime,
                Model?.Range,
                Components,
                Duration,
                Model?.Description,
                string.IsNullOrEmpty(Model?.HigherLevel) 
                    ? "" 
                    : string.Format("**At Higher Levels:** {0}\n\n", Model.HigherLevel),
                Source
            );

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

        public string LevelAndSchool
        {
            get
            {
                if (Model == null)
                    return "";
                string s = "";
                switch (Level)
                {
                    case (0):
                        s = School + " cantrip";
                        break;
                    case (1):
                        s = "1st-level " + School;
                        break;
                    case (2):
                        s = "2nd-level " + School;
                        break;
                    case (3):
                        s = "3rd-level " + School;
                        break;
                    case (4):
                    case (5):
                    case (6):
                    case (7):
                    case (8):
                    case (9):
                        s = Level + "th-level " + School;
                        break;
                }
                if (Ritual)
                    s += " (ritual)";

                return s;
            }
        }

        public string Components => Model != null
            ? string.Format("{0}{1}",
                    string.Join(", ", Model.ComponentInitials),
                    string.IsNullOrEmpty(Model.MaterialComponent)
                        ? ""
                        : string.Format(" ({0})", Model.MaterialComponent))
            : "";

        public string Duration => Model != null
            ? Model.IsConcentration
                ? "Concentration, up to " + Model.Duration
                : Model?.Duration
            : "";

        public string Source => Model?.Source != null
            ? Model.Source.ToString()
            : "Unknown";

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

using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.SpellViewer;
using Dragablz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.SpellViewer
{
    public class SpellViewerViewModel
    {
        private readonly SpellViewerModel _Model;
        private readonly SpellSelectionModel _SelectedSpell;

        public SpellViewerViewModel(SpellViewerModel model, SpellSelectionModel selected)
        {
            _Model = model;
            _SelectedSpell = selected;
        }

        public IEnumerable<SpellViewModel> Spells
        {
            get { return _Model.AllSpells.Select(s => new SpellViewModel(s)); }
        }

        public SpellViewModel SelectedSpell
        {
            get { return new SpellViewModel(_SelectedSpell.Value); }
            set { _SelectedSpell.Value = value.Model; }
        }
    }
}

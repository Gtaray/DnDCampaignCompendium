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

        public SpellViewerViewModel(SpellViewerModel model)
        {
            _Model = model;
        }

        public IEnumerable<SpellHeaderViewModel> Spells
        {
            get { return _Model.AllSpells.Select(s => new SpellHeaderViewModel(s)); }
        }

        private Observable<SpellHeaderViewModel> _SelectedSpell = 
            new Observable<SpellHeaderViewModel>(default(SpellHeaderViewModel));
        public SpellHeaderViewModel SelectedSpell
        {
            get { return _SelectedSpell; }
            set { _SelectedSpell.Value = value; }
        }
    }
}

using Compendium.Model.SpellViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.SpellViewer
{
    public class SpellListViewModel
    {
        private readonly SpellList _Model;

        public SpellListViewModel(SpellList model)
        {
            _Model = model;
        }

        public SpellList Model
        { get { return _Model; } }

        public string Name
        {
            get { return _Model.Name; }
            set { _Model.Name = value; }
        }

        public bool ReadOnly
        { get { return _Model.ReadOnly; } }

        public IEnumerable<SpellViewModel> Spells
        { get { return _Model.Spells.Select(s => new SpellViewModel(s)); } }

        public void AddSpell(SpellViewModel spell)
        {
            _Model.AddSpell(spell.Model);
        }
    }
}

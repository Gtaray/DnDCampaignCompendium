using Compendium.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class CharacterViewModel
    {
        private readonly CompendiumModel _Compendium;
        private readonly SelectionModel<CharacterModel> _Selected;

        public CharacterViewModel(CompendiumModel compendium, SelectionModel<CharacterModel> selection)
        {
            _Compendium = compendium;
            _Selected = selection;
        }

        private CharacterModel Selected => _Selected.Value;

        public string Name => Selected.Name;
        //public IEnumerable<SpellHeaderViewModel> Spells =>
        //    _Model.Spells.Select(s => new SpellHeaderViewModel(s));
    }
}

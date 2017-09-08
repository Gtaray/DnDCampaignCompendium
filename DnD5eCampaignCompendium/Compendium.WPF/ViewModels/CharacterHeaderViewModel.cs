using Compendium.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class CharacterHeaderViewModel
    {
        private readonly CharacterModel _Model;

        public CharacterHeaderViewModel(CharacterModel model)
        {
            _Model = model;
        }

        public string Name
        {
            get { return _Model.Name; }
        }
    }
}

using Assisticant;
using Compendium.Model.CharacterClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.ClassViewer
{
    public class ClassViewModel
    {
        private readonly CharacterClass _Model;

        public ClassViewModel(CharacterClass model)
        {
            _Model = model;
        }

        internal CharacterClass Model => _Model;

        public string Name
        {
            get { return _Model.Name; }
        }
    }
}

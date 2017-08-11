using Compendium.Model;
using Compendium.WPF.ViewModels.SpellViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class CompendiumViewModel
    {
        private readonly CompendiumModel _Model;

        public CompendiumViewModel()
        {
            _Model = new CompendiumModel();
        }

        private SpellViewerViewModel _SpellViewer;
        public SpellViewerViewModel SpellViewer
        {
            get
            {
                if (_SpellViewer == null)
                    _SpellViewer = new SpellViewerViewModel(_Model.SpellViewer);
                return _SpellViewer;
            }
        }
    }
}

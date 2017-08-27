using Compendium.Model.Common;
using Compendium.Model.Races;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.RaceViewer
{
    public class RaceViewerViewModel
    {
        private readonly RaceViewerModel _Model;
        private readonly SelectionModel<Race> _SelectedRace;

        public RaceViewerViewModel(RaceViewerModel model, SelectionModel<Race> selected)
        {
            _Model = model;
            _SelectedRace = selected;
        }

        public IEnumerable<RaceHeaderViewModel> Races
        {
            get
            {
                return _Model.Races
                    .Select(s => new RaceHeaderViewModel(s));
            }
        }

        public RaceHeaderViewModel SelectedRace
        {
            get
            {
                return _SelectedRace?.Value == null ? null : new RaceHeaderViewModel(_SelectedRace.Value);
            }
            set { _SelectedRace.Value = value?.Model; }
        }
            
    }
}

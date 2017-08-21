using Assisticant;
using Assisticant.Fields;
using Compendium.Model.CharacterClasses;
using Compendium.WPF.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.ClassViewer
{
    public class ClassViewerViewModel
    {
        private readonly ClassViewerModel _Model;
        private readonly ClassSelectionModel _SelectedClass;

        public ClassViewerViewModel(ClassViewerModel model, ClassSelectionModel selected)
        {
            _Model = model;
            _SelectedClass = selected;
        }

        public IEnumerable<ClassHeaderViewModel> Classes
        {
            get { return _Model.Classes.Select(s => new ClassHeaderViewModel(s, _SelectedClass)); }
        }

        public ClassHeaderViewModel SelectedClass =>
            _SelectedClass.Value != null ? new ClassHeaderViewModel(_SelectedClass.Value, _SelectedClass) : null;
    }
}

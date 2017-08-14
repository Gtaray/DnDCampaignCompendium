using Assisticant;
using Assisticant.Fields;
using Compendium.Model.CharacterClasses;
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

        public IEnumerable<ClassViewModel> Classes
        {
            get { return _Model.Classes.Select(s => new ClassViewModel(s)); }
        }

        public ClassViewModel SelectedClass
        {
            get { return new ClassViewModel(_SelectedClass.Value); }
            set { _SelectedClass.Value = value.Model; }
        }
    }
}

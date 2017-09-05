using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Models;
using Compendium.WPF.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.ClassViewer
{
    public class ClassPageViewModel
    {
        private readonly ClassPageModel _Model;

        public ClassPageViewModel(ClassPageModel model)
        {
            _Model = model;
            _Classes = new ObservableList<ClassHeaderViewModel>();
            foreach (var c in _Model.Content)
                if(c.ShowInClassList) _Classes.Add(new ClassHeaderViewModel(c, _Model.Selection));
        }

        private ObservableList<ClassHeaderViewModel> _Classes;
        public IEnumerable<ClassHeaderViewModel> Classes
        {
            get
            {
                return _Classes;
            }
        }


        public ClassHeaderViewModel SelectedClass =>
            _Model.SelectedItem != null ? new ClassHeaderViewModel(_Model.SelectedItem, _Model.Selection) : null;

        #region Filters
        Observable<string> _SearchFilter = new Observable<string>();
        public string SearchFilter
        {
            get { return _SearchFilter.Value; }
            set { _SearchFilter.Value = value; }
        }
        #endregion
    }
}

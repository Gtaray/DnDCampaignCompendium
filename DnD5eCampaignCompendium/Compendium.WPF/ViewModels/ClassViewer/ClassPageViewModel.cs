using Assisticant;
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
        }

        public IEnumerable<ClassHeaderViewModel> Classes
        {
            get
            {
                return _Model.Content
                    .Where(c => c.ShowInClassList)
                    .Select(s => new ClassHeaderViewModel(s, _Model.Selection));
            }
        }

        public ClassHeaderViewModel SelectedClass =>
            _Model.SelectedItem != null ? new ClassHeaderViewModel(_Model.SelectedItem, _Model.Selection) : null;

        #region Filters
        #endregion
    }
}

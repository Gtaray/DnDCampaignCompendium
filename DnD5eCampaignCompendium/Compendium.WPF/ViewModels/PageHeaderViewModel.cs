using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class PageHeaderViewModel
    {
        private readonly BasePageViewModel _Model;
        public PageHeaderViewModel(BasePageViewModel model)
        {
            _Model = model;
        }

        public string Header => _Model.Header;

        public BasePageViewModel Model => _Model;
    }
}

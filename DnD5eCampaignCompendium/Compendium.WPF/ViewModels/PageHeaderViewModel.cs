using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class PageHeaderViewModel
    {
        private readonly ContentPageViewModel _Model;
        public PageHeaderViewModel(ContentPageViewModel model)
        {
            _Model = model;
        }

        public string Header => _Model.Header;

        public ContentPageViewModel Model => _Model;
    }
}

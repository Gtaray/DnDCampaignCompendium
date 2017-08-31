using Compendium.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels.Common
{
    public class Page
    {
        public string Name;
        public object Content;

        public Page(string name, object content)
        {
            Name = name;
            Content = content;
        }
    }
}

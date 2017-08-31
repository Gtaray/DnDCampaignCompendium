using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model;
using Compendium.Model.Models;
using Compendium.WPF.ViewModels.Common;
using Compendium.WPF.Views;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Compendium.WPF.ViewModels
{
    public class CompendiumViewModel
    {
        private readonly CompendiumModel _Model;
        private readonly IEnumerable<Page> _AllPages;

        public CompendiumViewModel(CompendiumModel model, IEnumerable<Page> allPages)
        {
            _Model = model;
            _AllPages = allPages;
        }

        public IEnumerable<Page> AllPages => _AllPages;

        //public IEnumerable<Page> SpellPage =>
        //    new List<Page>() { new Page("Spells", new SpellViewerView())};

        //public IEnumerable<Page> ClassPage =>
        //    new List<Page>() { new Page("Classes", new ClassViewerView()) };

        //public IEnumerable<Page> OtherPages =>
        //    _Model.OtherPages.Select(p => new Page(p.Header, new ))
    }
}

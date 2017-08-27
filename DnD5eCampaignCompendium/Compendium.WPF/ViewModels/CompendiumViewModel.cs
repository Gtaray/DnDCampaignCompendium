using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model;
using Compendium.WPF.ViewModels.ClassViewer;
using Compendium.WPF.ViewModels.SpellViewer;
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

        public CompendiumViewModel(CompendiumModel model)
        {
            _Model = model;

            _NavList = new ObservableList<string>()
            {
                "Spells",
                "Classes",
                "Races"
            };
        }

        private ObservableList<string> _NavList;
        public IEnumerable<string> NavList
        {
            get { return _NavList; }
        }

        private Observable<int> _SelectedPage = new Observable<int>(1);
        public int SelectedPage
        {
            get { return _SelectedPage; }
            set { _SelectedPage.Value = value; }
        }

        private Computed<string> _PageTitle;
        public string PageTitle
        {
            get
            {
                if (_PageTitle == null)
                    _PageTitle = new Computed<string>(() => _NavList[SelectedPage]);
                return _PageTitle.Value;
            }
        }
    }
}

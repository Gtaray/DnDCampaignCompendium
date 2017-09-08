using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model;
using Compendium.Model.Models;
using Compendium.WPF.ViewModels;
using Compendium.WPF.ViewModels.Common;
using Compendium.WPF.Views;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Compendium.WPF.ViewModels
{
    public class CompendiumViewModel
    {
        private readonly CompendiumModel _Model;
        private readonly IEnumerable<Page> _CompendiumPages;

        public CompendiumViewModel(CompendiumModel model, IEnumerable<Page> compendiumPages)
        {
            _Model = model;
            _CompendiumPages = compendiumPages;
        }

        public IEnumerable<Page> CompendiumPages => _CompendiumPages;

        // Characters that appear in the navigation panel
        public IEnumerable<CharacterHeaderViewModel> Characters =>
            _Model.Characters.Select(c => new CharacterHeaderViewModel(c));

        public ICommand AddNewCharacterCommand =>
            MakeCommand.Do(() =>
            {
                _Model.AddNewCharacter();
            });

        public ICommand ExportSpellsToJson =>
            MakeCommand.Do(() =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".json";

                if(sfd.ShowDialog() == true)
                {
                    File.WriteAllText(
                        sfd.FileName, 
                        _Model.SpellPage.SerializeSpells());
                }
            });
    }
}

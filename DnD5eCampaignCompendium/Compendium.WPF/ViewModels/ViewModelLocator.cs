using Assisticant;
using Compendium.Model;
using Compendium.Model.CharacterClasses;
using Compendium.Model.SpellViewer;
using Compendium.WPF.ViewModels.ClassViewer;
using Compendium.WPF.ViewModels.SpellViewer;
using Compendium.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private const string SOURCES = "Compendium.Model.Json.Sources.json";
        private const string SPELLS = "Compendium.Model.Json.Spells.json";
        private const string SPELL_COMPONENTS = "Compendium.Model.Json.SpellComponents.json";
        private const string SPELL_SCHOOLS = "Compendium.Model.Json.SpellSchools.json";
        private const string CLASSES = "Compendium.Model.Json.Classes.json";

        private readonly CompendiumModel _Compendium;
        private readonly SpellViewerModel _SpellViewer;
        private readonly SpellSelectionModel _SelectedSpell;
        private readonly ClassViewerModel _ClassViewer;
        private readonly ClassSelectionModel _SelectedClass;

        public object Compendium => ViewModel(() => new CompendiumViewModel(_Compendium));

        public object SpellViewer => ViewModel(() => new SpellViewerViewModel(_Compendium, _SelectedSpell));
        public object SpellDisplay => ViewModel(() => new SpellDisplayViewModel(_SelectedSpell));

        public object ClassViewer => ViewModel(() => new ClassViewerViewModel(_ClassViewer, _SelectedClass));

        public ViewModelLocator()
        {
            _Compendium = new CompendiumModel();
            _SpellViewer = new SpellViewerModel(_Compendium);
            _ClassViewer = new ClassViewerModel(_Compendium);
            _SelectedSpell = new SpellSelectionModel();
            _SelectedClass = new ClassSelectionModel();

            if (DesignMode)
            {

            }
            else
            {
                // CONTENT SOURCES
                _Compendium.DeserializeContentSources(
                    ResourceHelper.ReadEmbeddedResourceContent(SOURCES));

                // SPELLS
                _SpellViewer.DeserializeSchools(
                    ResourceHelper.ReadEmbeddedResourceContent(SPELL_SCHOOLS));
                _SpellViewer.DeserializeComponents(
                    ResourceHelper.ReadEmbeddedResourceContent(SPELL_COMPONENTS));
                _SpellViewer.DeserializeSpells(
                    ResourceHelper.ReadEmbeddedResourceContent(SPELLS));

                // CLASSES
                // Classes HAVE to be done after spells, because classes contain spells, and thus the spell list
                // needs to be populated before creating classes that query for those spells
                _ClassViewer.DeserializeClasses(
                    ResourceHelper.ReadEmbeddedResourceContent(CLASSES));
            }
        }
    }
}

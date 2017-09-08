using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Helpers;
using Compendium.Model.Models;
using Compendium.WPF.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compendium.WPF.ViewModels
{
    public class SpellPageViewModel : BasePageViewModel
    {
        private readonly CompendiumModel _Compendium;

        public SpellPageViewModel(CompendiumModel compendium)
        {
            _Compendium = compendium;

            // Initialize filters
            _ClassFilters = new ObservableList<FilterFlagViewModel<ClassModel>>(
                _ClassViewerModel.Content
                    .Where(c => c.IsRoot && c.ShowInFilterList && c.HasSpells)
                    .Select(c => GetClassFilterObject(c)));

            _SchoolFilters = new ObservableList<FilterFlagViewModel<SpellSchool>>(
                _SpellViewerModel.SpellSchools.Select(s => new FilterFlagViewModel<SpellSchool>(s, () => s.Name)));

            _ComponentFilters = new ObservableList<FilterFlagViewModel<SpellComponent>>(
                _SpellViewerModel.Components.Select(c => new FilterFlagViewModel<SpellComponent>(c, () => c.Name)));

            _SourceFilters = new ObservableList<FilterFlagViewModel<ContentSource>>(
                _Compendium.ContentSources.Select(s => new FilterFlagViewModel<ContentSource>(s, () => s.Name)));
        }

        private SpellPageModel _SpellViewerModel => _Compendium.SpellPage;
        private ClassPageModel _ClassViewerModel => _Compendium.ClassPage;

        public IEnumerable<SpellHeaderViewModel> Spells =>
            _SpellViewerModel.Content.Select(s => new SpellHeaderViewModel(s));

        public IEnumerable<SpellHeaderViewModel> FilteredSpells
        {
            get
            {
                var list = _SpellViewerModel.Content;
                if (!string.IsNullOrEmpty(NameFilter))
                    list = list.Where(s => s.Name.IndexOf(NameFilter, StringComparison.OrdinalIgnoreCase) >= 0 || 
                    s.Description.IndexOf(NameFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                if (LevelFilters.Any(f => f.IsChecked))
                    list = list.Where(s => LevelFilters.ElementAt(s.Level).IsChecked);

                if (ClassFilters.Any(f => f.AnyChecked))
                {
                    var checkedClasses = GetCheckedClassFilters().Select(f => f.Filter);
                    list = list.Where(s => s.Classes.Intersect(checkedClasses).Count() > 0);
                }

                if (SchoolFilters.Any(f => f.IsChecked))
                {
                    var checkedSchools = SchoolFilters.Where(f => f.IsChecked);
                    list = list.Where(s => checkedSchools.Any(f => f.Filter == s.School));
                }

                if (ComponentFilters.Any(f => f.IsChecked))
                {
                    var checkedComps = ComponentFilters.Where(f => f.IsChecked).Select(f => f.Filter);
                    if (IgnoreUncheckedComponents)
                        list = list.Where(s => s.Components.Intersect(checkedComps).Count() > 0);
                    else
                        list = list.Where(s => ComponentFilters.All(f =>
                            (f.IsChecked && s.Components.Contains(f.Filter)) ||
                            (!f.IsChecked && !s.Components.Contains(f.Filter))));
                }

                if (SourceFilters.Any(f => f.IsChecked))
                {
                    var checkedSources = SourceFilters.Where(f => f.IsChecked);
                    list = list.Where(s => checkedSources.Any(f => f.Filter == s.Source));
                }

                if (ShowConcentrationSpells)
                    list = list.Where(s => s.IsConcentration);

                if (ShowRitualSpells)
                    list = list.Where(s => s.IsRitual);

                return list.Select(s => new SpellHeaderViewModel(s));
            }
        }

        public SpellHeaderViewModel SelectedSpell
        {
            get
            {
                return _SpellViewerModel.SelectedItem == null ? null : new SpellHeaderViewModel(_SpellViewerModel.SelectedItem);
            }
            set { _SpellViewerModel.SelectedItem = value?.Model; }
        }

        public bool SpellDisplayIsVisible => SelectedSpell != null;

        #region Filters
        #region By Name
        Observable<string> _NameFilter = new Observable<string>();
        public string NameFilter
        {
            get { return _NameFilter.Value; }
            set { _NameFilter.Value = value; }
        }
        #endregion

        #region By Level
        private ObservableList<FilterFlagViewModel<int>> _LevelFilters =
            new ObservableList<FilterFlagViewModel<int>>()
        {
            new FilterFlagViewModel<int>(0, () => "Cantrips", false),
            new FilterFlagViewModel<int>(1, () => "1st-level", false),
            new FilterFlagViewModel<int>(2, () => "2nd-level", false),
            new FilterFlagViewModel<int>(3, () => "3rd-level", false),
            new FilterFlagViewModel<int>(4, () => "4th-level", false),
            new FilterFlagViewModel<int>(5, () => "5th-level", false),
            new FilterFlagViewModel<int>(6, () => "6th-level", false),
            new FilterFlagViewModel<int>(7, () => "7th-level", false),
            new FilterFlagViewModel<int>(8, () => "8th-level", false),
            new FilterFlagViewModel<int>(9, () => "9th-level", false)
        };
        public IEnumerable<FilterFlagViewModel<int>> LevelFilters => _LevelFilters;
        #endregion

        #region By Class
        private ObservableList<FilterFlagViewModel<ClassModel>> _ClassFilters;
        public IEnumerable<FilterFlagViewModel<ClassModel>> ClassFilters => _ClassFilters;

        private FilterFlagViewModel<ClassModel> GetClassFilterObject(ClassModel cc)
        {
            FilterFlagViewModel<ClassModel> newfilter =
                new FilterFlagViewModel<ClassModel>(cc, () => string.IsNullOrEmpty(cc.ShortName) ? cc.Name : cc.ShortName, false);

            foreach (var child in cc.Subclasses)
            {
                if (child.ShowInFilterList && child.HasSpells)
                    newfilter.AddChildFilter(GetClassFilterObject(child));
            }

            return newfilter;
        }

        private IEnumerable<FilterFlagViewModel<ClassModel>> GetCheckedClassFilters()
        {
            return ClassFilters.Flatten(c => c.Children).Where(c => c.IsChecked);
        }
        #endregion

        #region By School
        private ObservableList<FilterFlagViewModel<SpellSchool>> _SchoolFilters;
        public IEnumerable<FilterFlagViewModel<SpellSchool>> SchoolFilters => _SchoolFilters;
        #endregion

        #region By Components
        private ObservableList<FilterFlagViewModel<SpellComponent>> _ComponentFilters;
        public IEnumerable<FilterFlagViewModel<SpellComponent>> ComponentFilters => _ComponentFilters;

        private Observable<bool> _IgnoreUncheckedComponents = new Observable<bool>(true);
        public bool IgnoreUncheckedComponents
        {
            get { return _IgnoreUncheckedComponents; }
            set { _IgnoreUncheckedComponents.Value = value; }
        }
        #endregion

        #region By Source
        private ObservableList<FilterFlagViewModel<ContentSource>> _SourceFilters;
        public IEnumerable<FilterFlagViewModel<ContentSource>> SourceFilters => _SourceFilters;
        #endregion

        #region By Misc
        Observable<bool> _ShowRitualSpells = new Observable<bool>(false);
        public bool ShowRitualSpells
        {
            get { return _ShowRitualSpells.Value; }
            set { _ShowRitualSpells.Value = value; }
        }

        Observable<bool> _ShowConcentrationSpells = new Observable<bool>(false);
        public bool ShowConcentrationSpells
        {
            get { return _ShowConcentrationSpells.Value; }
            set { _ShowConcentrationSpells.Value = value; }
        }
        #endregion
        #endregion
    }
}

using Assisticant;
using Assisticant.Collections;
using Assisticant.Collections.Impl;
using Assisticant.Fields;
using Compendium.Model.CharacterClasses;
using Compendium.Model.SpellViewer;
using Compendium.WPF.Extentions;
using Compendium.WPF.ViewModels.Common;
using Dragablz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Compendium.WPF.ViewModels.SpellViewer
{
    public class SpellViewerViewModel
    {
        private readonly SpellViewerModel _Model;
        private readonly SpellSelectionModel _SelectedSpell;

        public SpellViewerViewModel(SpellViewerModel model, SpellSelectionModel selected)
        {
            _Model = model;
            _SelectedSpell = selected;

            // Initialize filter
            _AllFilters = new GroupFilter();

            // Initialize collection view and apply filter
            SpellCollectionView = new CollectionViewSource();
            SpellCollectionView.Source = Spells;
            SpellCollectionView.SortDescriptions.Add(new SortDescription("Level", ListSortDirection.Ascending));
            SpellCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            SpellCollectionView.View.Filter = _AllFilters.Filter;
        }

        public IEnumerable<SpellHeaderViewModel> Spells =>
            _Model.AllSpells.Select(s => new SpellHeaderViewModel(s));

        public IEnumerable<SpellHeaderViewModel> FilteredSpells
        {
            get
            {
                var list = _Model.AllSpells;
                if (LevelFilters.Any(f => f.IsChecked))
                    list = list.Where(s => LevelFilters.ElementAt(s.Level).IsChecked);
                return list.Select(s => new SpellHeaderViewModel(s));
                //return _Model.AllSpells.Where(s => _AllFilters.Filter(s)).Select(s => new SpellHeaderViewModel(s));
            }
        }

        public CollectionViewSource SpellCollectionView;

        public SpellHeaderViewModel SelectedSpell
        {
            get { return new SpellHeaderViewModel(_SelectedSpell.Value); }
            set { _SelectedSpell.Value = value?.Model; }
        }

        #region Filters
        private GroupFilter _AllFilters;

        #region By Level

        private ObservableList<FilterFlagViewModel<int>> _LevelFilters = new ObservableList<FilterFlagViewModel<int>>()
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
        public IEnumerable<FilterFlagViewModel<int>> LevelFilters
        {
            get { return _LevelFilters; }
        }

        private ICommand _ToggleLevelFilterCommand;
        public ICommand ToggleLevelFilterCommand
        {
            get
            {
                if (_ToggleLevelFilterCommand == null)
                {
                    _ToggleLevelFilterCommand = new RelayCommand<FilterFlagViewModel<int>>(
                        param => this.ToggleLevelFilter()
                        );
                }
                return _ToggleLevelFilterCommand;
            }
        }

        private void ToggleLevelFilter()
        {
            // After toggling any of the level filter flags
            // if any of the level filters are checked, add the level filter
            if (_LevelFilters.Any(f => f.IsChecked == true))
                _AllFilters.AddFilter(FilterByLevel);
            // If none of the level filters are checked, remove the level filter
            else
                _AllFilters.RemoveFilter(FilterByLevel);
        }

        private bool FilterByLevel(object e)
        {
            var src = e as SpellHeaderViewModel;
            if (src == null)
                return false;
            bool accept = _LevelFilters.ElementAt(src.Level).IsChecked;
            return accept;
        }
        #endregion

        #region By Class

        private ObservableList<FilterFlagViewModel<CharacterClass>> _ClassFilters = new ObservableList<FilterFlagViewModel<CharacterClass>>();
        public IEnumerable<FilterFlagViewModel<CharacterClass>> ClassFilters
        {
            get { return _ClassFilters; }
        }

        private ICommand _ToggleClassFilterCommand;
        public ICommand ToggleClassFilterCommand
        {
            get
            {
                if (_ToggleClassFilterCommand == null)
                {
                    _ToggleClassFilterCommand = new RelayCommand<FilterFlagViewModel<CharacterClass>>(
                        param => this.ToggleClassFilter(param)
                    );
                }
                return _ToggleClassFilterCommand;
            }
        }

        private void ToggleClassFilter(FilterFlagViewModel<CharacterClass> flag)
        {
            // if checked, add
            //if (flag.IsChecked)
            //{
            //    if (!ClassFilters.Contains(flag.Filter))
            //        ClassFilters.Add(flag.Filter);
            //    // Since at least one filter is checked and added to the filter list
            //    // we add the class filter to the group filter (doesn't get added multiple times)
            //    _AllFilters.AddFilter(FilterByClass);
            //}
            //// if unchecked, remove
            //else
            //{
            //    if (ClassFilters.Contains(flag.Filter))
            //        ClassFilters.Remove(flag.Filter);
            //    // After removing a filter, check if there are still filters in the filter list
            //    // if not, remove the class filter from the group filter
            //    if (ClassFilters.Count() == 0)
            //        _AllFilters.RemoveFilter(FilterByClass);
            //}
        }

        private bool FilterByClass(object e)
        {
            var src = e as SpellHeaderViewModel;
            if (src == null)
                return false;
            bool accept = ClassFilters.Any(f => src.Model.Classes.Contains(f.Filter));
            return accept;
        }
        #endregion
        #endregion
    }
}

using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Filtering;
using Compendium.Model.Models;
using Compendium.WPF.Extentions;
using Compendium.WPF.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.WPF.ViewModels
{
    public class ClassPageViewModel : BasePageViewModel
    {
        private readonly ClassPageModel _Model;

        public ClassPageViewModel(ClassPageModel model)
        {
            _Model = model;
            foreach (FilterGroup group in _Model.FilterGroups)
                _FilterGroups.Add(new FilterGroupViewModel(group));
            //_Classes = new ObservableList<ClassHeaderViewModel>();
            //foreach (var c in _Model.Content)
            //    if (c.ShowInClassList) _Classes.Add(new ClassHeaderViewModel(c, _Model.Selection));
        }

        //private ObservableList<ClassHeaderViewModel> _Classes;
        //if (FilterGroups.Any(g => g.AnyChecked)) list = list.Where(c => FilterGroups.Any(g => g.FilterContent(c)));
        public IEnumerable<ClassHeaderViewModel> Classes =>
            _Model.Content
                .Where(c => c.ShowInClassList)
                .Where(c => c.ContainsText(SearchFilter))
                .Where(c => FilterGroups.All(g => g.FilterContent(c)))
                .Select(c => new ClassHeaderViewModel(c, _Model.Selection));

        public ClassHeaderViewModel SelectedClass =>
            _Model.SelectedItem != null ? new ClassHeaderViewModel(_Model.SelectedItem, _Model.Selection) : null;

        #region Filters
        private ObservableList<FilterGroupViewModel> _FilterGroups = new ObservableList<FilterGroupViewModel>();
        public IEnumerable<FilterGroupViewModel> FilterGroups
        {
            get { return _FilterGroups; }
        }

        private Observable<string> _SearchFilter = new Observable<string>("");
        public string SearchFilter
        {
            get { return _SearchFilter.Value; }
            set { _SearchFilter.Value = value; }
        }

        //private IEnumerable<TreeNode> ApplyFilters()
        //{
        //    var filteredList = new List<TreeNode>();
        //    foreach (var c in _Model.Content)
        //    {
        //        TreeNode t = ApplyFiltersToClass(c);
        //        if (t.Root != null)
        //            filteredList.Add(t);
        //    }

        //    return filteredList;
        //}

        //private TreeNode ApplyFiltersToClass(ClassModel c)
        //{
        //    TreeNode t = new TreeNode();

        //    // If there are subclasses, filter through them
        //    foreach(var sub in c.Subclasses)
        //        if (IncludeClass(sub))
        //            t.AddChild(sub);

        //    // If none of the subclasses were added, check if the root passes the filter
        //    if (t.Root == null && IncludeClass(c))
        //        t.Root = c;

        //    return t;
        //}

        //private bool IncludeClass(ClassModel c)
        //{
        //    if (c.ShowInClassList == false)
        //        return false;

        //    if (c.ContainsText(SearchFilter))
        //        return true;

        //    foreach (var filter in FilterGroups)
        //        if (filter.AnyChecked && filter.FilterContent(c))
        //            return true;

        //    return false;
        //}
        #endregion
    }

    //public struct TreeNode
    //{
    //    public ClassModel Root;
    //    public List<ClassModel> Children;

    //    public void AddChild(ClassModel c)
    //    {
    //        if (Children == null)
    //            Children = new List<ClassModel>();
    //        Children.Add(c);
    //        Root = c.Parent;
    //    }
    //} 
}

using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Filtering;
using Compendium.Model.Helpers;
using Compendium.Model.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Models
{
    public class ContentPageModel : IContentPage<ContentItemModel>
    {
        private readonly CompendiumModel _Compendium;
        private readonly SelectionModel<ContentItemModel> _Selection;

        public ContentPageModel(CompendiumModel compendium)
        {
            _Compendium = compendium;
            _Content = new ObservableList<ContentItemModel>();
            _Selection = new SelectionModel<ContentItemModel>();
        }

        #region Properties and Accessors
        public SelectionModel<ContentItemModel> Selection => _Selection;

        public ContentItemModel SelectedItem
        {
            get { return _Selection.Value; }
            set { _Selection.Value = value; }
        }

        private Observable<string> _Header = new Observable<string>("Header");
        public string Header
        {
            get { return _Header; }
            set { _Header.Value = value; }
        }

        private ObservableList<ContentItemModel> _Content;
        public IEnumerable<ContentItemModel> Content => _Content;

        private ObservableList<FilterGroup> _FilterGroups = new ObservableList<FilterGroup>();
        public IEnumerable<FilterGroup> FilterGroups => _FilterGroups;
        #endregion

        public void AddFilterGroup(FilterGroup fg)
        {
            if (_FilterGroups.Contains(fg))
                return;
            _FilterGroups.Add(fg);
        }

        #region Json Parsing
        public void DeserializeContent(string dataDir, string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var content in obj.content)
                DeserializeContentItem(content, dataDir);

            if(obj.filters != null)
            {
                foreach(var filter in obj.filters)
                {
                    FilterGroup fg = new FilterGroup()
                    {
                        ID = filter.id != null ? (string)filter.id : "",
                        Header = filter.title != null ? (string)filter.title : "Filter by Unknown",
                        ShowExlusiveToggle = filter.showExclusiveToggle != null ? (bool)filter.showExclusiveToggle : false,
                        ExclusiveToggleLabel = filter.exclusiveToggleLabel != null ? (string)filter.exclusiveToggleLabel : "Exclude unchecked options",
                        IsExclusive = filter.isExclusive != null ? (bool)filter.isExclusive : false
                    };
                    foreach (var option in filter.options)
                        fg.AddItem((string)option);

                    _FilterGroups.Add(fg);
                }
            }
        }

        private void DeserializeContentItem(dynamic content, string dataDir, ContentItemModel parent = null)
        {
            // Basic properties
            var newContent = new ContentItemModel()
            {
                Name = content.name != null ? (string)content.name : "",
                ID = content.id != null ? (string)content.id : "",
                Source = content.source != null ? _Compendium.GetSourceByID((string)content.source) : null,
                Markdown = content.file != null
                        ? ResourceHelper.ReadTextFromFile(Path.Combine(dataDir, (string)content.file))
                        : "",
                Parent = parent
            };

            // Set all filters
            if (content.filters != null)
                foreach (var filter in content.filters)
                    newContent.AddFilterProperty(
                        filter.id != null ? (string)filter.id : "",
                        filter.value != null ? (string)filter.value : "");

            _Content.Add(newContent);
            
            // Handle sub-content
            if(content.subcontent != null)
                foreach(var subcontent in content.subcontent)
                    DeserializeContentItem(subcontent, dataDir, newContent);
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1} items)", Header, Content.Count().ToString());
        }
    }
}

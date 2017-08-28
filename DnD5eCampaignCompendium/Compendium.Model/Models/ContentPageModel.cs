using Assisticant.Collections;
using Assisticant.Fields;
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
        #endregion

        #region Json Parsing
        public void DeserializeContent(string dataDir, string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var content in obj.content)
            {
                _Content.Add(new ContentItemModel()
                {
                    Name = content.name != null ? (string)content.name : "",
                    ID = content.id != null ? (string)content.id : "",
                    Source = content.source != null ? _Compendium.GetSourceByID((string)content.source) : null,
                    Markdown = ResourceHelper.ReadTextFromFile(Path.Combine(dataDir, (string)content.file))
                });
            }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1} items)", Header, Content.Count().ToString());
        }
    }
}

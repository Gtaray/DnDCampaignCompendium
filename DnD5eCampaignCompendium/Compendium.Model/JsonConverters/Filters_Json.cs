using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.JsonConverters
{
    public class PageFilters_Json : Base_Json
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool showExclusiveToggle { get; set; }
        public string exclusiveToggleLabel { get; set; }
        public bool isExclusive { get; set; }
        public List<PageFilterOption_Json> options { get; set; }
    }

    public class PageFilterOption_Json : Base_Json
    {
        public string id { get; set; }
        public string display { get; set; }
    }

    public class ContentFilter_Json : Base_Json
    {
        public string groupID { get; set; }
        public string valueID { get; set; }
    }

}

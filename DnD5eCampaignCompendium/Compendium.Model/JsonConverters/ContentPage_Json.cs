using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.JsonConverters
{
    public class ContentPage_Json : Base_Json
    {
        public List<Content_Json> content { get; set; }
        public List<PageFilters_Json> filters { get; set; }
    }

    public class Content_Json : Base_Json
    {
        public string name { get; set; }
        public string id { get; set; }
        public string source { get; set; }
        public string file { get; set; }
        public List<Content_Json> subcontent { get; set; }
        public List<ContentFilter_Json> filters { get; set; }
    }
}

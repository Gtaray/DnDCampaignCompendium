using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.JsonConverters
{
    public class Classes_Json : Base_Json
    {
        public List<Class_Json> classes { get; set; }
        public List<PageFilters_Json> filters { get; set; }
    }    

    public class Class_Json : Base_Json
    {
        public string name { get; set; }
        public string shortName { get; set; }
        public string id { get; set; }
        public string source { get; set; }
        public string file { get; set; }
        public List<Class_Json> subclasses { get; set; }
        public List<ContentFilter_Json> filters { get; set; }
        public bool showInFilterList { get; set; }
        public bool showInClassList { get; set; }
    }

    
}

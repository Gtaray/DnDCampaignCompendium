using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.JsonConverters
{
    public class Sources_Json : Base_Json
    {
        public List<Source_Json> sources { get; set; }
    }

    public class Source_Json : Base_Json
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}

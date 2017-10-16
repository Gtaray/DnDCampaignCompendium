using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.JsonConverters
{
    public class Index_Json : Base_Json
    {
        public string sources { get; set; }
        public string spells { get; set; }
        public string spellSchools { get; set; }
        public string spellComponents { get; set; }
        public string classes { get; set; }
        public string classSpells { get; set; }
        public List<Page_Json> otherPages { get; set; }
    }

    public class Page_Json : Base_Json
    {
        public string title { get; set; }
        public string content { get; set; }
    }
}

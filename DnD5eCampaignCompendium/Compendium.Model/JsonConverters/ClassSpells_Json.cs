using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.JsonConverters
{
    public class ClassSpellListCollection_Json
    {
        public List<ClassSpellList_Json> classes { get; set; }
    }

    public class ClassSpellList_Json
    {
        public string name { get; set; }
        public string id { get; set; }
        public List<string> spells { get; set; }
        public List<ClassSpellList_Json> subclasses { get; set; }
    }
}

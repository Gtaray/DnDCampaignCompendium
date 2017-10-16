using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.JsonConverters
{
    public class Spells_Json : Base_Json
    {
        public List<Spell_Json> spells { get; set; }
    }

    public class Spell_Json : Base_Json
    {
        public string id { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public bool concentration { get; set; }
        public bool isRitual { get; set; }
        public List<string> components { get; set; }
        public string material { get; set; }
        public string range { get; set; }
        public string castingTime { get; set; }
        public string duration { get; set; }
        public List<Errata_Json> errata { get; set; }
        public string school { get; set; }
        public List<string> description { get; set; }
        public List<string> higherLevel { get; set; }
        public string source { get; set; }
    }

    public class Schools_Json : Base_Json
    {
        public List<School_Json> schools { get; set; }
    }

    public class School_Json : Base_Json
    {
        public string name { get; set; }
    }

    public class Components_Json : Base_Json
    {
        public List<Component_Json> components { get; set; }
    }

    public class Component_Json : Base_Json
    {
        public string name { get; set; }
        public string initial { get; set; }
    }

    public class Errata_Json : Base_Json
    {
        public string month { get; set; }
        public string year { get; set; }
        public string text { get; set; }
    }
}

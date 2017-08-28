using Assisticant.Fields;
using Compendium.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Common
{
    public class SpellSchool
    {
        public SpellSchool() { }

        public SpellSchool(string name)
        {
            Name = name;
        }

        public SpellSchool(SpellSchool toClone)
        {
            Name = new string(toClone.Name.ToCharArray());
        }

        private Observable<string> _Name = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

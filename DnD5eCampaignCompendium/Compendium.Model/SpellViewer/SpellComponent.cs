using Assisticant.Fields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.SpellViewer
{
    public class SpellComponent
    {
        public SpellComponent()
        {
        }

        public SpellComponent(string name, string initial)
        {
            Name = name;
            Initial = initial;
        }

        public SpellComponent(SpellComponent toClone)
        {
            Name = new string(toClone.Name.ToCharArray());
            if (toClone.Initial != null)
                Initial = new string(toClone.Initial.ToCharArray());
            else
                Initial = Name[0].ToString();
        }

        private Observable<string> _Name = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private Observable<string> _Initial = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "initial")]
        public string Initial
        {
            get { return _Initial; }
            set { _Initial.Value = value; }
        }

        public override string ToString()
        {
            return Name + " (" + Initial + ")";
        }
    }
}

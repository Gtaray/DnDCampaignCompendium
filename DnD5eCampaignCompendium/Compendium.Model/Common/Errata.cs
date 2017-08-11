using Assisticant.Fields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Common
{
    public class Errata
    {
        public Errata(string m, string y, string t)
        {
            Month = m;
            Year = y;
            Description = t;
        }

        public Errata(string m, string y, string t, BaseModel parent)
        {
            Month = m;
            Year = y;
            Description = t;
            _Parent = parent;
        }

        private BaseModel _Parent;
        [JsonIgnore]
        public BaseModel Parent
        { get { return _Parent; } }

        private Observable<string> _Month = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "month")]
        public string Month
        {
            get { return _Month; }
            set { _Month.Value = value; }
        }

        private Observable<string> _Year = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "year")]
        public string Year
        {
            get { return _Year; }
            set { _Year.Value = value; }
        }

        private Observable<string> _Description = new Observable<string>(default(string));
        [JsonProperty(PropertyName = "text")]
        public string Description
        {
            get { return _Description; }
            set { _Description.Value = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}: {2}", Month, Year, Description);
        }
    }
}

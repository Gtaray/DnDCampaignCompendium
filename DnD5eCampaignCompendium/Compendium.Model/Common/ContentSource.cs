using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Common
{
    public class ContentSource
    {
        public ContentSource()
        { }

        public ContentSource(string name, string id)
        {
            Name = name;
            ID = id;
        }

        public ContentSource(ContentSource toClone)
        {
            Name = new string(toClone.Name.ToCharArray());
            ID = new string(toClone.ID.ToCharArray());
        }

        private Observable<string> _ID = new Observable<string>(default(string));
        public string ID
        {
            get { return _ID; }
            set { _ID.Value = value; }
        }

        private Observable<string> _Name = new Observable<string>(default(string));
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

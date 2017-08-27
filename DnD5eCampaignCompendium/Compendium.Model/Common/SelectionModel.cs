using Assisticant;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Common
{
    public class SelectionModel<T>
    {
        public Observable<T> _Value = new Observable<T>();
        public T Value
        {
            get { return _Value.Value; }
            set { _Value.Value = value; }
        }
    }
}

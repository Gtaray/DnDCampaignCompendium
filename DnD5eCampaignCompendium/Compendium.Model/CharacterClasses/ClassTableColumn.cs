using Assisticant.Collections;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.CharacterClasses
{
    public class ClassTableColumn
    {
        public ClassTableColumn()
        { }

        private Observable<string> _ColumnHeader = new Observable<string>("Header");
        public string ColumnHeader
        {
            get { return _ColumnHeader.Value; }
            set { _ColumnHeader.Value = value; }
        }

        private ObservableList<string> _ColumnValues = new ObservableList<string>();
        public IEnumerable<string> ColumnValues => _ColumnValues;

        public void AddColumnValue(string value)
        {
            _ColumnValues.Add(value);
        }
    }
}

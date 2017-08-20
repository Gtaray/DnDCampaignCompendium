using Assisticant;
using Assisticant.Collections;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.CharacterClasses
{
    public class ClassFeature
    {
        public ClassFeature()
        { }

        private Observable<string> _Name = new Observable<string>("New Feature");
        public string Name
        {
            get { return _Name.Value; }
            set { _Name.Value = value; }
        }

        private Observable<int> _Level = new Observable<int>(1);
        public int Level
        {
            get { return _Level.Value; }
            set { _Level.Value = value; }
        }

        private Observable<string> _TableName = new Observable<string>("New Feature");
        public string TableName
        {
            get { return _TableName.Value; }
            set { _TableName.Value = value; }
        }

        private Observable<bool> _TableOnly = new Observable<bool>(false);
        public bool TableOnly
        {
            get { return _TableOnly.Value; }
            set { _TableOnly.Value = value; }
        }

        private ObservableList<string> _Description = new ObservableList<string>();
        public IEnumerable<string> Description => _Description;

        public void AddLineToDescription(string line)
        {
            _Description.Add(line);
        }
    }
}

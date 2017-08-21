using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTest.Model
{
    public class MarkdownModel
    {
        public MarkdownModel()
        {

        }

        private Observable<string> _HTML = new Observable<string>();
        public string HTML
        {
            get { return _HTML; }
            set { _HTML.Value = value; }
        }
    }
}
